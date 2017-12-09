namespace SimpleBookmaker.Services.Contracts
{
    using Services.Models.Player;
    using System.Collections.Generic;

    public interface IPlayersService : IService
    {
        IEnumerable<PlayerListModel> ByTeam(int teamId);

        IEnumerable<PlayerListModel> ByTeam(string teamName);

        IEnumerable<PlayerListModel> Unassigned();

        void Create(string name, int age, int teamId);

        bool RemoveFromTeam(int playerId);

        bool Exists(int playerId);

        bool AddToTeam(int playerId, int teamId);

        void Remove(int playerId);
    }
}
