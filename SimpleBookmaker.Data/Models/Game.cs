namespace SimpleBookmaker.Data.Models
{
    using Bets;
    using Coefficients;
    using System;
    using System.Collections.Generic;

    public class Game
    { 
        public int Id { get; set; }

        public DateTime Time { get; set; }

        public int HomeTeamId { get; set; }
        public TournamentTeam HomeTeam { get; set; }

        public int AwayTeamId { get; set; }
        public TournamentTeam AwayTeam { get; set; }

        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }

        public ICollection<GameBetCoefficient> GameCoefficients { get; set; }
        public ICollection<PlayerGameBetCoefficient> PlayerCoefficients { get; set; }
    }
}
