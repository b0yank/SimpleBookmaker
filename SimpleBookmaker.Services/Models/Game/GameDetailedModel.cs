namespace SimpleBookmaker.Services.Models.Game
{
    using AutoMapper;
    using Data.Models;
    using Infrastructure.AutoMapper;

    public class GameDetailedModel : GameListModel, IMapFrom<Game>, IHaveCustomMapping
    {
        public int TournamentId { get; set; }

        public override void ConfigureMapping(Profile mapper)
        {
            mapper
                .CreateMap<Game, GameDetailedModel>()
                .ForMember(glm => glm.HomeTeam, cfg => cfg.MapFrom(g => g.HomeTeam.Team.Name))
                .ForMember(glm => glm.AwayTeam, cfg => cfg.MapFrom(g => g.AwayTeam.Team.Name))
                .ForMember(glm => glm.Kickoff, cfg => cfg.MapFrom(g => g.Time));
        }
    }
}
