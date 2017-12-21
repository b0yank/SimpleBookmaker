namespace SimpleBookmaker.Tests.Services
{
    using Data.Core.Enums;
    using SimpleBookmaker.Services;
    using SimpleBookmaker.Services.Models.Bet;
    using SimpleBookmaker.Services.Models.Game;
    using SimpleBookmaker.Services.Models.UserCoefficient;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Xunit;

    public class PlayersServiceTest : TestClass
    {
        private PlayersService InitializeService()
        {
            var context = Tests.Context;
            Tests.PopulateData(context, true);

            var service = new PlayersService(context);

            return service;
        }

        [Fact]
        public void ByTeam_ById_ShouldReturnEmptyCollectionWhenInvalidTeamId()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.ByTeam(invalidIdTestValue);

            // Assert
            Assert.True(result.Count() == 0);
        }

        [Fact]
        public void ByTeam_ById_ShouldReturAllTeamPlayersWhenValidTeamId()
        {
            // Arrange
            var service = this.InitializeService();

            var playersInTeamIds = new int[] { 1, 2, 3 };

            // Act
            var result = service.ByTeam(validIdTestValue);

            // Assert
            Assert.True(result.Count() == playersInTeamIds.Length);

            foreach (var playerId in playersInTeamIds)
            {
                Assert.Single(result.Where(p => p.Id == playerId));
            }
        }

        [Fact]
        public void ByTeam_ByName_ShouldReturnEmptyCollectionWhenInvalidTeamId()
        {
            // Arrange
            var service = this.InitializeService();

            var invalidTeamName = "invalid name";

            // Act
            var result = service.ByTeam(invalidTeamName);

            // Assert
            Assert.True(result.Count() == 0);
        }

        [Fact]
        public void ByTeam_ByName_ShouldReturAllTeamPlayersWhenValidTeamName()
        {
            // Arrange
            var service = this.InitializeService();

            var validTeamName = "Real Mavrud";
            var playersInTeamIds = new int[] { 1, 2, 3 };

            // Act
            var result = service.ByTeam(validTeamName);

            // Assert
            Assert.True(result.Count() == playersInTeamIds.Length);

            foreach (var playerId in playersInTeamIds)
            {
                Assert.Single(result.Where(p => p.Id == playerId));
            }
        }

        [Fact]
        public void Exists_ShouldReturnFalseWhenInvalidPlayer()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.Exists(invalidIdTestValue);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Exists_ShouldReturnTrueWhenValidPlayer()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.Exists(validIdTestValue);

            // Assert
            Assert.True(result);
        }


        [Fact]
        public void Remove_ShouldReturnFalseWhenInvalidPlayer()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.Remove(invalidIdTestValue);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Remove_ShouldReturnFalseWhenPlayerHasBets()
        {
            // Arrange
            var service = this.InitializeService();

            const int playerWithGameBetsId = 1;
            const int playerWithTournamentBetsId = 3;

            // Act
            var resultOne = service.Remove(playerWithGameBetsId);
            var resultTwo = service.Remove(playerWithTournamentBetsId);

            // Assert
            Assert.False(resultOne);
        }

        [Fact]
        public void Remove_ShouldReturnTruehenPlayerHasNoBets()
        {
            // Arrange
            var service = this.InitializeService();

            const int playerWithNoGameBetsId = 10;

            // Act
            var result = service.Remove(playerWithNoGameBetsId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Create_ShouldAddPlayerToDb()
        {
            // Arrange
            var context = Tests.Context;
            Tests.PopulateData(context, true);

            var service = new PlayersService(context);
            
            var playerToAddName = "Spas";
            var playerToAddAge = 25;

            var teamPlayersCount = context.Teams.Find(validIdTestValue).Players.Count;

            // Act
            service.Create(playerToAddName, playerToAddAge, validIdTestValue);

            // Assert
            Assert.True(context.Teams.Find(validIdTestValue).Players.Count == teamPlayersCount + 1);

            Assert.True(context.Teams.Find(validIdTestValue).Players.Any(p => p.Name == playerToAddName && p.Age == playerToAddAge));
        }

        [Fact]
        public void AddToTeam_ShouldReturnFalseWhenInvalidPlayerOrTeamId()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var resultWithInvalidPlayerId = service.AddToTeam(invalidIdTestValue, validIdTestValue);
            var resultWithInvalidTeamId = service.AddToTeam(validIdTestValue, invalidIdTestValue);

            // Assert
            Assert.False(resultWithInvalidPlayerId);
            Assert.False(resultWithInvalidTeamId);
        }

        [Fact]
        public void AddToTeam_ShouldReturnTrueWhenInputIsValid()
        {
            // Arrange
            var service = this.InitializeService();
            var playerNotInTeamId = 4;


            // Act
            var result = service.AddToTeam(playerNotInTeamId, validIdTestValue);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void RemoveFromTeam_ShouldReturnFalseWhenInvalidPlayer()
        {
            // Arrange
            var service = this.InitializeService();


            // Act
            var result = service.RemoveFromTeam(invalidIdTestValue);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RemoveFromTeam_ShouldReturnFalseWhenPlayerHasBets()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.RemoveFromTeam(validIdTestValue);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RemoveFromTeam_ShouldReturnTrueAndRemovePlayerFromTeamInDb()
        {
            // Arrange
            var context = Tests.Context;
            Tests.PopulateData(context, true);

            var service = new PlayersService(context);

            const int playerWithoutGameBetsId = 3;

            // Act
            var result = service.RemoveFromTeam(playerWithoutGameBetsId);

            // Assert
            Assert.Null(context.Players.Find(playerWithoutGameBetsId).TeamId);
            Assert.True(result);
        }

        [Fact]
        public void Unassigned_ShouldReturnCorrectUnassignedPlayers()
        {
            // Arrange
            var context = Tests.Context;
            Tests.PopulateData(context, true);

            var service = new PlayersService(context);

            // Act
            var result = service.Unassigned();

            var unassignedPlayers = context.Players.Where(p => p.TeamId == null);

            // Assert
            Assert.True(result.Count() == unassignedPlayers.Count());

            foreach (var player in unassignedPlayers)
            {
                Assert.Single(result.Where(p => p.Id == player.Id && p.Name == player.Name));
            }
        }
    }
}
