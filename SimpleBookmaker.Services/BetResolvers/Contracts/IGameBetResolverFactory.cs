namespace SimpleBookmaker.Services.BetResolvers.Contracts
{
    using Data.Core.Enums;
    using Services.Contracts;

    public interface IGameBetResolverFactory : IService
    {
        IGameBetResolver GetResolver(GameBetType betType);
    }
}
