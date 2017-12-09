namespace SimpleBookmaker.Services.Models.Team
{
    using AutoMapper;
    using Data.Models;
    using Infrastructure.AutoMapper;
    using System.Collections.Generic;
    using System.Linq;

    public class TeamListModel : IMapFrom<Team>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<string> Tournaments { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Team, TeamListModel>()
                .ForMember(m => m.Tournaments, cfg => cfg.MapFrom(t => t.Tournaments.Select(to => to.Tournament.Name)));
        }
    }
}
