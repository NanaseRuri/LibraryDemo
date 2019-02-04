using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryDemo.Models.DomainModels;

namespace LibraryDemo.Models
{
    public class PersonalInfoViewModel
    {
        public Student Student { get; set; }
        public Book BookingBook { get; set; }
    }
}
