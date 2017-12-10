namespace SimpleBookmaker.Services.Contracts
{
    using Data.Core.Enums;
    using Models.Bet;
    using System.Collections.Generic;

    public interface ITournamentBetsService : IService
    {
        bool AddTournamentCoefficient(int tournamentId,
            int subjectId,
            double coefficient,
            TournamentBetType betType);

        IEnumerable<TournamentCoefficientListModel> ExistingTournamentCoefficients(int tournamentId);

        IEnumerable<TournamentPossibleCoefficientModel> PossibleTournamentCoefficients();

        void EditCoefficient(int coefficientId, double newCoefficient);

        void RemoveCoefficient(int coefficientId);
    }
}
