namespace SimpleBookmaker.Services.BetResolvers.GameBetResolvers
{
    using Contracts;
    using Data.Core.Enums;
    using Infrastructure.EventStats;

    public class MoreCorners : IGameBetResolver
    {
        public bool Resolve(GameStats gameStats, BetSide betSide, object additional = null)
        {
            if (betSide == BetSide.Home)
            {
                return gameStats.HomeCorners > gameStats.AwayCorners;
            }

            return gameStats.HomeCorners < gameStats.AwayCorners;
        }
    }
}
