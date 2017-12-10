namespace SimpleBookmaker.Services
{
    using Contracts;
    using Data;
    using Data.Core.Enums;
    using Data.Models.Coefficients;
    using Services.Infrastructure.BetDescribers;
    using Services.Models.Bet;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TournamentBetsService : Service, ITournamentBetsService
    {
        public TournamentBetsService(SimpleBookmakerDbContext db)
            : base(db)
        {
        }

        public bool AddTournamentCoefficient(int tournamentId, int subjectId, double coefficient, TournamentBetType betType)
        {
            if (!this.TournamentExists(tournamentId))
            {
                return false;
            }

            var subjectType = TournamentBetDescriber.TournamentCoefficientSubjectType(betType);

            if ((subjectType == SubjectType.Team && !this.TeamExists(subjectId))
                || (subjectType == SubjectType.Player && !this.PlayerExists(subjectId)))
            {
                return false;
            }

            var exists = this.db.TournamentBetCoefficients
                    .Any(tbc => tbc.TournamentId == tournamentId
                    && tbc.BetSubjectId == subjectId
                    && tbc.BetType == betType);

            if (exists)
            {
                return false;
            }

            var betCoefficient = new TournamentBetCoefficient
            {
                TournamentId = tournamentId,
                BetSubjectId = subjectId,
                BetType = betType,
                Coefficient = coefficient
            };

            this.db.TournamentBetCoefficients.Add(betCoefficient);
            this.db.SaveChanges();

            return true;
        }

        public IEnumerable<TournamentCoefficientListModel> ExistingTournamentCoefficients(int tournamentId)
        {
            if (!this.TournamentExists(tournamentId))
            {
                return null;
            }

            var tournamentBetCoefficients = this.db.TournamentBetCoefficients
                    .Where(tbc => tbc.TournamentId == tournamentId);

            var subjectNames = new Dictionary<int, string>();
            foreach (var tournamentBetCoefficient in tournamentBetCoefficients)
            {
                subjectNames.Add(tournamentBetCoefficient.Id, this.GetTournamentBetSubjectName(tournamentBetCoefficient));
            }

            return tournamentBetCoefficients
                    .Select(tbc => new TournamentCoefficientListModel
                    {
                        SubjectId = tbc.TournamentId,
                        CoefficientId = tbc.Id,
                        Coefficient = tbc.Coefficient,
                        BetCondition = TournamentBetDescriber.Describe(tbc.BetType, subjectNames[tbc.Id]),
                        BetSubjectName = subjectNames[tbc.Id]
                    });
        }

        public IEnumerable<TournamentPossibleCoefficientModel> PossibleTournamentCoefficients()
        {
            var allPossibleTournamentBets = Enum.GetValues(typeof(TournamentBetType)).Cast<TournamentBetType>();

            return allPossibleTournamentBets.Select(pgb => new TournamentPossibleCoefficientModel
            {
                BetType = pgb,
                BetCondition = TournamentBetDescriber.RawDescription(pgb)
            });
        }

        public void EditCoefficient(int coefficientId, double newCoefficient)
        {
            var tournamentCoefficient = this.db.TournamentBetCoefficients.Find(coefficientId);

            if (tournamentCoefficient != null)
            {
                tournamentCoefficient.Coefficient = newCoefficient;
            }

            this.db.SaveChanges();
        }

        public void RemoveCoefficient(int coefficientId)
        {
            var tournamentCoefficient = this.db.TournamentBetCoefficients.Find(coefficientId);

            if (tournamentCoefficient != null)
            {
                this.db.Remove(tournamentCoefficient);
            }
        }
    }
}
