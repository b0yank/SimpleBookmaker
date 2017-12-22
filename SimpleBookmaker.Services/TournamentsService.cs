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

        public int Count(bool upcoming = false)
        {
            var tournaments = this.Upcoming(upcoming);

            return tournaments.Count();
        }

        public IEnumerable<TournamentListModel> AllImportant(int count = 5, bool upcoming = true)
        {
            var tournaments = this.Upcoming(upcoming);

            return tournaments
                    .OrderByDescending(t => t.Importance)
                    .Take(count)
                    .ProjectTo<TournamentListModel>();
        }

        public IEnumerable<TournamentListModel> All(bool upcoming = false)
        {
            var tournaments = this.Upcoming(upcoming);

            return this.db.Tournaments
                .ProjectTo<TournamentListModel>();
        }

        public IEnumerable<TournamentDetailedListModel> AllDetailed(int page = 1, int pageSize = 10, bool upcoming = false)
        {
            var tournaments = this.Upcoming(upcoming);

            return tournaments
                .OrderBy(t => t.EndDate)
                .ThenByDescending(t => t.Importance)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<TournamentDetailedListModel>();
        }

        public virtual bool Add(string name, DateTime startDate, DateTime endDate, int importance)
        {
            if (this.db.Tournaments.Any(t => t.Name == name))
            {
                return false;
            }

            var tournament = new Tournament
            {
                Name = name,
                StartDate = startDate,
                EndDate = endDate,
                Importance = importance
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
            => RemoveTournament(tournamentId);

        public virtual bool Exists(int tournamentId)
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

        public IEnumerable<TournamentDetailedListModel> AllDetailed(bool upcoming = false)
        {
            var tournaments = this.Upcoming(upcoming);

            return tournaments
                .ProjectTo<TournamentDetailedListModel>();
        }

        public IEnumerable<BaseTeamModel> GetTournamentTeams(int tournamentId)
            => this.db.TournamentsTeams
                .Where(tt => tt.TournamentId == tournamentId)
                .Select(tt => tt.Team)
                .ProjectTo<BaseTeamModel>();


        public virtual IEnumerable<BaseTeamModel> GetAvailableTeams(int tournamentId)
        {
            var availableTeamIds = this.db.Teams
                .Where(t => !this.db.TournamentsTeams.Any(tt => tt.TeamId == t.Id && tt.TournamentId == tournamentId))
                .Select(t => t.Id);

            return this.db.Teams
                .Where(t => availableTeamIds.Contains(t.Id))
                .ProjectTo<BaseTeamModel>();
        }

        public virtual bool AddTeam(int tournamentId, int teamId)
            => this.AddTeams(tournamentId, new List<int>() { teamId });

        public virtual bool AddTeams(int tournamentId, IEnumerable<int> teamIds)
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

                foreach (var playerId in this.db.Players.Where(p => p.TeamId == teamId).Select(p => p.Id))
                {
                    if (this.db.TournamentPlayers.Any(tp => tp.PlayerId == playerId && tp.TournamentId == tournamentId))
                    {
                        continue;
                    }

                    var tournamentPlayer = new TournamentPlayer
                    {
                        PlayerId = playerId,
                        TournamentId = tournamentId
                    };

                    this.db.TournamentPlayers.Add(tournamentPlayer);
                }

                this.db.TournamentsTeams.Add(tournamentTeam);
            }

            this.db.SaveChanges();

            return true;
        }

        public virtual bool RemoveTeam(int tournamentId, int teamId)
        {
            if (!this.TournamentExists(tournamentId)
                || !this.TeamExists(teamId))
            {
                return false;
            }

            var tournamentTeam = this.db.TournamentsTeams
                .FirstOrDefault(tt => tt.TeamId == teamId && tt.TournamentId == tournamentId);

            var teamPlayerIds = this.db.Players.Where(p => p.TeamId == teamId).Select(p => p.Id);

            var tournamentPlayers = this.db.TournamentPlayers.Where(tp => tp.TournamentId == tournamentId && teamPlayerIds.Contains(tp.PlayerId));

            this.db.TournamentPlayers.RemoveRange(tournamentPlayers);

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

        public virtual IEnumerable<GameListModel> Games(int tournamentId, bool upcoming = true, int page = 1, int pageSize = 20)
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

        public virtual int GamesCount(int tournamentId)
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

        private IQueryable<Tournament> Upcoming(bool upcoming)
        {
            var tournaments = this.db.Tournaments.AsQueryable();

            if (upcoming)
            {
                tournaments = tournaments.Where(t => t.EndDate > DateTime.UtcNow);
            }

            return tournaments;
        }
    }
}
