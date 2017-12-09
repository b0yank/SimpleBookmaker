namespace SimpleBookmaker.Services.Contracts
{
    using Data;
    using Data.Models.Bets;
    using Data.Models.BetSlips;
    using SimpleBookmaker.Data.Models;
    using SimpleBookmaker.Services.Infrastructure.BetDescribers;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class StatisticiansService : Service
    {
        public StatisticiansService(SimpleBookmakerDbContext db)
            : base(db)
        {
        }

        protected void PayUser(BetSlip betSlip, IEnumerable<Bet> bets)
        {
            var totalCoefficient = 1.0;

            foreach (var bet in bets)
            {
                totalCoefficient *= bet.Coefficient;
            }

            var totalAmount = totalCoefficient * betSlip.Amount;

            var user = this.db.Users.Find(betSlip.UserId);

            user.Balance += Convert.ToDecimal(totalAmount);

            this.db.SaveChanges();
        }

        protected void MoveBetsToHistory(BetSlip betSlip, IDictionary<Bet, string> betsWithDescription)
        {
            var betSlipHistory = new BetSlipHistory
            {
                Amount = betSlip.Amount,
                IsSuccess = betsWithDescription.Keys.All(b => b.IsSuccess),
                UserId = betSlip.UserId
            };

            var betHistories = new List<BetHistory>();

            foreach (var bet in betsWithDescription)
            {
                var betHistory = new BetHistory
                {
                    Bet = bet.Value,
                    BetSlipHistory = betSlipHistory,
                    Coefficient = bet.Key.Coefficient
                };

                betHistories.Add(betHistory);
            }

            this.db.Add(betSlipHistory);
            this.db.AddRange(betHistories);

            this.db.SaveChanges();
        }
    }
}
