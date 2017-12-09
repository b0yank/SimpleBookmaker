namespace SimpleBookmaker.Services.Models.Game
{
    using AutoMapper;
    using Data.Models;
    using Infrastructure.AutoMapper;

    public class GameTeamsModel : IMapFrom<Game>, IHaveCustomMapping
    {
        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Game, GameTeamsModel>()
                .ForMember(gtm => gtm.HomeTeam, cfg => cfg.MapFrom(g => g.HomeTeam.Team.Name))
                .ForMember(gtm => gtm.AwayTeam, cfg => cfg.MapFrom(g => g.AwayTeam.Team.Name));
        }
    }
}
