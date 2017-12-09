namespace SimpleBookmaker.Web.Areas.Admin.Models.Game
{
    using Data.Validation.Attributes;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class GameEditModel
    {
        public int Id { get; set; }

        [AfterToday(true, ErrorMessage = "Cannot change kick off time for games which have already started or already passed.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.Time)]
        public DateTime Time { get; set; }

        public int TournamentId { get; set; }
    }
}
