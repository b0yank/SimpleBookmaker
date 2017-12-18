namespace SimpleBookmaker.Services
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Core.Enums;
    using Data.Models.Coefficients;
    using Services.Models.Game;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Service
    {
        protected readonly SimpleBookmakerDbContext db;

        public Service(SimpleBookmakerDbContext db)
        {
            this.db = db;
        }

        protected virtual bool TeamExists(int teamId)
            => this.db.Teams.Find(teamId) != null;

        protected virtual bool TournamentExists(int tournamentId)
            => this.db.Tournaments.Find(tournamentId) != null;

        protected virtual bool GameExists(int gameId)
            => this.db.Games.Find(gameId) != null;

        protected virtual bool PlayerExists(int playerId)
            => this.db.Players.Find(playerId) != null;

        protected virtual GameTeamsModel GetTeams(int gameId)
            => this.db.Games
                .Where(g => g.Id == gameId)
                .ProjectTo<GameTeamsModel>()
                .FirstOrDefault();

        protected IEnumerable<TournamentBetCoefficient> GetTeamTournamentCoefficients(int teamId)
            => this.db.TournamentBetCoefficients
                .Where(tbc => tbc.BetType == TournamentBetType.Champion
                    && tbc.BetSubjectId == teamId);

        protected string GetTournamentBetSubjectName(TournamentBetCoefficient tbc)
        {
            if (tbc.BetType == TournamentBetType.Champion)
            {
                return this.db.Teams.First(t => t.Id == tbc.BetSubjectId).Name;
            }

            return this.db.Players.First(p => p.Id == tbc.BetSubjectId).Name;
        }

        protected virtual bool RemoveTournament(int tournamentId)
        {
            var tournament = this.db.Tournaments.Find(tournamentId);

            if (tournament == null)
            {
                return false;
            }

            bool hasGameBets = this.db.Games
                .Any(g => 
                    g.TournamentId == tournamentId
                    && ((g.GameCoefficients
                        .Any(gbc => gbc.GameBets
                            .Any(gb => !gb.IsEvaluated)))
                        || (g.PlayerCoefficients.Any(pc => pc.Bets.Any(pcb => !pcb.IsEvaluated)))));

            if (hasGameBets)
            {
                return false;
            }

            if (this.db.TournamentBets
                .Any(tb => tb.BetCoefficient.TournamentId == tournamentId && !tb.IsEvaluated))
            {
                return false;
            }

            var games = this.db.Games.Where(g => g.TournamentId == tournamentId);
            this.db.Games.RemoveRange(games);

            var tournamentCoefficients = this.db.TournamentBetCoefficients
                .Where(tbc => tbc.TournamentId == tournamentId);

            this.db.TournamentBetCoefficients.RemoveRange(tournamentCoefficients);

            var tournamentTeams = this.db.TournamentsTeams.Where(tt => tt.TournamentId == tournamentId);
            this.db.TournamentsTeams.RemoveRange(tournamentTeams);

            var tournamentPlayers = this.db.TournamentPlayers.Where(tp => tp.TournamentId == tournamentId);
            this.db.TournamentPlayers.RemoveRange(tournamentPlayers);

            this.db.Tournaments.Remove(tournament);

            this.db.SaveChanges();

            return true;
        }
    }
}
