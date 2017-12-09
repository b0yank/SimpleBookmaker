namespace SimpleBookmaker.Web.Areas.Admin.Models.Tournament
{
    using System.Collections.Generic;

    public class TournamentAddTeamBindingModel
    {
        public int TournamentId { get; set; }
        
        public IEnumerable<int> Teams { get; set; }
    }
}
