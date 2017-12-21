namespace SimpleBookmaker.Services
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Data.Core.Enums;
    using Data.Models;
    using System.Collections.Generic;
    using SimpleBookmaker.Services.Models.Team;
    using System.Linq;

    public class TeamsService : Service, ITeamsService
    {
        public TeamsService(SimpleBookmakerDbContext db)
            : base(db)
        {
        }

        public virtual bool Add(string name)
        {
            var team = new Team
            {
                Name = name
            };

            if (this.db.Teams.Any(t => t.Name == name))
            {
                return false;
            }

            this.db.Add(team);
            this.db.SaveChanges();

            return true;
        }

        public virtual bool Remove(int teamId)
        {
            var team = this.db.Teams.Find(teamId);
            var players = this.db.Players.Where(p => p.TeamId == teamId);

            if (team == null)
            {
                return false;
            }

            var hasTournamentBets = this.db.TournamentBets
                    .Any(tb => tb.BetCoefficient.BetType == TournamentBetType.Champion
                        && tb.BetCoefficient.BetSubjectId == teamId
                        && !tb.IsEvaluated);

            if (hasTournamentBets)
            {
                return false;
            }

            var hasGameBets = this.db.GameBets.Any(gb =>
                    !gb.IsEvaluated
                    && ((gb.BetCoefficient.Side == BetSide.Home && gb.BetCoefficient.Game.HomeTeamId == teamId)
                    || (gb.BetCoefficient.Side == BetSide.Away && gb.BetCoefficient.Game.AwayTeamId == teamId)));

            var hasPlayerGameBets = this.db.PlayerGameBets.Any(pgb =>
                    !pgb.IsEvaluated
                    && players
                        .Select(p => p.Id)
                        .Contains(pgb.BetCoefficient.PlayerId));

            if (hasGameBets || hasPlayerGameBets)
            {
                return false;
            }

            foreach (var player in players)
            {
                player.TeamId = null;
            }

            var games = this.db.Games.Where(g => g.HomeTeamId == teamId || g.AwayTeamId == teamId);

            var gameIds = games.Select(g => g.Id);

            var gameCoefficients = this.db.GameBetCoefficients
                .Where(gc => gameIds.Contains(gc.Id)
                    && ((gc.Side == BetSide.Home && gc.Game.HomeTeam.Team.Id == teamId)
                        || (gc.Side == BetSide.Away && gc.Game.AwayTeam.Team.Id == teamId)));

            var playerCoefficients = this.db.PlayerGameBetCoefficients
                .Where(pgc => gameIds.Contains(pgc.GameId)
                    && pgc.Player.TeamId == teamId);

            this.db.GameBetCoefficients.RemoveRange(gameCoefficients);
            this.db.PlayerGameBetCoefficients.RemoveRange(playerCoefficients);
            this.db.Games.RemoveRange(games);

            var tournamentTeams = this.db.TournamentsTeams.Where(tt => tt.TeamId == teamId);
            this.db.TournamentsTeams.RemoveRange(tournamentTeams);

            this.db.Teams.Remove(team);

            this.db.SaveChanges();

            return true;
        }

        public bool AddToTournament(int teamId, int tournamentId)
        {
            var team = this.db.Teams.Find(teamId);
            var tournament = this.db.TournamentsTeams.Find(tournamentId);

            if (!this.TeamExists(teamId)
                || !this.TournamentExists(tournamentId))
            {
                return false;
            }

            var tournamentTeam = new TournamentTeam
            {
                TeamId = teamId,
                TournamentId = tournamentId
            };

            this.db.TournamentsTeams.Add(tournamentTeam);
            this.db.SaveChanges();

            return true;
        }

        public virtual bool Exists(int id)
            => this.TeamExists(id);

        public string GetName(int id)
        {
            var team = this.db.Teams.Find(id);

            if (team == null)
            {
                return null;
            }

            return team.Name;
        }

        public IEnumerable<TeamListModel> All(int page = 1, int pageSize = 20, string keyword = null)
        {
            var allTeams = this.db.Teams.AsQueryable();

            if (keyword != null)
            {
                var keywordLower = keyword.ToLower();

                allTeams = allTeams.Where(t => t.Name.ToLower().Contains(keywordLower)
                                            || t.Tournaments.Any(to => 
                                                        to.Tournament.Name.ToLower().Contains(keywordLower)));
            }

            return allTeams
                        .OrderBy(t => t.Name)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ProjectTo<TeamListModel>();
        }

        public IEnumerable<BaseTeamModel> ByTournament(int tournamentId)
            => this.db.Teams
                .Where(t => t.Tournaments.Any(to => to.TournamentId == tournamentId))
                .ProjectTo<BaseTeamModel>();

        public int Count(string keyword = null)
        {
            var teams = this.db.Teams.AsQueryable();

            if (keyword != null)
            {
                var keywordLower = keyword.ToLower();

                teams = teams.Where(t => t.Name.ToLower().Contains(keywordLower)
                                            || t.Tournaments.Any(to =>
                                                        to.Tournament.Name.ToLower().Contains(keywordLower)));
            }

            return teams.Count();
        }
    }
}
