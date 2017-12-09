namespace SimpleBookmaker.Services.Infrastructure.BetDescribers
{
    using Data.Core.Enums;
    using System.Collections.Generic;

    public static class GamePlayerBetDescriber
    {
        private static readonly IDictionary<PlayerGameBetType, string> betDescriptions = new Dictionary<PlayerGameBetType, string>()
        {
            { PlayerGameBetType.ScoreGoal, "Player to score" },
            { PlayerGameBetType.Score2Goals, "Player to score 2 or more goals" },
            { PlayerGameBetType.ScoreMoreThan2Goals, "Player to score 3 or more goals" }
        };

        public static string Describe(PlayerGameBetType betType, string playerName)
            => betDescriptions[betType].Replace("Player", playerName);

        public static string RawDescription(PlayerGameBetType betType)
            => betDescriptions[betType];
    }
}
