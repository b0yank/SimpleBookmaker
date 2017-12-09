namespace SimpleBookmaker.Services.Models.Bet
{
    public class CoefficientModel
    {
        public string BetCondition { get; set; }

        public double Coefficient { get; set; }

        public int CoefficientId { get; set; }

        public int SubjectId { get; set; }
    }
}
