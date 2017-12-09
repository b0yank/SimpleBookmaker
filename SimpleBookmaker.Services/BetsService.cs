namespace SimpleBookmaker.Services
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Data.Core.Enums;
    using Data.Models.Coefficients;
    using Services.Models.Bet;
    using SimpleBookmaker.Services.Infrastructure.BetDescribers;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BetsService : Service, IBetsService
    {

        public BetsService(SimpleBookmakerDbContext db)
            : base(db)
        {
        }

        public bool AddGameCoefficient(int gameId, 
            GameBetType betType, 
            BetSide betSide, 
            double coefficient, 
            int homeGoals = -1, 
            int awayGoals = -1)
        {
            if (!this.GameExists(gameId))
            {
                return false;
            }

            if (this.db.GameBetCoefficients.Any(gbc => gbc.GameId == gameId 
            && ((gbc.BetType == betType && gbc.Side == betSide && betType != GameBetType.Result)
                || (gbc.BetType == GameBetType.Result 
                        && gbc.HomeScorePrediction == homeGoals 
                        && gbc.AwayScorePrediction == awayGoals))))
            {
                return false;
            }

            var gameCoefficient = new GameBetCoefficient
            {
                GameId = gameId,
                BetType = betType,
                Side = betSide,
                Coefficient = coefficient,
                HomeScorePrediction = homeGoals,
                AwayScorePrediction = awayGoals
            };

            this.db.GameBetCoefficients.Add(gameCoefficient);

            this.db.SaveChanges();

            return true;
        }

        public IEnumerable<GameCoefficientListModel> ExistingGameCoefficients(int gameId)
        {
            if (!this.GameExists(gameId))
            {
                return null;
            }

            return this.db.GameBetCoefficients
                    .Where(gbc => gbc.GameId == gameId)
                    .ProjectTo<GameCoefficientListModel>();
        }

        public IEnumerable<GamePossibleBetListModel> PossibleGameCoefficients(int gameId)
        {
            var existingCoefficients = this.db.GameBetCoefficients
                .Where(gbc => gbc.GameId == gameId)
                .Select(gbc => new { Side = gbc.Side, BetType = gbc.BetType });

            var allPossibleGameBets = this.PossibleGameCoefficients();

            return allPossibleGameBets.Select(pgb => new GamePossibleBetListModel
            {
                BetType = pgb.Key,
                BetSides = pgb.Value
                            .Where(bs => !existingCoefficients
                                .Any(ec => ec.BetType == pgb.Key && ec.Side == bs))
                                .ToList(),
                BetCondition = GameBetDescriber.RawDescription(pgb.Key)
            });
        }

        public void EditCoefficient(int coefficientId, double newCoefficient, BetType betType)
        {
            if (betType == BetType.Game)
            {
                var coefficient = this.db.GameBetCoefficients.Find(coefficientId);

                if (coefficient != null)
                {
                    coefficient.Coefficient = newCoefficient;
                }
            }
            else if (betType == BetType.PlayerGame)
            {
                var coefficient = this.db.PlayerGameBetCoefficients.Find(coefficientId);

                if (coefficient != null)
                {
                    coefficient.Coefficient = newCoefficient;
                }
            }
            else
            {
                var coefficient = this.db.TournamentBetCoefficients.Find(coefficientId);

                if (coefficient != null)
                {
                    coefficient.Coefficient = newCoefficient;
                }
            }

            this.db.SaveChanges();
        }

        public void RemoveCoefficient(int coefficientId, BetType betType)
        {
            if (betType == BetType.Game)
            {
                var gameCoefficient = this.db.GameBetCoefficients.Find(coefficientId);

                if (gameCoefficient != null)
                {
                    this.db.Remove(gameCoefficient);
                }
            }
            else if (betType == BetType.PlayerGame)
            {
                var playerGameCoefficient = this.db.PlayerGameBetCoefficients.Find(coefficientId);

                if (playerGameCoefficient != null)
                {
                    this.db.Remove(playerGameCoefficient);
                }
            }
            else
            {
                var tournamentCoefficient = this.db.TournamentBetCoefficients.Find(coefficientId);

                if (tournamentCoefficient != null)
                {
                    this.db.Remove(tournamentCoefficient);
                }
            }

            this.db.SaveChanges();
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

        public IEnumerable<GamePlayerCoefficientListModel> ExistingGamePlayerCoefficients(int gameId)
            => this.db.PlayerGameBetCoefficients
                .Where(pgbc => pgbc.GameId == gameId)
                .ProjectTo<GamePlayerCoefficientListModel>();

        public bool AddPlayerGameCoefficient(int playerId, 
            int gameId, 
            PlayerGameBetType betType, 
            double coefficient)
        {
            if (!this.GameExists(gameId) || !this.PlayerExists(playerId))
            {
                return false;
            }

            var exists = this.db.PlayerGameBetCoefficients
                .Any(pgbc => pgbc.GameId == gameId
                    && pgbc.PlayerId == playerId
                    && pgbc.BetType == betType);

            if (exists)
            {
                return false;
            }

            var playerGameCoefficient = new PlayerGameBetCoefficient
            {
                GameId = gameId,
                PlayerId = playerId,
                BetType = betType,
                Coefficient = coefficient
            };

            this.db.PlayerGameBetCoefficients.Add(playerGameCoefficient);
            this.db.SaveChanges();

            return true;
        }

        public IEnumerable<GamePlayerPossibleBetListModel> PossibleGamePlayerCoefficients()
        {
            var allPossibleGamePlayerBets = Enum.GetValues(typeof(PlayerGameBetType)).Cast<PlayerGameBetType>();

            return allPossibleGamePlayerBets.Select(pgb => new GamePlayerPossibleBetListModel
            {
                BetType = pgb,
                BetCondition = GamePlayerBetDescriber.RawDescription(pgb)
            });
        }

        private IDictionary<GameBetType, ICollection<BetSide>> PossibleGameCoefficients()
        {
            var gameBetTypes = Enum.GetValues(typeof(GameBetType))
                .Cast<GameBetType>();

            var possibleBets = new Dictionary<GameBetType, ICollection<BetSide>>();

            foreach (var gameBetType in gameBetTypes)
            {
                possibleBets[gameBetType] = new List<BetSide>();

                if (gameBetType == GameBetType.Outcome || gameBetType == GameBetType.BothTeamsScore)
                {
                    possibleBets[gameBetType].Add(BetSide.Neutral);
                }

                if (gameBetType != GameBetType.BothTeamsScore)
                {
                    possibleBets[gameBetType].Add(BetSide.Home);
                    possibleBets[gameBetType].Add(BetSide.Away);
                }
            }

            return possibleBets;
        }
    }
}
