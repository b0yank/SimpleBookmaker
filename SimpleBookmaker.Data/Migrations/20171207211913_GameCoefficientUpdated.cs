using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SimpleBookmaker.Data.Migrations
{
    public partial class GameCoefficientUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int?>(
                name: "AwayScorePrediction",
                table: "GameBetCoefficients",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int?>(
                name: "HomeScorePrediction",
                table: "GameBetCoefficients",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayScorePrediction",
                table: "GameBetCoefficients");

            migrationBuilder.DropColumn(
                name: "HomeScorePrediction",
                table: "GameBetCoefficients");
        }
    }
}
