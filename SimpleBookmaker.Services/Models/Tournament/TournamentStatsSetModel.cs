namespace SimpleBookmaker.Services.Models.Tournament
{
    using Data.Models;
    using Infrastructure.AutoMapper;
    using Models.Team;
    using System.Collections.Generic;
    using AutoMapper;
    using System.Linq;

    public class TournamentStatsSetModel : TournamentListModel, IMapFrom<Tournament>, IHaveCustomMapping
    {
        public IEnumerable<BaseTeamModel> Teams { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Tournament, TournamentStatsSetModel>()
                .ForMember(tss => tss.Teams, 
                    cfg => cfg.MapFrom(t => t
                        .Teams
                        .Select(te => new BaseTeamModel
                        {
                            Id = te.Team.Id,
                            Name = te.Team.Name
                        })));
        }
    }
}
