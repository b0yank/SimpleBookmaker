namespace SimpleBookmaker.Web.Areas.Admin.Models.User
{
    using System.Collections.Generic;

    public class UserManageRoleModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public IEnumerable<string> RolesNotAssigned { get; set; }
    }
}
