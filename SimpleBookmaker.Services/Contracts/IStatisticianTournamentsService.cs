namespace SimpleBookmaker.Services.Contracts
{
    using Services.Models.Tournament;
    using System.Collections.Generic;

    public interface IStatisticianTournamentsService : IService
    {
        TournamentStatsSetModel ById(int tournamentId);

        bool Exists(int tournamentId);

        IEnumerable<TournamentStatsListModel> Finished();

        bool ResolveBets(int tournamentId, int championId);
    }
}
