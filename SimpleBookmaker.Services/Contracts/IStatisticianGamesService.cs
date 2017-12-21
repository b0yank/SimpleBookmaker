namespace SimpleBookmaker.Services.Contracts
{
    using Services.Models.Game;
    using Services.Infrastructure.EventStats;
    using System.Collections.Generic;

    public interface IStatisticianGamesService : IService
    {
        IEnumerable<GameStatsListModel> Finished();

        GameStatsModel ById(int gameId);

        bool Exists(int gameId);

        void ResolveBets(GameStats gameStats);
    }
}
