namespace SimpleBookmaker.Web.Areas.Bookmaker.Models
{
    using SimpleBookmaker.Services.Models.Bet;
    using System.Collections.Generic;

    public class ExistingCoefficientViewModel
    {
        public string RemoveAction { get; set; }

        public string EditAction { get; set; }

        public IEnumerable<CoefficientModel> ExistingCoefficients { get; set; }
    }
}
