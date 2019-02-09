using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryDemo.Models.DomainModels;

namespace LibraryDemo.Models
{
    public class StudentInfoViewModel
    {
        public Student Student { get; set; }
        public StudentInfo StudentInfo { get; set; }
    }
}
