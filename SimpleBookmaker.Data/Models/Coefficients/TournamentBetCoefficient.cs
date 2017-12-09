namespace SimpleBookmaker.Data.Models.Coefficients
{
    using Bets;
    using Core.Enums;
    using System.Collections.Generic;

    public class TournamentBetCoefficient : BetCoefficient
    {
        public TournamentBetType BetType { get; set; }

        public int BetSubjectId { get; set; }

        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }

        public ICollection<TournamentBet> Bets { get; set; } = new List<TournamentBet>(); 
    }
}
