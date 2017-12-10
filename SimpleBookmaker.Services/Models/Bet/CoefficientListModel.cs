namespace SimpleBookmaker.Services.Models.Bet
{
    using AutoMapper;
    using Data.Core.Enums;
    using Data.Models.Coefficients;
    using Infrastructure.AutoMapper;
    using Infrastructure.BetDescribers;

    public class CoefficientListModel : IHaveCustomMapping
    {
        public string BetCondition { get; set; }

        public double Coefficient { get; set; }

        public int CoefficientId { get; set; }

        public virtual void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<GameBetCoefficient, CoefficientListModel>()
                .ForMember(clm => clm.BetCondition,
                    cfg => cfg.MapFrom(gbc =>
                        GameBetDescriber.Describe(
                            gbc.BetType,
                            gbc.Side,
                            (gbc.BetType != GameBetType.Result
                                ? null
                                : new ResultDescriber { HomeScore = gbc.HomeScorePrediction, AwayScore = gbc.AwayScorePrediction }))))
                .ForMember(clm => clm.CoefficientId, cfg => cfg.MapFrom(gbc => gbc.Id));

            mapper.CreateMap<PlayerGameBetCoefficient, CoefficientListModel>()
                .ForMember(clm => clm.BetCondition,
                    cfg => cfg.MapFrom(pgb =>
                        GamePlayerBetDescriber.Describe(pgb.BetType, pgb.Player.Name)))
                .ForMember(clm => clm.CoefficientId, cfg => cfg.MapFrom(pgb => pgb.Id));
        }
    }
}
