namespace SimpleBookmaker.Data.Models
{
    public class BetHistory
    {
        public int Id { get; set; }

        public double Coefficient { get; set; }

        public string Bet { get; set; }

        public int BetSlipHistoryId { get; set; }
        public BetSlipHistory BetSlipHistory { get; set; }
    }
}
