namespace SimpleBookmaker.Services.BetResolvers.Contracts
{
    using Infrastructure.EventStats;

    public interface ITournamentBetResolver
    {
        bool Resolve(TournamentStats tournamentStats, int subjectId);
    }
}
