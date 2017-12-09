namespace SimpleBookmaker.Services.Models.Game
{
    using AutoMapper;
    using Data.Models;
    using Services.Infrastructure.AutoMapper;

    public class GameStatsListModel : GameListModel, IMapFrom<Game>, IHaveCustomMapping
    {
        public string Tournament { get; set; }

        public override void ConfigureMapping(Profile mapper)
        {
            mapper
                .CreateMap<Game, GameStatsListModel>()
                .ForMember(gsm => gsm.HomeTeam, cfg => cfg.MapFrom(g => g.HomeTeam.Team.Name))
                .ForMember(gsm => gsm.AwayTeam, cfg => cfg.MapFrom(g => g.AwayTeam.Team.Name))
                .ForMember(gsm => gsm.Kickoff, cfg => cfg.MapFrom(g => g.Time))
                .ForMember(gsm => gsm.Tournament, cfg => cfg.MapFrom(g => g.Tournament.Name));
        }
    }
}
