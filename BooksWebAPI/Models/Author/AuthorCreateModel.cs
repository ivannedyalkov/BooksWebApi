using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebAPI.Models.Author
{
    public class AuthorCreateModel
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(1024)]
        public string ImageUrl { get; set; }

        [StringLength(1024)]
        public string Website { get; set; }

        [StringLength(512)]
        public string Description { get; set; }

        public IEnumerable<GenericComboBox> Books { get; set; }
    }
}
