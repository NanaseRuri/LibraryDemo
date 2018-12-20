using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryDemo.Models.DomainModels;

namespace LibraryDemo.Infrastructure
{
    interface IFineCalculate
    {
        decimal Calculate(IQueryable<Book> maturedBooks);
    }
}
