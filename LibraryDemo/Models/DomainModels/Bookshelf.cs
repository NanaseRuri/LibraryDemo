using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        //不自动增长
        [DatabaseGenerated(DatabaseGeneratedOption.None)] 
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
        public ICollection<Book> Books { get; set; }
    }
}
