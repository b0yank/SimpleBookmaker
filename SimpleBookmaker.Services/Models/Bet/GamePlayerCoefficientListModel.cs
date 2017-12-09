namespace SimpleBookmaker.Services.Models.Bet
{
    using AutoMapper;
    using Data.Core.Enums;
    using Data.Models.Coefficients;
    using Infrastructure.AutoMapper;
    using Infrastructure.BetDescribers;

    public class GamePlayerCoefficientListModel : CoefficientModel, IMapFrom<PlayerGameBetCoefficient>, IHaveCustomMapping
    {
        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<PlayerGameBetCoefficient, GamePlayerCoefficientListModel>()
                .ForMember(pgcl => pgcl.BetCondition, cfg => cfg.MapFrom(pgbc => GamePlayerBetDescriber.Describe(pgbc.BetType, pgbc.Player.Name)))
                .ForMember(pgcl => pgcl.SubjectId, cfg => cfg.MapFrom(pgbc => pgbc.GameId))
                .ForMember(pgcl => pgcl.CoefficientId, cfg => cfg.MapFrom(pgbc => pgbc.Id));
        }
    }
}
