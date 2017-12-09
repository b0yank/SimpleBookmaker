using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SimpleBookmaker.Data.Migrations
{
    public partial class UpdatedPlayerAndTournamentBets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BetSlipId",
                table: "TournamentBets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsEvaluated",
                table: "TournamentBets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSuccess",
                table: "TournamentBets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "BetSlipId",
                table: "PlayerGameBets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsEvaluated",
                table: "PlayerGameBets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSuccess",
                table: "PlayerGameBets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TournamentBetSlips",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<double>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentBetSlips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentBetSlips_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TournamentBets_BetSlipId",
                table: "TournamentBets",
                column: "BetSlipId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerGameBets_BetSlipId",
                table: "PlayerGameBets",
                column: "BetSlipId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentBetSlips_UserId",
                table: "TournamentBetSlips",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerGameBets_GameBetSlips_BetSlipId",
                table: "PlayerGameBets",
                column: "BetSlipId",
                principalTable: "GameBetSlips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentBets_TournamentBetSlips_BetSlipId",
                table: "TournamentBets",
                column: "BetSlipId",
                principalTable: "TournamentBetSlips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerGameBets_GameBetSlips_BetSlipId",
                table: "PlayerGameBets");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentBets_TournamentBetSlips_BetSlipId",
                table: "TournamentBets");

            migrationBuilder.DropTable(
                name: "TournamentBetSlips");

            migrationBuilder.DropIndex(
                name: "IX_TournamentBets_BetSlipId",
                table: "TournamentBets");

            migrationBuilder.DropIndex(
                name: "IX_PlayerGameBets_BetSlipId",
                table: "PlayerGameBets");

            migrationBuilder.DropColumn(
                name: "BetSlipId",
                table: "TournamentBets");

            migrationBuilder.DropColumn(
                name: "IsEvaluated",
                table: "TournamentBets");

            migrationBuilder.DropColumn(
                name: "IsSuccess",
                table: "TournamentBets");

            migrationBuilder.DropColumn(
                name: "BetSlipId",
                table: "PlayerGameBets");

            migrationBuilder.DropColumn(
                name: "IsEvaluated",
                table: "PlayerGameBets");

            migrationBuilder.DropColumn(
                name: "IsSuccess",
                table: "PlayerGameBets");
        }
    }
}
