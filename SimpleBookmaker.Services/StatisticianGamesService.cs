namespace SimpleBookmaker.Services
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data.Core.Enums;
    using Services.BetResolvers.Contracts;
    using Services.Infrastructure.BetDescribers;
    using Services.Models.Game;
    using Services.Infrastructure.EventStats;
    using SimpleBookmaker.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using SimpleBookmaker.Data.Models.Bets;

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

            this.RemoveResolvedBetSlips();
            this.RemoveResolvedGames();
        }

        private void ResolveGameBets(GameStats gameStats)
        {
            var gameBets = this.db.GameBets
                                .Include(gb => gb.BetCoefficient)
                                .Where(gb => gb.BetCoefficient.GameId == gameStats.GameId);

            foreach (var gameBet in gameBets)
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

            // move resolved bet slips to history and pay what is necessary to user
            foreach (var gameBet in gameBets)
            {
                var betSlip = this.db.GameBetSlips.Include(bs => bs.GameBets).First(bs => bs.Id == gameBet.BetSlipId);

                if (betSlip.GameBets.All(b => b.IsEvaluated))
                {
                    if (betSlip.GameBets.All(b => b.IsSuccess))
                    {
                        this.PayUser(betSlip, betSlip.GameBets);
                    }

                    var betsWithDescription = new Dictionary<Bet, string>();
                    foreach (var bet in betSlip.GameBets)
                    {
                        var additionalInfo = bet.BetCoefficient.BetType == GameBetType.Result ?
                            new ResultDescriber() { HomeScore = bet.BetCoefficient.HomeScorePrediction, AwayScore = bet.BetCoefficient.AwayScorePrediction } :
                            (object) GetTeams(bet.BetCoefficient.GameId);

                        var betDescription = GameBetDescriber
                            .Describe(bet.BetCoefficient.BetType, bet.BetCoefficient.Side, additionalInfo);

                        betsWithDescription.Add(bet, betDescription);
                    }

                    this.MoveBetsToHistory(betSlip, betsWithDescription);
                }
            }
        }

        private void RemoveResolvedBetSlips()
        {
            var betSlips = this.db.GameBetSlips
                .Include(gbs => gbs.GameBets)
                .Include(gbs => gbs.PlayerBets)
                .Where(bs => bs.GameBets.All(gb => gb.IsEvaluated)
                    && bs.PlayerBets.All(pb => pb.IsEvaluated));

            var gameBets = betSlips.SelectMany(bs => bs.GameBets);
            var playerBets = betSlips.SelectMany(bs => bs.PlayerBets);

            var gameCoefficientIds = gameBets.Select(gb => gb.GameBetCoefficientId).Distinct();
            var playerCoefficientIds = playerBets.Select(pb => pb.PlayerGameBetCoefficientId).Distinct();

            this.db.GameBets.RemoveRange(gameBets);
            this.db.PlayerGameBets.RemoveRange(playerBets);

            this.db.SaveChanges();

            var gameCoefficientsToRemove = this.db.GameBetCoefficients
                .Where(gbc => gameCoefficientIds.Contains(gbc.Id)
                    && gbc.GameBets.Count == 0);

            var playerCoefficientsToRemove = this.db.PlayerGameBetCoefficients
                .Where(pbc => playerCoefficientIds.Contains(pbc.Id)
                    && pbc.Bets.Count == 0);

            this.db.GameBetCoefficients.RemoveRange(gameCoefficientsToRemove);
            this.db.PlayerGameBetCoefficients.RemoveRange(playerCoefficientsToRemove);

            this.db.GameBetSlips.RemoveRange(betSlips);

            this.db.SaveChanges();
        }

        private void RemoveResolvedGames()
        {
            var resolvedGames = this.db.Games
                .Where(g => g.GameCoefficients.Count == 0
                    && g.PlayerCoefficients.Count == 0
                    && g.Time <= DateTime.UtcNow);

            this.db.Games.RemoveRange(resolvedGames);
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

            foreach (var playerGameBet in bets)
            {
                var betSlip = this.db.GameBetSlips
                    .Include(bs => bs.PlayerBets)
                    .First(bs => bs.Id == playerGameBet.BetSlipId);

                if (betSlip.PlayerBets.All(b => b.IsEvaluated))
                {
                    if (betSlip.PlayerBets.All(b => b.IsSuccess))
                    {
                        this.PayUser(betSlip, betSlip.PlayerBets);
                    }

                    var betsWithDescription = new Dictionary<Bet, string>();
                    foreach (var bet in betSlip.PlayerBets)
                    {
                        var betDescription = GamePlayerBetDescriber
                            .Describe(bet.BetCoefficient.BetType, bet.BetCoefficient.Player.Name);

                        betsWithDescription.Add(bet, betDescription);
                    }

                    this.MoveBetsToHistory(betSlip, betsWithDescription);
                }
            }
        }
    }
}
