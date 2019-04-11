using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone.Models.DALs;
using Capstone.Models;

namespace Capstone.Providers.Auth
{
    /// <summary>
    /// An implementation of the IAuthProvider that saves data within session.
    /// </summary>
    public class SessionAuthProvider : IAuthProvider
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IUsersDAL userDAL;
        public static string SessionKey = "Auth_User";

        public SessionAuthProvider(IHttpContextAccessor contextAccessor, IUsersDAL userDAL)
        {
            this.contextAccessor = contextAccessor;
            this.userDAL = userDAL;
        }

        /// <summary>
        /// Gets at the session attached to the http request.
        /// </summary>
        ISession Session => contextAccessor.HttpContext.Session;

        /// <summary>
        /// Returns true if the user is logged in.
        /// </summary>
        public bool IsLoggedIn => !String.IsNullOrEmpty(Session.GetString(SessionKey));

        /// <summary>
        /// Signs the user in and saves their email in session.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool SignIn(string email, string password)
        {
            var user = userDAL.GetUser(email);
            var hashProvider = new HashProvider();                        
            
            if (user != null && hashProvider.VerifyPasswordMatch(user.Password, password, user.Salt))
            {                
                Session.SetString(SessionKey, user.Email);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Logs the user out by clearing their session data.
        /// </summary>
        public void LogOff()
        {
            Session.Clear();
        }

        /// <summary>
        /// Changes the current user's password.
        /// </summary>
        /// <param name="existingPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public bool ChangePassword(string existingPassword, string newPassword)
        {            
            var hashProvider = new HashProvider();
            var user = GetCurrentUser();
            
            // Confirm existing password match
            if (user != null && hashProvider.VerifyPasswordMatch(user.Password, existingPassword, user.Salt))
            {
                // Hash new password
                var newHash = hashProvider.HashPassword(newPassword);
                user.Password = newHash.Password;
                user.Salt = newHash.Salt;

                // Save into the db
                userDAL.UpdateUser(user);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the user using the current email in session.
        /// </summary>
        /// <returns></returns>
        public Users GetCurrentUser()
        {
            var email = Session.GetString(SessionKey);

            if (!String.IsNullOrEmpty(email))
            {
                return userDAL.GetUser(email);
            }
            
            return null;
        }

        /// <summary>
        /// Creates a new user and saves their email in session.
        /// </summary>
        /// <returns></returns>
        public bool Register(string displayName, string email, string password, string role)
        {
            bool result = false;
            var hashProvider = new HashProvider();
            var passwordHash = hashProvider.HashPassword(password);

            var user = new Users
            {
                DisplayName = displayName,
                Email = email,
                Password = passwordHash.Password,
                Salt = passwordHash.Salt,
                Role = role
            };

            result = userDAL.CreateUser(user);
            Session.SetString(SessionKey, user.Email);
            return result;
        }

        /// <summary>
        /// Checks to see if the user has a given role.
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public bool UserHasRole(string[] roles)
        {            
            var user = GetCurrentUser();
            return (user != null) && 
                roles.Any(r => r.ToLower() == user.Role.ToLower());
        }
    }
}
