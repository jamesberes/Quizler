using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models.View_Models
{
    public class TagViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Tag")]
        public string Name { get; set; }

        public int CardId { get; set; }

        public int DeckId { get; set; }
    }
}
