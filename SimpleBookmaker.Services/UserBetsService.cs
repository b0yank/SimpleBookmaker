namespace SimpleBookmaker.Services
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Data.Core.Enums;
    using Data.Models.Bets;
    using Data.Models.BetSlips;
    using SimpleBookmaker.Services.Models.UserCoefficient;
    using System.Collections.Generic;
    using System.Linq;
    using SimpleBookmaker.Services.Models.Bet;
    using Microsoft.EntityFrameworkCore;
    using SimpleBookmaker.Data.Models.Coefficients;

    public class UserBetsService : Service, IUserBetsService
    {
        public UserBetsService(SimpleBookmakerDbContext db)
            : base(db)
        {
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
    }
}
