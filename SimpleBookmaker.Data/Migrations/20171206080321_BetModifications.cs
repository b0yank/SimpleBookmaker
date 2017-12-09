using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SimpleBookmaker.Data.Migrations
{
    public partial class BetModifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameBets_Games_GameId",
                table: "GameBets");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Tournaments_TournamentId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentPlayer_Players_PlayerId",
                table: "TournamentPlayer");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentPlayer_Tournaments_TournamentId",
                table: "TournamentPlayer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TournamentPlayer",
                table: "TournamentPlayer");

            migrationBuilder.DropIndex(
                name: "IX_GameBets_GameId",
                table: "GameBets");

            migrationBuilder.DropColumn(
                name: "Coefficient",
                table: "TournamentBets");

            migrationBuilder.DropColumn(
                name: "Side",
                table: "TournamentBets");

            migrationBuilder.DropColumn(
                name: "Coefficient",
                table: "PlayerGameBets");

            migrationBuilder.DropColumn(
                name: "Side",
                table: "PlayerGameBets");

            migrationBuilder.DropColumn(
                name: "AwayCorners",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "AwayFreeKicks",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "AwayPossession",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "HomeCorners",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "HomeFreeKicks",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "HomePossession",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "GameBets");

            migrationBuilder.RenameTable(
                name: "TournamentPlayer",
                newName: "TournamentPlayers");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentPlayer_PlayerId",
                table: "TournamentPlayers",
                newName: "IX_TournamentPlayers_PlayerId");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "GameBets",
                newName: "BetSlipId");

            migrationBuilder.RenameColumn(
                name: "Side",
                table: "GameBets",
                newName: "BetCoefficientId");

            migrationBuilder.RenameColumn(
                name: "Coefficient",
                table: "GameBets",
                newName: "Amount");

            migrationBuilder.AlterColumn<int>(
                name: "TournamentId",
                table: "Games",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TournamentPlayers",
                table: "TournamentPlayers",
                columns: new[] { "TournamentId", "PlayerId" });

            migrationBuilder.CreateTable(
                name: "BetSlipHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BetSlipHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BetSlipHistory_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameBetCoefficients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Coefficient = table.Column<double>(nullable: false),
                    GameId = table.Column<int>(nullable: false),
                    Side = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameBetCoefficients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameBetCoefficients_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameBetSlips",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameBetSlips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameBetSlips_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BetHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Bet = table.Column<string>(nullable: true),
                    BetSlipHistoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BetHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BetHistory_BetSlipHistory_BetSlipHistoryId",
                        column: x => x.BetSlipHistoryId,
                        principalTable: "BetSlipHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameBets_BetCoefficientId",
                table: "GameBets",
                column: "BetCoefficientId");

            migrationBuilder.CreateIndex(
                name: "IX_GameBets_BetSlipId",
                table: "GameBets",
                column: "BetSlipId");

            migrationBuilder.CreateIndex(
                name: "IX_BetHistory_BetSlipHistoryId",
                table: "BetHistory",
                column: "BetSlipHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BetSlipHistory_UserId",
                table: "BetSlipHistory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GameBetCoefficients_GameId",
                table: "GameBetCoefficients",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameBetSlips_UserId",
                table: "GameBetSlips",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameBets_GameBetCoefficients_BetCoefficientId",
                table: "GameBets",
                column: "BetCoefficientId",
                principalTable: "GameBetCoefficients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameBets_GameBetSlips_BetSlipId",
                table: "GameBets",
                column: "BetSlipId",
                principalTable: "GameBetSlips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Tournaments_TournamentId",
                table: "Games",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentPlayers_Players_PlayerId",
                table: "TournamentPlayers",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentPlayers_Tournaments_TournamentId",
                table: "TournamentPlayers",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameBets_GameBetCoefficients_BetCoefficientId",
                table: "GameBets");

            migrationBuilder.DropForeignKey(
                name: "FK_GameBets_GameBetSlips_BetSlipId",
                table: "GameBets");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Tournaments_TournamentId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentPlayers_Players_PlayerId",
                table: "TournamentPlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentPlayers_Tournaments_TournamentId",
                table: "TournamentPlayers");

            migrationBuilder.DropTable(
                name: "BetHistory");

            migrationBuilder.DropTable(
                name: "GameBetCoefficients");

            migrationBuilder.DropTable(
                name: "GameBetSlips");

            migrationBuilder.DropTable(
                name: "BetSlipHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TournamentPlayers",
                table: "TournamentPlayers");

            migrationBuilder.DropIndex(
                name: "IX_GameBets_BetCoefficientId",
                table: "GameBets");

            migrationBuilder.DropIndex(
                name: "IX_GameBets_BetSlipId",
                table: "GameBets");

            migrationBuilder.RenameTable(
                name: "TournamentPlayers",
                newName: "TournamentPlayer");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentPlayers_PlayerId",
                table: "TournamentPlayer",
                newName: "IX_TournamentPlayer_PlayerId");

            migrationBuilder.RenameColumn(
                name: "BetSlipId",
                table: "GameBets",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "BetCoefficientId",
                table: "GameBets",
                newName: "Side");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "GameBets",
                newName: "Coefficient");

            migrationBuilder.AddColumn<double>(
                name: "Coefficient",
                table: "TournamentBets",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Side",
                table: "TournamentBets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Coefficient",
                table: "PlayerGameBets",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Side",
                table: "PlayerGameBets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "TournamentId",
                table: "Games",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "AwayCorners",
                table: "Games",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AwayFreeKicks",
                table: "Games",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "AwayPossession",
                table: "Games",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "HomeCorners",
                table: "Games",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeFreeKicks",
                table: "Games",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "HomePossession",
                table: "Games",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "GameBets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TournamentPlayer",
                table: "TournamentPlayer",
                columns: new[] { "TournamentId", "PlayerId" });

            migrationBuilder.CreateIndex(
                name: "IX_GameBets_GameId",
                table: "GameBets",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameBets_Games_GameId",
                table: "GameBets",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Tournaments_TournamentId",
                table: "Games",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentPlayer_Players_PlayerId",
                table: "TournamentPlayer",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentPlayer_Tournaments_TournamentId",
                table: "TournamentPlayer",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
