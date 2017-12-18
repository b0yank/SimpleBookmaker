namespace SimpleBookmaker.Services.Contracts
{
    using Data.Core.Enums;
    using Models.Bet;
    using Models.UserCoefficient;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserBetsService : IService
    {
        IEnumerable<UserGameCoefficientModel> GameCoefficients(int gameId);

        IEnumerable<UserGamePlayerCoefficientModel> PlayerCoefficients(int gameId);

        bool PlaceBets(IEnumerable<BetUnconfirmedModel> bets, double amount, string username);

        bool? SlipHasConflictingBets(IDictionary<int, BetType> coefficientsByIdAndType, int coefficientId, BetType betType);

        Task<IEnumerable<UserCurrentBetSlipModel>> CurrentBets(string username);

        Task<IEnumerable<UserHistoryBetSlipModel>> UserHistory(string username, int page = 1, int pageSize = 5);

        Task<int> UserHistoryCount(string username);
    }
}
