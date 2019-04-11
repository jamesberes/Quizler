using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone.Models;
using Capstone.Models.DALs;
using Microsoft.AspNetCore.Mvc;

namespace Capstone.Controllers
{
    public class StudyController : Controller
    {
        private IDeckDAL decksSqlDAL;

        public StudyController(IDeckDAL decksSqlDAL)
        {
            this.decksSqlDAL = decksSqlDAL;
        }

        public IActionResult Index(int deckId)
        {
            return View(deckId);
        }
    }
}