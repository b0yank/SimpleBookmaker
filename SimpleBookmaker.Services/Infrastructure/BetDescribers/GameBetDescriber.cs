namespace SimpleBookmaker.Services.Infrastructure.BetDescribers
{
    using Data.Core.Enums;
    using System.Collections.Generic;

    public class GameBetDescriber
    {
        private static readonly IDictionary<GameBetType, string> gameBetDescriptions = new Dictionary<GameBetType, string>()
        {
            { GameBetType.AtLeast2GoalsMargin, "team to win by at least 2 goals" },
            { GameBetType.AtLeast3GoalsMargin, "team to win by at least 3 goals" },
            { GameBetType.BothTeamsScore, "Both teams to score" },
            { GameBetType.CleanSheet, "team to keep a clean sheet" },
            { GameBetType.MoreCorners, "team to have more corners" },
            { GameBetType.MoreFreeKicks, "team to have more free kicks" },
            { GameBetType.MorePossession, "team to have more than 50% possession" },
            { GameBetType.Result, "Game to finish {0} - {1}" }
        };

        public static string Describe(GameBetType betType, BetSide betSide, object additionalPrerequisites)
        {
            if (betType == GameBetType.Result)
            {
                var resultDescriber = (ResultDescriber)additionalPrerequisites;

                return string.Format(gameBetDescriptions[betType], resultDescriber.HomeScore, resultDescriber.AwayScore);
            }
            else if (betType == GameBetType.Outcome)
            {
                if (betSide == BetSide.Neutral)
                {
                    return "Game to end as draw";
                }

                return (betSide == BetSide.Home ? "Home " : "Away ") + "team to win";
            }
            else
            {
                if (betType == GameBetType.BothTeamsScore)
                {
                    return gameBetDescriptions[GameBetType.BothTeamsScore];
                }

                return (betSide == BetSide.Home ? "Home " : "Away ") + gameBetDescriptions[betType];
            }
        }

        public static string RawDescription(GameBetType betType)
        {
            if (betType == GameBetType.BothTeamsScore)
            {
                return gameBetDescriptions[betType];
            }
            else if(betType == GameBetType.Outcome)
            {
                return "One of the teams to win, or game to end as draw";
            }
            else if (betType == GameBetType.Result)
            {
                return "Game to finish with an exact score";
            }

             return "One " + gameBetDescriptions[betType];
        }
    }
}
