namespace SimpleBookmaker.Web.Areas.Admin.Models.Team
{
    using System.Collections.Generic;

    public class TeamListModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<string> Tournaments { get; set; }
    }
}
