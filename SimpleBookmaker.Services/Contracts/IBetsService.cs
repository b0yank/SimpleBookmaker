namespace SimpleBookmaker.Services.Contracts
{
    using Data.Core.Enums;
    using Models.Bet;
    using System.Collections.Generic;

    public interface IBetsService : IService
    {
        IEnumerable<GamePossibleBetListModel> PossibleGameCoefficients(int gameId);

        bool AddGameCoefficient(int gameId, 
            GameBetType betType, 
            BetSide betSide, 
            double coefficient, 
            int homeGoals = -1, 
            int awayGoals = -1);

        bool AddTournamentCoefficient(int tournamentId,
            int subjectId,
            double coefficient,
            TournamentBetType betType);

        bool AddPlayerGameCoefficient(int playerId, int gameId, PlayerGameBetType betType, double coefficient);

        void RemoveCoefficient(int coefficientId, BetType betType);

        void EditCoefficient(int coefficientId, double newCoefficient, BetType betType);

        IEnumerable<GameCoefficientListModel> ExistingGameCoefficients(int gameId);

        IEnumerable<TournamentCoefficientListModel> ExistingTournamentCoefficients(int tournamentId);

        IEnumerable<GamePlayerCoefficientListModel> ExistingGamePlayerCoefficients(int gameId);

        IEnumerable<GamePlayerPossibleBetListModel> PossibleGamePlayerCoefficients();

        IEnumerable<TournamentPossibleCoefficientModel> PossibleTournamentCoefficients();
    }
}
