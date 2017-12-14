namespace SimpleBookmaker.Web.Models.TournamentViewModels
{
    using Services.Models.Bet;
    using Services.Models.Game;
    using System.Collections.Generic;

    public class TournamentCoefficientsViewModel : PageModel
    {
        public string Tournament { get; set; }

        public IEnumerable<TournamentCoefficientListModel> Coefficients { get; set; }

        public IEnumerable<GameListModel> UpcomingGames { get; set; }
    }
}
