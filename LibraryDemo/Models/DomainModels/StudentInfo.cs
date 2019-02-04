using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryDemo.Models.DomainModels
{
    public class StudentInfo
    {
        [Key]
        public string UserName { get; set; }

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

        [StringLength(14, MinimumLength = 11)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 罚款
        /// </summary>
        public decimal Fine { get; set; }        
    }
}