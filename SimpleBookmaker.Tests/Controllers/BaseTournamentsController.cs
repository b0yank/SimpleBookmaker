namespace SimpleBookmaker.Tests.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using SimpleBookmaker.Services;
    using Web.Controllers;
    using Xunit;

    public class BaseTournamentsController : TestClass
    {
        public BaseTournamentsController()
            : base()
        {
        }

        private TournamentsController InitializeController()
        {
            var context = Tests.Context;
            Tests.PopulateData(context);
            
            var tournamentBetsServiceMock = new Mock<TournamentBetsService>(context);

            return new TournamentsController(new GamesService(context), tournamentBetsServiceMock.Object, new TournamentsService(context));
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