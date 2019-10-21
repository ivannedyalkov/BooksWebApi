using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebAPI.Models.Category
{
    public class CategoryCreateModel
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(512)]
        public string Description { get; set; }
    }
}
