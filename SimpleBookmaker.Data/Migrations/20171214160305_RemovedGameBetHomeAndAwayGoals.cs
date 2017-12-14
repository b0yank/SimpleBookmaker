using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SimpleBookmaker.Data.Migrations
{
    public partial class RemovedGameBetHomeAndAwayGoals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayGoals",
                table: "GameBets");

            migrationBuilder.DropColumn(
                name: "HomeGoals",
                table: "GameBets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
