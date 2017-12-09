namespace SimpleBookmaker.Data.Models.Bets
{
    using BetSlips;
    using Data.Models.Coefficients;

    public class GameBet : Bet
    {
        public int GameBetCoefficientId { get; set; }
        public GameBetCoefficient BetCoefficient { get; set; }

        public int HomeGoals { get; set; }

        public int AwayGoals { get; set; }

        public int BetSlipId { get; set; }
        public GameBetSlip BetSlip { get; set; }
    }
}
