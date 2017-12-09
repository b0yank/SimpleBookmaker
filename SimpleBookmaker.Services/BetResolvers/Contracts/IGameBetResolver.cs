namespace SimpleBookmaker.Services.BetResolvers.Contracts
{
    using Data.Core.Enums;
    using Infrastructure.EventStats;

    public interface IGameBetResolver
    {
        bool Resolve(GameStats gameStats, BetSide betSide, object additional = null);
    }
}
