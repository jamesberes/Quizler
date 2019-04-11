using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone.Models;

namespace Capstone.Models.DALs
{
    public interface IUsersDAL
    {
        /// <summary>
        /// Retrieves a user from the system by email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Users GetUser(string email);

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user"></param>
        ///  /// <returns></returns>
        bool CreateUser(Users user);

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="user"></param>
        void UpdateUser(Users user);

        /// <summary>
        /// Deletes a user from the system.
        /// </summary>
        /// <param name="user"></param>
        void DeleteUser(Users user);
    }
}