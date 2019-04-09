using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models.DALs
{
    public interface IDeckDAL
    {
        int CreateDeck(Deck newDeck);
        //Deck GetRandomDeck();
        Deck GetDeckById(int deckId);
        List<Deck> GetDecksbyUserId(int userId);
        Deck UpdateDeck(Deck updatedDeck);
        int GetNextCardOrder(int deckId);
        bool DeleteDeck(int deckId);
    }
}
