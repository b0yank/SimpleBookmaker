namespace SimpleBookmaker.Web.Areas.Admin.Controllers
{
    using Infrastructure;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Mvc;
    using Models.Player;
    using Services.Contracts;

    public class PlayersController : AdminBaseController
    {
        private readonly IPlayersService players;
        private readonly ITeamsService teams;

        public PlayersController(ITeamsService teams, IPlayersService players)
        {
            this.players = players;
            this.teams = teams;
        }
        
        [RestoreModelErrorsFromTempData]
        public IActionResult All(int teamId)
        {
            var teamPlayers = this.players.ByTeam(teamId);

            var team = this.teams.GetName(teamId);

            if (team == null)
            {
                return BadRequest();
            }

            var viewModel = new TeamPlayersListModel
            {
                Players = teamPlayers,
                TeamId = teamId,
                Team = team
            };

            return View(viewModel);
        }
        
        public IActionResult Create(int teamId)
        {
            var teamName = this.teams.GetName(teamId);

            if (teamName == null)
            {
                return BadRequest();
            }

            TempData["TeamId"] = teamId;
            TempData["Team"] = teamName;

            return View();
        }

        [HttpPost]
        public IActionResult Create(PlayerAddModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["TeamId"] = model.TeamId;
                TempData["Team"] = this.teams.GetName(model.TeamId);

                return View(model);
            }

            this.players.Create(model.Name, model.Age, model.TeamId);

            return RedirectToAction(nameof(All), new { teamId = model.TeamId });
        }

        [RestoreModelErrorsFromTempData]
        public IActionResult Remove(string returnUrl = null)
        {
            var unassignedPlayers = this.players.Unassigned();

            ViewData["returnUrl"] = returnUrl ?? "admin/teams/all";

            return View(unassignedPlayers);
        }

        [HttpPost]
        [SetTempDataModelErrors]
        public IActionResult Remove(int playerId)
        {
            var success = this.players.Remove(playerId);

            if (!success)
            {
                ModelState.AddModelError("", ErrorMessages.PlayerRemoveFailed);
            }

            return RedirectToAction(nameof(Remove));
        }
        
        public IActionResult AddToTeam(int teamId)
        {
            var unassignedPlayers = this.players.Unassigned();

            var team = this.teams.GetName(teamId);

            if (team == null)
            {
                return BadRequest();
            }

            var viewModel = new TeamPlayersListModel
            {
                Players = unassignedPlayers,
                TeamId = teamId,
                Team = team
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddToTeam(PlayerEditTeamModel model)
        {
            var success = this.players.AddToTeam(model.PlayerId, model.TeamId);

            if (!success)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All), new { teamId = model.TeamId });
        }

        [HttpPost]
        [SetTempDataModelErrors]
        public IActionResult RemoveFromTeam(PlayerEditTeamModel model)
        {
            var success = this.players.RemoveFromTeam(model.PlayerId);

            if (!success)
            {
                ModelState.AddModelError("", ErrorMessages.PlayerRemoveFailed);
            }

            return RedirectToAction(nameof(All), new { teamId = model.TeamId });
        }
    }
}
