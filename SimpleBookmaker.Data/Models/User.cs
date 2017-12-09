namespace SimpleBookmaker.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using Models.BetSlips;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User : IdentityUser
    {
        [Required]
        [StringLength(15)]
        public string Account { get; set; }

        public decimal Balance { get; set; }

        [Required]
        [StringLength(60)]
        public string Name { get; set; }

        public ICollection<GameBetSlip> GameBetSlips { get; set; }

        public ICollection<BetSlipHistory> BetSlipHistories { get; set; }
    }
}
