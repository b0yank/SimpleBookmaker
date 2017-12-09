namespace SimpleBookmaker.Services.Models.Tournament
{
    using Data.Models;
    using Infrastructure.AutoMapper;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TournamentModel : IMapFrom<Tournament>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tournament name is a required field.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 symbols.")]
        [RegularExpression("[a-zA-Z0-9 /]+", ErrorMessage = "Tournament name can only contain letters, digits and whitespace.")]
        public string Name { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}
