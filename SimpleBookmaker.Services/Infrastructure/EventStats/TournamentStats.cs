namespace SimpleBookmaker.Services.Infrastructure.EventStats
{
    using System.Collections.Generic;

    public class TournamentStats
    {
        public int TournamentId { get; set; }

        public int ChampionId { get; set; }

        public IEnumerable<int> TopScorers { get; set; }
    }
}
