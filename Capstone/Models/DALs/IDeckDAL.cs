using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models.DALs
{
    public interface IDeckDAL
    {
        //Deck GetRandomDeck();
        int CreateDeck(Deck newDeck);
        Deck GetDeckById(int deckId);
        List<Deck> GetDecksbyUserId(int userId);
        Deck UpdateDeck(Deck updatedDeck);
        int GetNextCardOrder(int deckId);
        bool DeleteDeck(int deckId);
        List<SelectListItem> GetUserDecksSelectList(int userId);
        List<Deck> LazyLoadDecks(int userId, int startId);
    }
}
