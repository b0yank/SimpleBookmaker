using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SimpleBookmaker.Data.Migrations
{
    public partial class BetHistoryAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BetHistory_BetSlipHistory_BetSlipHistoryId",
                table: "BetHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_BetSlipHistory_AspNetUsers_UserId",
                table: "BetSlipHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BetSlipHistory",
                table: "BetSlipHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BetHistory",
                table: "BetHistory");

            migrationBuilder.RenameTable(
                name: "BetSlipHistory",
                newName: "BetSlipHistories");

            migrationBuilder.RenameTable(
                name: "BetHistory",
                newName: "BetHistories");

            migrationBuilder.RenameIndex(
                name: "IX_BetSlipHistory_UserId",
                table: "BetSlipHistories",
                newName: "IX_BetSlipHistories_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BetHistory_BetSlipHistoryId",
                table: "BetHistories",
                newName: "IX_BetHistories_BetSlipHistoryId");

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "BetSlipHistories",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddPrimaryKey(
                name: "PK_BetSlipHistories",
                table: "BetSlipHistories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BetHistories",
                table: "BetHistories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BetHistories_BetSlipHistories_BetSlipHistoryId",
                table: "BetHistories",
                column: "BetSlipHistoryId",
                principalTable: "BetSlipHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BetSlipHistories_AspNetUsers_UserId",
                table: "BetSlipHistories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BetHistories_BetSlipHistories_BetSlipHistoryId",
                table: "BetHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_BetSlipHistories_AspNetUsers_UserId",
                table: "BetSlipHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BetSlipHistories",
                table: "BetSlipHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BetHistories",
                table: "BetHistories");

            migrationBuilder.RenameTable(
                name: "BetSlipHistories",
                newName: "BetSlipHistory");

            migrationBuilder.RenameTable(
                name: "BetHistories",
                newName: "BetHistory");

            migrationBuilder.RenameIndex(
                name: "IX_BetSlipHistories_UserId",
                table: "BetSlipHistory",
                newName: "IX_BetSlipHistory_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BetHistories_BetSlipHistoryId",
                table: "BetHistory",
                newName: "IX_BetHistory_BetSlipHistoryId");

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "BetSlipHistory",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddPrimaryKey(
                name: "PK_BetSlipHistory",
                table: "BetSlipHistory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BetHistory",
                table: "BetHistory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BetHistory_BetSlipHistory_BetSlipHistoryId",
                table: "BetHistory",
                column: "BetSlipHistoryId",
                principalTable: "BetSlipHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BetSlipHistory_AspNetUsers_UserId",
                table: "BetSlipHistory",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
