using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BooksWebAPI.Models.User;
using BooksWebAPI.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BooksWebAPI.Services.Implementation
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> userManager;

        public IdentityService(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<UserResultModel> LoginAsync(string email, string password)
        {
            UserResultModel result = new UserResultModel();

            IdentityUser existingUser = await userManager.FindByEmailAsync(email);
            bool userHasValidPassword = await userManager.CheckPasswordAsync(existingUser, password);

            if (existingUser == null || !userHasValidPassword)
            {
                result.Success = false;
                result.Errors = new[] { "User/password combination is wrong." };
                return result; 
            }

            result.Success = true;
            result.Token = JWTGenerator.Create(existingUser);

            return result;
        }

        public async Task<UserResultModel> RegisterAsync(string email, string password)
        {
            UserResultModel result = new UserResultModel();

            IdentityUser existingUser = await userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                result.Success = false;
                result.Errors = new[] { "User with this email address already exists." };
                return result;
            }

            IdentityUser newUser = new IdentityUser()
            {
                Email = email,
                UserName = email,
            };

            IdentityResult createdUser = await userManager.CreateAsync(newUser, password);

            if (!createdUser.Succeeded)
            {
                result.Success = false;
                result.Errors = createdUser.Errors.Select(x => x.Description);
                return result;
            }
            
            result.Success = true;
            result.Token = JWTGenerator.Create(newUser);

            return result;
        }
    }
}
