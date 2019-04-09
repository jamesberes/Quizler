﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Card
    {
        public int Id { get; set; }

        [Display(Name = "Front of Card")]
        [Required(ErrorMessage = "Please put something on the front of the card")]
        public string Front { get; set; }

        [Display(Name = "Back of Card")]
        [Required(ErrorMessage = "Please put something on the back of the card")]
        public string Back { get; set; }

        public string ImageURL { get; set; } = "";

        public int DeckId { get; set; }

        public int CardOrder { get; set; } = 1;

        [Display(Name = @"Search tags")]
        public List<Tag> Tags { get; set; }
    }
}
