namespace SimpleBookmaker.Data.Models
{
    public class TournamentPlayer
    {
        public int PlayerId { get; set; }
        public Player Player { get; set; }

        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }

        public int GoalsScored { get; set; }

        public int YellowCards { get; set; }

        public int RedCards { get; set; }
    }
}
