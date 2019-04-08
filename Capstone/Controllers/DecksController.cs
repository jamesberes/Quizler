using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Capstone.Models;
using Capstone.Models.DALs;

namespace Capstone.Controllers
{
    public class DecksController : Controller
    {
        private IDeckDAL decksSqlDAL;

        public DecksController(IDeckDAL decksSqlDAL)
        {
            this.decksSqlDAL = decksSqlDAL;
        }

        public IActionResult Index(int userId = 1)
        {
            List<Deck> decks = decksSqlDAL.GetDecksbyUserId(userId);
            return View(decks);
        }

        [HttpGet]
        public IActionResult CreateDeck()
        {
            Deck deck = new Deck();
            return View(deck);
        }

        [HttpPost]
        public IActionResult CreateDeck(Deck newDeck)
        {
            int deckId = decksSqlDAL.CreateDeck(newDeck);
            if (deckId != 0)
            {
                newDeck.Cards = new List<Card>();
                return RedirectToAction("ViewDeck", new { deckId = newDeck.Id });
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult ViewDeck(int deckId)
        {
            Deck deck = decksSqlDAL.GetDeckById(deckId);
            return View(deck);
        }
    }
}