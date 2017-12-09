namespace SimpleBookmaker.Data.Models.BetSlips
{
    using Bets;
    using System.Collections.Generic;

    public class PlayerGameBetSlip : BetSlip
    {
        public ICollection<PlayerGameBet> Bets { get; set; } = new List<PlayerGameBet>();
    }
}
