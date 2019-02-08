using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LibraryDemo.Models.DomainModels
{
    public class AppointmentOrLending
    {
        public Book Book { get; set; }
        public string BookId { get; set; }
        public StudentInfo Student { get; set; }
        public string StudentId { get; set; }
        public DateTime? AppointingDateTime { get; set; }
    }
}
