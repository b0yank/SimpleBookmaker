namespace SimpleBookmaker.Web.Areas.Admin.Models.Team
{
    using Services.Models.Team;
    using SimpleBookmaker.Web.Models;
    using System.Collections.Generic;

    public class TeamListPageModel : PageModel
    {
        public IEnumerable<TeamListModel> Teams { get; set; }
    }
}
