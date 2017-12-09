namespace SimpleBookmaker.Data.Models.Bets
{
    using BetSlips;
    using Coefficients;

    public class PlayerGameBet : Bet
    {
        public int PlayerGameBetCoefficientId { get; set; }
        public PlayerGameBetCoefficient BetCoefficient { get;set;}

        public int BetSlipId { get; set; }
        public GameBetSlip BetSlip { get; set; }
    }
}
