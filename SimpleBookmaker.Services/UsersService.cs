namespace SimpleBookmaker.Services
{
    using Contracts;
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Services.Models.User;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UsersService : IUsersService
    {
        private readonly SimpleBookmakerDbContext db;

        private readonly UserManager<User> userManager;

        public UsersService(SimpleBookmakerDbContext db, UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<UserListModel>> AllAsync(int page = 1, int pageSize = 20, string keyword = null)
        {
            var users = this.db.Users.AsQueryable();

            if (keyword != null)
            {
                var keywordLower = keyword.ToLower();

                users = this.UsersWithKeyword(users, keyword);
            }

            users = users
                .OrderBy(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var userList = new List<UserListModel>(); 
            foreach (var user in users)
            {
                var roles = await this.userManager.GetRolesAsync(user);

                userList.Add(new UserListModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Roles = roles
                });
            }

            return userList;
        }

        public int Count(string keyword = null)
        {
            if (keyword == null)
            {
                return this.db.Users.Count();
            }

            return this.UsersWithKeyword(this.db.Users.AsQueryable(), keyword).Count();
        }

        private IQueryable<User> UsersWithKeyword(IQueryable<User> users, string keyword)
        {
            var keywordLower = keyword.ToLower();

            return users.Where(u =>
                u.Email.ToLower().Contains(keywordLower)
                || u.Name.ToLower().Contains(keywordLower));
        }
    }
}
