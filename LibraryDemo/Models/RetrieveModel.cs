using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryDemo.Models
{
    public enum RetrieveType
    {
        UserName,
        Email
    }

    public class RetrieveModel
    {
        [Required]
        public RetrieveType RetrieveWay { get;set; }
        [Required]
        public string Account { get; set; }
    }
}
