namespace SimpleBookmaker.Web.Controllers
{
    using Data.Core.Enums;
    using Infrastructure;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.UserViewModels;
    using Services.Contracts;
    using Services.Models.Bet;
    using System.Collections.Generic;

    [Authorize]
    public class UsersController : BaseController
    {
        private readonly IUserBetsService userBets;
        private readonly IUsersService users;

        public UsersController(IUserBetsService userBets, IUsersService users)
        {
            this.userBets = userBets;
            this.users = users;
        }
        
        public IActionResult Profile()
        {
            var profile = this.users.Profile(User.Identity.Name);

            return View(profile);
        }

        [RestoreModelErrorsFromTempData]
        public IActionResult Slip()
        {
            var betSlip = GetBetSlip();

            var userBets = betSlip != null ? betSlip.GetBets() : new List<BetUnconfirmedModel>();

            var viewModel = new BetSlipViewModel
            {
                Bets = userBets,
                Balance = this.users.GetBalance(User.Identity.Name)
            };

            return View(viewModel);
        }

        [SetTempDataModelErrors]
        [HttpPost]
        public IActionResult PlaceBets(double amount)
        {
            var betSlip = GetBetSlip();

            var success = this.userBets.PlaceBets(betSlip.GetBets(), amount, User.Identity.Name);

            if (!success)
            {
                ModelState.AddModelError("", ErrorMessages.InsufficientAccountBalance);
            }

            betSlip.Clear();
            SetBetSlip(betSlip);

            return RedirectToAction(nameof(Slip));
        }

        [HttpPost]
        public IActionResult RemoveBet(int coefficientId, BetType betType)
        {
            var betSlip = GetBetSlip();

            betSlip.Remove(coefficientId, betType);
            SetBetSlip(betSlip);

            return RedirectToAction(nameof(Slip));
        }
    }
}
