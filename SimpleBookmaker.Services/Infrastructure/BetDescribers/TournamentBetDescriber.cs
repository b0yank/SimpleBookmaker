namespace SimpleBookmaker.Services.Infrastructure.BetDescribers
{
    using Data.Core.Enums;
    using System.Collections.Generic;

    public static class TournamentBetDescriber
    {
        private static readonly IDictionary<TournamentBetType, string> betDescriptions = new Dictionary<TournamentBetType, string>()
        {
            { TournamentBetType.Champion, "Team to finish as champions" },
            { TournamentBetType.TopScorer, "Player to finish as top scorer" }
        };

        public static string Describe(TournamentBetType betType, string subjectName)
        {
            var subjectType = TournamentCoefficientSubjectType(betType);

            var description = betDescriptions[betType];

            if (subjectType == SubjectType.Team)
            {
                return description.Replace("Team", subjectName);
            }

            return description.Replace("Player", subjectName);
        }

        public static string RawDescription(TournamentBetType betType)
            => betDescriptions[betType];

        public static SubjectType TournamentCoefficientSubjectType(TournamentBetType betType)
        {
            if (betType == TournamentBetType.TopScorer)
            {
                return SubjectType.Player;
            }

            return SubjectType.Team;
        }
    }
}
