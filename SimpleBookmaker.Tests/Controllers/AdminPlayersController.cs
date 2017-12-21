namespace SimpleBookmaker.Tests.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Moq;
    using SimpleBookmaker.Services;
    using SimpleBookmaker.Services.Infrastructure;
    using SimpleBookmaker.Web.Areas.Admin.Models.Player;
    using System.Linq;
    using Web.Areas.Admin.Controllers;
    using Xunit;

    public class AdminPlayersController : TestClass
    {
        public AdminPlayersController()
            : base()
        {
        }

        private PlayersController InitializeController()
        {
            var context = Tests.Context;
            Tests.PopulateData(context);

            var teamsService = new TeamsService(context);
            var playersServiceMock = new Mock<PlayersService>(context);

            playersServiceMock.Setup(ps => ps.Create(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));

            playersServiceMock.Setup(ps => ps.Remove(It.Is<int>(id => id <= 0)))
                .Returns(false);

            playersServiceMock.Setup(ps => ps.Remove(It.Is<int>(id => id > 0)))
                .Returns(true);

            playersServiceMock.Setup(ps => ps.AddToTeam(It.Is<int>(id => id <= 0), It.Is<int>(id => id <= 0)))
                .Returns(false);

            playersServiceMock.Setup(ps => ps.AddToTeam(It.Is<int>(id => id > 0), It.Is<int>(id => id > 0)))
                .Returns(true);

            playersServiceMock.Setup(ps => ps.RemoveFromTeam(It.Is<int>(id => id <= 0)))
                .Returns(false);

            playersServiceMock.Setup(ps => ps.RemoveFromTeam(It.Is<int>(id => id > 0)))
                .Returns(true);

            return new PlayersController(teamsService, playersServiceMock.Object);
        }

        [Fact]
        public void PlayersController_ShouldHaveAuthorizationAttributeWithAdminRole()
        {
            // Arrange

            // Act
            var attributes = typeof(PlayersController)
                .GetCustomAttributes(typeof(AuthorizeAttribute), true);

            // Assert
            Assert.True(attributes.Any(a => ((AuthorizeAttribute)a).Roles == Roles.Administrator));
        }

        [Fact]
        public void All_ShouldReturnBadRequest()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.All(invalidIdTestValue);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void All_ShouldReturnView()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.All(validIdTestValue);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_ShouldReturnBadRequest()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.Create(invalidIdTestValue);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Create_ShouldReturnView()
        {
            // Arrange
            var controller = this.InitializeController();

            var featureCollection = new FeatureCollection();
            var httpContextMock = new Mock<DefaultHttpContext>(featureCollection);
            var tempDataProviderMock = new Mock<SessionStateTempDataProvider>();

            var tempDataDictionaryMock = new Mock<TempDataDictionary>(httpContextMock.Object, tempDataProviderMock.Object);
            controller.TempData = tempDataDictionaryMock.Object;

            // Act
            var result = controller.Create(validIdTestValue);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_Post_ShouldReturnView()
        {
            // Arrange
            var controller = this.InitializeController();

            var featureCollection = new FeatureCollection();
            var httpContextMock = new Mock<DefaultHttpContext>(featureCollection);
            var tempDataProviderMock = new Mock<SessionStateTempDataProvider>();

            var tempDataDictionaryMock = new Mock<TempDataDictionary>(httpContextMock.Object, tempDataProviderMock.Object);
            controller.TempData = tempDataDictionaryMock.Object;

            controller.ModelState.AddModelError("", "Random error");
            var model = new PlayerAddModel
            {
                Age = -50,
                Name = "Tench0",
                TeamId = 1
            };

            // Act
            var result = controller.Create(model);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_Post_ShouldReturnRedirectToAction()
        {
            // Arrange
            var controller = this.InitializeController();

            var featureCollection = new FeatureCollection();
            var httpContextMock = new Mock<DefaultHttpContext>(featureCollection);
            var tempDataProviderMock = new Mock<SessionStateTempDataProvider>();

            var tempDataDictionaryMock = new Mock<TempDataDictionary>(httpContextMock.Object, tempDataProviderMock.Object);
            controller.TempData = tempDataDictionaryMock.Object;

            var model = new PlayerAddModel
            {
                Age = 50,
                Name = "Tencho",
                TeamId = 1
            };

            // Act
            var result = controller.Create(model);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void Remove_Post_ShouldRedirectWithError()
        {
            // Arrange
            var controller = this.InitializeController();

            var featureCollection = new FeatureCollection();
            var httpContextMock = new Mock<DefaultHttpContext>(featureCollection);
            var tempDataProviderMock = new Mock<SessionStateTempDataProvider>();

            var tempDataDictionaryMock = new Mock<TempDataDictionary>(httpContextMock.Object, tempDataProviderMock.Object);
            controller.TempData = tempDataDictionaryMock.Object;

            // Act
            var result = controller.Remove(invalidIdTestValue);

            // Assert
            Assert.True(controller.ModelState.Count > 0, "Should contain model state error");
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void Remove_Post_ShouldRedirectWithoutError()
        {
            // Arrange
            var controller = this.InitializeController();

            var featureCollection = new FeatureCollection();
            var httpContextMock = new Mock<DefaultHttpContext>(featureCollection);
            var tempDataProviderMock = new Mock<SessionStateTempDataProvider>();

            var tempDataDictionaryMock = new Mock<TempDataDictionary>(httpContextMock.Object, tempDataProviderMock.Object);
            controller.TempData = tempDataDictionaryMock.Object;

            // Act
            var result = controller.Remove(2);

            // Assert
            Assert.True(controller.ModelState.Count == 0, "Should not contain model state errors");
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void AddToTeam_ShouldReturnBadRequest()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.AddToTeam(invalidIdTestValue);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void AddToTeam_ShouldReturnView()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.AddToTeam(validIdTestValue);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void AddToTeam_Post_ShouldReturnBadRequest()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new PlayerEditTeamModel
            {
                PlayerId = invalidIdTestValue,
                TeamId = invalidIdTestValue
            };

            // Act
            var result = controller.AddToTeam(model);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void AddToTeam_Post_ShouldReturnRedirectToAction()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new PlayerEditTeamModel
            {
                PlayerId = validIdTestValue,
                TeamId = validIdTestValue
            };

            // Act
            var result = controller.AddToTeam(model);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void RemoveFromTeam_Post_ShouldRedirectWithError()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new PlayerEditTeamModel
            {
                PlayerId = invalidIdTestValue,
                TeamId = invalidIdTestValue
            };

            // Act
            var result = controller.RemoveFromTeam(model);

            // Assert
            Assert.True(controller.ModelState.Count > 0, "Should contain model state error");
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void RemoveFromTeam_Post_ShouldReturnRedirectToAction()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new PlayerEditTeamModel
            {
                PlayerId = validIdTestValue,
                TeamId = validIdTestValue
            };

            // Act
            var result = controller.RemoveFromTeam(model);

            // Assert
            Assert.True(controller.ModelState.Count == 0, "Should contain model state error");
            Assert.IsType<RedirectToActionResult>(result);
        }
    }
}
