namespace SimpleBookmaker.Tests.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using SimpleBookmaker.Services;
    using SimpleBookmaker.Services.Infrastructure;
    using System.Linq;
    using Web.Areas.Admin.Controllers;
    using Web.Areas.Admin.Models;
    using Xunit;

    public class AdminTeamsController : TestClass
    {
        public AdminTeamsController()
            : base()
        {
        }

        private TeamsController InitializeController()
        {
            var context = Tests.Context;
            Tests.PopulateData(context);

            var teamsServiceMock = new Mock<TeamsService>(context);

            teamsServiceMock.Setup(ts => ts.Add(It.Is<string>(name => name != null)))
                .Returns(true);

            teamsServiceMock.Setup(ts => ts.Add(It.Is<string>(name => name == null)))
                .Returns(false);

            teamsServiceMock.Setup(ts => ts.Exists(It.Is<int>(id => id > 0)))
                .Returns(true);

            teamsServiceMock.Setup(ts => ts.Exists(It.Is<int>(id => id <= 0)))
                .Returns(false);

            teamsServiceMock.Setup(ts => ts.Remove(It.Is<int>(id => id > 0)))
                .Returns(true);

            teamsServiceMock.Setup(ts => ts.Remove(It.Is<int>(id => id <= 0 || id == invalidModelStateTestValue)))
                .Returns(false);

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

            return new TeamsController(teamsServiceMock.Object, tournamentsServiceMock.Object);
        }

        [Fact]
        public void TeamsController_ShouldHaveAuthorizationAttributeWithAdminRole()
        {
            // Arrange

            // Act
            var attributes = typeof(TeamsController)
                .GetCustomAttributes(typeof(AuthorizeAttribute), true);

            // Assert
            Assert.True(attributes.Any(a => ((AuthorizeAttribute)a).Roles == Roles.Administrator));
        }

        [Fact]
        public void Add_Post_ShouldReturnViewWithErrors()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new TeamAddModel
            {
                Name = null
            };

            // Act
            var result = controller.Add(model);

            // Assert
            Assert.True(controller.ModelState.Count > 0, "Should contain model state error");
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Add_Post_ShouldReturnRedirectToAction()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new TeamAddModel
            {
                Name = "Ime"
            };

            // Act
            var result = controller.Add(model);

            // Assert
            Assert.True(controller.ModelState.Count == 0, "Should not contain model state errors");
            Assert.IsType<RedirectToActionResult>(result);
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
            var result = controller.Edit(1);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void AddTournament_ShouldReturnBadRequest()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.AddTournament(invalidIdTestValue, invalidIdTestValue);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void AddTournament_ShouldReturnBadRequestWhenAddTeamFails()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.AddTournament(invalidModelStateTestValue, invalidModelStateTestValue);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void AddTournament_ShouldReturnRedirectToAction()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.AddTournament(2, 2);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void RemoveTournament_ShouldReturnBadRequest()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.RemoveTournament(invalidIdTestValue, invalidIdTestValue);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void RemoveTournament_ShouldReturnBadRequestWhenRemoveTeamFails()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.RemoveTournament(invalidModelStateTestValue, invalidModelStateTestValue);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void RemoveTournament_ShouldReturnRedirectToAction()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.RemoveTournament(2, 2);

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
            var result = controller.Remove(1);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Destroy_ShouldReturnBadRequest()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.Destroy(invalidIdTestValue);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Destroy_ShouldReturnRedirectWithError()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.Destroy(invalidModelStateTestValue);

            // Assert
            Assert.True(controller.ModelState.Count > 0, "Should contain model error");
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void Destroy_ShouldReturnRedirectWithoutError()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.Destroy(2);

            // Assert
            Assert.True(controller.ModelState.Count == 0, "Should not contain model errors");
            Assert.IsType<RedirectToActionResult>(result);
        }
    }
}
