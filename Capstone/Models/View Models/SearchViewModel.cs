using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models.View_Models
{
    public class SearchViewModel
    {
        public List<Tag> SearchTerms { get; set; }
        public List<Card> SearchResults { get; set; }
        public List<SelectListItem> UserDecksSelectList { get; set; }
        public Card Card { get; set; }

        public SearchViewModel()
        {
            SearchTerms = new List<Tag>();
            SearchResults = new List<Card>();
            UserDecksSelectList = new List<SelectListItem>();
        }
    }
}
