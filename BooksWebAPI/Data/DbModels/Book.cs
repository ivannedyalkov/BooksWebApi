using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebAPI.Data.DbModels
{
    public class Book
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(512)]
        public string Description { get; set; }

        [StringLength(1024)]
        public string ImageUrl { get; set; }

        public Guid? AuthorId { get; set; }

        public Author Author { get; set; }

        [Column(TypeName = "SmallDateTime")]
        public DateTime? ReleaseDate { get; set; }

        public ICollection<XRefBookCategory> BookCategories { get; set; }

        public ICollection<XRefUserBook> UserBooks { get; set; }
    }
}
