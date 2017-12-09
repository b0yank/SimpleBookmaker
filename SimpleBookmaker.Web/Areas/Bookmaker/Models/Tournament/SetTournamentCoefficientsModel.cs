namespace SimpleBookmaker.Web.Areas.Bookmaker.Models.Tournament
{
    using Services.Models.Bet;
    using Services.Models.Team;
    using System.Collections.Generic;

    public class SetTournamentCoefficientsModel
    {
        public int TournamentId { get; set; }

        public string Tournament { get; set; }

        public IEnumerable<TournamentCoefficientListModel> ExistingCoefficients { get; set; }

        public IEnumerable<TournamentPossibleCoefficientModel> PossibleCoefficients { get; set; }

        public IEnumerable<BaseTeamModel> Teams { get; set; }
    }
}
