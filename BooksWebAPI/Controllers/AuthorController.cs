using BooksWebAPI.Models;
using BooksWebAPI.Models.Author;
using BooksWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebAPI.Controllers
{
    [Route("api/author")]
    public class AuthorController : Controller
    {
        private readonly IAuthorService authorService;

        public AuthorController(IAuthorService authorService)
        {
            this.authorService = authorService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] AuthorCreateModel author)
        {
            if (ModelState.IsValid)
            {
                AuthorResultModel response = await this.authorService.CreateAuthor(author.Name, author.ImageUrl, author.Website, author.Description, author.Books);

                if (!response.Success)
                {
                    FailedResponseModel badResponse = new FailedResponseModel()
                    {
                        Errors = response.Errors
                    };

                    return BadRequest(badResponse);
                }

                AuthorSuccessResponseModel successResponse = new AuthorSuccessResponseModel()
                {
                    Name = response.Name
                };

                return Ok(successResponse);
            }

            return BadRequest(new FailedResponseModel { Errors = ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)) });
        }

        [HttpPut("update")]
        public async Task<IActionResult> Create([FromBody] AuthorEditModel author)
        {
            if (ModelState.IsValid)
            {
                AuthorResultModel response = await this.authorService.UpdateAuthor(author.Id, author.Name, author.ImageUrl, author.Website, author.Description, author.Books);

                if (!response.Success)
                {
                    FailedResponseModel badResponse = new FailedResponseModel()
                    {
                        Errors = response.Errors
                    };

                    return BadRequest(badResponse);
                }

                AuthorSuccessResponseModel successResponse = new AuthorSuccessResponseModel()
                {
                    Name = response.Name
                };

                return Ok(successResponse);
            }

            return BadRequest(new FailedResponseModel { Errors = ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)) });
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            AuthorResultModel response = await this.authorService.DeleteAuthor(id);

            if (!response.Success)
            {
                FailedResponseModel badResponse = new FailedResponseModel()
                {
                    Errors = response.Errors
                };

                return BadRequest(badResponse);
            }

            AuthorSuccessResponseModel successResponse = new AuthorSuccessResponseModel()
            {
                Name = response.Name
            };

            return Ok(successResponse);
        }

        [HttpGet("list-info")]
        public async Task<IActionResult> GetInfoList()
             => Ok(await this.authorService.GetAuthorsInfo());

        [HttpGet("edit")]
        public async Task<IActionResult> GetEdit(Guid id)
             => Ok(await this.authorService.GetAuthor(id));

        [HttpGet("list")]
        public async Task<IActionResult> GetList()
             => Ok(await this.authorService.GetAuthors());
    }
}
