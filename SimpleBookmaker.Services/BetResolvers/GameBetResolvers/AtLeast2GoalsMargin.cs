namespace SimpleBookmaker.Services.BetResolvers.GameBetResolvers
{
    using Contracts;
    using Data.Core.Enums;
    using Infrastructure.EventStats;
    using System.Linq;

    public class AtLeast2GoalsMargin : IGameBetResolver
    {
        public bool Resolve(GameStats gameStats, BetSide betSide, object additional = null)
        {
            if (betSide == BetSide.Home)
            {
                return gameStats.HomeGoalscorers.Count() - 2 >= gameStats.AwayGoalscorers.Count();
            }

            return gameStats.AwayGoalscorers.Count() - 2 >= gameStats.HomeGoalscorers.Count();
        }
    }
}
