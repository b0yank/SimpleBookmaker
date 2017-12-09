namespace SimpleBookmaker.Web.Areas.Bookmaker.Models.Tournament
{
    using Data.Core.Enums;
    using System.ComponentModel.DataAnnotations;

    public class SaveTournamentCoefficientModel
    {
        public int SubjectId { get; set; }

        public int TournamentId { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Coefficient must be larger than zero")]
        public double Coefficient { get; set; }

        public TournamentBetType BetType { get; set; }
    }
}
