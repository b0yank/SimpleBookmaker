namespace SimpleBookmaker.Data.Models.BetSlips
{
    using Bets;
    using System.Collections.Generic;

    public class GameBetSlip : BetSlip
    {
        public ICollection<GameBet> GameBets { get; set; }

        public ICollection<PlayerGameBet> PlayerBets { get; set; } = new List<PlayerGameBet>();
    }
}
