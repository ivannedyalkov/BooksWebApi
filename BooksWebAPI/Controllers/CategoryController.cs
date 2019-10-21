using BooksWebAPI.Models;
using BooksWebAPI.Models.Category;
using BooksWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebAPI.Controllers
{
    [Route("api/category")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CategoryCreateModel category)
        {
            if (ModelState.IsValid)
            {
                CategoryResultModel response = await this.categoryService.CreateCategory(category.Name, category.Description);

                if (!response.Success)
                {
                    FailedResponseModel badResponse = new FailedResponseModel()
                    {
                        Errors = response.Errors
                    };

                    return BadRequest(badResponse);
                }

                CategorySuccessResponseModel successResponse = new CategorySuccessResponseModel()
                {
                    Name = response.Name
                };

                return Ok(successResponse);
            }

            return BadRequest(new FailedResponseModel { Errors = ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)) });
        }

        [HttpPut("update")]
        public async Task<IActionResult> Create([FromBody] CategoryEditModel category)
        {
            if (ModelState.IsValid)
            {
                CategoryResultModel response = await this.categoryService.UpdateCategory(category.Id, category.Name, category.Description);

                if (!response.Success)
                {
                    FailedResponseModel badResponse = new FailedResponseModel()
                    {
                        Errors = response.Errors
                    };

                    return BadRequest(badResponse);
                }

                CategorySuccessResponseModel successResponse = new CategorySuccessResponseModel()
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
            CategoryResultModel response = await this.categoryService.DeleteCategory(id);

            if (!response.Success)
            {
                FailedResponseModel badResponse = new FailedResponseModel()
                {
                    Errors = response.Errors
                };

                return BadRequest(badResponse);
            }

            CategorySuccessResponseModel successResponse = new CategorySuccessResponseModel()
            {
                Name = response.Name
            };

            return Ok(successResponse);
        }

        [HttpGet("list-info")]
        public async Task<IActionResult> GetInfoList()
             => Ok(await this.categoryService.GetCategoriesInfo());

        [HttpGet("edit")]
        public async Task<IActionResult> GetEdit(Guid id)
             => Ok(await this.categoryService.GetCategory(id));

        [HttpGet("list")]
        public async Task<IActionResult> GetList()
             => Ok(await this.categoryService.GetCategories());
    }
}
