using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models.View_Models
{
    public class TagViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CardId { get; set; }

        public int DeckId { get; set; }
    }
}
