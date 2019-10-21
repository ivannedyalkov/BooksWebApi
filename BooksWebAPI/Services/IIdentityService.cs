using BooksWebAPI.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebAPI.Services
{
    public interface IIdentityService
    {
        Task<UserResultModel> RegisterAsync(string email, string password);
        Task<UserResultModel> LoginAsync(string email, string password);
    }
}
