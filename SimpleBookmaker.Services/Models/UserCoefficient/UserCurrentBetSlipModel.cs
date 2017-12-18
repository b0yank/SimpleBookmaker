namespace SimpleBookmaker.Services.Models.UserCoefficient
{
    using System.Collections.Generic;

    public class UserCurrentBetSlipModel
    {
        public IEnumerable<UserCurrentBetModel> Bets { get; set; }

        public double Amount { get; set; }
    }
}
