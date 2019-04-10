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
        public HashSet<Card> SearchResults { get; set; }
        public List<SelectListItem> UserDecks { get; set; }

        public SearchViewModel()
        {
            SearchTerms = new List<Tag>();
            SearchResults = new HashSet<Card>();
            UserDecks = new List<SelectListItem>();
        }
    }
}
