namespace SimpleBookmaker.Web.Models.GameViewModels
{
    using Services.Models.Bet;
    using Services.Models.UserCoefficient;
    using System;
    using System.Collections.Generic;

    public class GameCoefficientsViewModel
    {
        public IEnumerable<UserGameCoefficientModel> GameCoefficients { get; set; }

        public IEnumerable<GamePlayerCoefficientListModel> PlayerCoefficients { get; set; }

        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }

        public DateTime GameTime { get; set; }
    }
}
