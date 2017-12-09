﻿namespace SimpleBookmaker.Services.Contracts
{
    using Models.Tournament;
    using Services.Models.Game;
    using Services.Models.Team;
    using System;
    using System.Collections.Generic;

    public interface ITournamentsService : IService
    {
        TournamentModel ById(int tournamentId);

        bool Add(string name, DateTime startDate, DateTime endDate);

        bool Edit(int id, string name, DateTime startDate, DateTime endDate);

        bool Remove(int tournamentd);

        bool Exists(int tournamentId);

        IEnumerable<TournamentListModel> All();

        IEnumerable<TournamentListModel> WithTeam(int teamId);

        IEnumerable<TournamentListModel> WithoutTeam(int teamId);

        IEnumerable<TournamentDetailedListModel> AllDetailed();

        IEnumerable<BaseTeamModel> GetTeams(int tournamentId);

        IEnumerable<BaseTeamModel> GetAvailableTeams(int tournamentId);

        bool AddTeam(int tournamentId, int teamId);

        bool RemoveTeam(int tournamentId, int teamId);

        bool AddTeams(int tournamentId, IEnumerable<int> teamIds);

        string GetName(int tournamentId);

        bool IsInTournament(int tournamentId, int teamId);

        IEnumerable<GameListModel> Games(int tournamentId, bool upcoming = false, int page = 1, int pageSize = 20);

        int GamesCount(int tournamentId);

        bool IsDuringTournament(int tournamentId, DateTime date);
    }
}