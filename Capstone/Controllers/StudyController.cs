using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone.Models;
using Capstone.Models.DALs;
using Capstone.Providers.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Capstone.Controllers
{
    public class StudyController : Controller
    {
        private IAuthProvider authProvider;

        public StudyController(IAuthProvider authProvider)
        {
            this.authProvider = authProvider;
        }

        public IActionResult Index(int deckId)
        {
            Users currentUser = authProvider.GetCurrentUser();
            if (currentUser == null)
            {
                return RedirectToAction("login", "account");
            }
            else
            {
                return View(deckId);
            }
        }
    }
}