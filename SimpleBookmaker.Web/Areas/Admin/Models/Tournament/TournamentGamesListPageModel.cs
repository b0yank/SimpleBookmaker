namespace SimpleBookmaker.Web.Areas.Admin.Models.Tournament
{
    using Services.Models.Game;
    using SimpleBookmaker.Web.Models;
    using System.Collections.Generic;

    public class TournamentGamesListPageModel : PageModel
    {
        public int TournamentId { get; set; }

        public string Tournament { get; set; }

        public IEnumerable<GameListModel> Games { get; set; }
    }
}
