using BooksWebAPI.Models;
using BooksWebAPI.Models.Book;
using BooksWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebAPI.Controllers
{
    [Route("api/book")]
    public class BookController : Controller
    {
        private readonly IBookService bookService;

        public BookController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] BookCreateModel book)
        {
            if (ModelState.IsValid)
            {
                BookResultModel response = await this.bookService.CreateBook(book.Name, book.Description, book.ImageUrl, book.Author,  book.ReleaseDate, book.Categories);

                if (!response.Success)
                {
                    FailedResponseModel badResponse = new FailedResponseModel()
                    {
                        Errors = response.Errors
                    };

                    return BadRequest(badResponse);
                }

                BookSuccessResponseModel successResponse = new BookSuccessResponseModel()
                {
                    Name = response.Name
                };

                return Ok(successResponse);
            }

            return BadRequest(new FailedResponseModel { Errors = ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)) });
        }

        [HttpPut("update")]
        public async Task<IActionResult> Create([FromBody] BookEditModel book)
        {
            if (ModelState.IsValid)
            {
                BookResultModel response = await this.bookService.UpdateBook(book.Id, book.Name, book.Description, book.ImageUrl, book.Author, book.ReleaseDate, book.Categories);

                if (!response.Success)
                {
                    FailedResponseModel badResponse = new FailedResponseModel()
                    {
                        Errors = response.Errors
                    };

                    return BadRequest(badResponse);
                }

                BookSuccessResponseModel successResponse = new BookSuccessResponseModel()
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
            BookResultModel response = await this.bookService.DeleteBook(id);

            if (!response.Success)
            {
                FailedResponseModel badResponse = new FailedResponseModel()
                {
                    Errors = response.Errors
                };

                return BadRequest(badResponse);
            }

            BookSuccessResponseModel successResponse = new BookSuccessResponseModel()
            {
                Name = response.Name
            };

            return Ok(successResponse);
        }

        [HttpGet("list-info")]
        public async Task<IActionResult> GetInfoList()
             => Ok(await this.bookService.GetBooksInfo());

        [HttpGet("edit")]
        public async Task<IActionResult> GetEdit(Guid id)
             => Ok(await this.bookService.GetBook(id));

        [HttpGet("list")]
        public async Task<IActionResult> GetList()
             => Ok(await this.bookService.GetBooks());
    }
}
