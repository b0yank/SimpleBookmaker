namespace SimpleBookmaker.Web.Models.UserViewModels
{
    using Services.Models.UserCoefficient;
    using System.Collections.Generic;

    public class UserHistoryPageModel : PageModel
    {
        public IEnumerable<UserHistoryBetSlipModel> BetSlips { get; set; }
    }
}
