namespace SimpleBookmaker.Services.BetResolvers.TournamentBetResolvers
{
    using Contracts;
    using Infrastructure.EventStats;
    using System.Linq;

    public class TopScorer : ITournamentBetResolver
    {
        public bool Resolve(TournamentStats tournamentStats, int subjectId)
        {
            return tournamentStats.TopScorers.Contains(subjectId);
        }
    }
}
