namespace SimpleBookmaker.Web.Models.UserViewModels
{
    using Services.Models.Bet;
    using System.Collections.Generic;

    public class BetSlipViewModel
    {
        public IEnumerable<BetUnconfirmedModel> Bets { get; set; }

        public decimal Balance { get; set; }
    }
}
