namespace SimpleBookmaker.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class BetHistory
    {
        public int Id { get; set; }

        [Range(0.01, double.MaxValue)]
        public double Coefficient { get; set; }

        [Required]
        public string Bet { get; set; }
        
        public string EventName { get; set; }

        public DateTime Date { get; set; }

        public int BetSlipHistoryId { get; set; }
        public BetSlipHistory BetSlipHistory { get; set; }
    }
}
