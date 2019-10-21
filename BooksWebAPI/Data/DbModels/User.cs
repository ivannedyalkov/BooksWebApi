using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebAPI.Data.DbModels
{
    public class User : IdentityUser
    {
        public ICollection<XRefUserBook> UserBooks { get; set; }
    }
}
