namespace SimpleBookmaker.Services.Models.Bet
{
    using AutoMapper;
    using Data.Core.Enums;
    using Data.Models.Coefficients;
    using Infrastructure.AutoMapper;
    using Infrastructure.BetDescribers;

    public class GameCoefficientListModel : CoefficientModel, IHaveCustomMapping
    {
        public override void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<GameBetCoefficient, GameCoefficientListModel>()
                .ForMember(gbc => gbc.BetCondition,
                    cfg => cfg.MapFrom(g =>
                        GameBetDescriber.Describe(
                            g.BetType,
                            g.Side,
                            (g.BetType != GameBetType.Result
                                ? null
                                : new ResultDescriber { HomeScore = g.HomeScorePrediction, AwayScore = g.AwayScorePrediction }))))
                .ForMember(gbc => gbc.SubjectId, cfg => cfg.MapFrom(gbc => gbc.GameId))
                .ForMember(gbc => gbc.CoefficientId, cfg => cfg.MapFrom(gbc => gbc.Id));
        }
    }
}
