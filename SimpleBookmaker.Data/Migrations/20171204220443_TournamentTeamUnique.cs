using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SimpleBookmaker.Data.Migrations
{
    public partial class TournamentTeamUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TournamentsTeams_TeamId",
                table: "TournamentsTeams");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentsTeams_TeamId_TournamentId",
                table: "TournamentsTeams",
                columns: new[] { "TeamId", "TournamentId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TournamentsTeams_TeamId_TournamentId",
                table: "TournamentsTeams");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentsTeams_TeamId",
                table: "TournamentsTeams",
                column: "TeamId");
        }
    }
}
