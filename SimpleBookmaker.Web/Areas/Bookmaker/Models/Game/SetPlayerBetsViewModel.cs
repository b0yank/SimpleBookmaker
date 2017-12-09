namespace SimpleBookmaker.Web.Areas.Bookmaker.Models.Game
{
    using Services.Models.Player;
    using Services.Models.Bet;
    using System.Collections.Generic;

    public class SetPlayerBetsViewModel
    {
        public int GameId { get; set; }

        public IEnumerable<GamePlayerPossibleBetListModel> PossibleCoefficients { get; set; }
        public IEnumerable<GamePlayerCoefficientListModel> ExistingCoefficients { get; set; }

        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }

        public IEnumerable<PlayerListModel> HomePlayers { get; set; }
        public IEnumerable<PlayerListModel> AwayPlayers { get; set; }
    }
}
