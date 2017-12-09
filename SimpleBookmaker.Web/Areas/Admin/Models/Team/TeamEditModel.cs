namespace SimpleBookmaker.Web.Areas.Admin.Models.Team
{
    using SimpleBookmaker.Services.Models.Tournament;
    using System.Collections.Generic;

    public class TeamEditModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<TournamentListModel> CompetingTournaments { get; set; }

        public IEnumerable<TournamentListModel> NotCompetingTournaments { get; set; }
    }
}
