namespace SimpleBookmaker.Web.Areas.Admin.Models.Tournament
{
    using Services.Models.Team;
    using System.Collections.Generic;

    public class TournamentAddTeamViewModel
    {
        public int TournamentId { get; set; }

        public string TournamentName { get; set; }

        public IEnumerable<BaseTeamModel> AvailableTeams { get; set; }
    }
}
