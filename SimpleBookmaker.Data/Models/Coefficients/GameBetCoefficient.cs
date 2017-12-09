namespace SimpleBookmaker.Data.Models.Coefficients
{
    using Data.Core.Enums;
    using Data.Models.Bets;
    using System.Collections.Generic;

    public class GameBetCoefficient : BetCoefficient
    {
        public GameBetType BetType { get; set; }

        public BetSide Side { get; set; }

        public int HomeScorePrediction { get; set; }

        public int AwayScorePrediction { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }

        public ICollection<GameBet> GameBets { get; set; } = new List<GameBet>();
    }
}
