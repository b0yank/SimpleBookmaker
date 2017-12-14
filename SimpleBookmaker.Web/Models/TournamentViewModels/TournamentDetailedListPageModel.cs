namespace SimpleBookmaker.Web.Models.TournamentViewModels
{
    using Services.Models.Tournament;
    using System.Collections.Generic;

    public class TournamentDetailedListPageModel : PageModel
    {
        public IEnumerable<TournamentDetailedListModel> Tournaments { get; set; }
    }
}
