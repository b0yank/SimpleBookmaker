namespace SimpleBookmaker.Web.Areas.Admin.Models.Player
{
    using System.ComponentModel.DataAnnotations;

    public class PlayerAddModel
    {
        [Required(ErrorMessage = "Player name is a required field")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 80 symbols.")]
        [RegularExpression("[a-zA-Z -]+", ErrorMessage = "Player name can only contain letters, dash and whitespace")]
        public string Name { get; set; }

        [Range(14, 55, ErrorMessage = "Age must be between 14 and 55 years")]
        public int Age { get; set; }

        public int TeamId { get; set; }
    }
}
