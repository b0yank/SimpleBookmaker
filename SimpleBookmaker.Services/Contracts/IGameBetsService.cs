﻿namespace SimpleBookmaker.Services.Contracts
{
    using Data.Core.Enums;
    using Models.Bet;
    using Models.UserCoefficient;
    using System.Collections.Generic;

    public interface IGameBetsService : IService
    {
        GameBasicCoefficientsListModel ByGameBasic(int gameId);

        IEnumerable<CoefficientListModel> ByGameAll(int gameId);

        IEnumerable<GamePossibleBetListModel> PossibleGameCoefficients(int gameId);

        bool AddGameCoefficient(int gameId,
            GameBetType betType,
            BetSide betSide,
            double coefficient,
            int homeGoals = -1,
            int awayGoals = -1);

        bool AddPlayerGameCoefficient(int playerId, int gameId, PlayerGameBetType betType, double coefficient);

        IEnumerable<GameCoefficientListModel> ExistingGameCoefficients(int gameId);

        IEnumerable<GamePlayerCoefficientListModel> ExistingGamePlayerCoefficients(int gameId);

        IEnumerable<GamePlayerPossibleBetListModel> PossibleGamePlayerCoefficients();

        void EditCoefficient(int coefficientId, double newCoefficient, BetType betType);

        bool RemoveCoefficient(int coefficientId, BetType betType);

        bool HasBasicCoefficients(int gameId);
    }
}
