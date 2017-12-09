namespace SimpleBookmaker.Services.BetResolvers.GameBetResolvers
{
    using Contracts;
    using Data.Core.Enums;
    using Infrastructure.EventStats;
    using System.Linq;

    public class BothTeamsScore : IGameBetResolver
    {
        public bool Resolve(GameStats gameStats, BetSide betSide, object additional = null)
        {
            return gameStats.HomeGoalscorers.Count() > 0
                && gameStats.AwayGoalscorers.Count() > 0;
        }
    }
}
