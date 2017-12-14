namespace SimpleBookmaker.Services.Models.UserCoefficient
{
    using AutoMapper;
    using Data.Core.Enums;
    using Data.Models.Coefficients;
    using Infrastructure.BetDescribers;
    using Models.Bet;

    public class UserGamePlayerCoefficientModel : UserCoefficientModel
    {
        public PlayerGameBetType PlayerBetType { get; set; }

        public BetSide Side { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<PlayerGameBetCoefficient, UserGamePlayerCoefficientModel>()
                .ForMember(pgbc => pgbc.CoefficientId, cfg => cfg.MapFrom(pgc => pgc.Id))
                .ForMember(pgbc => pgbc.PlayerBetType, cfg => cfg.MapFrom(pgc => pgc.BetType))
                .ForMember(pgbc => pgbc.BetType, cfg => cfg.UseValue(BetType.PlayerGame))
                .ForMember(pgbc => pgbc.BetCondition, cfg =>
                    cfg.MapFrom(pgc =>
                        GamePlayerBetDescriber.Describe(
                            pgc.BetType, pgc.Player.Name)));
        }
    }
}
