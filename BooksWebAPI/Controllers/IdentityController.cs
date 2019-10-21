using BooksWebAPI.Models;
using BooksWebAPI.Models.User;
using BooksWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebAPI.Controllers
{
    [Route("api/identity")]
    public class IdentityController : Controller
    {
        private readonly IIdentityService identityService;

        public IdentityController(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationModel registrationModel)
        {
            if (ModelState.IsValid)
            {
                UserResultModel authResponse = await identityService.RegisterAsync(registrationModel.Email, registrationModel.Password);

                if (!authResponse.Success)
                {
                    FailedResponseModel badResponse = new FailedResponseModel()
                    {
                        Errors = authResponse.Errors
                    };

                    return BadRequest(badResponse);
                }

                UserSuccessResponseModel successResponse = new UserSuccessResponseModel()
                {
                    Token = authResponse.Token
                };

                return Ok(successResponse);
            }

            return BadRequest(new FailedResponseModel { Errors = ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)) });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                UserResultModel authResponse = await identityService.LoginAsync(loginModel.Email, loginModel.Password);

                if (!authResponse.Success)
                {
                    FailedResponseModel badResponse = new FailedResponseModel()
                    {
                        Errors = authResponse.Errors
                    };

                    return BadRequest(badResponse);
                }

                UserSuccessResponseModel successResponse = new UserSuccessResponseModel()
                {
                    Token = authResponse.Token
                };

                return Ok(successResponse);
            }

            return BadRequest(new FailedResponseModel { Errors = ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)) });
        }
    }
}
