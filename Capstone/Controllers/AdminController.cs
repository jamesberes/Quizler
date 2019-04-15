using Capstone.Models;
using Capstone.Models.DALs;
using Capstone.Models.View_Models;
using Capstone.Providers.Auth;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Capstone.Controllers
{
    public class AdminController : Controller
    {
        private IDeckDAL decksSqlDAL;
        private ICardDAL cardSqlDAL;
        private ITagDAL tagSqlDAL;
        private IUsersDAL userSqlDAL;
        private IAuthProvider authProvider;

        public AdminController(IDeckDAL decksSqlDAL, ICardDAL cardSqlDAL, ITagDAL tagSqlDAL, IUsersDAL userSqlDAL, IAuthProvider authProvider)
        {
            this.decksSqlDAL = decksSqlDAL;
            this.cardSqlDAL = cardSqlDAL;
            this.tagSqlDAL = tagSqlDAL;
            this.userSqlDAL = userSqlDAL;
            this.authProvider = authProvider;
        }

        [AuthorizationFilter("User")]
        public IActionResult Index()
        {
            if (authProvider.IsAdmin())
            {
                //returns view with table of all the decks up for review
                List<Deck> decks = decksSqlDAL.GetAllDecksForReview();
                //should be able to switch a list from for_review to public/private
                return View(decks);
            }
            else
            {
                return NotFound();
            }
        }
    }
}