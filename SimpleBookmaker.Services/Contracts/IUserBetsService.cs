namespace SimpleBookmaker.Services.Contracts
{
    using Data.Core.Enums;
    using Models.Bet;
    using Models.UserCoefficient;
    using System.Collections.Generic;

    public interface IUserBetsService : IService
    {
        IEnumerable<UserGameCoefficientModel> GameCoefficients(int gameId);

        IEnumerable<UserGamePlayerCoefficientModel> PlayerCoefficients(int gameId);

        bool PlaceBets(IEnumerable<BetUnconfirmedModel> bets, double amount, string username);

        bool? SlipHasConflictingBets(IDictionary<int, BetType> coefficientsByIdAndType, int coefficientId, BetType betType);
    }
}
