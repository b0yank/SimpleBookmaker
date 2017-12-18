namespace SimpleBookmaker.Services.Models.UserCoefficient
{
    using Data.Models;
    using Infrastructure.AutoMapper;
    using System.Collections.Generic;
    using AutoMapper;
    using System.Linq;
    using AutoMapper.QueryableExtensions;

    public class UserHistoryBetSlipModel : IMapFrom<BetSlipHistory>, IHaveCustomMapping
    {
        public IEnumerable<UserHistoryBetModel> Bets { get; set; }

        public double Amount { get; set; }

        public bool IsSuccess { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<BetSlipHistory, UserHistoryBetSlipModel>()
                .ForMember(ubsh => ubsh.Bets, cfg =>
                    cfg.MapFrom(bsh => bsh.BetHistories.AsQueryable().ProjectTo<UserHistoryBetModel>()));
        }
    }
}
