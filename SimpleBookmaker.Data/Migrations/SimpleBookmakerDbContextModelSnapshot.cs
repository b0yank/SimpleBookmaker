﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using SimpleBookmaker.Data;
using SimpleBookmaker.Data.Core.Enums;
using System;

namespace SimpleBookmaker.Data.Migrations
{
    [DbContext(typeof(SimpleBookmakerDbContext))]
    partial class SimpleBookmakerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.BetHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bet");

                    b.Property<int>("BetSlipHistoryId");

                    b.Property<double>("Coefficient");

                    b.HasKey("Id");

                    b.HasIndex("BetSlipHistoryId");

                    b.ToTable("BetHistories");
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.Bets.GameBet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AwayGoals");

                    b.Property<int>("BetSlipId");

                    b.Property<double>("Coefficient");

                    b.Property<int>("GameBetCoefficientId");

                    b.Property<int>("HomeGoals");

                    b.Property<bool>("IsEvaluated");

                    b.Property<bool>("IsSuccess");

                    b.HasKey("Id");

                    b.HasIndex("BetSlipId");

                    b.HasIndex("GameBetCoefficientId");

                    b.ToTable("GameBets");
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.Bets.PlayerGameBet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BetSlipId");

                    b.Property<double>("Coefficient");

                    b.Property<bool>("IsEvaluated");

                    b.Property<bool>("IsSuccess");

                    b.Property<int>("PlayerGameBetCoefficientId");

                    b.HasKey("Id");

                    b.HasIndex("BetSlipId");

                    b.HasIndex("PlayerGameBetCoefficientId");

                    b.ToTable("PlayerGameBets");
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.Bets.TournamentBet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BetCoefficientId");

                    b.Property<int>("BetSlipId");

                    b.Property<double>("Coefficient");

                    b.Property<bool>("IsEvaluated");

                    b.Property<bool>("IsSuccess");

                    b.HasKey("Id");

                    b.HasIndex("BetCoefficientId");

                    b.HasIndex("BetSlipId");

                    b.ToTable("TournamentBets");
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.BetSlipHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<bool>("IsSuccess");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("BetSlipHistories");
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.BetSlips.GameBetSlip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("GameBetSlips");
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.BetSlips.TournamentBetSlip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("TournamentBetSlips");
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.Coefficients.GameBetCoefficient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AwayScorePrediction");

                    b.Property<int>("BetType");

                    b.Property<double>("Coefficient");

                    b.Property<int>("GameId");

                    b.Property<int>("HomeScorePrediction");

                    b.Property<int>("Side");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("GameBetCoefficients");
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.Coefficients.PlayerGameBetCoefficient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BetType");

                    b.Property<double>("Coefficient");

                    b.Property<int>("GameId");

                    b.Property<int>("PlayerId");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("PlayerId");

                    b.ToTable("PlayerGameBetCoefficients");
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.Coefficients.TournamentBetCoefficient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BetSubjectId");

                    b.Property<int>("BetType");

                    b.Property<double>("Coefficient");

                    b.Property<int>("TournamentId");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId");

                    b.ToTable("TournamentBetCoefficients");
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AwayTeamId");

                    b.Property<int>("HomeTeamId");

                    b.Property<DateTime>("Time");

                    b.Property<int>("TournamentId");

                    b.HasKey("Id");

                    b.HasIndex("AwayTeamId");

                    b.HasIndex("HomeTeamId");

                    b.HasIndex("TournamentId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Age");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80);

                    b.Property<int?>("TeamId");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.Tournament", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ChampionId");

                    b.Property<DateTime>("EndDate");

                    b.Property<int>("Importance");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("ChampionId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Tournaments");
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.TournamentPlayer", b =>
                {
                    b.Property<int>("TournamentId");

                    b.Property<int>("PlayerId");

                    b.Property<int>("GoalsScored");

                    b.Property<int>("RedCards");

                    b.Property<int>("YellowCards");

                    b.HasKey("TournamentId", "PlayerId");

                    b.HasIndex("PlayerId");

                    b.ToTable("TournamentPlayers");
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.TournamentTeam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("TeamId");

                    b.Property<int>("TournamentId");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId");

                    b.HasIndex("TeamId", "TournamentId")
                        .IsUnique();

                    b.ToTable("TournamentsTeams");
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasMaxLength(15);

                    b.Property<decimal>("Balance");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("SimpleBookmaker.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("SimpleBookmaker.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SimpleBookmaker.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("SimpleBookmaker.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.BetHistory", b =>
                {
                    b.HasOne("SimpleBookmaker.Data.Models.BetSlipHistory", "BetSlipHistory")
                        .WithMany("BetHistories")
                        .HasForeignKey("BetSlipHistoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.Bets.GameBet", b =>
                {
                    b.HasOne("SimpleBookmaker.Data.Models.BetSlips.GameBetSlip", "BetSlip")
                        .WithMany("GameBets")
                        .HasForeignKey("BetSlipId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SimpleBookmaker.Data.Models.Coefficients.GameBetCoefficient", "BetCoefficient")
                        .WithMany("GameBets")
                        .HasForeignKey("GameBetCoefficientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.Bets.PlayerGameBet", b =>
                {
                    b.HasOne("SimpleBookmaker.Data.Models.BetSlips.GameBetSlip", "BetSlip")
                        .WithMany("PlayerBets")
                        .HasForeignKey("BetSlipId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SimpleBookmaker.Data.Models.Coefficients.PlayerGameBetCoefficient", "BetCoefficient")
                        .WithMany("Bets")
                        .HasForeignKey("PlayerGameBetCoefficientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.Bets.TournamentBet", b =>
                {
                    b.HasOne("SimpleBookmaker.Data.Models.Coefficients.TournamentBetCoefficient", "BetCoefficient")
                        .WithMany("Bets")
                        .HasForeignKey("BetCoefficientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SimpleBookmaker.Data.Models.BetSlips.TournamentBetSlip", "BetSlip")
                        .WithMany("Bets")
                        .HasForeignKey("BetSlipId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.BetSlipHistory", b =>
                {
                    b.HasOne("SimpleBookmaker.Data.Models.User", "User")
                        .WithMany("BetSlipHistories")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.BetSlips.GameBetSlip", b =>
                {
                    b.HasOne("SimpleBookmaker.Data.Models.User", "User")
                        .WithMany("GameBetSlips")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.BetSlips.TournamentBetSlip", b =>
                {
                    b.HasOne("SimpleBookmaker.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.Coefficients.GameBetCoefficient", b =>
                {
                    b.HasOne("SimpleBookmaker.Data.Models.Game", "Game")
                        .WithMany("GameCoefficients")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.Coefficients.PlayerGameBetCoefficient", b =>
                {
                    b.HasOne("SimpleBookmaker.Data.Models.Game", "Game")
                        .WithMany("PlayerCoefficients")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SimpleBookmaker.Data.Models.Player", "Player")
                        .WithMany("GameCoefficients")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.Coefficients.TournamentBetCoefficient", b =>
                {
                    b.HasOne("SimpleBookmaker.Data.Models.Tournament", "Tournament")
                        .WithMany("BetCoefficients")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.Game", b =>
                {
                    b.HasOne("SimpleBookmaker.Data.Models.TournamentTeam", "AwayTeam")
                        .WithMany("AwayGames")
                        .HasForeignKey("AwayTeamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SimpleBookmaker.Data.Models.TournamentTeam", "HomeTeam")
                        .WithMany("HomeGames")
                        .HasForeignKey("HomeTeamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SimpleBookmaker.Data.Models.Tournament", "Tournament")
                        .WithMany("Games")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.Player", b =>
                {
                    b.HasOne("SimpleBookmaker.Data.Models.Team", "Team")
                        .WithMany("Players")
                        .HasForeignKey("TeamId");
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.Tournament", b =>
                {
                    b.HasOne("SimpleBookmaker.Data.Models.Team", "Champion")
                        .WithMany()
                        .HasForeignKey("ChampionId");
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.TournamentPlayer", b =>
                {
                    b.HasOne("SimpleBookmaker.Data.Models.Player", "Player")
                        .WithMany("Tournaments")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("SimpleBookmaker.Data.Models.Tournament", "Tournament")
                        .WithMany("Players")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("SimpleBookmaker.Data.Models.TournamentTeam", b =>
                {
                    b.HasOne("SimpleBookmaker.Data.Models.Team", "Team")
                        .WithMany("Tournaments")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SimpleBookmaker.Data.Models.Tournament", "Tournament")
                        .WithMany("Teams")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
