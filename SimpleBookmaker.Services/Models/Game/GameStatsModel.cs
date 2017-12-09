namespace SimpleBookmaker.Services.Models.Game
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data.Models;
    using Services.Models.Player;
    using SimpleBookmaker.Data.Validation.Attributes;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class GameStatsModel : GameStatsListModel
    {
        [Range(0, int.MaxValue, ErrorMessage = "Corners cannot be negative.")]
        public int HomeCorners { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Corners cannot be negative.")]
        public int AwayCorners { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Free kicks cannot be negative.")]
        public int HomeFreeKicks { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Free kicks cannot be negative.")]
        public int AwayFreeKicks { get; set; }

        [Sum(100, "AwayPossession", ErrorMessage = "Possession must total 100%.")]
        [Range(0, 100, ErrorMessage = "Possession must be between 0 and 100.")]
        public int HomePossession { get; set; }

        [Range(0, 100, ErrorMessage = "Possession must be between 0 and 100.")]
        public int AwayPossession { get; set; }

        public IEnumerable<PlayerListModel> HomePlayers { get; set; }
        public IEnumerable<PlayerListModel> AwayPlayers { get; set; }

        public string HomeGoalscorers { get; set; } = string.Empty;
        public string AwayGoalscorers { get; set; } = string.Empty;

        public override void ConfigureMapping(Profile mapper)
        {
            mapper
                .CreateMap<Game, GameStatsModel>()
                .ForMember(gsm => gsm.HomeTeam, cfg => cfg.MapFrom(g => g.HomeTeam.Team.Name))
                .ForMember(gsm => gsm.AwayTeam, cfg => cfg.MapFrom(g => g.AwayTeam.Team.Name))
                .ForMember(gsm => gsm.Kickoff, cfg => cfg.MapFrom(g => g.Time))
                .ForMember(gsm => gsm.Tournament, cfg => cfg.MapFrom(g => g.Tournament.Name))
                .ForMember(gsm => gsm.HomePlayers, cfg => cfg.MapFrom(g => g.HomeTeam.Team.Players.AsQueryable().ProjectTo<PlayerListModel>()))
                .ForMember(gsm => gsm.AwayPlayers, cfg => cfg.MapFrom(g => g.AwayTeam.Team.Players.AsQueryable().ProjectTo<PlayerListModel>()));
        }
    }
}
