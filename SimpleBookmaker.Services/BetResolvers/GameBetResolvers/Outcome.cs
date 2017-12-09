namespace SimpleBookmaker.Services.BetResolvers.GameBetResolvers
{
    using Contracts;
    using Data.Core.Enums;
    using Infrastructure.EventStats;
    using System.Linq;

    public class Outcome : IGameBetResolver
    {
        public bool Resolve(GameStats gameStats, BetSide betSide, object additional = null)
        {
            if (betSide == BetSide.Home)
            {
                return gameStats.HomeGoalscorers.Count() > gameStats.AwayGoalscorers.Count();
            }
            else if (betSide == BetSide.Away)
            {
                return gameStats.HomeGoalscorers.Count() == gameStats.AwayGoalscorers.Count();
            }

            return gameStats.HomeGoalscorers.Count() < gameStats.AwayGoalscorers.Count();
        }
    }
}
