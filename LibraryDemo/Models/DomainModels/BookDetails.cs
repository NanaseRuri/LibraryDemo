using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryDemo.Models.DomainModels
{
    /// <summary>
    /// 书籍自身的详细信息
    /// </summary>
    public class BookDetails
    {
        [Key]
        public string ISBN { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Press { get; set; }
        
        /// <summary>
        /// 出版时间
        /// </summary>
        [Required]
        public DateTime PublishDateTime{ get; set; }

        /// <summary>
        /// 书籍版本
        /// </summary>
        [Required]
        public int Version { get; set; }

        /// <summary>
        /// 载体形态，包括页数、媒介等信息
        /// </summary>
        public string SoundCassettes { get; set; }
    }
}
