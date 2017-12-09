﻿namespace SimpleBookmaker.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Models.Team;
    using Services.Contracts;
    using SimpleBookmaker.Web.Infrastructure;
    using System;

    public class TeamsController : AdminBaseController
    {
        private const int allTeamsListPageSize = 20;

        private ITeamsService teams;
        private ITournamentsService tournaments;

        public TeamsController(ITeamsService teams, ITournamentsService tournaments)
        {
            this.teams = teams;
            this.tournaments = tournaments;
        }

        public IActionResult All(int page = 1, string keyword = null)
        {
            var teamsList = this.teams.All(page, allTeamsListPageSize, keyword);

            var viewModel = new TeamListPageModel
            {
                Teams = teamsList,
                CurrentPage = page,
                TotalPages = (int) Math.Ceiling(this.teams.Count(keyword) / (double)allTeamsListPageSize)
            };

            return View(viewModel);
        }

        public IActionResult Add() => View();

        [HttpPost]
        public IActionResult Add(TeamAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = this.teams.Add(model.Name);

            if (!success)
            {
                ModelState.AddModelError("Name", $"A team named {model.Name} already exists.");

                return View(model);
            }

            return this.RedirectToAction(nameof(All));
        }

        public IActionResult Edit(int id)
        {
            if (!this.teams.Exists(id))
            {
                return NotFound(ErrorMessages.InvalidTeam);
            }

            var competingTournaments = this.tournaments.WithTeam(id);
            var notCompetingTournaments = this.tournaments.WithoutTeam(id);

            var teamName = this.teams.GetName(id);

            var viewModel = new TeamEditModel
            {
                Id = id,
                Name = teamName,
                CompetingTournaments = competingTournaments,
                NotCompetingTournaments = notCompetingTournaments
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddTournament(int teamId, int tournamentId)
        {
            if (!this.teams.Exists(teamId))
            {
                return NotFound(ErrorMessages.InvalidTeam);
            }
            if (!this.tournaments.Exists(tournamentId))
            {
                return NotFound(ErrorMessages.InvalidTournament);
            }

            var success = this.tournaments.AddTeam(tournamentId, teamId);

            if (!success)
            {
                return BadRequest("Failed to add team to tournament - is the team already participating?");
            }

            return RedirectToAction(nameof(Edit), new { id = teamId });
        }

        [HttpPost]
        public IActionResult RemoveTournament(int teamId, int tournamentId)
        {
            if (!this.teams.Exists(teamId))
            {
                return NotFound(ErrorMessages.InvalidTeam);
            }
            if (!this.tournaments.Exists(tournamentId))
            {
                return NotFound(ErrorMessages.InvalidTournament);
            }

            var success = this.tournaments.RemoveTeam(tournamentId, teamId);

            if (!success)
            {
                return BadRequest("Could not remove team from tournament. Could the team still have games to play?");
            }

            return RedirectToAction(nameof(Edit), new { id = teamId });
        }
    }
}