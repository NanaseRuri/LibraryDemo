using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LibraryDemo.Models
{
    public enum LoginType
    {
        UserName,
        Email,
        Phone
    }

    public class LoginModel
    {
        [Required(ErrorMessage = "请输入您的学号 / 邮箱 / 手机号码")]
        [Display(Name = "学号 / 邮箱 / 手机号码")]
        public string Account { get; set; }

        [Required(ErrorMessage = "请输入您的密码")]
        [UIHint("password")]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Required]
        public LoginType LoginType { get; set; }
    }
}
