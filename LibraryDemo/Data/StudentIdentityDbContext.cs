using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryDemo.Models.DomainModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryDemo.Data
{
    public class StudentIdentityDbContext:IdentityDbContext<Student>
    {
        public StudentIdentityDbContext(DbContextOptions<StudentIdentityDbContext> options) : base(options)
        {
        }
    }
}
