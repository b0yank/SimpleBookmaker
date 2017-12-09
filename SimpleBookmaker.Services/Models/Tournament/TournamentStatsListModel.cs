namespace SimpleBookmaker.Services.Models.Tournament
{
    using Data.Models;
    using Infrastructure.AutoMapper;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TournamentStatsListModel : TournamentListModel, IMapFrom<Tournament>
    {
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
    }
}
