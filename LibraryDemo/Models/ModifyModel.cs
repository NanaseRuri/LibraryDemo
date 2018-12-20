using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryDemo.Models
{
    public class ModifyModel
    {
        [UIHint("password")]
        [Display(Name = "原密码")]
        [Required]
        public string OriginalPassword { get; set; }

        [Required]
        [Display(Name = "新密码")]
        [UIHint("password")]
        public string ModifiedPassword { get; set; }

        [Required]
        [Display(Name = "确认密码")]
        [UIHint("password")]
        [Compare("ModifiedPassword", ErrorMessage = "两次密码不匹配")]
        public string ConfirmedPassword { get; set; }
    }
}
