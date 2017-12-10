namespace SimpleBookmaker.Web.Models.BetViewModels
{
    using Data.Core.Enums;

    public class BetUnconfirmedModel
    {
        public int CoefficientId { get; set; }

        public double Coefficient { get; set; }

        public string BetCondition { get; set; }

        public BetType BetType { get; set; }
    }
}
