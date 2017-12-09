namespace SimpleBookmaker.Services.Models.Game
{
    using Data.Models;
    using Infrastructure.AutoMapper;
    using System;
    using AutoMapper;
    using System.ComponentModel.DataAnnotations;

    public class GameListModel : IMapFrom<Game>, IHaveCustomMapping
    {
        public int Id { get; set; }

        [Display(Name = "Home Team")]
        public string HomeTeam { get; set; }

        [Display(Name = "Away Team")]
        public string AwayTeam { get; set; }

        [Display(Name = "Kick off")]
        public DateTime Kickoff { get; set; }

        public virtual void ConfigureMapping(Profile mapper)
        {
            mapper
                .CreateMap<Game, GameListModel>()
                .ForMember(glm => glm.HomeTeam, cfg => cfg.MapFrom(g => g.HomeTeam.Team.Name))
                .ForMember(glm => glm.AwayTeam, cfg => cfg.MapFrom(g => g.AwayTeam.Team.Name))
                .ForMember(glm => glm.Kickoff, cfg => cfg.MapFrom(g => g.Time));
        }
    }
}
