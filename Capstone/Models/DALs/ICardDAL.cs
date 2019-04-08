﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models.DALs
{
    public interface ICardDAL
    {
        string ConnectionString { get; }

        bool AddCardToDeck(int deckID, Card card);
        List<Card> GetCardsByDeckId(int deckId);
    }
}