using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SimpleBookmaker.Data.Migrations
{
    public partial class BetModificationsVol2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameBets_GameBetCoefficients_BetCoefficientId",
                table: "GameBets");

            migrationBuilder.DropColumn(
                name: "IsSuccessful",
                table: "TournamentBets");

            migrationBuilder.DropColumn(
                name: "ResolutionDate",
                table: "TournamentBets");

            migrationBuilder.DropColumn(
                name: "IsSuccessful",
                table: "PlayerGameBets");

            migrationBuilder.DropColumn(
                name: "ResolutionDate",
                table: "PlayerGameBets");

            migrationBuilder.DropColumn(
                name: "IsSuccessful",
                table: "GameBets");

            migrationBuilder.DropColumn(
                name: "ResolutionDate",
                table: "GameBets");

            migrationBuilder.RenameColumn(
                name: "BetCoefficientId",
                table: "GameBets",
                newName: "GameId");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "GameBets",
                newName: "Coefficient");

            migrationBuilder.RenameIndex(
                name: "IX_GameBets_BetCoefficientId",
                table: "GameBets",
                newName: "IX_GameBets_GameId");

            migrationBuilder.AddColumn<double>(
                name: "Coefficient",
                table: "TournamentBets",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Coefficient",
                table: "PlayerGameBets",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "GameBetSlips",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "BetType",
                table: "GameBets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Side",
                table: "GameBets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BetType",
                table: "GameBetCoefficients",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_GameBets_Games_GameId",
                table: "GameBets",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameBets_Games_GameId",
                table: "GameBets");

            migrationBuilder.DropColumn(
                name: "Coefficient",
                table: "TournamentBets");

            migrationBuilder.DropColumn(
                name: "Coefficient",
                table: "PlayerGameBets");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "GameBetSlips");

            migrationBuilder.DropColumn(
                name: "BetType",
                table: "GameBets");

            migrationBuilder.DropColumn(
                name: "Side",
                table: "GameBets");

            migrationBuilder.DropColumn(
                name: "BetType",
                table: "GameBetCoefficients");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "GameBets",
                newName: "BetCoefficientId");

            migrationBuilder.RenameColumn(
                name: "Coefficient",
                table: "GameBets",
                newName: "Amount");

            migrationBuilder.RenameIndex(
                name: "IX_GameBets_GameId",
                table: "GameBets",
                newName: "IX_GameBets_BetCoefficientId");

            migrationBuilder.AddColumn<bool>(
                name: "IsSuccessful",
                table: "TournamentBets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResolutionDate",
                table: "TournamentBets",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsSuccessful",
                table: "PlayerGameBets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResolutionDate",
                table: "PlayerGameBets",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsSuccessful",
                table: "GameBets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResolutionDate",
                table: "GameBets",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_GameBets_GameBetCoefficients_BetCoefficientId",
                table: "GameBets",
                column: "BetCoefficientId",
                principalTable: "GameBetCoefficients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
