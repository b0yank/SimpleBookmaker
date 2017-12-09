namespace SimpleBookmaker.Services.Models.Bet
{
    using AutoMapper;
    using Data.Core.Enums;
    using Data.Models.Coefficients;
    using Infrastructure.AutoMapper;
    using Infrastructure.BetDescribers;

    public class TournamentPossibleCoefficientModel : IMapFrom<TournamentBetCoefficient>, IHaveCustomMapping
    {
        public string BetCondition { get; set; }

        public TournamentBetType BetType { get; set; }

        public virtual void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<TournamentBetCoefficient, TournamentPossibleCoefficientModel>()
                .ForMember(tcl => tcl.BetCondition, cfg => cfg.MapFrom(tbc => TournamentBetDescriber.RawDescription(tbc.BetType)));
        }
    }
}
