using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BooksWebAPI.Data;
using BooksWebAPI.Data.DbModels;
using BooksWebAPI.Models;
using BooksWebAPI.Models.Category;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BooksWebAPI.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly BooksDbContext dbContext;

        public CategoryService(BooksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<CategoryResultModel> CreateCategory(string name, string description)
        {
            CategoryResultModel result = new CategoryResultModel();

            if (await this.dbContext.Categories.AnyAsync(p => p.Name.ToLower().Trim() == name.ToLower().Trim()))
            {
                result.Success = false;
                result.Errors = new[] { "Category with this name already exist." };
                return result;
            }

            Category category = new Category
            {
                Id = Guid.NewGuid(),
                Name = name.Trim(),
                Description = description.Trim()
            };

            await this.dbContext.Categories.AddAsync(category);
            await this.dbContext.SaveChangesAsync();

            result.Name = category.Name;
            result.Success = true;

            return result;
        }

        public async Task<CategoryResultModel> UpdateCategory(Guid id, string name, string description)
        {
            CategoryResultModel result = new CategoryResultModel();

            Category category = await this.dbContext.Categories.FirstOrDefaultAsync(p => p.Id == id);

            if (category == null)
            {
                result.Success = false;
                result.Errors = new[] { "Category with this Id not exist." };
                return result;
            }

            if (category.Name != name &&  await this.dbContext.Categories.AnyAsync(p => p.Name.ToLower().Trim() == name.ToLower().Trim() && p.Id != id))
            {
                result.Success = false;
                result.Errors = new[] { "Category with this name already exist." };
                return result;
            }

            this.dbContext.Categories.Attach(category);

            category.Name = name.Trim();
            category.Description = description.Trim();

            await this.dbContext.SaveChangesAsync();

            result.Name = category.Name;
            result.Success = true;

            return result;
        }

        public async Task<CategoryResultModel> DeleteCategory(Guid id)
        {
            CategoryResultModel result = new CategoryResultModel();

            Category category = await this.dbContext.Categories.FirstOrDefaultAsync(p => p.Id == id);

            if (category == null)
            {
                result.Success = false;
                result.Errors = new[] { "Category with this Id not exist." };
                return result;
            }

            this.dbContext.Categories.Remove(category);

            await this.dbContext.SaveChangesAsync();

            result.Name = category.Name;
            result.Success = true;

            return result;
        }

        public async Task<CategoryEditModel> GetCategory(Guid id)
            => await this.dbContext
                         .Categories
                         .Where(x => x.Id == id)
                         .Select(x => new CategoryEditModel() 
                         {
                             Id = x.Id, 
                             Name = x.Name, 
                             Description = x.Description 
                         })
                         .FirstOrDefaultAsync();

        public async Task<IEnumerable<CategoryEditModel>> GetCategories()
            => await this.dbContext
                         .Categories
                         .Select(x => new CategoryEditModel()
                         {
                             Id = x.Id,
                             Name = x.Name,
                             Description = x.Description
                         })
                         .ToListAsync();

        public async Task<IEnumerable<GenericComboBox>> GetCategoriesInfo()
             => await this.dbContext
                          .Categories
                          .Select(x => new GenericComboBox()
                          {
                              Id = x.Id,
                              Name = x.Name,
                          })
                          .ToListAsync();
    }
}
