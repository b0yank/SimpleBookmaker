namespace SimpleBookmaker.Tests.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using SimpleBookmaker.Services;
    using SimpleBookmaker.Web.Areas.Admin.Models.Game;
    using System;
    using Web.Areas.Admin.Controllers;
    using Xunit;
    using Microsoft.AspNetCore.Authorization;
    using System.Linq;
    using SimpleBookmaker.Services.Infrastructure;
    using SimpleBookmaker.Services.Models.Game;

    public class AdminGamesController : TestClass
    {
        public AdminGamesController()
            : base()
        {
        }

        private GamesController InitializeController()
        {
            var context = Tests.Context;
            Tests.PopulateData(context);

            var gamesServiceMock = new Mock<GamesService>(context);
            gamesServiceMock.Setup(gs => gs.Add(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(true);

            gamesServiceMock.Setup(gs => gs.ById(It.Is<int>(id => id > 0)))
                .Returns(new GameDetailedModel { Id = 1, Kickoff = DateTime.MinValue, TournamentId = 1 });


            var teamsService = new TeamsService(context);
            var tournamentsService = new TournamentsService(context);

           return new GamesController(gamesServiceMock.Object, teamsService, tournamentsService);
        }

        [Fact]
        public void GamesController_ShouldHaveAuthorizationAttributeWithAdminRole()
        {
            // Arrange

            // Act
            var attributes = typeof(GamesController)
                .GetCustomAttributes(typeof(AuthorizeAttribute), true);

            // Assert
            Assert.True(attributes.Any(a => ((AuthorizeAttribute)a).Roles == Roles.Administrator));
        }

        [Fact]
        public void Add_ShouldReturnBadRequest()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.Add(invalidIdTestValue);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Add_ShouldReturnView()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.Add(validIdTestValue);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Add_PostShouldReturnViewWithError()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new GameAddModel
            {
                TournamentId = 1,
                Date = DateTime.UtcNow.AddMonths(2),
                Time = DateTime.UtcNow.AddMonths(2),
                HomeTeamId = 2,
                AwayTeamId = 3
            };

            controller.ModelState.Clear();

            // Act
            var result = controller.Add(model);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.True(controller.ModelState.Count == 1, "Game must be held within tournament times");
        }

        [Fact]
        public void Add_PostShouldReturnViewWithoutError()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new GameAddModel
            {
                TournamentId = 1,
                Date = DateTime.UtcNow.AddDays(6),
                Time = DateTime.UtcNow.AddDays(6),
                HomeTeamId = 1,
                AwayTeamId = 2
            };

            controller.ModelState.Clear();

            // Act
            var result = controller.Add(model);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.True(controller.ModelState.Count == 0, "Should not contain model state errors");
        }

        [Fact]
        public void Remove_ShouldReturnBadRequest()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.Remove(invalidIdTestValue);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Remove_ShouldReturnView()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.Remove(1);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Remove_PostShouldReturnViewWithError()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new GameDestroyModel
            {
                Id = invalidIdTestValue,
                TournamentId = invalidIdTestValue
            };

            // Act
            var result = controller.Remove(model);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.True(controller.ModelState.Count == 1, "Game model must be validated");
        }

        [Fact]
        public void Edit_ShouldReturnBadRequest()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.Edit(invalidIdTestValue);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Edit_ShouldReturnView()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.Edit(validIdTestValue);

            // Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}
