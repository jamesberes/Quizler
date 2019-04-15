using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models.DALs
{
    public interface ICardDAL
    {
        string ConnectionString { get; }

        Card AddCardToDeck(Card card);
        List<Card> GetCardsByDeckId(int deckId);
        Card UpdateCard(Card updatedCard);
        Card GetCardById(int cardId);
        bool DeleteCard(int cardId);
        HashSet<int> SearchForCard(Tag tag);
        List<Card> ViewAllAdminCards();
        List<Card> AddCardListToDeck(List<Card> cards);

    }
}
