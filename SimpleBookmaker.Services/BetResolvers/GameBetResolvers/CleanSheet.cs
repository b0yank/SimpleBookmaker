namespace SimpleBookmaker.Services.BetResolvers.GameBetResolvers
{
    using Contracts;
    using Data.Core.Enums;
    using Infrastructure.EventStats;
    using System.Linq;

    public class CleanSheet : IGameBetResolver
    {
        public bool Resolve(GameStats gameStats, BetSide betSide, object additional = null)
        {
            if (betSide == BetSide.Home)
            {
                return gameStats.AwayGoalscorers.Count() == 0;
            }

            return gameStats.HomeGoalscorers.Count() == 0;
        }
    }
}
