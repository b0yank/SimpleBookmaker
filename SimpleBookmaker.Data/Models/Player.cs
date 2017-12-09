namespace SimpleBookmaker.Data.Models
{
    using Coefficients;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Player
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Player name is a required field")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 80 symbols.")]
        [RegularExpression("[a-zA-Z- ]", ErrorMessage = "Player name can only contain letters, dash and whitespace")]
        public string Name { get; set; }

        [Range(14, 55, ErrorMessage = "Age must be between 14 and 55 years")]
        public int Age { get; set; }

        public int? TeamId { get; set; }
        public Team Team { get; set; }

        public ICollection<TournamentPlayer> Tournaments { get; set; } = new List<TournamentPlayer>();
        public ICollection<PlayerGameBetCoefficient> GameCoefficients { get; set; } = new List<PlayerGameBetCoefficient>();
    }
}
