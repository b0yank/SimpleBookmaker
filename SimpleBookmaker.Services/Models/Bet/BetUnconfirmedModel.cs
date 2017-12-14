namespace SimpleBookmaker.Services.Models.Bet
{
    using Data.Core.Enums;
    using System;

    [Serializable]
    public class BetUnconfirmedModel
    {
        public int CoefficientId { get; set; }

        public double Coefficient { get; set; }

        public string BetCondition { get; set; }

        public string EventName { get; set; }

        public BetType BetType { get; set; }
    }
}
