using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LibraryDemo.Data;
using LibraryDemo.Infrastructure;
using LibraryDemo.Models;
using LibraryDemo.Models.DomainModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace LibraryDemo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminAccountController : Controller
    {
        private UserManager<Student> _userManager;
        private SignInManager<Student> _signInManager;

        public AdminAccountController(UserManager<Student> studentManager, SignInManager<Student> signInManager)
        {
            _userManager = studentManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View(CurrentAccountData());
        }



        Dictionary<string, object> CurrentAccountData()
        {
            var userName = HttpContext.User.Identity.Name;
            var user = _userManager.FindByNameAsync(userName).Result;

            return new Dictionary<string, object>()
            {
                ["学号"] = userName,
                ["姓名"] = user.Name,
                ["邮箱"] = user.Email,
                ["手机号"] = user.PhoneNumber,
            };
        }
    }
}