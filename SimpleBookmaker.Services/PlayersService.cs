namespace SimpleBookmaker.Services
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using SimpleBookmaker.Data.Models;
    using SimpleBookmaker.Services.Models.Player;
    using System.Collections.Generic;
    using System.Linq;

    public class PlayersService : IPlayersService
    {
        private readonly SimpleBookmakerDbContext db;

        public PlayersService(SimpleBookmakerDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<PlayerListModel> ByTeam(int teamId)
            => this.db
                .Players
                .Where(p => p.Team.Id == teamId)
                .ProjectTo<PlayerListModel>();

        public IEnumerable<PlayerListModel> ByTeam(string teamName)
            => this.db
                .Players
                .Where(p => p.Team.Name == teamName)
                .ProjectTo<PlayerListModel>();

        public bool Exists(int playerId)
            => this.db.Players.Find(playerId) != null;

        public void Remove(int playerId)
        {
            var player = this.db.Players.Find(playerId);

            if (player != null)
            {
                this.db.Players.Remove(player);

                this.db.SaveChanges();
            }
        }

        public void Create(string name, int age, int teamId)
        {
            var player = new Player
            {
                Name = name,
                Age = age,
                TeamId = teamId
            };

            this.db.Players.Add(player);
            this.db.SaveChanges();
        }

        public bool AddToTeam(int playerId, int teamId)
        {
            var player = this.db.Players.Find(playerId);
            var team = this.db.Teams.Find(teamId);

            if (player == null || team == null)
            {
                return false;
            }

            player.TeamId = teamId;
            this.db.SaveChanges();

            return true;
        }

        public bool RemoveFromTeam(int playerId)
        {
            var player = this.db.Players.Find(playerId);

            if (player == null)
            {
                return false;
            }

            player.TeamId = null;
            this.db.SaveChanges();

            return true;
        }

        public IEnumerable<PlayerListModel> Unassigned()
            => this.db.Players
                    .Where(p => p.TeamId == null)
                    .ProjectTo<PlayerListModel>();
                
    }
}
