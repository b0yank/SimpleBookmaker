namespace SimpleBookmaker.Data.Models.Bets
{
    using BetSlips;
    using Coefficients;

    public class TournamentBet : Bet
    {
        public int BetCoefficientId { get; set; }
        public TournamentBetCoefficient BetCoefficient { get; set; }

        public int BetSlipId { get; set; }
        public TournamentBetSlip BetSlip { get; set; }
    }
}
