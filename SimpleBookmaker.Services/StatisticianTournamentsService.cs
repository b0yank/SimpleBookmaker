namespace SimpleBookmaker.Services
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Data.Models.BetSlips;
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

        public virtual TournamentStatsSetModel ById(int tournamentId)
        {
            var tournament = this.db.Tournaments.Where(t => t.Id == tournamentId);

            if (tournament == null)
            {
                return null;
            }

            return tournament.ProjectTo<TournamentStatsSetModel>().First();
        }

        public virtual bool Exists(int tournamentId)
            => this.TournamentExists(tournamentId);

        public IEnumerable<TournamentStatsListModel> Finished()
            => this.db.Tournaments
                    .Where(t => t.EndDate <= DateTime.UtcNow)
                    .ProjectTo<TournamentStatsListModel>();

        public virtual bool ResolveBets(int tournamentId, int championId)
        {
            this.ResolveTournamentBets(tournamentId, championId);

            var resolvedBetSlips = this.db.TournamentBetSlips
                .Where(tbs => tbs.Bets.All(b => b.IsEvaluated) || tbs.Bets.Count == 0)
                .ToList();

            this.AddBetSlipsToHistory(resolvedBetSlips);

            this.PayUsers();

            this.db.TournamentBetSlips.RemoveRange(resolvedBetSlips);
            this.db.SaveChanges();

            return RemoveTournament(tournamentId);
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
                .Include(b => b.BetCoefficient)
                .Where(tb => tb.BetCoefficient.TournamentId == tournamentStats.TournamentId);

            foreach (var bet in bets)
            {
                var resolver = this.tournamentBetResolverFactory.GetResolver(bet.BetCoefficient.BetType);

                bet.IsSuccess = resolver.Resolve(tournamentStats, bet.BetCoefficient.BetSubjectId);
                bet.IsEvaluated = true;
            }

            this.db.SaveChanges();
        }

        private void AddBetSlipsToHistory(IEnumerable<TournamentBetSlip> betSlips)
        {
            foreach (var betSlip in betSlips)
            {
                var tournamentBets = this.db.TournamentBets
                    .Where(tb => tb.BetSlipId == betSlip.Id)
                    .Select(tb => new
                    {
                        BetCoefficient = tb.BetCoefficient,
                        Coefficient = tb.Coefficient,
                        IsSuccess = tb.IsSuccess,
                        EventName = tb.BetCoefficient.Tournament.Name,
                        TournamentEndDate = tb.BetCoefficient.Tournament.EndDate
                    }).ToList();

                var bets = tournamentBets
                    .Select(tb => new BetHistoryModel
                    {
                        Coefficient = tb.Coefficient,
                        EventName = tb.EventName,
                        Date = tb.TournamentEndDate,
                        BetCondition = TournamentBetDescriber.Describe(tb.BetCoefficient.BetType,
                            this.GetTournamentBetSubjectName(tb.BetCoefficient))
                    });

                AddBetsToHistory(betSlip.Amount,
                    betSlip.UserId,
                    tournamentBets.All(tb => tb.IsSuccess),
                    bets);
            }

            this.db.SaveChanges();
        }

        private void PayUsers()
        {
            var betSlips = this.db.TournamentBetSlips
                .Where(tbs => tbs.Bets.All(tb => tb.IsSuccess))
                .Select(bs => new
                {
                    Amount = bs.Amount,
                    UserId = bs.UserId,
                    Coefficients = bs.Bets.Select(gb => gb.Coefficient)
                });

            foreach (var betSlip in betSlips)
            {
                PayUser(betSlip.Amount, betSlip.UserId, betSlip.Coefficients);
            }

            this.db.SaveChanges();
        }

        private IEnumerable<int> FindTopScorers(int tournamentId)
        {
            var tournamentPlayers = this.db.TournamentPlayers.Where(tp => tp.TournamentId == tournamentId);

            var topScorerTally = tournamentPlayers.Max(p => p.GoalsScored);

            return tournamentPlayers
                .Where(p => p.GoalsScored == topScorerTally)
                .Select(p => p.Player.Id);
        }
    }
}
