using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryDemo.Models.DomainModels
{
    public enum Degrees
    {
        [Display(Name = "本科生")]
        CollegeStudent,
        [Display(Name = "研究生")]
        Postgraduate,
        [Display(Name = "博士生")]
        DoctorateDegree
    }
}
