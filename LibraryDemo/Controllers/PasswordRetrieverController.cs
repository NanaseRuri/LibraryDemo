using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using LibraryDemo.Infrastructure;
using LibraryDemo.Models;
using LibraryDemo.Models.DomainModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LibraryDemo.Controllers
{
    public class PasswordRetrieverController : Controller
    {
        private UserManager<Student> _userManager;
        public EmailSender _emailSender;

        public PasswordRetrieverController(UserManager<Student> studentManager, EmailSender emailSender)
        {
            _userManager = studentManager;  
            _emailSender = emailSender;
        }

        public IActionResult Retrieve()
        {
            RetrieveModel model = new RetrieveModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RetrievePassword(RetrieveModel model)
        {
            bool sendResult=false;
            if (ModelState.IsValid)
            {
                Student student = new Student();
                switch (model.RetrieveWay)
                {
                    case RetrieveType.UserName:
                        student = await _userManager.FindByNameAsync(model.Account);
                        if (student != null)
                        {
                            string code = await _userManager.GeneratePasswordResetTokenAsync(student);
                            sendResult = await SendEmail(student.Id, code, student.Email);
                        }
                        break;
                    case RetrieveType.Email:
                        student = await _userManager.FindByEmailAsync(model.Account);
                        if (student != null)
                        {
                            string code = await _userManager.GeneratePasswordResetTokenAsync(student);
                            sendResult = await SendEmail(student.Id, code, student.Email);
                        }
                        break;
                }
                if (student == null)
                {
                    ViewBag.Error("用户不存在,请重新输入");
                    return View("Retrieve",model);
                }
            }
            ViewBag.Message = "已发送邮件至您的邮箱，请注意查收";
            ViewBag.Failed = "信息发送失败";
            return View(sendResult);
        }

        public IActionResult ResetPassword(string userId,string code)
        {
            ResetPasswordModel model=new ResetPasswordModel()
            {
                UserId = userId,
                Code = code
            };
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.FindByIdAsync(model.UserId);
                if (user!=null)
                {
                    var result = await _userManager.ResetPasswordAsync(user.Result, model.Code, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(ResetSuccess));
                    }
                }
            }
            return View(model);
        }

        public IActionResult ResetSuccess()
        {
            return View();
        }

        
        async Task<bool> SendEmail(string userId, string code, string mailAddress)
        {
            Student student = await _userManager.FindByIdAsync(userId);
            if (student!=null)
            {
                string url = Url.Action("ResetPassword","PasswordRetriever",new{userId=userId,code=code}, Url.ActionContext.HttpContext.Request.Scheme);
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"  请点击<a href=\"{url}\">此处</a>重置您的密码");
                MailMessage message = new MailMessage(from: "xxxxx@163.com", to: mailAddress, subject: "重置密码", body: sb.ToString());
                message.BodyEncoding=Encoding.UTF8;
                message.IsBodyHtml = true;
                try
                {
                    _emailSender.SmtpClient.Send(message);
                }
                catch (Exception e)
                {
                    return false;
                }

                return true;
            }
            return false;
        }
    }
}