using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models.View_Models
{
    public class OtherUsersDeckViewModel
    {
        public string DeckOwnerName { get; set; }
        public List<SelectListItem> UserDecksSelectList { get; set; }
        public Card Card { get; set; }
        public Deck Deck { get; set; }

        public OtherUsersDeckViewModel()
        {
            UserDecksSelectList = new List<SelectListItem>();
        }
    }
}
