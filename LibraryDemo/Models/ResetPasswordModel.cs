using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryDemo.Models
{
    public class ResetPasswordModel
    {
        public string Code { get; set; }

        public string UserId { get; set; }

        [Required]
        [Display(Name="密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "确认密码")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "两次密码不匹配")]
        public string ConfirmPassword { get; set; }
    }
}
