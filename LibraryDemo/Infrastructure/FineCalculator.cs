using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryDemo.Models.DomainModels;

namespace LibraryDemo.Infrastructure
{
    public class FineCalculator:IFineCalculate
    {
        public decimal Calculate(IQueryable<Book> maturedBooks)
        {
            throw new NotImplementedException();
        }
    }
}
