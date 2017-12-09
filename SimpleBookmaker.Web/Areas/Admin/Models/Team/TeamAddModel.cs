namespace SimpleBookmaker.Web.Areas.Admin.Models
{
    using System.ComponentModel.DataAnnotations;

    public class TeamAddModel
    {
        [Required(ErrorMessage = "Team name is a required field.")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 60 symbols.")]
        [RegularExpression("[a-zA-Z0-9- ]+", ErrorMessage = "Team name can only contain letters, digits, dash and whitespace")]
        public string Name { get; set; }
    }
}
