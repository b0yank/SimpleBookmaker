namespace SimpleBookmaker.Web.Areas.Admin.Models.Tournament
{
    using Data.Models;
    using Services.Infrastructure.AutoMapper;
    using System;
    using AutoMapper;

    public class TournamentDetailedListModel : IMapFrom<Tournament>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Teams { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Tournament, TournamentDetailedListModel>()
                .ForMember(td => td.Teams, cfg => cfg.MapFrom(t => t.Teams.Count));
        }
    }
}
