using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Deck
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DateCreated { get; set; }

        public bool PublicDeck { get; set; }

        public string User { get; set; }

        public bool ForReview { get; set; }

        public string Description { get; set; }

        public List<Card> Cards { get; set; }
    }
}
