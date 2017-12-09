namespace SimpleBookmaker.Data.Models
{
    using System.Collections.Generic;

    public class BetSlipHistory
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public double Amount { get; set; }

        public bool IsSuccess { get; set; }

        public ICollection<BetHistory> BetHistories { get; set; } = new List<BetHistory>();
    }
}
