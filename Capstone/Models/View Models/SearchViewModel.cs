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

        public SearchViewModel()
        {
            SearchTerms = new List<Tag>();
            SearchResults = new HashSet<Card>();
        }
    }
}
