namespace SimpleBookmaker.Services.Models.UserCoefficient
{
    using Data.Core.Enums;

    public class UserCoefficientModel
    {
        public int CoefficientId { get; set; }

        public double Coefficient { get; set; }

        public string BetCondition { get; set; }

        public string EventName { get; set; }

        public BetType BetType { get; set; }
    }
}
