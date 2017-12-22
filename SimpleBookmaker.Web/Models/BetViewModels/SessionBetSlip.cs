namespace SimpleBookmaker.Web.Models.BetViewModels
{
    using Data.Core.Enums;
    using Infrastructure;
    using Newtonsoft.Json;
    using Services.Models.Bet;
    using System.Collections.Generic;
    using System.Linq;

    public class SessionBetSlip
    {
        [JsonProperty("bets")]
        private ICollection<BetUnconfirmedModel> bets = new List<BetUnconfirmedModel>();

        [JsonProperty("isTournamentType")]
        private bool? isTournamentType;

        public void Clear()
        {
            this.isTournamentType = null;

            this.bets = new List<BetUnconfirmedModel>();
        }

        public IEnumerable<BetUnconfirmedModel> GetBets()
            => this.bets;

        public void Remove(int coefficientId, BetType betType)
        {
            var betToRemove = this.bets.FirstOrDefault(b => b.CoefficientId == coefficientId && b.BetType == betType);

            if (betToRemove != null)
            {
                this.bets.Remove(betToRemove);
            }
        }

        public BetSlipAdditionResult AddBet(BetUnconfirmedModel bet)
        {
            if (this.bets == null)
            {
                this.bets = new List<BetUnconfirmedModel>();
            }

            var result = new BetSlipAdditionResult();

            if (this.bets.Any(b => b.CoefficientId == bet.CoefficientId))
            {
                result.AddError(ErrorMessages.BetCannotBeAddedTwiceToSlip);
            }

            if (this.isTournamentType == null)
            {
                this.isTournamentType = bet.BetType == BetType.Tournament;
            }

            else if ((bool)this.isTournamentType && bet.BetType != BetType.Tournament
                || (!(bool)this.isTournamentType && bet.BetType == BetType.Tournament))
            {
                result.AddError(ErrorMessages.InvalidBetSlipType);
            }

            if (result.Success)
            {
                this.bets.Add(bet);
            }

            return result;
        }
    }
}
