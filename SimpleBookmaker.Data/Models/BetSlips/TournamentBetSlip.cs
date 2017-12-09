namespace SimpleBookmaker.Data.Models.BetSlips
{
    using Bets;
    using System.Collections.Generic;

    public class TournamentBetSlip : BetSlip
    {
        public ICollection<TournamentBet> Bets { get; set; } = new List<TournamentBet>();
    }
}
