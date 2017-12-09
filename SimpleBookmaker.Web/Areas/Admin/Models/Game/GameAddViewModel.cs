namespace SimpleBookmaker.Web.Areas.Admin.Models.Game
{
    using Services.Models.Team;
    using System.Collections.Generic;

    public class GameAddViewModel
    {
        public int TournamentId { get; set; }

        public string Tournament { get; set; }

        public IEnumerable<BaseTeamModel> Teams { get; set; }
    }
}
