using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models.DALs
{
    public interface ITagDAL
    {
        bool AddTagList(List<Tag> input);
        bool AddTag(Tag input);
        List<Tag> GetTagsForDeck(int deckId);
        List<Tag> GetTagsForCard(int cardId);
    }
}
