namespace SimpleBookmaker.Tests.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using SimpleBookmaker.Services;
    using Web.Areas.Admin.Controllers;
    using Microsoft.AspNetCore.Identity;
    using SimpleBookmaker.Data.Models;
    using Xunit;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using SimpleBookmaker.Services.Infrastructure;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using SimpleBookmaker.Web.Areas.Admin.Models.User;

    public class AdminUsersController : TestClass
    {
        public AdminUsersController()
            : base()
        {
        }

        private UsersController InitializeController()
        {
            var context = Tests.Context;
            Tests.PopulateData(context);

            var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

            var usersServiceMock = new Mock<UsersService>(context, userManagerMock.Object);

            userManagerMock.Setup(um => um.FindByIdAsync(It.Is<string>(s => s == "invalid")))
                .Returns(Task.FromResult<User>(null));

            userManagerMock.Setup(um => um.FindByIdAsync(It.Is<string>(s => s != "invalid")))
                .Returns(Task.FromResult(new User { Id = "404", Email = "user@user.bg", Name = "Name", UserName = "Username" }));

            userManagerMock.Setup(um => um.GetRolesAsync(It.IsAny<User>()))
                .Returns(Task.FromResult<IList<string>>(new List<string> { "Administrator", "Bookmaker" }));

            userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<User>(), It.Is<string>(role => role != "invalid")))
                .Returns(Task.FromResult<IdentityResult>(new FakeIdentitySuccessResult()));

            userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<User>(), It.Is<string>(role => role == "invalid")))
                .Returns(Task.FromResult<IdentityResult>(new FakeIdentityFailResult()));

            userManagerMock.Setup(um => um.RemoveFromRoleAsync(It.IsAny<User>(), It.Is<string>(role => role != "invalid")))
                .Returns(Task.FromResult<IdentityResult>(new FakeIdentitySuccessResult()));

            userManagerMock.Setup(um => um.RemoveFromRoleAsync(It.IsAny<User>(), It.Is<string>(role => role == "invalid")))
                .Returns(Task.FromResult<IdentityResult>(new FakeIdentityFailResult()));

            return new UsersController(usersServiceMock.Object, userManagerMock.Object);
        }

        [Fact]
        public void UsersController_ShouldHaveAuthorizationAttributeWithAdminRole()
        {
            // Arrange

            // Act
            var attributes = typeof(UsersController)
                .GetCustomAttributes(typeof(AuthorizeAttribute), true);

            // Assert
            Assert.True(attributes.Any(a => ((AuthorizeAttribute)a).Roles == Roles.Administrator));
        }

        [Fact]
        public async void ManageRoles_ShouldReturnBadRequestWhenIdIsNull()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = await controller.ManageRoles(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void ManageRoles_ShouldReturnBadRequestWhenInvalidUser()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = await controller.ManageRoles("invalid");

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void ManageRoles_ShouldReturnView()
        {
            // Arrange
            var controller = this.InitializeController();

            // Act
            var result = await controller.ManageRoles("any ol' user");

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void AddRole_Post_ShouldReturnBadRequestWhenInvalidUser()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new UserEditRoleModel
            {
                Id = "invalid",
                Role = "don' matta"
            };

            // Act
            var result = await controller.AddRole(model);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void AddRole_Post_ShouldReturnRedirectWithErrorWhenInvalidRole()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new UserEditRoleModel
            {
                Id = "any",
                Role = "invalid"
            };

            // Act
            var result = await controller.AddRole(model);

            // Assert
            Assert.True(controller.ModelState.Count > 0, "Should contain model state error");
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async void AddRole_Post_ShouldReturnRedirectWithoutErrorWhenSuccess()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new UserEditRoleModel
            {
                Id = "any",
                Role = "any"
            };

            // Act
            var result = await controller.AddRole(model);

            // Assert
            Assert.True(controller.ModelState.Count == 0, "Should contain model state error");
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async void RemoveRole_Post_ShouldReturnBadRequestWhenInvalidUser()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new UserEditRoleModel
            {
                Id = "invalid",
                Role = "don' matta"
            };

            // Act
            var result = await controller.RemoveRole(model);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void RemoveRole_Post_ShouldReturnRedirectWithErrorWhenInvalidRole()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new UserEditRoleModel
            {
                Id = "any",
                Role = "invalid"
            };

            // Act
            var result = await controller.RemoveRole(model);

            // Assert
            Assert.True(controller.ModelState.Count > 0, "Should contain model state error");
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async void RemoveRole_Post_ShouldReturnRedirectWithoutErrorWhenSuccess()
        {
            // Arrange
            var controller = this.InitializeController();

            var model = new UserEditRoleModel
            {
                Id = "any",
                Role = "any"
            };

            // Act
            var result = await controller.RemoveRole(model);

            // Assert
            Assert.True(controller.ModelState.Count == 0, "Should contain model state error");
            Assert.IsType<RedirectToActionResult>(result);
        }
    }

    internal class FakeIdentitySuccessResult : IdentityResult
    {
        public FakeIdentitySuccessResult()
        {
            this.Succeeded = true;
        }
    }

    internal class FakeIdentityFailResult : IdentityResult
    {
        public FakeIdentityFailResult()
        {
            this.Succeeded = false;
        }
    }
}