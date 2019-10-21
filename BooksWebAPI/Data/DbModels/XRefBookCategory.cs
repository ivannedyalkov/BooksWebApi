using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebAPI.Data.DbModels
{
    public class XRefBookCategory
    {
        public Guid Id { get; set; }

        public Guid BookId { get; set; }

        public Book Book { get; set; }

        public Guid CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
