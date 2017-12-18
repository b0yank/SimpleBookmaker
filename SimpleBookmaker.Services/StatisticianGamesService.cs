namespace SimpleBookmaker.Services
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data.Core.Enums;
    using Data.Models.BetSlips;
    using Services.BetResolvers.Contracts;
    using Services.Infrastructure.BetDescribers;
    using Services.Models.Game;
    using Services.Infrastructure.EventStats;
    using SimpleBookmaker.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public class StatisticianGamesService : StatisticiansService, IStatisticianGamesService
    {
        private readonly IGameBetResolverFactory gameBetResolverFactory;
        private readonly IPlayerGameBetResolverFactory playerGameBetResolverFactory;

        public StatisticianGamesService(SimpleBookmakerDbContext db, 
            IGameBetResolverFactory gameBetResolverFactory,
            IPlayerGameBetResolverFactory playerGameBetResolverFactory)
            : base(db)
        {
            this.gameBetResolverFactory = gameBetResolverFactory;
            this.playerGameBetResolverFactory = playerGameBetResolverFactory;
        }

        public bool Exists(int gameId)
            => this.GameExists(gameId);

        public IEnumerable<GameStatsListModel> Finished()
            => this.db.Games
                .Where(g => g.Time <= DateTime.UtcNow)
                .ProjectTo<GameStatsListModel>()
                .ToList();

        public GameStatsModel ById(int gameId)
        {
            if (!GameExists(gameId))
            {
                return null;
            }

            return this.db.Games
                    .Where(g => g.Id == gameId)
                    .ProjectTo<GameStatsModel>()
                    .First();
        }

        public void ResolveBets(GameStats gameStats)
        {
            this.ResolveGameBets(gameStats);
            this.ResolvePlayerGameBets(gameStats);

            var resolvedBetSlips =  this.db.GameBetSlips
                .Where(bs => (bs.GameBets.All(gb => gb.IsEvaluated) || bs.GameBets.Count() == 0)
                    && (bs.PlayerBets.All(pb => pb.IsEvaluated) || bs.PlayerBets.Count() == 0))
                .ToList();

            this.AddBetSlipsToHistory(resolvedBetSlips);
            
            this.PayUsers();

            this.AddToTournamentStats(gameStats);

            this.db.GameBetSlips.RemoveRange(resolvedBetSlips);
            this.db.SaveChanges();

            this.RemoveResolvedGames();
        }

        private void ResolveGameBets(GameStats gameStats)
        {
            var bets = this.db.GameBets
                                .Include(gb => gb.BetCoefficient)
                                .Where(gb => gb.BetCoefficient.GameId == gameStats.GameId);

            foreach (var gameBet in bets)
            {
                var betResolver = this.gameBetResolverFactory.GetResolver(gameBet.BetCoefficient.BetType);

                var additional = gameBet.BetCoefficient.BetType == GameBetType.Result
                    ? new ResultDescriber() { HomeScore = gameBet.BetCoefficient.HomeScorePrediction,
                                            AwayScore = gameBet.BetCoefficient.AwayScorePrediction }
                    : (object) GetTeams(gameBet.BetCoefficient.GameId);

                gameBet.IsSuccess = betResolver.Resolve(gameStats, gameBet.BetCoefficient.Side, additional);
                gameBet.IsEvaluated = true;
            }

            this.db.SaveChanges();
        }

        private void PayUsers()
        {
            var betSlips = this.db.GameBetSlips
                .Where(gbs => (gbs.GameBets.Count() == 0 || gbs.GameBets.All(gb => gb.IsSuccess))
                    && (gbs.PlayerBets.Count() == 0 || gbs.PlayerBets.All(pb => pb.IsSuccess)))
                .Select(bs => new
                {
                    Amount = bs.Amount,
                    UserId = bs.UserId,
                    Coefficients = bs.GameBets.Select(gb => gb.Coefficient).Union(bs.PlayerBets.Select(pb => pb.Coefficient))
                });

            foreach (var betSlip in betSlips)
            {
                this.PayUser(betSlip.Amount, betSlip.UserId, betSlip.Coefficients);
            }

            this.db.SaveChanges();
        }

        private void RemoveResolvedGames()
        {
            var resolvedGames = this.db.Games
                .Include(g => g.GameCoefficients)
                .Include(g => g.PlayerCoefficients)
                .Where(g => (g.GameCoefficients.Count == 0 || g.GameCoefficients.All(gc => gc.GameBets.All(gb => gb.IsEvaluated)))
                    && (g.PlayerCoefficients.Count == 0 || g.PlayerCoefficients.All(pc => pc.Bets.All(gb => gb.IsEvaluated)))
                   && g.Time <= DateTime.UtcNow);

            this.db.GameBetCoefficients.RemoveRange(resolvedGames.SelectMany(g => g.GameCoefficients));
            this.db.PlayerGameBetCoefficients.RemoveRange(resolvedGames.SelectMany(g => g.PlayerCoefficients));

            this.db.Games.RemoveRange(resolvedGames);

            this.db.SaveChanges();
        }

        private void ResolvePlayerGameBets(GameStats gameStats)
        {
            var bets = this.db.PlayerGameBets
                                .Include(pgb => pgb.BetCoefficient)
                                .Where(pgb => pgb.BetCoefficient.GameId == gameStats.GameId);

            foreach (var bet in bets)
            {
                var betResolver = this.playerGameBetResolverFactory.GetResolver(bet.BetCoefficient.BetType);

                bet.IsSuccess = betResolver.Resolve(gameStats, bet.BetCoefficient.PlayerId);
                bet.IsEvaluated = true;
            }

            this.db.SaveChanges();
        }

        private void AddBetSlipsToHistory(IEnumerable<GameBetSlip> betSlips)
        {
            foreach (var betSlip in betSlips)
            {
                var gameBets = this.db.GameBets
                        .Where(gb => gb.BetSlipId == betSlip.Id)
                        .Select(gb => new
                        {
                            HomeScore = gb.BetCoefficient.HomeScorePrediction,
                            AwayScore = gb.BetCoefficient.AwayScorePrediction,
                            GameId = gb.BetCoefficient.GameId,
                            Coefficient = gb.Coefficient,
                            BetType = gb.BetCoefficient.BetType,
                            Side = gb.BetCoefficient.Side,
                            IsSuccess = gb.IsSuccess,
                            EventName = $"{gb.BetCoefficient.Game.HomeTeam.Team.Name} vs {gb.BetCoefficient.Game.AwayTeam.Team.Name}",
                            GameDate = gb.BetCoefficient.Game.Time
                        });

                var playerBets = this.db.PlayerGameBets.Where(pgb => pgb.BetSlipId == betSlip.Id)
                        .Select(pb => new
                        {
                            BetType = pb.BetCoefficient.BetType,
                            Coefficient = pb.Coefficient,
                            PlayerName = pb.BetCoefficient.Player.Name,
                            IsSuccess = pb.IsSuccess,
                            EventName = $"{pb.BetCoefficient.Game.HomeTeam.Team.Name} vs {pb.BetCoefficient.Game.AwayTeam.Team.Name}",
                            GameDate = pb.BetCoefficient.Game.Time
                        });

                var bets = new List<BetHistoryModel>();
                foreach (var bet in gameBets)
                {
                    var additionalInfo = bet.BetType == GameBetType.Result ?
                        new ResultDescriber() { HomeScore = bet.HomeScore, AwayScore = bet.AwayScore } :
                        (object)GetTeams(bet.GameId);

                    var betDescription = GameBetDescriber
                        .Describe(bet.BetType, bet.Side, additionalInfo);

                    var gameBetHistory = new BetHistoryModel
                    {
                        Coefficient = bet.Coefficient,
                        BetCondition = betDescription,
                        EventName = bet.EventName,
                        Date = bet.GameDate
                    };

                    bets.Add(gameBetHistory);
                }

                foreach (var bet in playerBets)
                {
                    var betDescription = GamePlayerBetDescriber
                        .Describe(bet.BetType, bet.PlayerName);

                    var playerBetHistory = new BetHistoryModel
                    {
                        Coefficient = bet.Coefficient,
                        BetCondition = betDescription,
                        EventName = bet.EventName,
                        Date = bet.GameDate
                    };

                    bets.Add(playerBetHistory);
                }

                AddBetsToHistory(betSlip.Amount, 
                    betSlip.UserId, 
                    gameBets.All(gb => gb.IsSuccess) && playerBets.All(pb => pb.IsSuccess), 
                    bets);
            }
        }

        private void AddToTournamentStats(GameStats gameStats)
        {
            var tournamentId = this.db.Games
                .Where(g => g.Id == gameStats.GameId)
                .Select(g => g.TournamentId)
                .First();

            var tournamentPlayerGoalscorers = this.db.TournamentPlayers.Where(tp => tp.TournamentId == tournamentId
                                                                && (gameStats.HomeGoalscorers.Contains(tp.PlayerId)
                                                                    || gameStats.AwayGoalscorers.Contains(tp.PlayerId)));

            foreach (var tournamentPlayer in tournamentPlayerGoalscorers)
            {
                tournamentPlayer.GoalsScored += 
                    (gameStats.HomeGoalscorers.Where(g => g == tournamentPlayer.PlayerId).Count() 
                    + gameStats.AwayGoalscorers.Where(g => g == tournamentPlayer.PlayerId).Count());
            }

            this.db.SaveChanges();
        }
    }
}
