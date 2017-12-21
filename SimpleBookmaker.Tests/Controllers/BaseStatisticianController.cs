namespace SimpleBookmaker.Tests.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using SimpleBookmaker.Services;
    using SimpleBookmaker.Services.Models.Game;
    using Xunit;
    using Web.Controllers;
    using SimpleBookmaker.Services.Models.Tournament;
    using SimpleBookmaker.Web.Models.StatisticianViewModels;

    public class BaseStatisticianController : TestClass
    {
        public BaseStatisticianController()
            : base()
        {
        }

        private StatisticianController InitializeController()
        {
            var context = Tests.Context;
            Tests.PopulateData(context);

            var gamesServiceMock = new Mock<StatisticianGamesService>(context, null, null);

            gamesServiceMock.Setup(gs => gs.ById(It.Is<int>(id => id <= 0)))
                .Returns(null as GameStatsModel);

            gamesServiceMock.Setup(gs => gs.ById(It.Is<int>(id => id > 0)))
                 .Returns(new GameStatsModel { Id = 1, HomeGoalscorers = "1 2 3", AwayGoalscorers = "4 5 6" });

            gamesServiceMock.Setup(gs => gs.Exists(It.Is<int>(id => id <= 0)))
                .Returns(false);

            gamesServiceMock.Setup(gs => gs.Exists(It.Is<int>(id => id > 0)))
                 .Returns(true);

            var tournamentsServiceMock = new Mock<StatisticianTournamentsService>(context, null);

            tournamentsServiceMock.Setup(ts => ts.ById(It.Is<int>(id => id <= 0)))
                .Returns(null as TournamentStatsSetModel);

            tournamentsServiceMock.Setup(ts => ts.ById(It.Is<int>(id => id > 0)))
                 .Returns(new TournamentStatsSetModel());

            tournamentsServiceMock.Setup(ts => ts.Exists(It.Is<int>(id => id <= 0)))
                .Returns(false);

            tournamentsServiceMock.Setup(ts => ts.Exists(It.Is<int>(id => id > 0)))
                 .Returns(true);

            tournamentsServiceMock.Setup(ts => ts.ResolveBets(It.Is<int>(tourId => tourId == invalidModelStateTestValue), It.IsAny<int>()))
                .Returns(false);

            tournamentsServiceMock.Setup(ts => ts.ResolveBets(It.Is<int>(tourId => tourId != invalidModelStateTestValue), It.Is<int>(teamId => teamId != invalidModelStateTestValue)))
                .Returns(true);

            return new StatisticianController(new PlayersService(context),
                new TeamsService(context),
                tournamentsServiceMock.Object,
                gamesServiceMock.Object);
        }

        [Fact]
        public void Index_ShouldReturnView()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void SetStats_ShouldReturnBadRequestWhenInvalidGameId()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.SetStats(invalidIdTestValue);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void SetStats_ShouldReturnViewWhenValid()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.SetStats(validIdTestValue);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void SetStats_Post_ShouldRedirectWithErrorWhenInvalidModelState()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new GameStatsModel
            {
                Id = 1,
                HomeGoalscorers = "1 2 3",
                AwayGoalscorers = "4 5 6"
            };

            // Act
            controller.ModelState.AddModelError("", "error!");
            var result = controller.SetStats(model);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void SetStats_Post_ShouldRedirectWithErrorWhenValid()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new GameStatsModel
            {
                Id = validIdTestValue,
                HomeGoalscorers = "1 2 3",
                AwayGoalscorers = "4 5 6"
            };

            // Act
            var result = controller.SetStats(model);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void SetTournament_ShouldReturnBadRequestWhenInvalidTournamentId()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.SetTournament(invalidIdTestValue);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void SetTournament_ShouldReturnViewWhenValid()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.SetTournament(validIdTestValue);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void SetTournament_Post_ShouldReturnBadRequestWhenInvalidTournamentId()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new TournamentStatsSaveModel
            {
                TournamentId = invalidIdTestValue,
                ChampionId = validIdTestValue
            };

            // Act
            var result = controller.SetTournament(model);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void SetTournament_Post_ShouldReturnBadRequestWhenInvalidChampionId()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new TournamentStatsSaveModel
            {
                TournamentId = validIdTestValue,
                ChampionId = invalidIdTestValue
            };

            // Act
            var result = controller.SetTournament(model);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void SetTournament_Post_ShouldReturnRedirectWithErrorWhenModelIsInvalid()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new TournamentStatsSaveModel
            {
                TournamentId = invalidModelStateTestValue,
                ChampionId = validIdTestValue
            };

            // Act
            var result = controller.SetTournament(model);

            // Assert
            Assert.True(controller.ModelState.Count > 0, "Should contain model state error");
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void SetTournament_Post_ShouldRedirectWhenSuccess()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new TournamentStatsSaveModel
            {
                TournamentId = validIdTestValue,
                ChampionId = validIdTestValue
            };

            // Act
            var result = controller.SetTournament(model);

            // Assert
            Assert.True(controller.ModelState.Count == 0, "Should not contain model state errors");
            Assert.IsType<RedirectToActionResult>(result);
        }
    }
}