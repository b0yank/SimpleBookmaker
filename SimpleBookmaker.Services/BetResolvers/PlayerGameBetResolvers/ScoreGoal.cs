namespace SimpleBookmaker.Services.BetResolvers.PlayerGameBetResolvers
{
    using Contracts;
    using Infrastructure.EventStats;
    using System.Linq;

    public class ScoreGoal : IPlayerGameBetResolver
    {
        public bool Resolve(GameStats gameStats, int playerId)
        {
            return gameStats.HomeGoalscorers.Contains(playerId)
                || gameStats.AwayGoalscorers.Contains(playerId);
        }
    }
}
