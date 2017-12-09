namespace SimpleBookmaker.Web.Areas.Bookmaker.Models.Game
{
    using Services.Models.Game;
    using Services.Models.Tournament;
    using SimpleBookmaker.Web.Models;
    using System.Collections.Generic;

    public class BookieGameListPageModel : PageModel
    {
        public IEnumerable<GameListModel> Games { get; set; }

        public IEnumerable<TournamentListModel> Tournaments { get; set; }
    }
}
