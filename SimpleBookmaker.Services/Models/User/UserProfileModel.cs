namespace SimpleBookmaker.Services.Models.User
{
    using Data.Models;
    using Infrastructure.AutoMapper;

    public class UserProfileModel : IMapFrom<User>
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public decimal Balance { get; set; }
    }
}
