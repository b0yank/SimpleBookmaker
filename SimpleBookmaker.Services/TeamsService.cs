namespace SimpleBookmaker.Services
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
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

        public bool Add(string name)
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

        public bool Exists(int id)
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
