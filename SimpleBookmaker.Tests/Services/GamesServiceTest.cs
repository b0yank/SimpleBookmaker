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

    public class GamesServiceTest : TestClass
    {
        private GamesService InitializeService()
        {
            var context = Tests.Context;
            Tests.PopulateData(context, true);

            var service = new GamesService(context);

            return service;
        }

        [Fact]
        public void Add_ShouldReturnFalseWhenInvalidTournamentOrTeamId()
        {
            // Arrange
            var service = this.InitializeService();
            const int teamNotInTournamentId = 2;

            // Act
            var invalidTournamentIdResult = service.Add(invalidIdTestValue, validIdTestValue, validIdTestValue + 1, DateTime.UtcNow);
            var invalidTeamIdResult = service.Add(validIdTestValue, invalidIdTestValue, validIdTestValue, DateTime.UtcNow);
            var teamNotInTournamentResult = service.Add(validIdTestValue, teamNotInTournamentId, validIdTestValue, DateTime.UtcNow);

            // Assert
            Assert.False(invalidTournamentIdResult);
            Assert.False(invalidTeamIdResult);
            Assert.False(teamNotInTournamentResult);
        }

        [Fact]
        public void Add_ShouldReturnTrueWhenValidInputData()
        {
            // Arrange
            var service = this.InitializeService();
            const int teamOneInValidTournamentId = 1;
            const int teamTwoInValidTournamentId = 5;

            // Act
            var result = service.Add(validIdTestValue, teamOneInValidTournamentId, teamTwoInValidTournamentId, DateTime.UtcNow);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Exists_ShouldReturnFalseWhenGameDoesNotExist()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.Exists(invalidIdTestValue);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Exists_ShouldReturnTrueWhenGameExists()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.Exists(validIdTestValue);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Edit_ShouldEditGameInDb()
        {
            // Arrange
            var context = Tests.Context;
            Tests.PopulateData(context, true);

            var service = new GamesService(context);

            var newGameKickoff = context.Games.Find(validIdTestValue).Time.AddDays(2);

            // Act
            service.Edit(validIdTestValue, newGameKickoff);

            // Assert
            Assert.True(context.Games.Find(validIdTestValue).Time == newGameKickoff);
        }

        [Fact]
        public void Upcoming_ShouldReturnEmptyCollectionWhenInvalidPageOrPageSize()
        {
            // Arrange
            var context = Tests.Context;
            Tests.PopulateData(context, true);

            var service = new GamesService(context);

            // Act
            var resultWithInvalidPage = service.Upcoming(invalidIdTestValue, validIdTestValue);
            var resultWithInvalidPageSize = service.Upcoming(validIdTestValue, invalidIdTestValue);

            // Assert
            Assert.True(resultWithInvalidPage.Count() == 0);
            Assert.True(resultWithInvalidPageSize.Count() == 0);
        }

        [Fact]
        public void Upcoming_ShouldReturnUpcomingTournamentGames()
        {
            // Arrange
            var context = Tests.Context;
            Tests.PopulateData(context, true);

            const int tournamentId = validIdTestValue;
            const int pageSize = 1;

            var service = new GamesService(context);

            // Act
            var result = service.Upcoming(validIdTestValue, pageSize, tournamentId);

            // Assert
            Assert.True(result.All(g => g.Kickoff > DateTime.UtcNow 
                && context.Games.Find(g.Id).TournamentId == tournamentId));

            Assert.True(result.Count() <= pageSize);
        }

        [Fact]
        public void Upcoming_ShouldReturnUpcomingGames()
        {
            // Arrange
            var context = Tests.Context;
            Tests.PopulateData(context, true);

            const int pageSize = 1;

            var service = new GamesService(context);

            // Act
            var result = service.Upcoming(validIdTestValue, pageSize);

            // Assert
            Assert.True(result.All(g => g.Kickoff > DateTime.UtcNow));

            Assert.True(result.Count() <= pageSize);
        }

        [Fact]
        public void GetGameTime_ShouldReturnNullWhenInvalidGame()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.GetGametime(invalidIdTestValue);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetGameTime_ShouldReturnGameTImeWhenValidGame()
        {
            // Arrange
            var context = Tests.Context;
            Tests.PopulateData(context, true);

            var service = new GamesService(context);

            // Act
            var result = service.GetGametime(validIdTestValue);

            Assert.True(result.Value == context.Games.Find(validIdTestValue).Time);
        }

        [Fact]
        public void UpcomingCount_ShouldReturnCorrectCountWithoutTournament()
        {
            // Arrange
            const int upcomingTestInputGamesCount = 5;

            var service = this.InitializeService();

            // Act
            var result = service.UpcomingCount();

            Assert.True(result == upcomingTestInputGamesCount);
        }

        [Fact]
        public void UpcomingCount_ShouldReturnCorrectCountWithTournament()
        {
            // Arrange
            const int upcomingTestInputGamesCount = 1;

            var service = this.InitializeService();

            // Act
            var result = service.UpcomingCount(validIdTestValue);

            Assert.True(result == upcomingTestInputGamesCount);
        }

        [Fact]
        public void ById_ShouldReturnModelWhenValidGame()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.ById(validIdTestValue);

            // Assert
            Assert.IsType<GameDetailedModel>(result);
        }

        [Fact]
        public void ById_ShouldReturnNullWhenInvalidGame()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.ById(invalidIdTestValue);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Remove_ShouldReturnFalseWhenInvalidGame()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.Remove(invalidIdTestValue);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Remove_ShouldReturnFalseWhenGameHasBets()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.Remove(validIdTestValue);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Remove_ShouldReturnTrue()
        {
            // Arrange
            var service = this.InitializeService();

            const int gameWithNoBetsId = 5;

            // Act
            var result = service.Remove(gameWithNoBetsId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetGameTeams_ShouldReturnNullWhenInvalidGame()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.GetGameTeams(invalidIdTestValue);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetGameTeams_ShouldReturnModelhenValidGame()
        {
            // Arrange
            var service = this.InitializeService();

            // Act
            var result = service.GetGameTeams(validIdTestValue);

            // Assert
            Assert.IsType<GameTeamsModel>(result);
        }
    }
}
