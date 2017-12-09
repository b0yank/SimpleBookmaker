namespace SimpleBookmaker.Services.BetResolvers.GameBetResolvers
{
    using Contracts;
    using Data.Core.Enums;
    using Infrastructure.EventStats;
    using System.Linq;

    public class AtLeast3GoalsMargin : IGameBetResolver
    {
        public bool Resolve(GameStats gameStats, BetSide betSide, object additional = null)
        {
            if (betSide == BetSide.Home)
            {
                return gameStats.HomeGoalscorers.Count() - 3 >= gameStats.AwayGoalscorers.Count();
            }

            return gameStats.AwayGoalscorers.Count() - 3 >= gameStats.HomeGoalscorers.Count();
        }
    }
}
