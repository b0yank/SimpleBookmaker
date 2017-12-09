using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SimpleBookmaker.Data.Migrations
{
    public partial class DbBetUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AwayGoals",
                table: "GameBets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeGoals",
                table: "GameBets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsEvaluated",
                table: "GameBets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSuccess",
                table: "GameBets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "BetSlipHistory",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsSuccess",
                table: "BetSlipHistory",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Coefficient",
                table: "BetHistory",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayGoals",
                table: "GameBets");

            migrationBuilder.DropColumn(
                name: "HomeGoals",
                table: "GameBets");

            migrationBuilder.DropColumn(
                name: "IsEvaluated",
                table: "GameBets");

            migrationBuilder.DropColumn(
                name: "IsSuccess",
                table: "GameBets");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "BetSlipHistory");

            migrationBuilder.DropColumn(
                name: "IsSuccess",
                table: "BetSlipHistory");

            migrationBuilder.DropColumn(
                name: "Coefficient",
                table: "BetHistory");
        }
    }
}
