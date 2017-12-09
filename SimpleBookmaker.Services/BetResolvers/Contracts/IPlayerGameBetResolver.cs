namespace SimpleBookmaker.Services.BetResolvers.Contracts
{
    using Infrastructure.EventStats;

    public interface IPlayerGameBetResolver
    {
        bool Resolve(GameStats gameStats, int playerId);
    }
}
