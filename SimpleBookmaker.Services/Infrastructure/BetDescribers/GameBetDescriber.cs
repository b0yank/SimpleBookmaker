using AutoMapper.QueryableExtensions;

namespace SimpleBookmaker.Services.Infrastructure.BetDescribers
{
    using Data.Core.Enums;
    using Models.Game;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GameBetDescriber
    {
        private static readonly IDictionary<GameBetType, string> gameBetDescriptions = new Dictionary<GameBetType, string>()
        {
            { GameBetType.AtLeast2GoalsMargin, " to win by at least 2 goals" },
            { GameBetType.AtLeast3GoalsMargin, " to win by at least 3 goals" },
            { GameBetType.BothTeamsScore, "Both teams to score" },
            { GameBetType.CleanSheet, " to keep a clean sheet" },
            { GameBetType.MoreCorners, " to have more corners" },
            { GameBetType.MoreFreeKicks, " to have more free kicks" },
            { GameBetType.MorePossession, " to have more than 50% possession" },
            { GameBetType.Result, "Game to finish {0} - {1}" }
        };

        public static string Describe(GameBetType betType, BetSide betSide, object additionalPrerequisites)
        {
            if (betType == GameBetType.Result)
            {
                var resultDescriber = (ResultDescriber)additionalPrerequisites;

                return string.Format(gameBetDescriptions[betType], resultDescriber.HomeScore, resultDescriber.AwayScore);
            }
            else if (betType == GameBetType.BothTeamsScore)
            {
                return gameBetDescriptions[GameBetType.BothTeamsScore];
            }
            else
            {
                var teams = additionalPrerequisites as GameTeamsModel;

                var homeOrAwayTeamDescription = teams == null
                        ? ((betSide == BetSide.Home ? "Home" : "Away") + " team")
                        : (betSide == BetSide.Home ? teams.HomeTeam : teams.AwayTeam);

                if (betType == GameBetType.Outcome)
                {
                    if (betSide == BetSide.Neutral)
                    {
                        return "Game to end as draw";
                    }

                    return homeOrAwayTeamDescription + " to win";
                }
                else
                {
                    return homeOrAwayTeamDescription + gameBetDescriptions[betType];
                }
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

             return "One team" + gameBetDescriptions[betType];
        }
    }
}
