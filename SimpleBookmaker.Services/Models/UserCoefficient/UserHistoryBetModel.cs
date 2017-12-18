namespace SimpleBookmaker.Services.Models.UserCoefficient
{
    using Data.Models;
    using Infrastructure.AutoMapper;
    using System;

    public class UserHistoryBetModel : IMapFrom<BetHistory>
    {
        public double Coefficient { get; set; }

        public string Bet { get; set; }

        public string EventName { get; set; }

        public DateTime Date { get; set; }
    }
}
