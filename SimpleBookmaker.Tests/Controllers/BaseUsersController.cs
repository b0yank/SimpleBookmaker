namespace SimpleBookmaker.Tests.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using SimpleBookmaker.Services;
    using Web.Controllers;
    using Xunit;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Text;
    using SimpleBookmaker.Services.Models.Bet;
    using SimpleBookmaker.Services.Models.UserCoefficient;

    public class BaseUsersController : TestClass
    {
        public BaseUsersController()
            : base()
        {
        }

        private UsersController InitializeController()
        {
            var context = Tests.Context;
            Tests.PopulateData(context);

            var userBetsMock = new Mock<UserBetsService>(context, null);

            userBetsMock.Setup(ub => ub.PlaceBets(It.IsAny<IEnumerable<BetUnconfirmedModel>>(), It.Is<double>(a => a == invalidModelStateTestValue), It.IsAny<string>()))
                .Returns(false);

            userBetsMock.Setup(ub => ub.PlaceBets(It.IsAny<IEnumerable<BetUnconfirmedModel>>(), It.Is<double>(a => a != invalidModelStateTestValue), It.IsAny<string>()))
                .Returns(true);

            userBetsMock.Setup(ub => ub.CurrentBets(It.IsAny<string>()))
                .Returns(Task.FromResult<IEnumerable<UserCurrentBetSlipModel>>(new List<UserCurrentBetSlipModel>()));

            userBetsMock.Setup(ub => ub.UserHistory(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult<IEnumerable<UserHistoryBetSlipModel>>(new List<UserHistoryBetSlipModel>()));

            userBetsMock.Setup(ub => ub.UserHistoryCount(It.IsAny<string>()))
                .Returns(Task.FromResult(validIdTestValue));

            var usersMock = new Mock<UsersService>(context, null);

            var mockContext = new Mock<HttpContext>();
            mockContext.SetupGet(hc => hc.User.Identity.Name).Returns("Username");
            mockContext.SetupGet(hc => hc.Session).Returns(new FakeSession());

            var controller = new UsersController(userBetsMock.Object, usersMock.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = mockContext.Object
            };

            return controller;
        }

        [Fact]
        public void Profile_ShouldReturnView()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.Profile();

            //Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Slip_ShouldReturnView()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.Slip();

            //Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void Current_ShouldReturnView()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = await controller.Current();

            //Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void PlaceBets_ShouldReturnRedirectWithModelStateError()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.PlaceBets(invalidModelStateTestValue);

            //Assert
            Assert.True(controller.ModelState.Count > 0, "Should have model state error");
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void PlaceBets_ShouldReturnRedirectWithoutModelStateError()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.PlaceBets(invalidModelStateTestValue + 1);

            //Assert
            Assert.True(controller.ModelState.Count == 0, "Should not have model state errors");
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void RemoveBet_Post_ShouldReturnRedirectToAction()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = controller.RemoveBet(validIdTestValue, Data.Core.Enums.BetType.Game);

            //Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async void History_ShouldReturnView()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = await controller.History();

            //Assert
            Assert.IsType<ViewResult>(result);
        }
    }

    internal class FakeSession : ISession
    {
        private IDictionary<string, string> keyValuePairs = new Dictionary<string, string>();

        public bool IsAvailable => throw new System.NotImplementedException();

        public string Id => "fake id";

        public IEnumerable<string> Keys => this.keyValuePairs.Keys;

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public Task LoadAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public void Remove(string key)
        {
            this.keyValuePairs.Remove(key);
        }

        public void Set(string key, byte[] value)
        {
            var stringValue = Encoding.UTF8.GetString(value);

            this.keyValuePairs.Add(key, stringValue);
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            if (this.keyValuePairs.ContainsKey(key))
            {
                value = Encoding.UTF8.GetBytes(this.keyValuePairs[key]);

                return true;
            }

            value = null;
            return false;
        }
    }
}