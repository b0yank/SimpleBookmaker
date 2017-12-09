namespace SimpleBookmaker.Web.Areas.Bookmaker.Models.Game
{
    using Services.Models.Bet;
    using Services.Models.Game;
    using System.Collections.Generic;

    public class SetBetsViewModel
    {
        public int GameId { get; set; }

        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }

        public IEnumerable<GameCoefficientListModel> ExistingCoefficients { get; set; }

        public IEnumerable<GamePossibleBetListModel> PossibleCoefficients { get; set; }
    }
}
