using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryDemo.Models.DomainModels
{
    public enum BookState
    {
        /// <summary>
        /// 可借阅
        /// </summary>
        Normal,

        /// <summary>
        /// 管内阅览
        /// </summary>
        Readonly,

        /// <summary>
        /// 已借出
        /// </summary>
        Borrowed,

        /// <summary>
        /// 被续借
        /// </summary>
        ReBorrowed,

        /// <summary>
        /// 被预约
        /// </summary>
        Appointed        
    }
}
