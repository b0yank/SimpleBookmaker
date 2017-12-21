namespace SimpleBookmaker.Services
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Data.Core.Enums;
    using SimpleBookmaker.Data.Models;
    using SimpleBookmaker.Services.Models.Player;
    using System.Collections.Generic;
    using System.Linq;

    public class PlayersService : Service, IPlayersService
    {
        public PlayersService(SimpleBookmakerDbContext db)
            : base(db)
        {
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

        public virtual bool Remove(int playerId)
        {
            var player = this.db.Players.Find(playerId);

            if (player == null)
            {
                return false;
            }

            var hasPlayerBets = this.db.PlayerGameBets
                .Any(pgb => pgb.BetCoefficient.PlayerId == playerId
                    && !pgb.IsEvaluated);

            var hasTournamentBets = this.db.TournamentBets
                .Any(tb => tb.BetCoefficient.BetType == TournamentBetType.TopScorer
                    && tb.BetCoefficient.BetSubjectId == playerId);

            if (hasPlayerBets || hasTournamentBets)
            {
                return false;
            }

            var playerCoefficients = this.db.PlayerGameBetCoefficients.Where(pbc => pbc.PlayerId == playerId);
            this.db.PlayerGameBetCoefficients.RemoveRange(playerCoefficients);

            var tournamentPlayers = this.db.TournamentPlayers.Where(tp => tp.PlayerId == playerId);
            this.db.TournamentPlayers.RemoveRange(tournamentPlayers);

            this.db.Players.Remove(player);

            this.db.SaveChanges();

            return true;
        }

        public virtual void Create(string name, int age, int teamId)
        {
            var player = new Player
            {
                Name = name,
                Age = age,
                TeamId = teamId
            };

            this.AddPlayerToTeamTournaments(player, teamId);

            this.db.Players.Add(player);
            this.db.SaveChanges();
        }

        public virtual bool AddToTeam(int playerId, int teamId)
        {
            var player = this.db.Players.Find(playerId);
            var team = this.db.Teams.Find(teamId);

            if (player == null || team == null)
            {
                return false;
            }

            if (player.TeamId == teamId)
            {
                return true;
            }

            this.AddPlayerToTeamTournaments(player, teamId);

            player.TeamId = teamId;
            this.db.SaveChanges();

            return true;
        }

        public virtual bool RemoveFromTeam(int playerId)
        {
            var player = this.db.Players.Find(playerId);

            if (player == null)
            {
                return false;
            }

            if (this.db.PlayerGameBets.Any(pgb => pgb.BetCoefficient.PlayerId == playerId && !pgb.IsEvaluated))
            {
                return false;
            }

            var tournamentPlayers = this.db.TournamentPlayers.Where(tp => tp.PlayerId == playerId);

            this.db.TournamentPlayers.RemoveRange(tournamentPlayers);

            player.TeamId = null;
            this.db.SaveChanges();

            return true;
        }

        public IEnumerable<PlayerListModel> Unassigned()
            => this.db.Players
                    .Where(p => p.TeamId == null)
                    .ProjectTo<PlayerListModel>();

        private void AddPlayerToTeamTournaments(Player player, int teamId)
        {
            var tournamentIds = this.db.TournamentsTeams
                .Where(tt => tt.TeamId == teamId)
                .Select(t => t.TournamentId);

            foreach (var tournamentId in tournamentIds)
            {
                if (this.db.TournamentPlayers.Any(tp => tp.PlayerId == player.Id && tp.TournamentId == tournamentId))
                {
                    continue;
                }

                var tournamentPlayer = new TournamentPlayer
                {
                    Player = player,
                    TournamentId = tournamentId
                };

                this.db.TournamentPlayers.Add(tournamentPlayer);
            }
        }       
    }
}
