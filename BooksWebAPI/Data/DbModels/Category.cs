using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebAPI.Data.DbModels
{
    public class Category
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(512)]
        public string Description { get; set; }

        public ICollection<XRefBookCategory> BookCategories { get; set; }
    }
}
