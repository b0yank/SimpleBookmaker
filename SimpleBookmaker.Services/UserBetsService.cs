namespace SimpleBookmaker.Services
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Data.Core.Enums;
    using Data.Models;
    using Data.Models.Bets;
    using Data.Models.BetSlips;
    using Microsoft.AspNetCore.Identity;
    using SimpleBookmaker.Services.Models.UserCoefficient;
    using Services.Infrastructure.BetDescribers;
    using System.Collections.Generic;
    using System.Linq;
    using SimpleBookmaker.Services.Models.Bet;
    using System.Threading.Tasks;

    public class UserBetsService : Service, IUserBetsService
    {
        private readonly UserManager<User> userManager;

        public UserBetsService(SimpleBookmakerDbContext db, UserManager<User> userManager)
            : base(db)
        {
            this.userManager = userManager;
        }
        
        public async Task<IEnumerable<UserCurrentBetSlipModel>> CurrentBets(string username)
        {
            var userId = (await this.userManager.FindByNameAsync(username)).Id;

            var currentBetSlips = new List<UserCurrentBetSlipModel>();

            var gameBetSlips = this.db.GameBetSlips.Where(gbs => gbs.UserId == userId);

            foreach (var betSlip in gameBetSlips)
            {
                var currentBetSlip = new UserCurrentBetSlipModel
                {
                    Amount = betSlip.Amount
                };

                currentBetSlip.Bets = this.db.GameBets
                    .Where(gb => gb.BetSlipId == betSlip.Id)
                    .ProjectTo<UserCurrentBetModel>()
                    .Union(this.db.PlayerGameBets
                        .Where(pgb => pgb.BetSlipId == betSlip.Id)
                        .ProjectTo<UserCurrentBetModel>());


                currentBetSlips.Add(currentBetSlip);
            }

            var tournamentBetSlips = this.db.TournamentBetSlips.Where(tbs => tbs.UserId == userId);

            var tournamentIds = tournamentBetSlips
                .SelectMany(t => t.Bets.Select(tb => tb.BetCoefficient.TournamentId))
                .Distinct();

            var tournamentBetCoefficients = this.db.TournamentBetCoefficients
                .Where(tbc => tournamentIds
                    .Contains(tbc.TournamentId));

            var subjectNames = new Dictionary<int, string>();
            foreach (var tournamentBetCoefficient in tournamentBetCoefficients)
            {
                subjectNames.Add(tournamentBetCoefficient.Id, this.GetTournamentBetSubjectName(tournamentBetCoefficient));
            }

            foreach (var betSlip in tournamentBetSlips)
            {
                var currentBetSlip = new UserCurrentBetSlipModel
                {
                    Amount = betSlip.Amount
                };

                currentBetSlip.Bets = this.db.TournamentBets
                    .Where(bet => bet.BetSlipId == betSlip.Id)
                    .Select(bet => new UserCurrentBetModel
                    {
                        Coefficient = bet.Coefficient,
                        EventName = bet.BetCoefficient.Tournament.Name,
                        EventDate = bet.BetCoefficient.Tournament.EndDate,
                        BetCondition = TournamentBetDescriber.Describe(
                            bet.BetCoefficient.BetType, 
                            subjectNames[bet.BetCoefficient.Id])                        
                    });

                currentBetSlips.Add(currentBetSlip);
            }

            return currentBetSlips;
        }

        public IEnumerable<UserGameCoefficientModel> GameCoefficients(int gameId)
        {
            if (!this.GameExists(gameId))
            {
                return null;
            }

            return this.db.GameBetCoefficients
                .Where(gc => gc.GameId == gameId)
                .ProjectTo<UserGameCoefficientModel>();
        }

        public bool PlaceBets(IEnumerable<BetUnconfirmedModel> bets, double amount, string username)
        {
            var user = this.db.Users.First(u => u.UserName == username);

            if (user.Balance < (decimal) amount)
            {
                return false;
            }

            user.Balance -= (decimal) amount;

            if (bets.First().BetType == BetType.Tournament)
            {
                var betSlip = new TournamentBetSlip
                {
                    Amount = amount,
                    UserId = user.Id
                };

                var betsToAdd = bets.Select(b => new TournamentBet
                {
                    BetSlip = betSlip,
                    BetCoefficientId = b.CoefficientId,
                    Coefficient = b.Coefficient,
                    IsEvaluated = false,
                    IsSuccess = false
                });

                this.db.TournamentBetSlips.Add(betSlip);
                this.db.TournamentBets.AddRange(betsToAdd);
            }
            else
            {
                var betSlip = new GameBetSlip
                {
                    Amount = amount,
                    UserId = user.Id
                };

                var gameBetsToAdd = bets
                    .Where(b => b.BetType == BetType.Game)
                    .Select(b => new GameBet
                {
                        BetSlip = betSlip,
                        GameBetCoefficientId = b.CoefficientId,
                        Coefficient = b.Coefficient,
                        IsEvaluated = false,
                        IsSuccess = false
                });
                
                var playerGameBetsToAdd = bets
                    .Where(b => b.BetType == BetType.PlayerGame)
                    .Select(b => new PlayerGameBet
                    {
                        BetSlip = betSlip,
                        PlayerGameBetCoefficientId = b.CoefficientId,
                        Coefficient = b.Coefficient,
                        IsEvaluated = false,
                        IsSuccess = false
                    });

                this.db.GameBetSlips.Add(betSlip);

                this.db.GameBets.AddRange(gameBetsToAdd);
                this.db.PlayerGameBets.AddRange(playerGameBetsToAdd);
            }

            this.db.SaveChanges();

            return true;
        }

        public IEnumerable<UserGamePlayerCoefficientModel> PlayerCoefficients(int gameId)
        {
            if (!this.GameExists(gameId))
            {
                return null;
            }

            return this.db.GameBetCoefficients
                .Where(gc => gc.GameId == gameId)
                .ProjectTo<UserGamePlayerCoefficientModel>();
        }

        public bool? SlipHasConflictingBets(IDictionary<int, BetType> coefficientsByIdAndType, int coefficientId, BetType betType)
        {
            if (betType == BetType.Game)
            {
                var gameCoefficientIds = coefficientsByIdAndType
                .Keys
                .Where(key => coefficientsByIdAndType[key] == BetType.Game);

                var gameBetCoefficients = this.db.GameBetCoefficients
                    .Where(gbc => gameCoefficientIds.Contains(gbc.Id))
                    .Select(gbc => new
                    {
                        gbc.BetType,
                        gbc.GameId
                    });

                var coefficientToAdd = this.db.GameBetCoefficients.Find(coefficientId);

                if (coefficientToAdd == null)
                {
                    return null;
                }

                return gameBetCoefficients.Any(gbc => gbc.BetType == coefficientToAdd.BetType
                                && gbc.GameId == coefficientToAdd.GameId);
            }
            else if (betType == BetType.Tournament)
            {
                var tournamentCoefficientIds = coefficientsByIdAndType
                    .Keys
                    .Where(key => coefficientsByIdAndType[key] == BetType.Tournament);

                var tournamentBetCoefficients = this.db.TournamentBetCoefficients
                    .Where(tbc => tournamentCoefficientIds.Contains(tbc.Id))
                    .Select(tbc => new
                    {
                        tbc.BetType,
                        tbc.TournamentId
                    });

                var coefficientToAdd = this.db.TournamentBetCoefficients.Find(coefficientId);

                if (coefficientToAdd == null)
                {
                    return null;
                }

                return tournamentBetCoefficients.Any(tbc => tbc.BetType == coefficientToAdd.BetType
                                && tbc.TournamentId == coefficientToAdd.TournamentId);
            }

            return false;
        }

        public async Task<IEnumerable<UserHistoryBetSlipModel>> UserHistory(string username, int page = 1, int pageSize = 5)
        {
            var userId = (await this.userManager.FindByNameAsync(username)).Id;

            var userBetSlipHistories = this.db.BetSlipHistories
                .Where(bsh => bsh.UserId == userId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<UserHistoryBetSlipModel>();

            return userBetSlipHistories;
        }

        public async Task<int> UserHistoryCount(string username)
        {
            var userId = (await this.userManager.FindByNameAsync(username)).Id;

            return this.db.BetSlipHistories
                .Where(bsh => bsh.UserId == userId)
                .Count(); 
        }
    }
}
