namespace SimpleBookmaker.Web.Areas.Admin.Controllers
{
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.User;
    using Services.Contracts;
    using Services.Infrastructure;
    using SimpleBookmaker.Web.Infrastructure;
    using SimpleBookmaker.Web.Infrastructure.Filters;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class UsersController : AdminBaseController
    {
        private const int usersListPageSize = 20;

        private readonly UserManager<User> userManager;
        private readonly IUsersService users;

        public UsersController(IUsersService users, UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.users = users;
        }

        public async Task<IActionResult> All(int page = 1, string keyword = null)
        {
            var usersList = await this.users.AllAsync(page, usersListPageSize, keyword);

            var viewModel = new UserListPageModel
            {
                Users = usersList,
                CurrentPage = page,
                TotalPages = (int) Math.Ceiling(this.users.Count(keyword) / (double)usersListPageSize),
                RequestPath = "admin/users/all"
            };

            return View(viewModel);
        }
        
        [RestoreModelErrorsFromTempData]
        public async Task<IActionResult> ManageRoles(string id)
        {
            if (id == null)
            {
                return BadRequest(ErrorMessages.InvalidUser);
            }

            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                return BadRequest(ErrorMessages.InvalidUser);
            }
            
            var userRoles = await this.userManager.GetRolesAsync(user);

            var model = new UserManageRoleModel
            {
                Id = id,
                Email = user.Email,
                Roles = userRoles,
                RolesNotAssigned = Roles.All().Where(r => !userRoles.Contains(r))
            };

            return View(model);
        }
        
        [HttpPost]
        [SetTempDataModelErrors]
        public async Task<IActionResult> AddRole(UserEditRoleModel model)
        {
            var user = await this.userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                return BadRequest(ErrorMessages.InvalidUser);
            }

            var result = await this.userManager.AddToRoleAsync(user, model.Role);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("AddFailed", ErrorMessages.UserAddRoleFailed);
            }

            return this.RedirectToAction(nameof(ManageRoles), new { id = model.Id });
        }
        
        [HttpPost]
        [SetTempDataModelErrors]
        public async Task<IActionResult> RemoveRole(UserEditRoleModel model)
        {
            var user = await this.userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                return BadRequest(ErrorMessages.InvalidUser);
            }

            var result = await this.userManager.RemoveFromRoleAsync(user, model.Role);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("AddFailed", string.Format(ErrorMessages.InvalidRole, model.Role));
            }

            return this.RedirectToAction(nameof(ManageRoles), new { id = model.Id });
        }
    }
}