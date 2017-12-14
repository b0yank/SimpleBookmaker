namespace SimpleBookmaker.Web.Controllers
{
    using Infrastructure;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Mvc;
    using Models.TournamentViewModels;
    using Services.Contracts;
    using System;

    public class TournamentsController : Controller
    {
        private const int tournamentsListPageSize = 10;
        private const int upcomingGamesListPageSize = 10;

        private readonly IGamesService games;
        private readonly ITournamentBetsService tournamentBets;
        private readonly ITournamentsService tournaments;

        public TournamentsController(IGamesService games, 
            ITournamentBetsService tournamentBets, 
            ITournamentsService tournaments)
        {
            this.games = games;
            this.tournamentBets = tournamentBets;
            this.tournaments = tournaments;
        }

        public IActionResult All(int page = 1)
        {
            var allTournaments = this.tournaments.AllDetailed(page, tournamentsListPageSize, true);
            var tournamentsCount = this.tournaments.Count(true);

            var viewModel = new TournamentDetailedListPageModel
            {
                Tournaments = allTournaments,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(tournamentsCount / (double)tournamentsListPageSize),
                RequestPath = "tournaments/all"
            };

            return View(viewModel);
        }

        [RestoreModelErrorsFromTempData]
        public IActionResult View(int id, int page = 1)
        {
            if (!this.tournaments.Exists(id))
            {
                return NotFound(ErrorMessages.InvalidTournament);
            }

            var tournamentCoefficients = this.tournamentBets.ExistingTournamentCoefficients(id);

            var viewModel = new TournamentCoefficientsViewModel
            {
                Coefficients = tournamentCoefficients,
                Tournament = this.tournaments.GetName(id),
                UpcomingGames = this.games.Upcoming(page, upcomingGamesListPageSize, id),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(this.games.UpcomingCount(id) / (double)upcomingGamesListPageSize),
                RequestPath = $"tournaments/view/{id}"
            };

            return View(viewModel);
        }
    }
}
