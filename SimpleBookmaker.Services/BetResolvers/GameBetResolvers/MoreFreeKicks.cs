namespace SimpleBookmaker.Services.BetResolvers.GameBetResolvers
{
    using Contracts;
    using Data.Core.Enums;
    using Infrastructure.EventStats;

    public class MoreFreeKicks : IGameBetResolver
    {
        public bool Resolve(GameStats gameStats, BetSide betSide, object additional = null)
        {
            if (betSide == BetSide.Home)
            {
                return gameStats.HomeFreeKicks > gameStats.AwayFreeKicks;
            }

            return gameStats.HomeFreeKicks < gameStats.AwayFreeKicks;
        }
    }
}
