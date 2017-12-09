namespace SimpleBookmaker.Services.BetResolvers.PlayerGameBetResolvers
{
    using Contracts;
    using Infrastructure.EventStats;
    using System.Linq;

    public class ScoreMoreThan2Goals : IPlayerGameBetResolver
    {
        public bool Resolve(GameStats gameStats, int playerId)
        {
            return gameStats.HomeGoalscorers.Count(gs => gs == playerId) > 2
                || gameStats.AwayGoalscorers.Count(gs => gs == playerId) > 2;
        }
    }
}
