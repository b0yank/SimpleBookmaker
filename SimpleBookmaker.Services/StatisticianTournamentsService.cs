namespace SimpleBookmaker.Services
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Data.Models.Bets;
    using Infrastructure.BetDescribers;
    using Infrastructure.EventStats;
    using Services.BetResolvers.Contracts;
    using Services.Models.Tournament;
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using Microsoft.EntityFrameworkCore;

    public class StatisticianTournamentsService : StatisticiansService, IStatisticianTournamentsService
    {
        private readonly ITournamentBetResolverFactory tournamentBetResolverFactory;

        public StatisticianTournamentsService(SimpleBookmakerDbContext db,
            ITournamentBetResolverFactory tournamentBetResolverFactory)
            : base(db)
        {
            this.tournamentBetResolverFactory = tournamentBetResolverFactory;
        }

        public TournamentStatsSetModel ById(int tournamentId)
        {
            var tournament = this.db.Tournaments.Where(t => t.Id == tournamentId);

            if (tournament == null)
            {
                return null;
            }

            return tournament.ProjectTo<TournamentStatsSetModel>().First();
        }

        public bool Exists(int tournamentId)
            => this.TournamentExists(tournamentId);

        public IEnumerable<TournamentStatsListModel> Finished()
            => this.db.Tournaments
                    .Where(t => t.EndDate <= DateTime.UtcNow)
                    .ProjectTo<TournamentStatsListModel>();

        public bool ResolveBets(int tournamentId, int championId)
        {
            this.ResolveTournamentBets(tournamentId, championId);
            return this.RemoveTournament(tournamentId);
        }

        private void ResolveTournamentBets(int tournamentId, int championId)
        {
            var topScorerIds = this.FindTopScorers(tournamentId);

            var tournamentStats = new TournamentStats
            {
                TournamentId = tournamentId,
                ChampionId = championId,
                TopScorers = topScorerIds
            };

            var bets = this.db.TournamentBets
                .Where(tb => tb.BetCoefficient.TournamentId == tournamentStats.TournamentId);

            foreach (var bet in bets)
            {
                var resolver = this.tournamentBetResolverFactory.GetResolver(bet.BetCoefficient.BetType);

                bet.IsSuccess = resolver.Resolve(tournamentStats, bet.BetCoefficient.BetSubjectId);
                bet.IsEvaluated = true;
            }

            this.db.SaveChanges();

            foreach (var bet in bets)
            {
                var betSlip = this.db.TournamentBetSlips.Include(bs => bs.Bets).First(bs => bs.Id == bet.BetSlipId);

                if (betSlip.Bets.All(b => b.IsEvaluated))
                {
                    if (betSlip.Bets.All(b => b.IsSuccess))
                    {
                        this.PayUser(betSlip, betSlip.Bets);
                    }

                    var betsWithDescription = betSlip
                        .Bets
                        .ToDictionary(b => (Bet)b,
                            b => TournamentBetDescriber.Describe(b.BetCoefficient.BetType,
                                this.GetTournamentBetSubjectName(b.BetCoefficient)));

                    this.MoveBetsToHistory(betSlip, betsWithDescription);
                }
            }
        }

        private IEnumerable<int> FindTopScorers(int tournamentId)
        {
            var tournament = this.db.Tournaments.Find(tournamentId);

            var topScorerTally = tournament.Players.Max(p => p.GoalsScored);

            return tournament.Players
                .Where(p => p.GoalsScored == topScorerTally)
                .Select(p => p.Player.Id);
        }
    }
}
