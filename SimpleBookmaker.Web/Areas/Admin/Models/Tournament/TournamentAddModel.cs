namespace SimpleBookmaker.Web.Areas.Admin.Models.Tournament
{
    using Data.Validation.Attributes;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TournamentAddModel
    {
        [Required(ErrorMessage = "Tournament name is a required field.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 symbols.")]
        [RegularExpression("[a-zA-Z0-9 /]+", ErrorMessage = "Tournament name can only contain letters, digits, whitespace and '/'.")]
        public string Name { get; set; }

        [Range(1, 10, ErrorMessage = "Tournament importance must be between 1 and 10")]
        public int Importance { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DateAfter("StartDate", ErrorMessage = "Tournament end date must be later than its start date.")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
    }
}
