using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebAPI.Models.Book
{
    public class BookCreateModel
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(512)]
        public string Description { get; set; }

        [StringLength(1024)]
        public string ImageUrl { get; set; }

        public GenericComboBox Author { get; set; }

        [Column(TypeName = "SmallDateTime")]
        public DateTime? ReleaseDate { get; set; }

        public IEnumerable<GenericComboBox> Categories { get; set; }
    }
}
