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

        protected void PayUser(double amount, string userId, IEnumerable<double> coefficients)
        {
            var totalCoefficient = 1.0;

            foreach (var coefficient in coefficients)
            {
                totalCoefficient *= coefficient;
            }

            var totalAmount = totalCoefficient * amount;

            var user = this.db.Users.Find(userId);

            user.Balance += Convert.ToDecimal(totalAmount);
        }

        protected void AddBetsToHistory(double amount, string userId, bool isSuccess, IEnumerable<BetHistoryModel> bets)
        {
            var betSlipHistory = new BetSlipHistory
            {
                Amount = amount,
                IsSuccess = isSuccess,
                UserId = userId
            };

            var betHistories = new List<BetHistory>();

            foreach (var bet in bets)
            {
                var betHistory = new BetHistory
                {
                    Bet = bet.BetCondition,
                    BetSlipHistory = betSlipHistory,
                    Coefficient = bet.Coefficient,
                    EventName = bet.EventName,
                    Date = bet.Date
                };

                betHistories.Add(betHistory);
            }

            this.db.Add(betSlipHistory);
            this.db.AddRange(betHistories);
        }

        protected class BetHistoryModel
        {
            public double Coefficient { get; set; }

            public string BetCondition { get; set; }

            public string EventName { get; set; }

            public DateTime Date { get; set; }
        }
    }
}
