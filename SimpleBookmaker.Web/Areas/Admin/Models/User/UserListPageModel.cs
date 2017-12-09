namespace SimpleBookmaker.Web.Areas.Admin.Models.User
{
    using Services.Models.User;
    using SimpleBookmaker.Web.Models;
    using System.Collections.Generic;

    public class UserListPageModel : PageModel
    {
        public IEnumerable<UserListModel> Users { get; set; }
    }
}
