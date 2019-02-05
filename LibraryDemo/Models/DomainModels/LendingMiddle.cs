using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryDemo.Models.DomainModels
{
    public class LendingMiddle
    {
        public Book Book { get; set; }
        public string BookId { get; set; }
        public StudentInfo Student { get; set; }
        public string StudentId { get; set; }
        public DateTime? BookingDateTime { get; set; }
    }
}
