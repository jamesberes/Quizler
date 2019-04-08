using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models.DALs
{
    interface ICardDAL
    {
        string ConnectionString { get; }

        bool AddCardToDeck(int deckID, Card card);
    }
}
