namespace SimpleBookmaker.Services
{
    using System;
    using Contracts;
    using SimpleBookmaker.Data.Models;
    using SimpleBookmaker.Data;
    using System.Linq;
    using SimpleBookmaker.Services.Models.Tournament;
    using System.Collections.Generic;
    using AutoMapper.QueryableExtensions;
    using SimpleBookmaker.Services.Models.Team;
    using SimpleBookmaker.Services.Models.Game;

    public class TournamentsService : Service, ITournamentsService
    {
        public TournamentsService(SimpleBookmakerDbContext db)
            : base(db)
        {
        }

        public IEnumerable<TournamentListModel> All()
            => this.db.Tournaments
                .ProjectTo<TournamentListModel>();

        public bool Add(string name, DateTime startDate, DateTime endDate)
        {
            if (this.db.Tournaments.Any(t => t.Name == name))
            {
                return false;
            }

            var tournament = new Tournament
            {
                Name = name,
                StartDate = startDate,
                EndDate = endDate
            };

            this.db.Tournaments.Add(tournament);
            this.db.SaveChanges();

            return true;
        }

        public bool Edit(int tournamentId, string name, DateTime startDate, DateTime endDate)
        {
            var tournament = this.db.Tournaments.Find(tournamentId);

            if (tournament == null)
            {
                return false;
            }

            tournament.Name = name;
            tournament.StartDate = startDate;
            tournament.EndDate = endDate;

            this.db.SaveChanges();

            return true;
        }

        public bool Remove(int tournamentId)
        {
            var tournament = this.db.Tournaments.Find(tournamentId);

            if (tournament == null)
            {
                return false;
            }

            return RemoveTournament(tournamentId);
        }

        public bool Exists(int tournamentId)
            => this.TournamentExists(tournamentId);
       
        public TournamentModel ById(int tournamentId)
            => this.db.Tournaments
                .Where(t => t.Id == tournamentId)
                .ProjectTo<TournamentModel>()
                .First();

        public string GetName(int tournamentId)
        {
            var tournament = this.db.Tournaments.Find(tournamentId);

            if (tournament == null)
            {
                return null;
            }

            return tournament.Name;
        }

        public bool IsDuringTournament(int tournamentId, DateTime date)
        {
            var tournament = this.db.Tournaments.Find(tournamentId);

            return date >= tournament.StartDate && date <= tournament.EndDate;
        }

        public IEnumerable<TournamentDetailedListModel> AllDetailed()
            => this.db.Tournaments.ProjectTo<TournamentDetailedListModel>();

        public IEnumerable<BaseTeamModel> GetTeams(int tournamentId)
        {
            if (!this.TournamentExists(tournamentId))
            {
                return null;
            }

            return this.db.Teams.
                Where(t => this.db.TournamentsTeams
                                    .Any(tt => tt.TournamentId == tournamentId
                                            && tt.TeamId == t.Id))
                                    .ProjectTo<BaseTeamModel>();
        }

        public IEnumerable<BaseTeamModel> GetAvailableTeams(int tournamentId)
        {
            var availableTeamIds = this.db.Teams
                .Where(t => !this.db.TournamentsTeams.Any(tt => tt.TeamId == t.Id && tt.TournamentId == tournamentId))
                .Select(t => t.Id);

            return this.db.Teams
                .Where(t => availableTeamIds.Contains(t.Id))
                .ProjectTo<BaseTeamModel>();
        }

        public bool AddTeam(int tournamentId, int teamId)
            => this.AddTeams(tournamentId, new List<int>() { teamId });

        public bool AddTeams(int tournamentId, IEnumerable<int> teamIds)
        {
            if (!this.TournamentExists(tournamentId))
            {
                return false;
            }

            foreach (var teamId in teamIds)
            {
                var team = this.db.Teams.Find(teamId);

                if (team == null)
                {
                    return false;
                }

                if (this.IsInTournament(tournamentId, teamId))
                {
                    return false;
                }

                var tournamentTeam = new TournamentTeam
                {
                    TeamId = teamId,
                    TournamentId = tournamentId
                };

                foreach (var player in team.Players)
                {
                    var tournamentPlayer = new TournamentPlayer
                    {
                        PlayerId = player.Id,
                        TournamentId = tournamentId
                    };

                    this.db.TournamentPlayers.Add(tournamentPlayer);
                }

                this.db.TournamentsTeams.Add(tournamentTeam);
            }

            this.db.SaveChanges();

            return true;
        }

        public bool RemoveTeam(int tournamentId, int teamId)
        {
            if (!this.TournamentExists(tournamentId)
                || !this.TeamExists(teamId))
            {
                return false;
            }

            var tournamentTeam = this.db.TournamentsTeams
                .FirstOrDefault(tt => tt.TeamId == teamId && tt.TournamentId == tournamentId);

            if (tournamentTeam != null)
            {
                this.db.TournamentsTeams.Remove(tournamentTeam);

                this.db.SaveChanges();
            }

            return true;
        }

        public bool IsInTournament(int tournamentId, int teamId)
            => this.db.TournamentsTeams.Any(tt => 
                    tt.TeamId == teamId & tt.TournamentId == tournamentId);

        public IEnumerable<TournamentListModel> WithTeam(int teamId)
            => this.db.Tournaments
                    .Where(t => t.Teams.Any(team => team.TeamId == teamId))
                    .ProjectTo<TournamentListModel>();

        public IEnumerable<TournamentListModel> WithoutTeam(int teamId)
            => this.db.Tournaments
                    .Where(t => !t.Teams.Any(team => team.TeamId == teamId))
                    .ProjectTo<TournamentListModel>();

        public IEnumerable<GameListModel> Games(int tournamentId, bool upcoming = true, int page = 1, int pageSize = 20)
        {
            if (!this.TournamentExists(tournamentId))
            {
                return new List<GameListModel>();
            }

            var games = this.db.Games.AsQueryable();

            if (upcoming)
            {
                games = games.Where(g => g.Time > DateTime.UtcNow);
            }

            return games
                .Where(g => g.TournamentId == tournamentId)
                .OrderBy(g => g.Time)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<GameListModel>();
        }

        public int GamesCount(int tournamentId)
        {
            if (!this.TournamentExists(tournamentId))
            {
                return 0;
            }

            return this.db.Tournaments
                .Find(tournamentId)
                .Games
                .Count();
        }
    }
}
