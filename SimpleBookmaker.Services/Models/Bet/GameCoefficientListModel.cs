namespace SimpleBookmaker.Services.Models.Bet
{
    using AutoMapper;
    using Data.Core.Enums;
    using Data.Models.Coefficients;
    using Services.Infrastructure.AutoMapper;
    using SimpleBookmaker.Services.Infrastructure.BetDescribers;

    public class GameCoefficientListModel : CoefficientModel, IMapFrom<GameBetCoefficient>, IHaveCustomMapping
    {
        //public int HomeScorePrediction { get; set; }

        //public int AwayScorePrediction { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<GameBetCoefficient, GameCoefficientListModel>()
                .ForMember(gbc => gbc.BetCondition, cfg => cfg.MapFrom(g => GameBetDescriber.Describe(g.BetType, g.Side, (g.BetType != GameBetType.Result ? null : new ResultDescriber { HomeScore = g.HomeScorePrediction, AwayScore = g.AwayScorePrediction }))))
                .ForMember(gbc => gbc.SubjectId, cfg => cfg.MapFrom(gbc =>  gbc.GameId))
                .ForMember(gbc => gbc.CoefficientId, cfg => cfg.MapFrom(gbc => gbc.Id));
        }
    }
}
