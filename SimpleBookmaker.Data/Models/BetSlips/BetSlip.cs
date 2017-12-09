namespace SimpleBookmaker.Data.Models.BetSlips
{
    using Models.Bets;
    using System.Collections.Generic;

    public abstract class BetSlip
    {
        public int Id { get; set; }

        public double Amount { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
