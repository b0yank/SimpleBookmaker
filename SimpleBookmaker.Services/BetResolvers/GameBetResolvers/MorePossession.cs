namespace SimpleBookmaker.Services.BetResolvers.GameBetResolvers
{
    using Contracts;
    using Data.Core.Enums;
    using Infrastructure.EventStats;

    public class MorePossession : IGameBetResolver
    {
        public bool Resolve(GameStats gameStats, BetSide betSide, object additional = null)
        {
            if (betSide == BetSide.Home)
            {
                return gameStats.HomePossession > gameStats.AwayPossession;
            }

            return gameStats.HomePossession < gameStats.AwayPossession;
        }
    }
}
