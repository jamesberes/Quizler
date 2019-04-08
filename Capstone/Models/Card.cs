using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Card
    {
        public int ID { get; set; }

        public string Front { get; set; }

        public string Back { get; set; }

        public string ImageURL { get; set; }

        public int DeckID { get; set; }

        public int CardOrder { get; set; }
    }
}
