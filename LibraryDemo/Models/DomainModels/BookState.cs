using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryDemo.Models.DomainModels
{
    public enum BookState
    {
        /// <summary>
        /// 可借阅
        /// </summary>
        [Display(Name = "正常")]
        Normal,

        /// <summary>
        /// 馆内阅览
        /// </summary>
        [Display(Name = "馆内阅览")]
        Readonly,

        /// <summary>
        /// 已借出
        /// </summary>
        [Display(Name = "已借出")]
        Borrowed,

        /// <summary>
        /// 被续借
        /// </summary>
        [Display(Name = "被续借")]
        ReBorrowed,

        /// <summary>
        /// 被预约
        /// </summary>
        [Display(Name = "被预约")]
        Appointed        
    }
}
