namespace SimpleBookmaker.Services.BetResolvers.TournamentBetResolvers
{
    using Contracts;
    using Infrastructure.EventStats;

    public class Champion : ITournamentBetResolver
    {
        public bool Resolve(TournamentStats tournamentStats, int subjectId)
        {
            return tournamentStats.ChampionId == subjectId;
        }
    }
}
