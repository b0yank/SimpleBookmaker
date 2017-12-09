namespace SimpleBookmaker.Data.Models
{
    using System.Collections.Generic;

    public class TournamentTeam
    {
        public int Id { get; set; }

        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }

        public ICollection<Game> HomeGames { get; set; }

        public ICollection<Game> AwayGames { get; set; }
    }
}
