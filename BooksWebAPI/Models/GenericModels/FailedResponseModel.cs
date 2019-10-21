using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksWebAPI.Models
{
    public class FailedResponseModel
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
