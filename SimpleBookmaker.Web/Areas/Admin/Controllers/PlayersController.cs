namespace SimpleBookmaker.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models.Player;
    using Services.Contracts;
    using SimpleBookmaker.Services.Models.Player;
    using SimpleBookmaker.Web.Infrastructure;
    
    public class PlayersController : AdminBaseController
    {
        private readonly IPlayersService players;
        private readonly ITeamsService teams;

        public PlayersController(ITeamsService teams, IPlayersService players)
        {
            this.players = players;
            this.teams = teams;
        }
        
        public IActionResult All(int teamId)
        {
            var teamPlayers = this.players.ByTeam(teamId);

            var team = this.teams.GetName(teamId);

            if (team == null)
            {
                return NotFound($"Team with id {teamId} does not exist.");
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
                return NotFound(ErrorMessages.InvalidTeam);
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

        public IActionResult Remove()
        {
            var unassignedPlayers = this.players.Unassigned();

            return View(unassignedPlayers);
        }

        [HttpPost]
        public IActionResult Remove(int playerId)
        {
            this.players.Remove(playerId);

            return RedirectToAction(nameof(Remove));
        }
        
        public IActionResult AddToTeam(int teamId)
        {
            var unassignedPlayers = this.players.Unassigned();

            var team = this.teams.GetName(teamId);

            if (team == null)
            {
                return NotFound(ErrorMessages.InvalidTeam);
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
                return NotFound("Invalid player or team");
            }

            return RedirectToAction(nameof(All), new { teamId = model.TeamId });
        }

        [HttpPost]
        public IActionResult RemoveFromTeam(PlayerEditTeamModel model)
        {
            var success = this.players.RemoveFromTeam(model.PlayerId);

            if (!success)
            {
                return NotFound(ErrorMessages.InvalidPlayer);
            }

            return RedirectToAction(nameof(All), new { teamId = model.TeamId });
        }
    }
}
