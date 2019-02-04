using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LibraryDemo.Models.DomainModels
{
    public class Student : IdentityUser
    {
        /// <summary>
        /// 学号
        /// </summary>
        [ProtectedPersonalData]
        [RegularExpression("[UIA]\\d{9}")]
        public override string UserName { get; set; }

        [StringLength(14, MinimumLength = 11)] public override string PhoneNumber { get; set; }

        public string Name { get; set; }
        public Degrees Degree { get; set; }
        public int MaxBooksNumber { get; set; }
    }
}
