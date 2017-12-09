namespace SimpleBookmaker.Services.Models.Tournament
{
    using AutoMapper;
    using Data.Models;
    using Services.Infrastructure.AutoMapper;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TournamentDetailedListModel : IMapFrom<Tournament>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public int Teams { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Tournament, TournamentDetailedListModel>()
                .ForMember(td => td.Teams, cfg => cfg.MapFrom(t => t.Teams.Count));
        }
    }
}
