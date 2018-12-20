using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryDemo.Models;
using LibraryDemo.Models.DomainModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace LibraryDemo.Controllers
{
    [Authorize]
    public class StudentAccountController : Controller
    {
        private UserManager<Student> _userManager;
        private SignInManager<Student> _signInManager;

        public StudentAccountController(UserManager<Student> studentManager, SignInManager<Student> signInManager)
        {
            _userManager = studentManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("AccountInfo");
            }

            LoginModel loginInfo = new LoginModel();
            ViewBag.returnUrl = returnUrl;
            return View(loginInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel loginInfo, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Student student =await GetStudentByLoginModel(loginInfo);

                if (student == null)
                {
                    return View(loginInfo);
                }
                SignInResult signInResult = await _signInManager.PasswordSignInAsync(student, loginInfo.Password, false, false);

                if (signInResult.Succeeded)
                {
                    return Redirect(returnUrl ?? "/StudentAccount/"+nameof(AccountInfo));
                }

                ModelState.AddModelError("", "账号或密码错误");
                            
            }

            return View(loginInfo);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return View("Login");
        }

        public IActionResult ModifyPassword()
        {
            ModifyModel model=new ModifyModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModifyPassword(ModifyModel model)
        {
            if (ModelState.IsValid)
            {
                string username = HttpContext.User.Identity.Name;
                var student = _userManager.Users.FirstOrDefault(s => s.UserName == username);
                var result =
                    await _userManager.ChangePasswordAsync(student, model.OriginalPassword, model.ModifiedPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignOutAsync();
                    return View("ModifySuccess");
                }
                ModelState.AddModelError("","原密码输入错误");
            }
            return View(model);
        }

        [Authorize]
        public IActionResult AccountInfo()
        {
            return View(CurrentAccountData());
        }


        async Task<Student> GetStudentByLoginModel(LoginModel loginInfo)
        {
            Student student=new Student();
            switch (loginInfo.LoginType)
            {
                case LoginType.UserName:
                    student = await _userManager.FindByNameAsync(loginInfo.Account);
                    break;
                case LoginType.Email:
                    student = await _userManager.FindByEmailAsync(loginInfo.Account);
                    break;
                case LoginType.Phone:
                    student = _userManager.Users.First(s => s.PhoneNumber == loginInfo.Account);
                    break;
                default:
                    student = null;
                    break;
            }
            return student;
        }

        Dictionary<string, object> CurrentAccountData()
        {
            var userName = HttpContext.User.Identity.Name;
            var user = _userManager.FindByNameAsync(userName).Result;

            return new Dictionary<string, object>()
            {
                ["学号"]=userName,
                ["姓名"]=user.Name,
                ["邮箱"]=user.Email,
                ["手机号"]=user.PhoneNumber,
            };
        }
    }
}