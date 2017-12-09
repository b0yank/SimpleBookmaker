namespace SimpleBookmaker.Services.BetResolvers.Contracts
{
    using Data.Core.Enums;
    using Services.Contracts;

    public interface IPlayerGameBetResolverFactory : IService
    {
        IPlayerGameBetResolver GetResolver(PlayerGameBetType betType);
    }
}
