namespace SimpleBookmaker.Data.Models
{
    using Bets;
    using Validation.Attributes;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Data.Models.Coefficients;

    public class Tournament
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tournament name is a required field.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 symbols.")]
        [RegularExpression("[a-zA-Z0-9 /]+", ErrorMessage = "Tournament name can only contain letters, digits and whitespace.")]
        public string Name { get; set; }

        public ICollection<TournamentTeam> Teams { get; set; } = new List<TournamentTeam>();

        public ICollection<TournamentPlayer> Players { get; set; } = new List<TournamentPlayer>();

        public ICollection<TournamentBetCoefficient> BetCoefficients { get; set; } = new List<TournamentBetCoefficient>();

        public ICollection<Game> Games { get; set; } = new List<Game>();

        [Range(1, 10, ErrorMessage = "Tournament importance must be between 1 and 10")]
        public int Importance { get; set; }

        public DateTime StartDate { get; set; }

        [DateAfter("StartDate")]
        public DateTime EndDate { get; set; }

        public int? ChampionId { get; set; }
        public Team Champion { get; set; }
    }
}
