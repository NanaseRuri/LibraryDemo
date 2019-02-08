using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using LibraryDemo.Models.DomainModels;
using Microsoft.EntityFrameworkCore;
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
        [Display(Name = "二维码")]
        [Required(ErrorMessage = "未填写二维码")]
        public string BarCode { get; set; }

        public string ISBN { get; set; }

        /// <summary>
        /// 书名
        /// </summary>
        [Display(Name = "书名")]
        public string Name { get; set; }

        /// <summary>
        /// 取书号
        /// </summary>
        [Display(Name = "取书号")]
        public string FetchBookNumber { get; set; }

        /// <summary>
        /// 所在书架
        /// </summary>
        public Bookshelf Bookshelf { get; set; }

        [Display(Name = "书架号")]
        public int BookshelfId { get; set; }

        /// <summary>
        /// 借出时间
        /// </summary>
        [Display(Name = "借出时间")]
        public DateTime? BorrowTime { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        [Display(Name = "到期时间")]
        public DateTime? MatureTime { get; set; }

        /// <summary>
        /// 预约最晚借书日期
        /// </summary>
        [Display(Name = "预约取书时间")]
        public DateTime? AppointedLatestTime { get; set; }

        /// <summary>
        /// 借阅状态
        /// </summary>
        [Display(Name = "书籍状态")]
        public BookState State { get; set; }

        /// <summary>
        /// 持有者，指定外键
        /// </summary>
        public StudentInfo Keeper { get; set; }
        [Display(Name = "持有者学号")]
        public string KeeperId{ get; set; }

        [Display(Name = "位置")]
        public string Location { get; set; }

        [Display(Name = "分类")]
        public string Sort { get; set; }

        public ICollection<AppointmentOrLending> Appointments { get; set; }
    }
}
