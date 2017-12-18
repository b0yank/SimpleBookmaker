namespace SimpleBookmaker.Services
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Data.Models;
    using Models.Game;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GamesService : Service, IGamesService
    {
        public GamesService(SimpleBookmakerDbContext db)
            : base(db)
        {
        }

        public bool Add(int tournamentId, int homeTeamId, int awayTeamId, DateTime time)
        {
            var homeTeam = this.db.TournamentsTeams.FirstOrDefault(tt => tt.TournamentId == tournamentId && tt.TeamId == homeTeamId);
            var awayTeam = this.db.TournamentsTeams.FirstOrDefault(tt => tt.TournamentId == tournamentId && tt.TeamId == awayTeamId);

            if (homeTeam == null || awayTeam == null)
            {
                return false;
            }

            var game = new Game
            {
                TournamentId = tournamentId,
                HomeTeamId = homeTeam.Id,
                AwayTeamId = awayTeam.Id,
                Time = time
            };

            this.db.Games.Add(game);
            this.db.SaveChanges();

            return true;
        }

        public bool Exists(int gameId)
            => this.db.Games.Find(gameId) != null;

        public void Edit(int gameId, DateTime kickoff)
        {
            var game = this.db.Games.Find(gameId);

            if (game == null)
            {
                return;
            }

            game.Time = kickoff;
            this.db.SaveChanges();
        }

        public IEnumerable<GameListModel> Upcoming(int page = 1, int pageSize = 20, int tournamentId = 0)
        {
            var games = this.db.Games.AsQueryable();

            if (this.TournamentExists(tournamentId))
            {
                games = games.Where(g => g.TournamentId == tournamentId);
            }

            return games
                .Where(g => g.Time > DateTime.UtcNow)
                .OrderBy(g => g.Time)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<GameListModel>();
        }
            
        public DateTime? GetGametime(int gameId)
        {
            var game = this.db.Games.Find(gameId);

            if (game == null)
            {
                return null;
            }

            return game.Time;
        }

        public int UpcomingCount(int tournamentId)
        {
            var games = this.db.Games.Where(g => g.Time > DateTime.UtcNow);

            if (this.TournamentExists(tournamentId))
            {
                games = games.Where(g => g.TournamentId == tournamentId);
            }

            return games.Count();
        }

        public GameDetailedModel ById(int id)
        {
            var game = this.db.Games.Where(g => g.Id == id);

            if (game.Count() == 0)
            {
                return null;
            }

            return game.ProjectTo<GameDetailedModel>().First();
        }

        public bool Remove(int gameId)
        {
            var game = this.db.Games.Find(gameId);

            if (game == null)
            {
                return false;
            }

            var hasGameBets = this.db.GameBets
                .Any(gb => gb.BetCoefficient.GameId == gameId && !gb.IsEvaluated);

            var hasPlayerBets = this.db.PlayerGameBets
                .Any(pgb => pgb.BetCoefficient.GameId == gameId && !pgb.IsEvaluated);

            if (hasGameBets || hasPlayerBets)
            {
                return false;
            }

            this.db.Games.Remove(game);

            this.db.SaveChanges();

            return true;
        }

        public GameTeamsModel GetGameTeams(int gameId)
            => GetTeams(gameId);
    }
}
