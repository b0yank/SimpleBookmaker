namespace SimpleBookmaker.Web.Areas.Bookmaker.Models.Game
{
    using Data.Core.Enums;
    using System.ComponentModel.DataAnnotations;

    public class GameCoefficientEditModel
    {
        public int GameId { get; set; }

        public GameBetType BetType { get; set; }

        public BetSide Side { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "Number of home goals cannot be a negative number")]
        public int HomeGoals { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Number of away goals cannot be a negative number")]
        public int AwayGoals { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Coefficient must be a positive number.")]
        public double Coefficient { get; set; }
    }
}
