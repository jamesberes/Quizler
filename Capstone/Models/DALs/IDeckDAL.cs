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
        string GetUserNameFromDeckId(int deckId);
        List<Deck> LazyLoadDecks(int userId, int startId);
        List<Deck> LazyLoadPublicDecks(int startId);
        bool SetDeckForReferral(int deckId, bool bit);
        bool MakePrivate(int deckId);
        bool MakePublic(int deckId);
        List<Deck> GetAllDecksForReview();
        List<Deck> GetAllPublicDecks();
    }
}
