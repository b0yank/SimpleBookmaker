namespace SimpleBookmaker.Web.Areas.Admin.Models.Game
{
    using Data.Validation.Attributes;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class GameAddModel
    {
        public int TournamentId { get; set; }

        [Display(Name = "Home Team")]
        public int HomeTeamId { get; set; }

        [NotEqualTo("HomeTeamId")]
        [Display(Name = "Away Team")]
        public int AwayTeamId { get; set; }

        [AfterToday(ErrorMessage = "Game must be held later than today.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }
    }
}
