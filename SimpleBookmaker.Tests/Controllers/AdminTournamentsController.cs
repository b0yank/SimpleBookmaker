namespace SimpleBookmaker.Tests.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using SimpleBookmaker.Services;
    using Web.Areas.Admin.Models.Tournament;
    using Web.Areas.Admin.Controllers;
    using Xunit;
    using System;
    using SimpleBookmaker.Services.Models.Team;
    using System.Collections.Generic;
    using SimpleBookmaker.Services.Models.Game;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using SimpleBookmaker.Services.Infrastructure;

    public class AdminTournamentsController : TestClass
    {
        public AdminTournamentsController()
            : base()
        {
        }

        private TournamentsController InitializeController()
        {
            var context = Tests.Context;
            Tests.PopulateData(context);

            var tournamentsServiceMock = new Mock<TournamentsService>(context);

            tournamentsServiceMock.Setup(ts => ts.Exists(It.Is<int>(id => id <= 0)))
                .Returns(false);

            tournamentsServiceMock.Setup(ts => ts.Exists(It.Is<int>(id => id > 0)))
                .Returns(true);

            tournamentsServiceMock.Setup(ts => ts.AddTeam(It.Is<int>(id => id == invalidModelStateTestValue), It.Is<int>(id => id == invalidModelStateTestValue)))
                .Returns(false);

            tournamentsServiceMock.Setup(ts => ts.AddTeam(It.Is<int>(id => id != invalidModelStateTestValue), It.Is<int>(id => id != invalidModelStateTestValue)))
                .Returns(true);

            tournamentsServiceMock.Setup(ts => ts.RemoveTeam(It.Is<int>(id => id == invalidModelStateTestValue), It.Is<int>(id => id == invalidModelStateTestValue)))
                .Returns(false);

            tournamentsServiceMock.Setup(ts => ts.RemoveTeam(It.Is<int>(id => id != invalidModelStateTestValue), It.Is<int>(id => id != invalidModelStateTestValue)))
                .Returns(true);

            tournamentsServiceMock.Setup(ts => ts.Add(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(true);

            tournamentsServiceMock.Setup(ts => ts.Add(It.Is<string>(name => name == "fail"), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(false);

            tournamentsServiceMock.Setup(ts => ts.AddTeams(It.IsAny<int>(), It.IsAny<IEnumerable<int>>()))
                .Returns(true);

            tournamentsServiceMock.Setup(ts => ts.GamesCount(It.IsAny<int>()))
                .Returns(5);

            tournamentsServiceMock.Setup(ts => ts.Games(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new List<GameListModel>
                {
                    new GameListModel { Id = 1, HomeTeam = "HomeTeam", AwayTeam = "AwayTeam", Kickoff = DateTime.MinValue },
                    new GameListModel { Id = 2, HomeTeam = "HomeTeam2", AwayTeam = "AwayTeam2", Kickoff = DateTime.MinValue }
                });

            tournamentsServiceMock.Setup(ts => ts.GetAvailableTeams(It.IsAny<int>()))
                .Returns(new List<BaseTeamModel>
                {
                    new BaseTeamModel { Id = 1, Name = "TeamOne" },
                    new BaseTeamModel { Id = 2, Name = "TeamTwo" }
                });

            return new TournamentsController(tournamentsServiceMock.Object);
        }

        [Fact]
        public void TournamentsController_ShouldHaveAuthorizationAttributeWithAdminRole()
        {
            // Arrange

            // Act
            var attributes = typeof(TournamentsController)
                .GetCustomAttributes(typeof(AuthorizeAttribute), true);

            // Assert
            Assert.True(attributes.Any(a => ((AuthorizeAttribute)a).Roles == Roles.Administrator));
        }

        [Fact]
        public void Add_Post_ShouldReturnView()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new TournamentAddModel
            {
                Name = "Name",
                StartDate = DateTime.UtcNow.AddDays(5),
                EndDate = DateTime.UtcNow.AddDays(15),
                Importance = 9
            };

            // Act
            controller.ModelState.AddModelError("", "Random error");

            var result = controller.Add(model);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Add_Post_ShouldReturnRedirectToAction()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new TournamentAddModel
            {
                Name = "Name",
                StartDate = DateTime.UtcNow.AddDays(5),
                EndDate = DateTime.UtcNow.AddDays(15),
                Importance = 9
            };

            // Act
            var result = controller.Add(model);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void Add_Post_ShouldReturnRedirectWithModelError()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new TournamentAddModel
            {
                Name = "fail",
                StartDate = DateTime.UtcNow.AddDays(5),
                EndDate = DateTime.UtcNow.AddDays(15),
                Importance = 9
            };

            // Act
            var result = controller.Add(model);

            // Assert
            Assert.True(controller.ModelState.Count > 0, "Should contain model state error");
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void AddTeam_ShouldReturnBadRequest()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.AddTeam(invalidIdTestValue);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void AddTeam_ShouldReturnView()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.AddTeam(validIdTestValue);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void AddTeam_Post_InvalidTournamentShouldReturnBadRequest()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new TournamentAddTeamBindingModel
            {
                TournamentId = invalidIdTestValue,
                Teams = new[] { 1, 2 }
            };

            // Act
            var result = controller.AddTeam(model);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void AddTeam_Post_InvalidTeamsShouldReturnBadRequest()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new TournamentAddTeamBindingModel
            {
                TournamentId = validIdTestValue,
                Teams = new[] { 1, 5 }
            };

            // Act
            var result = controller.AddTeam(model);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void AddTeam_Post_InvalidTeamsShouldReturnRedirectToAction()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new TournamentAddTeamBindingModel
            {
                TournamentId = validIdTestValue,
                Teams = new[] { 2, 3 }
            };

            // Act
            var result = controller.AddTeam(model);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
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
            var result = controller.Remove(validIdTestValue);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Games_ShouldReturnBadRequest()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.Games(invalidIdTestValue, validIdTestValue);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Games_ShouldReturnView()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.Games(validIdTestValue, validIdTestValue);

            // Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}
