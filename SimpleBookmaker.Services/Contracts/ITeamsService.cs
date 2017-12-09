namespace SimpleBookmaker.Services.Contracts
{
    using Models.Team;
    using System.Collections.Generic;

    public interface ITeamsService : IService
    {
        bool Add(string name);

        bool Exists(int id);

        int Count(string keyword);

        string GetName(int id);

        bool AddToTournament(int teamId, int tournamentId);

        IEnumerable<BaseTeamModel> ByTournament(int tournamentId);

        IEnumerable<TeamListModel> All(int page = 1, int pageSize = 20, string keyword = null);
    }
}
