using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SimpleBookmaker.Data.Migrations
{
    public partial class BetHistoryUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Bet",
                table: "BetHistories",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "BetHistories",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.AddColumn<string>(
                name: "EventName",
                table: "BetHistories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "BetHistories");

            migrationBuilder.DropColumn(
                name: "EventName",
                table: "BetHistories");

            migrationBuilder.AlterColumn<string>(
                name: "Bet",
                table: "BetHistories",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
