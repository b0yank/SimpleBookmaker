namespace SimpleBookmaker.Services.BetResolvers.GameBetResolvers
{
    using Contracts;
    using Data.Core.Enums;
    using Infrastructure.BetDescribers;
    using Infrastructure.EventStats;
    using System.Linq;

    public class Result : IGameBetResolver
    {
        public bool Resolve(GameStats gameStats, BetSide betSide, object additional = null)
        {
            var resultDescriber = (ResultDescriber) additional;

            return gameStats.HomeGoalscorers.Count() == resultDescriber.HomeScore
                && gameStats.AwayGoalscorers.Count() == resultDescriber.AwayScore;
        }
    }
}
