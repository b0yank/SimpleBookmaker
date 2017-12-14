namespace SimpleBookmaker.Services
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Data.Core.Enums;
    using Data.Models.Coefficients;
    using Infrastructure.BetDescribers;
    using Models.Bet;
    using Models.Game;
    using Models.UserCoefficient;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GameBetsService : Service, IGameBetsService
    {
        public GameBetsService(SimpleBookmakerDbContext db)
            : base(db)
        { }

        public GameBasicCoefficientsListModel ByGameBasic(int gameId)
        {
            var game = this.db.Games.Select(g => new
            {
                Id = g.Id,
                HomeTeam = g.HomeTeam.Team.Name,
                AwayTeam = g.AwayTeam.Team.Name,
                Kickoff= g.Time
            })
            .FirstOrDefault(g => g.Id == gameId);

            if (game == null)
            {
                return null;
            }

            var coefficientHome = this.db.GameBetCoefficients
                .Where(gbc => gbc.GameId == gameId && gbc.BetType == GameBetType.Outcome && gbc.Side == BetSide.Home)
                .ProjectTo<UserGameCoefficientModel>()
                .FirstOrDefault(); ;

            var coefficientDraw = this.db.GameBetCoefficients
                .Where(gbc => gbc.GameId == gameId && gbc.BetType == GameBetType.Outcome && gbc.Side == BetSide.Neutral)
                .ProjectTo<UserGameCoefficientModel>()
                .FirstOrDefault();

            var coefficientAway = this.db.GameBetCoefficients
                .Where(gbc => gbc.GameId == gameId && gbc.BetType == GameBetType.Outcome && gbc.Side == BetSide.Away)
                .ProjectTo<UserGameCoefficientModel>()
                .FirstOrDefault(); ;

            if (coefficientHome == null
                || coefficientDraw == null
                || coefficientAway == null)
            {
                return null;
            }

            var gameBasicCoefficients = new GameBasicCoefficientsListModel
            {
                Id = gameId,
                Teams = new GameTeamsModel { HomeTeam = game.HomeTeam, AwayTeam = game.AwayTeam },
                Kickoff = game.Kickoff,
                CoefficientHome = coefficientHome,
                CoefficientDraw = coefficientDraw,
                CoefficientAway = coefficientAway
            };

            return gameBasicCoefficients;
        }

        public IEnumerable<CoefficientListModel> ByGameAll(int gameId)
        {
            if (!this.GameExists(gameId))
            {
                return null;
            }

            var coefficients = new List<CoefficientListModel>();

            coefficients.AddRange(
                this.db.GameBetCoefficients
                    .Where(gbc => gbc.GameId == gameId)
                    .ProjectTo<CoefficientListModel>());

            coefficients.AddRange(
                this.db.PlayerGameBetCoefficients
                    .Where(pgbc => pgbc.GameId == gameId)
                    .ProjectTo<CoefficientListModel>());

            return coefficients;
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
                    .OrderBy(gbc => gbc.BetType)
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
            })
            .Where(pgb => pgb.BetSides.Count > 0);
        }

        public IEnumerable<GamePlayerCoefficientListModel> ExistingGamePlayerCoefficients(int gameId)
            => this.db.PlayerGameBetCoefficients
                .Where(pgbc => pgbc.GameId == gameId)
                .OrderBy(pgbc => pgbc.BetType)
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

        public bool HasBasicCoefficients(int gameId)
        {
            var gameCoefficients = this.db.GameBetCoefficients.Where(gbc => gbc.GameId == gameId);

            return gameCoefficients.FirstOrDefault(gc => gc.BetType == GameBetType.Outcome && gc.Side == BetSide.Home) != null
                && gameCoefficients.FirstOrDefault(gc => gc.BetType == GameBetType.Outcome && gc.Side == BetSide.Neutral) != null
                && gameCoefficients.FirstOrDefault(gc => gc.BetType == GameBetType.Outcome && gc.Side == BetSide.Away) != null;
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

                if (gameBetType == GameBetType.Outcome 
                || gameBetType == GameBetType.BothTeamsScore
                || gameBetType == GameBetType.BothTeamsScore)
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

        public void EditCoefficient(int coefficientId, double newCoefficient, BetType betType)
        {
            switch (betType)
            {
                case BetType.Game:
                    var gameCoefficient = this.db.GameBetCoefficients.Find(coefficientId);

                    if (gameCoefficient != null)
                    {
                        gameCoefficient.Coefficient = newCoefficient;
                    }
                    break;
                case BetType.PlayerGame:
                    var playerCoefficient = this.db.PlayerGameBetCoefficients.Find(coefficientId);

                    if (playerCoefficient != null)
                    {
                        playerCoefficient.Coefficient = newCoefficient;
                    }
                    break;
            }

            this.db.SaveChanges();
        }

        public void RemoveCoefficient(int coefficientId, BetType betType)
        {
            switch (betType)
            {
                case BetType.Game:
                    var gameCoefficient = this.db.GameBetCoefficients.Find(coefficientId);

                    if (gameCoefficient != null)
                    {
                        this.db.Remove(gameCoefficient);
                    }
                    break;
                case BetType.PlayerGame:
                    var playerGameCoefficient = this.db.PlayerGameBetCoefficients.Find(coefficientId);

                    if (playerGameCoefficient != null)
                    {
                        this.db.Remove(playerGameCoefficient);
                    }
                    break;
            }

            this.db.SaveChanges();
        }
    }
}
