﻿namespace SimpleBookmaker.Web.Areas.Bookmaker.Controllers
{
    using Data.Core.Enums;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Models.Tournament;
    using Services.Contracts;
    using Services.Infrastructure.BetDescribers;
    using System;
    using Web.Infrastructure;

    public class TournamentsController : BookmakerBaseController
    {
        private readonly ITournamentBetsService tournamentBets;
        private readonly ITeamsService teams;
        private readonly ITournamentsService tournaments;
        private readonly IPlayersService players;

        public TournamentsController(ITournamentBetsService tournamentBets,
            ITeamsService teams,
            ITournamentsService tournaments,
            IPlayersService players)
        {
            this.tournamentBets = tournamentBets;
            this.teams = teams;
            this.tournaments = tournaments;
            this.players = players;
        }

        public IActionResult Index()
        {
            var allTournaments = this.tournaments.All();

            return View(allTournaments);
        }

        [RestoreModelErrorsFromTempData]
        public IActionResult SetBets(int tournamentId)
        {
            if (!this.tournaments.Exists(tournamentId))
            {
                return BadRequest();
            }

            var existingCoefficients = this.tournamentBets.ExistingTournamentCoefficients(tournamentId);
            var possibleCoefficients = this.tournamentBets.PossibleTournamentCoefficients();
            var teams = this.tournaments.GetTournamentTeams(tournamentId);

            var viewModel = new SetTournamentCoefficientsModel
            {
                TournamentId = tournamentId,
                Tournament = this.tournaments.GetName(tournamentId),
                ExistingCoefficients = existingCoefficients,
                PossibleCoefficients = possibleCoefficients,
                Teams = teams
            };

            return View(viewModel);
        }

        [HttpPost]
        [SetTempDataModelErrors]
        public IActionResult SetBets(SaveTournamentCoefficientModel model)
        {
            if (!this.tournaments.Exists(model.TournamentId))
            {
                ModelState.AddModelError("Tournament", ErrorMessages.InvalidTournament);
            }

            var subjectType = TournamentBetDescriber.TournamentCoefficientSubjectType(model.BetType);

            if (subjectType == SubjectType.Team)
            {
                if (!this.teams.Exists(model.SubjectId))
                {
                    ModelState.AddModelError("Team", ErrorMessages.InvalidTeam);
                }
            }
            else if(!this.players.Exists(model.SubjectId))
            {
                ModelState.AddModelError("Player", ErrorMessages.InvalidPlayer);
            }

            if (ModelState.IsValid)
            {
                var success = this.tournamentBets.AddTournamentCoefficient(model.TournamentId, model.SubjectId, model.Coefficient, model.BetType);

                if (!success)
                {
                    ModelState.AddModelError("CoefficientExists", "A coefficient already exists for this condition");
                }
            }

            return RedirectToAction(nameof(SetBets), new { tournamentId = model.TournamentId });
        }

        [SetTempDataModelErrors]
        public IActionResult ChoosePlayer(int subjectId, int tournamentId, double coefficient, TournamentBetType betType)
        {
            if (!this.teams.Exists(subjectId))
            {
                return BadRequest();
            }

            if (!this.tournaments.Exists(tournamentId))
            {
                return BadRequest();
            }

            if (!Enum.IsDefined(typeof(TournamentBetType), betType))
            {
                return BadRequest();
            }

            if (coefficient <= 0)
            {
                ModelState.Clear();

                ModelState.AddModelError("coefficient", ErrorMessages.InvalidCoefficientValue);

                return RedirectToAction(nameof(SetBets), new { tournamentId = tournamentId });
            }

            var teamPlayers = this.players.ByTeam(subjectId);
            var team = this.teams.GetName(subjectId);
            var tournament = this.tournaments.GetName(tournamentId);

            var model = new TournamentCoefficientPlayerChoiceModel
            {
                TeamId = subjectId,
                Team = team,
                TournamentId = tournamentId,
                Tournament =  tournament,
                Coefficient = coefficient,
                BetType = betType,
                BetCondition = TournamentBetDescriber.RawDescription(betType),
                Players = teamPlayers
            };

            return View(model);
        }

        [HttpPost]
        [SetTempDataModelErrors]
        public IActionResult EditCoefficient(EditCoefficientModel model)
        {
            if (ModelState.IsValid)
            {
                this.tournamentBets.EditCoefficient(model.CoefficientId, model.NewCoefficient);
            }

            return RedirectToAction(nameof(SetBets), new { tournamentId = model.SubjectId });
        }

        [HttpPost]
        [SetTempDataModelErrors]
        public IActionResult RemoveCoefficient(RemoveCoefficientModel model)
        {
            var success = this.tournamentBets.RemoveCoefficient(model.CoefficientId);

            if (!success)
            {
                ModelState.AddModelError("", ErrorMessages.CoefficientDeleteFailed);
            }

            return RedirectToAction(nameof(SetBets), new { tournamentId = model.SubjectId });
        }
    }
}
