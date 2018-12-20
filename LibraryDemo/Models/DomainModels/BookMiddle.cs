using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryDemo.Models.DomainModels
{
    public class BookMiddle
    {
        public int BookMiddleId { get; set; }
        public string BookId { get; set; }
        public int BookshelfId { get; set; }
        public Book Book { get; set; }
        public Bookshelf Bookshelf { get; set; }
    }
}
