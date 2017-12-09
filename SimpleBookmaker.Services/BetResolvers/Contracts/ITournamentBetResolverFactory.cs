namespace SimpleBookmaker.Services.BetResolvers.Contracts
{
    using Services.Contracts;
    using Data.Core.Enums;

    public interface ITournamentBetResolverFactory : IService
    {
        ITournamentBetResolver GetResolver(TournamentBetType betType);
    }
}
