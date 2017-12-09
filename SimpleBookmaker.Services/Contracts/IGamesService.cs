﻿namespace SimpleBookmaker.Services.Contracts
{
    using Models.Game;
    using System;
    using System.Collections.Generic;

    public interface IGamesService : IService
    {
        bool Add(int tournamentId, int homeTeamId, int awayTeamId, DateTime time);

        void Remove(int gameId);

        void Edit(int gameId, DateTime kickoff);

        bool Exists(int gameId);

        IEnumerable<GameListModel> Upcoming(int page = 1, int pageSize = 20);

        GameDetailedModel ById(int id);

        int UpcomingCount(int tournamentId);

        GameTeamsModel GetTeams(int gameId);
    }
}