using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LibraryDemo.Models.DomainModels
{
    public class Student:IdentityUser
    {
        /// <summary>
        /// 学号
        /// </summary>
        [ProtectedPersonalData]
        [RegularExpression("[UIA]\\d{9}")]    
        public override string UserName { get; set; }

        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 学位，用来限制借书数目
        /// </summary>
        [Required]
        public Degrees Degree { get; set; }
        
        /// <summary>
        /// 最大借书数目
        /// </summary>
        [Required]        
        public int MaxBooksNumber { get; set; }        

        /// <summary>
        /// 已借图书
        /// </summary>
        public ICollection<Book> KeepingBooks { get; set; }

        /// <summary>
        /// 预约的书
        /// </summary>
        public string AppointingBookBarCode { get; set; }

        [StringLength(14,MinimumLength = 11)]
        public override string PhoneNumber { get; set; }

        /// <summary>
        /// 罚款
        /// </summary>
        public decimal Fine { get; set; }
    }
}
