using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using LibraryDemo.Models.DomainModels;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace LibraryDemo.Models.DomainModels
{
    /// <summary>
    /// 用于借阅的书籍信息
    /// </summary>
    public class Book
    {                                
        /// <summary>
        /// 二维码
        /// </summary>
        [Key]
        public string BarCode { get; set; }

        public string ISBN { get; set; }

        /// <summary>
        /// 书名
        /// </summary>
        [Required]
        public string Name { get; set; }         

        /// <summary>
        /// 取书号
        /// </summary>
        public string FetchBookNumber { get; set; }

        /// <summary>
        /// 所在书架
        /// </summary>
        //public Bookshelf Bookshelf { get; set; }
        public ICollection<BookMiddle> BookMiddles { get; set; }

        /// <summary>
        /// 借出时间
        /// </summary>
        public DateTime? BorrowTime { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime? MatureTime { get; set; } 

        /// <summary>
        /// 预约最晚借书日期
        /// </summary>
        public DateTime? AppointedLatestTime { get; set; }

        /// <summary>
        /// 借阅状态
        /// </summary>
        public BookState State { get; set; }

        /// <summary>
        /// 持有者，指定外键
        /// </summary>
        public Student Keeper { get; set; }
    }
}
