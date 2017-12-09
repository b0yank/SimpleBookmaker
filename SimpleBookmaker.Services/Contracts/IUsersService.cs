namespace SimpleBookmaker.Services.Contracts
{
    using Services.Models.User;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUsersService : IService
    {
        Task<IEnumerable<UserListModel>> AllAsync(int page = 1, int pageSize = 20, string keyword = null);

        int Count(string keyword = null);
    }
}
