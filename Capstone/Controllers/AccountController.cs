using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Capstone.Models.Account;
using Capstone.Providers.Auth;

namespace Capstone.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthProvider authProvider;
        public AccountController(IAuthProvider authProvider)
        {
            this.authProvider = authProvider;
        }

        //[AuthorizationFilter] // actions can be filtered to only those that are logged in -- or filtered to only those that have a certain role [array of roles]
        //
        // --> Checks if the role property attached to the user is in the given array --
        //      (defines user by grabbing the session key, which is set to be the email upon registration/login) --  
        // 
        // --> if user role not in given array, returns Status Code 403 as http result. 
        // --> if not logged in, there is no user, so no session key was set, which means none will be found. In this case, the http result will be a redirect to account/login. 
        //
        [AuthorizationFilter("Admin", "User")]
        [HttpGet]
        public IActionResult Index()
        {
            var user = authProvider.GetCurrentUser();
            return View(user);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            // Ensure the fields were filled out
            if (ModelState.IsValid)
            {
                // Check that they provided correct credentials
                bool validLogin = authProvider.SignIn(loginViewModel.Email, loginViewModel.Password);
                if (validLogin)
                {
                    // Redirect the user where you want them to go after successful login
                    return RedirectToAction("MyDecks", "Decks");
                }
            }

            return View(loginViewModel);
        }

        [HttpGet]
        public IActionResult LogOff()
        {
            // Clear user from session
            authProvider.LogOff();

            // Redirect the user where you want them to go after logoff
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel rvm)
        {
            if (ModelState.IsValid)
            {
                // Register them as a new user (and set default role)
                // When a user registeres they need to be given a role. If you don't need anything special
                // just give them "User".
                if (authProvider.Register(rvm.DisplayName, rvm.Email, rvm.Password, role: "User") == false)
                {
                    return RedirectToAction("Error", "Home");
                }

                // Redirect the user where you want them to go after registering
                return RedirectToAction("Index", "Home");
            }

            return View(rvm);
        }
    }
}