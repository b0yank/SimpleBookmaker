namespace SimpleBookmaker.Data.Models.Coefficients
{
    using Bets;
    using Core.Enums;
    using System.Collections.Generic;

    public class PlayerGameBetCoefficient : BetCoefficient
    {
        public PlayerGameBetType BetType { get; set; }

        public int PlayerId { get; set; }
        public Player Player { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }

        public ICollection<PlayerGameBet> Bets { get; set; } = new List<PlayerGameBet>();
    }
}
