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
        private IAuthProvider authProvider;

        public AdminController(IDeckDAL decksSqlDAL, IAuthProvider authProvider)
        {
            this.decksSqlDAL = decksSqlDAL;
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

        public IActionResult ViewDeck(int deckId)
        {
            int? userId = authProvider.GetCurrentUser().Id;
            Deck deck = decksSqlDAL.GetDeckById(deckId);
            if (authProvider.IsAdmin())
            {
                return View("ViewDeckAdmin", deck);
            }
            else
            {
                return NotFound();
            }
        }
    }
}