using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebAPI.Data.DbModels
{
    public class XRefUserBook
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public Guid BookId { get; set; }

        public Book Book { get; set; }
    }
}
