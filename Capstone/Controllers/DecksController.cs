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
        private ICardDAL cardSqlDAL;

        public DecksController(IDeckDAL decksSqlDAL, ICardDAL cardSqlDAL)
        {
            this.decksSqlDAL = decksSqlDAL;
            this.cardSqlDAL = cardSqlDAL;
        }

        public IActionResult Index()
        {
            return View();
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

        [HttpPost]
        public IActionResult AddCard(Card newCard, int deckId)
        {
            newCard.DeckID = deckId;
            cardSqlDAL.AddCardToDeck(newCard.DeckID, newCard);
            return RedirectToAction("ViewDeck", new { deckId = newCard.DeckID });
        }

        [HttpGet]
        public IActionResult AddCard(int deckID)
        {
            Card card = new Card();
            card.DeckID = deckID;
            return View(card);
        }
    }
}