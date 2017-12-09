namespace SimpleBookmaker.Services.Models.User
{
    using Data.Models;
    using Services.Infrastructure.AutoMapper;
    using System.Collections.Generic;

    public class UserListModel : IMapFrom<User>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}
