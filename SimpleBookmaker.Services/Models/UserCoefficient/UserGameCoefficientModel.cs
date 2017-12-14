namespace SimpleBookmaker.Services.Models.UserCoefficient
{
    using AutoMapper;
    using Data.Core.Enums;
    using Data.Models.Coefficients;
    using Infrastructure.AutoMapper;
    using Infrastructure.BetDescribers;
    using Models.Game;

    public class UserGameCoefficientModel : UserCoefficientModel, IHaveCustomMapping
    {
        public GameBetType GameBetType { get; set; }

        public BetSide Side { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<GameBetCoefficient, UserGameCoefficientModel>()
                .ForMember(ugc => ugc.BetType, cfg => cfg.UseValue(BetType.Game))
                .ForMember(ugc => ugc.GameBetType, cfg => cfg.MapFrom(gbc => gbc.BetType))
                .ForMember(ugc => ugc.CoefficientId, cfg => cfg.MapFrom(gbc => gbc.Id))
                .ForMember(ugc => ugc.EventName, cfg =>
                    cfg.MapFrom(gbc =>
                        $"{gbc.Game.HomeTeam.Team.Name} vs {gbc.Game.AwayTeam.Team.Name}"))
                .ForMember(ugc => ugc.BetCondition, cfg =>
                    cfg.MapFrom(gbc =>
                        GameBetDescriber.Describe(gbc.BetType,
                            gbc.Side,
                            gbc.BetType != GameBetType.Result
                                ? (object)new GameTeamsModel { HomeTeam = gbc.Game.HomeTeam.Team.Name, AwayTeam = gbc.Game.AwayTeam.Team.Name }
                                : new ResultDescriber { HomeScore = gbc.HomeScorePrediction, AwayScore = gbc.AwayScorePrediction })));
        }
    }
}
