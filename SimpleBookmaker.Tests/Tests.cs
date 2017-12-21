namespace SimpleBookmaker.Tests
{
    using AutoMapper;
    using Data;
    using Data.Core.Enums;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using SimpleBookmaker.Services.Infrastructure.AutoMapper;
    using System;
    using System.Collections.Generic;
    using SimpleBookmaker.Data.Models.Bets;
    using SimpleBookmaker.Data.Models.Coefficients;
    using SimpleBookmaker.Data.Models.BetSlips;

    public class Tests
    {
        public static bool testsInitialized = false;

        public static void Initialize()
        {
            if (!testsInitialized)
            {
                Mapper.Initialize(config => config.AddProfile<AutoMapperProfile>());
                testsInitialized = true;
            }
        }

        public static void PopulateData(SimpleBookmakerDbContext context, bool includeBets = false)
        {
            context.Games.AddRange(GetTestGamesData());
            context.Tournaments.AddRange(GetTestTournamentsData());
            context.TournamentsTeams.AddRange(GetTestTournamentTeamsData());
            context.Teams.AddRange(GetTestTeamsData());
            context.Players.AddRange(GetTestPlayersData());
            context.Users.AddRange(GetTestUsersData());

            if (includeBets)
            {
                context.TournamentPlayers.AddRange(GetTestTournamentPlayersData());
                context.GameBetSlips.AddRange(GetTestGameBetSlipsData());
                context.GameBetCoefficients.AddRange(GetTestGameBetCoefficientsData());
                context.GameBets.AddRange(GetTestGameBetsData());
                context.PlayerGameBetCoefficients.AddRange(GetTestPlayerGameBetCoefficientsData());
                context.PlayerGameBets.AddRange(GetTestPlayerGameBetsData());
                context.TournamentBetSlips.AddRange(GetTestTournamentBetSlipsData());
                context.TournamentBetCoefficients.AddRange(GetTestTournamentBetCoefficientsData());
                context.TournamentBets.AddRange(GetTestTournamentBetsData());
            }

            context.SaveChanges();
        }

        private static IEnumerable<TournamentBet> GetTestTournamentBetsData()
        {
            return new List<TournamentBet>()
            {
                new TournamentBet { BetCoefficientId = 1, BetSlipId = 1, Coefficient = 5, IsEvaluated = false, IsSuccess = false },
                new TournamentBet { BetCoefficientId = 1, BetSlipId = 1, Coefficient = 10, IsEvaluated = false, IsSuccess = false },
                new TournamentBet { BetCoefficientId = 2, BetSlipId = 1, Coefficient = 15, IsEvaluated = false, IsSuccess = false },
                new TournamentBet { BetCoefficientId = 2, BetSlipId = 2, Coefficient = 20, IsEvaluated = true, IsSuccess = false },
                new TournamentBet { BetCoefficientId = 3, BetSlipId = 2, Coefficient = 2.5, IsEvaluated = true, IsSuccess = false },
                new TournamentBet { BetCoefficientId = 4, BetSlipId = 3, Coefficient = 3.0, IsEvaluated = true, IsSuccess = false },
                new TournamentBet { BetCoefficientId = 5, BetSlipId = 3, Coefficient = 3.5, IsEvaluated = true, IsSuccess = true },
                new TournamentBet { BetCoefficientId = 5, BetSlipId = 4, Coefficient = 5.5, IsEvaluated = true, IsSuccess = true },
            };
        }

        private static IEnumerable<TournamentBetCoefficient> GetTestTournamentBetCoefficientsData()
        {
            return new List<TournamentBetCoefficient>
            {
                new TournamentBetCoefficient { BetSubjectId = 1, Coefficient = 5, TournamentId = 1, BetType = TournamentBetType.Champion },
                new TournamentBetCoefficient { BetSubjectId = 2, Coefficient = 10, TournamentId = 1, BetType = TournamentBetType.TopScorer },
                new TournamentBetCoefficient { BetSubjectId = 5, Coefficient = 20, TournamentId = 1, BetType = TournamentBetType.Champion },
                new TournamentBetCoefficient { BetSubjectId = 4, Coefficient = 35, TournamentId = 3, BetType = TournamentBetType.Champion },
                new TournamentBetCoefficient { BetSubjectId = 5, Coefficient = 40, TournamentId = 4, BetType = TournamentBetType.TopScorer },
                new TournamentBetCoefficient { BetSubjectId = 3, Coefficient = 45, TournamentId = 5, BetType = TournamentBetType.Champion },
                new TournamentBetCoefficient { BetSubjectId = 3, Coefficient = 15, TournamentId = 1, BetType = TournamentBetType.TopScorer }
            };
        }

        private static IEnumerable<TournamentBetSlip> GetTestTournamentBetSlipsData()
        {
            return new List<TournamentBetSlip>
            {
                new TournamentBetSlip { UserId = "Id1", Amount = 25 },
                new TournamentBetSlip { UserId = "Id1", Amount = 10 },
                new TournamentBetSlip { UserId = "Id2", Amount = 5 },
                new TournamentBetSlip { UserId = "Id3", Amount = 40 }
            };
        }

        private static IEnumerable<TournamentPlayer> GetTestTournamentPlayersData()
        {
            return new List<TournamentPlayer>
            {
                new TournamentPlayer { PlayerId = 1, TournamentId = 1, GoalsScored = 0 },
                new TournamentPlayer { PlayerId = 2, TournamentId = 1, GoalsScored = 2 },
                new TournamentPlayer { PlayerId = 3, TournamentId = 1, GoalsScored = 1 },
                new TournamentPlayer { PlayerId = 1, TournamentId = 2, GoalsScored = 5 },
                new TournamentPlayer { PlayerId = 2, TournamentId = 2, GoalsScored = 5 },
                new TournamentPlayer { PlayerId = 4, TournamentId = 2, GoalsScored = 4 },
                new TournamentPlayer { PlayerId = 5, TournamentId = 4, GoalsScored = 4 },
                new TournamentPlayer { PlayerId = 6, TournamentId = 5, GoalsScored = 3 }
            };
        }

        private static IEnumerable<PlayerGameBetCoefficient> GetTestPlayerGameBetCoefficientsData()
        {
            return new List<PlayerGameBetCoefficient>
            {
                new PlayerGameBetCoefficient { Coefficient = 20, GameId = 1, PlayerId = 1, BetType = PlayerGameBetType.ScoreGoal },
                new PlayerGameBetCoefficient { Coefficient = 10, GameId = 1, PlayerId = 2, BetType = PlayerGameBetType.Score2Goals },
                new PlayerGameBetCoefficient { Coefficient = 8, GameId = 4, PlayerId = 4, BetType = PlayerGameBetType.ScoreMoreThan2Goals },
                new PlayerGameBetCoefficient { Coefficient = 4, GameId = 4, PlayerId = 6, BetType = PlayerGameBetType.ScoreGoal },
                new PlayerGameBetCoefficient { Coefficient = 2, GameId = 3, PlayerId = 5, BetType = PlayerGameBetType.ScoreGoal },
            };
        }

        private static IEnumerable<PlayerGameBet> GetTestPlayerGameBetsData()
        {
            return new List<PlayerGameBet>
            {
                new PlayerGameBet { BetSlipId = 1, Coefficient = 34, IsEvaluated = false, IsSuccess = false, PlayerGameBetCoefficientId = 1 },
                new PlayerGameBet { BetSlipId = 1, Coefficient = 5, IsEvaluated = true, IsSuccess = true, PlayerGameBetCoefficientId = 2 },
                new PlayerGameBet { BetSlipId = 2, Coefficient = 1.95, IsEvaluated = false, IsSuccess = false, PlayerGameBetCoefficientId = 1 },
                new PlayerGameBet { BetSlipId = 2, Coefficient = 4, IsEvaluated = true, IsSuccess = false, PlayerGameBetCoefficientId = 3 },
                new PlayerGameBet { BetSlipId = 4, Coefficient = 4.2, IsEvaluated = true, IsSuccess = false, PlayerGameBetCoefficientId = 4 },
                new PlayerGameBet { BetSlipId = 5, Coefficient = 3.4, IsEvaluated = true, IsSuccess = true, PlayerGameBetCoefficientId = 4 },
            };
        }

        private static IEnumerable<GameBetSlip> GetTestGameBetSlipsData()
        {
            return new List<GameBetSlip>
            {
                new GameBetSlip { Amount = 10, UserId = "Id1" },
                new GameBetSlip { Amount = 9, UserId = "Id2" },
                new GameBetSlip { Amount = 8, UserId = "Id3" },
                new GameBetSlip { Amount = 7, UserId = "Id4" },
                new GameBetSlip { Amount = 6, UserId = "Id1" }
            };
        }

        private static IEnumerable<GameBetCoefficient> GetTestGameBetCoefficientsData()
        {
            return new List<GameBetCoefficient>
            {
                new GameBetCoefficient { HomeScorePrediction = 0, AwayScorePrediction = 0, Coefficient = 2, GameId = 1, BetType = GameBetType.Outcome, Side = BetSide.Home  },
                new GameBetCoefficient { HomeScorePrediction = 0, AwayScorePrediction = 0, Coefficient = 3, GameId = 1, BetType = GameBetType.Outcome, Side = BetSide.Away  },
                new GameBetCoefficient { HomeScorePrediction = 0, AwayScorePrediction = 0, Coefficient = 5, GameId = 2, BetType = GameBetType.CleanSheet, Side = BetSide.Neutral  },
                new GameBetCoefficient { HomeScorePrediction = 0, AwayScorePrediction = 0, Coefficient = 8, GameId = 2, BetType = GameBetType.Result, Side = BetSide.Home  },
                new GameBetCoefficient { HomeScorePrediction = 0, AwayScorePrediction = 0, Coefficient = 10, GameId = 3, BetType = GameBetType.BothTeamsScore, Side = BetSide.Home  },
                new GameBetCoefficient { HomeScorePrediction = 3, AwayScorePrediction = 2, Coefficient = 6, GameId = 4, BetType = GameBetType.Result, Side = BetSide.Home  },
                new GameBetCoefficient { HomeScorePrediction = 0, AwayScorePrediction = 0, Coefficient = 3, GameId = 1, BetType = GameBetType.Outcome, Side = BetSide.Neutral  },
                new GameBetCoefficient { HomeScorePrediction = 0, AwayScorePrediction = 0, Coefficient = 8, GameId = 2, BetType = GameBetType.Outcome, Side = BetSide.Home  },
                new GameBetCoefficient { HomeScorePrediction = 0, AwayScorePrediction = 0, Coefficient = 7, GameId = 2, BetType = GameBetType.Outcome, Side = BetSide.Away  }
            };
        }

        private static IEnumerable<GameBet> GetTestGameBetsData()
        {
            return new List<GameBet>
            {
                new GameBet { IsSuccess = false, IsEvaluated = false, Coefficient = 2, BetSlipId = 1, GameBetCoefficientId = 1 },
                new GameBet { IsSuccess = false, IsEvaluated = false, Coefficient = 5, BetSlipId = 1, GameBetCoefficientId = 2 },
                new GameBet { IsSuccess = false, IsEvaluated = false, Coefficient = 10, BetSlipId = 2, GameBetCoefficientId = 1 },
                new GameBet { IsSuccess = false, IsEvaluated = false, Coefficient = 1, BetSlipId = 2, GameBetCoefficientId = 2 },
                new GameBet { IsSuccess = false, IsEvaluated = false, Coefficient = 0.6, BetSlipId = 3, GameBetCoefficientId = 3 },
                new GameBet { IsSuccess = false, IsEvaluated = true, Coefficient = 2.6, BetSlipId = 3, GameBetCoefficientId = 4 },
                new GameBet { IsSuccess = false, IsEvaluated = false, Coefficient = 8, BetSlipId = 4, GameBetCoefficientId = 3 },
                new GameBet { IsSuccess = false, IsEvaluated = true, Coefficient = 8.2, BetSlipId = 4, GameBetCoefficientId = 4 },
                new GameBet { IsSuccess = true, IsEvaluated = true, Coefficient = 9.6, BetSlipId = 5, GameBetCoefficientId = 5 },
                new GameBet { IsSuccess = true, IsEvaluated = true, Coefficient = 2.4, BetSlipId = 5, GameBetCoefficientId = 6 }
            };
        }

        public static SimpleBookmakerDbContext Context
        {
            get
            {
                var options = new DbContextOptionsBuilder<SimpleBookmakerDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                return new SimpleBookmakerDbContext(options);
            }
        }

        private static IEnumerable<Game> GetTestGamesData()
        {
            return new List<Game>
            {
                new Game
                {
                    TournamentId = 2,
                    Time = DateTime.UtcNow.AddDays(1),
                    HomeTeamId = 5,
                    AwayTeamId = 6,
                },
                new Game { TournamentId = 3, Time = DateTime.UtcNow.AddDays(3), HomeTeamId = 3, AwayTeamId = 4 },
                new Game { TournamentId = 4, Time = DateTime.UtcNow.AddDays(5), HomeTeamId = 2, AwayTeamId = 5 },
                new Game { TournamentId = 2, Time = DateTime.UtcNow.AddDays(7), HomeTeamId = 1, AwayTeamId = 3 },
                new Game { TournamentId = 5, Time = DateTime.UtcNow.AddDays(-9), HomeTeamId = 3, AwayTeamId = 2 },
                new Game { TournamentId = 1, Time = DateTime.UtcNow.AddDays(1), HomeTeamId = 1, AwayTeamId = 5 },
                new Game { TournamentId = 1, Time = DateTime.UtcNow.AddDays(-10), HomeTeamId = 5, AwayTeamId = 1 }
            };
        }

        private static IEnumerable<User> GetTestUsersData()
        {
            return new List<User>
            {
                new User { Id = "Id1", UserName = "Username", Email = "email1@test.bg" },
                new User { Id = "Id2", UserName = "Username2", Email = "email2@test.bg" },
                new User { Id = "Id3", UserName = "Username3", Email = "email3@test.bg" },
                new User { Id = "Id4", UserName = "Username4", Email = "email4@test.bg" },
                new User { Id = "Id5", UserName = "Username5", Email = "email5@test.bg" }
            };
        }

        private static IEnumerable<TournamentTeam> GetTestTournamentTeamsData()
        {
            return new List<TournamentTeam>
            {
                new TournamentTeam { TeamId = 1, TournamentId = 2 },
                new TournamentTeam { TeamId = 2, TournamentId = 2 },
                new TournamentTeam { TeamId = 3, TournamentId = 2 },
                new TournamentTeam { TeamId = 4, TournamentId = 3 },
                new TournamentTeam { TeamId = 1, TournamentId = 1 },
                new TournamentTeam { TeamId = 5, TournamentId = 1 },
                new TournamentTeam { TeamId = 2, TournamentId = 4 },
                new TournamentTeam { TeamId = 3, TournamentId = 5 },
            };
        }

        private static IEnumerable<Tournament> GetTestTournamentsData()
        {
            return new List<Tournament>
            {
                new Tournament { Name = "A Grupa", StartDate = DateTime.UtcNow.AddDays(5), EndDate = DateTime.UtcNow.AddDays(10) },
                new Tournament { Name = "B Grupa", StartDate = DateTime.UtcNow.AddDays(8), EndDate = DateTime.UtcNow.AddDays(18) },
                new Tournament { Name = "C Grupa", StartDate = DateTime.UtcNow.AddDays(10), EndDate = DateTime.UtcNow.AddDays(14) },
                new Tournament { Name = "D Grupa", StartDate = DateTime.UtcNow.AddDays(12), EndDate = DateTime.UtcNow.AddDays(16) },
                new Tournament { Name = "Bash Selskata", StartDate = DateTime.UtcNow.AddDays(15), EndDate = DateTime.UtcNow.AddDays(25) },
            };
        }

        private static IEnumerable<Team> GetTestTeamsData()
        {
            return new List<Team>
            {
                new Team { Name = "Real Mavrud" },
                new Team { Name = "Deportivo Lesnichei" },
                new Team { Name = "Obrochishte United" },
                new Team { Name = "Inter Popovo" },
                new Team { Name = "Karnobat City" }
            };
        }

        private static IEnumerable<Player> GetTestPlayersData()
        {
            return new List<Player>
            {
                new Player { Name = "Pesho", TeamId = 1, Age = 20 },
                new Player { Name = "Gosho", TeamId = 1, Age = 30 },
                new Player { Name = "Gicho", TeamId = 1, Age = 20 },
                new Player { Name = "Micho", TeamId = 2, Age = 18 },
                new Player { Name = "Dicho", TeamId = 2, Age = 19 },
                new Player { Name = "Vicho", TeamId = 3, Age = 25 },
                new Player { Name = "Kicho", TeamId = 4, Age = 26 },
                new Player { Name = "Picho", TeamId = 4, Age = 28 },
                new Player { Name = "Ticho", TeamId = 3, Age = 32 },
                new Player { Name = "Sicho", TeamId = 5, Age = 34 },
                new Player { Name = "Richo", TeamId = null, Age = 20 },
                new Player { Name = "William", TeamId = null, Age = 22 },
            };
        }
    }
}
