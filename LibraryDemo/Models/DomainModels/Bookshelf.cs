using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryDemo.Models.DomainModels
{
    public class Bookshelf
    {
        /// <summary>
        /// 书架ID
        /// </summary>
        [Key]
        public int BookshelfId { get; set; }

        /// <summary>
        /// 书架的书籍类别
        /// </summary>

        [Required]
        public string Sort { get; set; }               
        /// <summary>
        /// 最小取书号
        /// </summary>
        [Required]
        public string MinFetchNumber { get; set; }
        [Required]
        public string MaxFetchNumber { get; set; }

        /// <summary>
        /// 书架位置
        /// </summary>
        [Required]
        public string Location { get; set; }

        /// <summary>
        /// 全部藏书
        /// </summary>
        //public ICollection<Book> Books { get; set; }
        public ICollection<BookMiddle> BookMiddles { get; set; }
    }
}
