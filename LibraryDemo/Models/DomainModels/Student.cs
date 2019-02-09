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
        [Display(Name = "学号")]
        public override string UserName { get; set; }

        [Display(Name = "手机号")]
        [StringLength(14, MinimumLength = 11)]
        public override string PhoneNumber { get; set; }

        [Display(Name = "姓名")]
        public string Name { get; set; }
        [Display(Name = "学历")]
        public Degrees Degree { get; set; }
        [Display(Name = "最大借书数目")]
        public int MaxBooksNumber { get; set; }
    }
}
