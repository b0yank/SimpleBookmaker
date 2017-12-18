namespace SimpleBookmaker.Services.Models.UserCoefficient
{
    using AutoMapper;
    using Data.Models.Bets;
    using Data.Core.Enums;
    using Infrastructure.AutoMapper;
    using Infrastructure.BetDescribers;
    using Models.Game;
    using System;

    public class UserCurrentBetModel : IHaveCustomMapping
    {
        public string BetCondition { get; set; }

        public string EventName { get; set; }

        public DateTime EventDate { get; set; }

        public double Coefficient { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<GameBet, UserCurrentBetModel>()
                .ForMember(ucbm => ucbm.EventName, cfg =>
                    cfg.MapFrom(gb =>
                        $"{gb.BetCoefficient.Game.HomeTeam.Team.Name} vs {gb.BetCoefficient.Game.AwayTeam.Team.Name}"))
                .ForMember(ucbm => ucbm.EventDate, cfg =>
                    cfg.MapFrom(gb => gb.BetCoefficient.Game.Time))
                .ForMember(ucbm => ucbm.BetCondition, cfg =>
                    cfg.MapFrom(gb => GameBetDescriber.Describe(
                                gb.BetCoefficient.BetType,
                                gb.BetCoefficient.Side,
                                gb.BetCoefficient.BetType == GameBetType.Result
                                    ? new ResultDescriber
                                    {
                                        HomeScore = gb.BetCoefficient.HomeScorePrediction,
                                        AwayScore = gb.BetCoefficient.AwayScorePrediction
                                    }
                                    : (object)new GameTeamsModel
                                    {
                                        HomeTeam = gb.BetCoefficient.Game.HomeTeam.Team.Name,
                                        AwayTeam = gb.BetCoefficient.Game.AwayTeam.Team.Name
                                    }
                                )));

            mapper.CreateMap<PlayerGameBet, UserCurrentBetModel>()
                .ForMember(ucbm => ucbm.EventName, cfg =>
                    cfg.MapFrom(pgb =>
                        $"{pgb.BetCoefficient.Game.HomeTeam.Team.Name} vs {pgb.BetCoefficient.Game.AwayTeam.Team.Name}"))
                .ForMember(ucbm => ucbm.EventDate, cfg =>
                    cfg.MapFrom(pgb => pgb.BetCoefficient.Game.Time))
                .ForMember(ucbm => ucbm.BetCondition, cfg =>
                    cfg.MapFrom(pgb =>
                        GamePlayerBetDescriber.Describe(pgb.BetCoefficient.BetType, pgb.BetCoefficient.Player.Name)));
        }
    }
}
