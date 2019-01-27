using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryDemo.Models.DomainModels;

namespace LibraryDemo.Models
{
    public class BookListViewModel
    {
        public IEnumerable<BookDetails> BookDetails { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
