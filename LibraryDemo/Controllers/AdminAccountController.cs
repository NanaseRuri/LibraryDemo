using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public AdminAccountController(UserManager<Student> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            ICollection<Student> students = _userManager.Users.ToList();
            return View(students);
        }

        [HttpPost]
        public async Task<JsonResult> AddStudent([FromBody]Student student)
        {            
            if (_userManager.CreateAsync(student,"123456").Result.Succeeded)
            {
                return await AddedStudent(student.UserName);
            }

                return Json("Failed");
        }

        public async Task<JsonResult> AddedStudent(string userName)
        {
            Student student=await _userManager.Users.FirstOrDefaultAsync(s => s.UserName == userName);
            return Json(new
            {
                userName = student.UserName,
                name = student.Name,
                degree = student.Degree == Degrees.CollegeStudent ? "本科生" : (student.Degree == Degrees.Postgraduate ? "研究生" : "博士生"),
                phoneNumber = student.PhoneNumber,
                email = student.Email,
                maxBooksNumber = student.MaxBooksNumber
            });
        }

        public JsonResult GetStudentData()
        {
            var students = _userManager.Users.Select(s =>new
            {
                userName=s.UserName,
                name=s.Name,
                degree=s.Degree==Degrees.CollegeStudent?"本科生":(s.Degree==Degrees.Postgraduate?"研究生":"博士生"),
                phoneNumber = s.PhoneNumber,
                email = s.Email,
                maxBooksNumber = s.MaxBooksNumber                
            });            
            return Json(students);
        }

        [HttpPost]
        public async Task<JsonResult> RemoveStudent([FromBody]IEnumerable<string> userNames)
        {
            Student removedStudent;
            foreach (var userName in userNames)
            {
                removedStudent =await _userManager.FindByNameAsync(userName);
                if (removedStudent!=null)
                {
                    await _userManager.DeleteAsync(removedStudent);
                }                
            }
            return GetStudentData();
        }
    }
}