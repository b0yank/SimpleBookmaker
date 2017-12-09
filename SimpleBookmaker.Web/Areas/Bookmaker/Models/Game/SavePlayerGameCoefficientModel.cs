namespace SimpleBookmaker.Web.Areas.Bookmaker.Models.Game
{
    using Data.Core.Enums;
    using System.ComponentModel.DataAnnotations;

    public class SavePlayerGameCoefficientModel
    {
        public int GameId { get; set; }

        public int PlayerId { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Coefficient must be larger than zero")]
        public double Coefficient { get; set; }

        public PlayerGameBetType BetType { get; set; }
    }
}
