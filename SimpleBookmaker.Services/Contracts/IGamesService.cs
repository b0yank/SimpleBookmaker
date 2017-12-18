namespace SimpleBookmaker.Services.Contracts
{
    using Models.Game;
    using System;
    using System.Collections.Generic;

    public interface IGamesService : IService
    {
        bool Add(int tournamentId, int homeTeamId, int awayTeamId, DateTime time);

        bool Remove(int gameId);

        void Edit(int gameId, DateTime kickoff);

        bool Exists(int gameId);

        IEnumerable<GameListModel> Upcoming(int page = 1, int pageSize = 20, int tournamentId = 0);

        GameDetailedModel ById(int id);

        int UpcomingCount(int tournamentId);

        GameTeamsModel GetGameTeams(int gameId);

        DateTime? GetGametime(int gameId);
    }
}
