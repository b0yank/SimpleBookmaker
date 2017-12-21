namespace SimpleBookmaker.Tests.Services
{
    using Data.Core.Enums;
    using SimpleBookmaker.Services;
    using SimpleBookmaker.Services.Models.Bet;
    using SimpleBookmaker.Services.Models.UserCoefficient;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Xunit;

    public class GameBetsServiceTest : TestClass
    {
        private GameBetsService InitializeService()
        {
            var context = Tests.Context;
            Tests.PopulateData(context, true);

            var service = new GameBetsService(context);

            return service;
        }

        [Fact]
        public void ByGameBasic_ShouldReturnNullWhenInvalidGame()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.ByGameBasic(invalidIdTestValue);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ByGameBasic_ShouldReturnNullWhenGameLacksBasicCoefficients()
        {
            // Arrange
            const int gameIdGameLacksAllBasicCoefficients = 2;

            var service = this.InitializeService();

            // Act
            var result = service.ByGameBasic(gameIdGameLacksAllBasicCoefficients);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ByGameBasic_ShouldReturnModelWhenValid()
        {

            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.ByGameBasic(validIdTestValue);

            // Assert
            Assert.IsType<GameBasicCoefficientsListModel>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public void ByGameAll_ShouldReturnNullWhenInvalidGame()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.ByGameAll(invalidIdTestValue);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ByGameAll_ShouldReturnModelWhenValid()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.ByGameAll(validIdTestValue);

            // Assert
            Assert.IsAssignableFrom<IEnumerable<CoefficientListModel>>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public void AddGameCoefficient_ShouldReturnFalseWhenInvalidGame()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.AddGameCoefficient(invalidIdTestValue, GameBetType.CleanSheet, BetSide.Home, 2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AddGameCoefficient_ShouldReturnFalseWhenSameNonResultCoefficientExists()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.AddGameCoefficient(validIdTestValue, GameBetType.Outcome, BetSide.Home, 2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AddGameCoefficient_ShouldReturnFalseWhenSameResultCoefficientExists()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.AddGameCoefficient(4, GameBetType.Result, BetSide.Home, 2, 3, 2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AddGameCoefficient_ShouldReturnTrueWhenNonResultCoefficientDoesNotExist()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.AddGameCoefficient(validIdTestValue, GameBetType.BothTeamsScore, BetSide.Home, 2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AddGameCoefficient_ShouldReturnTrueWhenResultCoefficientDoesNotExist()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.AddGameCoefficient(validIdTestValue, GameBetType.Result, BetSide.Home, 2, 1, 1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ExistingGameCoefficients_ShouldReturnNullWhenInvalidGame()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.ExistingGameCoefficients(invalidIdTestValue);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ExistingGameCoefficients_ShouldReturnModelWhenValidGame()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.ExistingGameCoefficients(validIdTestValue);

            // Assert
            Assert.IsAssignableFrom<IEnumerable<GameCoefficientListModel>>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public void PossibleGameCoefficients_ShouldReturnNullWhenInvalidGame()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.PossibleGameCoefficients(invalidIdTestValue);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void PossibleGameCoefficients_ShouldReturnModelWhenValidGame()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.PossibleGameCoefficients(validIdTestValue);

            // Assert
            Assert.IsAssignableFrom<IEnumerable<GamePossibleBetListModel>>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public void ExistingGamePlayerCoefficients_ShouldReturnModelWhenValidGame()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.ExistingGamePlayerCoefficients(validIdTestValue);

            // Assert
            Assert.IsAssignableFrom<IEnumerable<GamePlayerCoefficientListModel>>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public void AddPlayerGameCoefficient_ShouldReturnFalseWhenInvalidGame()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.AddPlayerGameCoefficient(validIdTestValue, invalidIdTestValue, PlayerGameBetType.ScoreGoal, 2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AddPlayerGameCoefficient_ShouldReturnFalseWhenInvalidPlayer()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.AddPlayerGameCoefficient(invalidIdTestValue, validIdTestValue, PlayerGameBetType.ScoreGoal, 2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AddPlayerGameCoefficient_ShouldReturnFalseWhenCoefficientExists()
        {
            // Arrange
            const int playerIdWhereCoefficientExists = 1;
            const int gameIdWhereCoefficientExists = 1;

            var service = this.InitializeService();

            // Act
            var result = service.AddPlayerGameCoefficient(playerIdWhereCoefficientExists,
                gameIdWhereCoefficientExists,
                PlayerGameBetType.ScoreGoal,
                2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AddPlayerGameCoefficient_ShouldReturnTrueWhenCoefficientDoesNotExist()
        {
            // Arrange
            const int playerIdWhereCoefficientDoesNotExist = 3;
            const int gameIdWhereCoefficientDoesNotExist = 3;

            var service = this.InitializeService();

            // Act
            var result = service.AddPlayerGameCoefficient(playerIdWhereCoefficientDoesNotExist,
                gameIdWhereCoefficientDoesNotExist,
                PlayerGameBetType.ScoreGoal,
                2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasBasicCoefficients_ShouldReturnFalse()
        {
            // Arrange
            const int gameWithNoBasicCoefficientsId = 3;
            var service = this.InitializeService();

            // Act
            var result = service.HasBasicCoefficients(gameWithNoBasicCoefficientsId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HasBasicCoefficients_ShouldReturnTrue()
        {
            // Arrange
            const int gameWithBasicCoefficientsId = 1;
            var service = this.InitializeService();

            // Act
            var result = service.HasBasicCoefficients(gameWithBasicCoefficientsId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void PossibleGamePlayerCoefficients_ShouldReturnModel()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.PossibleGamePlayerCoefficients();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<GamePlayerPossibleBetListModel>>(result);
        }

        [Fact]
        public void PossibleGameCoefficients_Private_ShouldReturnAllPossibleGameCoefficients()
        {
            // Arrange
            var service = this.InitializeService();

            var method = typeof(GameBetsService)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .First(m => m.IsPrivate && m.Name == "PossibleGameCoefficients");

            var gameBetTypeValues = Enum.GetValues(typeof(GameBetType)).Cast<GameBetType>();

            // Act
            var result = (IDictionary<GameBetType, ICollection<BetSide>>)method.Invoke(service, null);

            // Assert
            Assert.True(result.Keys.Count() == gameBetTypeValues.Count());

            foreach (var gameBetType in gameBetTypeValues)
            {
                Assert.True(result.Keys.Contains(gameBetType));

                if (gameBetType == GameBetType.Outcome
                || gameBetType == GameBetType.BothTeamsScore
                || gameBetType == GameBetType.BothTeamsScore)
                {
                    Assert.True(result[gameBetType].Contains(BetSide.Neutral));
                }

                if (gameBetType != GameBetType.BothTeamsScore)
                {
                    Assert.True(result[gameBetType].Contains(BetSide.Home));
                    Assert.True(result[gameBetType].Contains(BetSide.Away));
                }
            }
        }

        [Fact]
        public void RemoveCoefficient_ShouldReturnFalseWhenInvalidGameCoefficient()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.RemoveCoefficient(invalidIdTestValue, BetType.Game);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RemoveCoefficient_ShouldReturnFalseWhenGameCoefficientHasUnresolvedBets()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.RemoveCoefficient(validIdTestValue, BetType.Game);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RemoveCoefficient_ShouldReturnFalseWhenInvalidPlayerCoefficient()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.RemoveCoefficient(invalidIdTestValue, BetType.PlayerGame);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RemoveCoefficient_ShouldReturnFalseWhenPlayerCoefficientHasUnresolvedBets()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.RemoveCoefficient(validIdTestValue, BetType.PlayerGame);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RemoveCoefficient_ShouldReturnTrueWhenGameCoefficientHasNoUnresolvedBets()
        {
            // Arrange
            const int gameCoefficientWithNoUnresolvedBets = 7;
            var service = this.InitializeService();

            // Act
            var result = service.RemoveCoefficient(gameCoefficientWithNoUnresolvedBets, BetType.Game);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void RemoveCoefficient_ShouldReturnTrueWhenPlayerCoefficientHasNoUnresolvedBets()
        {
            // Arrange
            const int playerCoefficientWithNoUnresolvedBets = 5;
            var service = this.InitializeService();

            // Act
            var result = service.RemoveCoefficient(playerCoefficientWithNoUnresolvedBets, BetType.PlayerGame);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void RemoveCoefficient_ShouldRemoveCoefficientFromDb()
        {
            // Arrange
            var context = Tests.Context;
            Tests.PopulateData(context, true);

            var service = new GameBetsService(context);

            // Act
            service.RemoveCoefficient(validIdTestValue, BetType.Game);
            service.RemoveCoefficient(validIdTestValue, BetType.PlayerGame);

            // Assert
            Assert.True(context.GameBetCoefficients.Find(validIdTestValue) == null);
            Assert.True(context.PlayerGameBetCoefficients.Find(validIdTestValue) == null);
        }

        [Fact]
        public void EditCoefficient_ShouldPutNewCoefficientInDb()
        {
            // Arrange
            var context = Tests.Context;
            Tests.PopulateData(context, true);

            var service = new GameBetsService(context);

            var gameTestCoefficient = context.GameBetCoefficients.Find(validIdTestValue).Coefficient;
            var playerTestCoefficeint = context.PlayerGameBetCoefficients.Find(validIdTestValue).Coefficient;

            var newGameCoefficient = gameTestCoefficient + 1;
            var newPlayerCoefficient = playerTestCoefficeint + 1;

            // Act
            service.EditCoefficient(validIdTestValue, newGameCoefficient, BetType.Game);
            service.EditCoefficient(validIdTestValue, newPlayerCoefficient, BetType.PlayerGame);

            // Assert
            Assert.True(context.GameBetCoefficients.Find(validIdTestValue).Coefficient == newGameCoefficient);
            Assert.True(context.PlayerGameBetCoefficients.Find(validIdTestValue).Coefficient == newPlayerCoefficient);
        }
    }
}
