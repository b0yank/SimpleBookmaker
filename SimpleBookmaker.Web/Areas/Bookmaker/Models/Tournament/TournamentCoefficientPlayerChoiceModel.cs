namespace SimpleBookmaker.Web.Areas.Bookmaker.Models.Tournament
{
    using Data.Core.Enums;
    using Services.Models.Player;
    using System.Collections.Generic;

    public class TournamentCoefficientPlayerChoiceModel
    {
        public int TeamId { get; set; } 

        public string Team { get; set; }

        public int TournamentId { get; set; } 

        public string Tournament { get; set; }

        public double Coefficient { get; set; }

        public TournamentBetType BetType { get; set; }

        public string BetCondition { get; set; }

        public IEnumerable<PlayerListModel> Players { get; set; }
    }
}
