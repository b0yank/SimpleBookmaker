namespace SimpleBookmaker.Tests.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using SimpleBookmaker.Services;
    using Web.Controllers;
    using Xunit;

    public class BaseGamesController : TestClass
    {
        public BaseGamesController()
            : base()
        {
        }

        private GamesController InitializeController()
        {
            var context = Tests.Context;
            Tests.PopulateData(context);
            
            var userBetsServiceMock = new Mock<UserBetsService>(context, null);
            var gameBetsServiceMock = new Mock<GameBetsService>(context);

            return new GamesController(gameBetsServiceMock.Object, userBetsServiceMock.Object, new GamesService(context));
        }

        [Fact]
        public void View_ShouldReturnBadRequestWhenInvalidGameId()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.View(invalidIdTestValue);

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void View_ShouldReturnView()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.View(validIdTestValue);

            //Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}