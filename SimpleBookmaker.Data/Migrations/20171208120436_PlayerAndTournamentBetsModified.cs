using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SimpleBookmaker.Data.Migrations
{
    public partial class PlayerAndTournamentBetsModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameBets_Games_GameId",
                table: "GameBets");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerGameBets_Games_GameId",
                table: "PlayerGameBets");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerGameBets_Players_PlayerId",
                table: "PlayerGameBets");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentBets_Tournaments_TournamentId",
                table: "TournamentBets");

            migrationBuilder.DropIndex(
                name: "IX_PlayerGameBets_GameId",
                table: "PlayerGameBets");

            migrationBuilder.DropColumn(
                name: "BetEntityId",
                table: "TournamentBets");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "PlayerGameBets");

            migrationBuilder.RenameColumn(
                name: "TournamentId",
                table: "TournamentBets",
                newName: "BetCoefficientId");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentBets_TournamentId",
                table: "TournamentBets",
                newName: "IX_TournamentBets_BetCoefficientId");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "PlayerGameBets",
                newName: "PlayerGameBetCoefficientId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerGameBets_PlayerId",
                table: "PlayerGameBets",
                newName: "IX_PlayerGameBets_PlayerGameBetCoefficientId");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "GameBets",
                newName: "GameBetCoefficientId");

            migrationBuilder.RenameIndex(
                name: "IX_GameBets_GameId",
                table: "GameBets",
                newName: "IX_GameBets_GameBetCoefficientId");

            migrationBuilder.CreateTable(
                name: "PlayerGameBetCoefficients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BetType = table.Column<int>(nullable: false),
                    Coefficient = table.Column<double>(nullable: false),
                    GameId = table.Column<int>(nullable: false),
                    PlayerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerGameBetCoefficients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerGameBetCoefficients_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerGameBetCoefficients_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TournamentBetCoefficients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BetSubjectId = table.Column<int>(nullable: false),
                    BetType = table.Column<int>(nullable: false),
                    Coefficient = table.Column<double>(nullable: false),
                    TournamentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentBetCoefficients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentBetCoefficients_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerGameBetCoefficients_GameId",
                table: "PlayerGameBetCoefficients",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerGameBetCoefficients_PlayerId",
                table: "PlayerGameBetCoefficients",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentBetCoefficients_TournamentId",
                table: "TournamentBetCoefficients",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameBets_GameBetCoefficients_GameBetCoefficientId",
                table: "GameBets",
                column: "GameBetCoefficientId",
                principalTable: "GameBetCoefficients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerGameBets_PlayerGameBetCoefficients_PlayerGameBetCoefficientId",
                table: "PlayerGameBets",
                column: "PlayerGameBetCoefficientId",
                principalTable: "PlayerGameBetCoefficients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentBets_TournamentBetCoefficients_BetCoefficientId",
                table: "TournamentBets",
                column: "BetCoefficientId",
                principalTable: "TournamentBetCoefficients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameBets_GameBetCoefficients_GameBetCoefficientId",
                table: "GameBets");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerGameBets_PlayerGameBetCoefficients_PlayerGameBetCoefficientId",
                table: "PlayerGameBets");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentBets_TournamentBetCoefficients_BetCoefficientId",
                table: "TournamentBets");

            migrationBuilder.DropTable(
                name: "PlayerGameBetCoefficients");

            migrationBuilder.DropTable(
                name: "TournamentBetCoefficients");

            migrationBuilder.RenameColumn(
                name: "BetCoefficientId",
                table: "TournamentBets",
                newName: "TournamentId");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentBets_BetCoefficientId",
                table: "TournamentBets",
                newName: "IX_TournamentBets_TournamentId");

            migrationBuilder.RenameColumn(
                name: "PlayerGameBetCoefficientId",
                table: "PlayerGameBets",
                newName: "PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerGameBets_PlayerGameBetCoefficientId",
                table: "PlayerGameBets",
                newName: "IX_PlayerGameBets_PlayerId");

            migrationBuilder.RenameColumn(
                name: "GameBetCoefficientId",
                table: "GameBets",
                newName: "GameId");

            migrationBuilder.RenameIndex(
                name: "IX_GameBets_GameBetCoefficientId",
                table: "GameBets",
                newName: "IX_GameBets_GameId");

            migrationBuilder.AddColumn<int>(
                name: "BetEntityId",
                table: "TournamentBets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "PlayerGameBets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerGameBets_GameId",
                table: "PlayerGameBets",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameBets_Games_GameId",
                table: "GameBets",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerGameBets_Games_GameId",
                table: "PlayerGameBets",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerGameBets_Players_PlayerId",
                table: "PlayerGameBets",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentBets_Tournaments_TournamentId",
                table: "TournamentBets",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
