namespace SimpleBookmaker.Web.Models.BetViewModels
{
    using System.Collections.Generic;

    public class SessionBetSlip
    {
        public ICollection<BetUnconfirmedModel> Bets { get; private set; } = new List<BetUnconfirmedModel>();
    }
}
