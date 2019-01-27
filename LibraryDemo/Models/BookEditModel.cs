using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryDemo.Models.DomainModels;

namespace LibraryDemo.Models
{
    public class BookEditModel
    {
        public BookDetails BookDetails { get; set; }
        public IEnumerable<Book> Books { get; set; }
    }
}
