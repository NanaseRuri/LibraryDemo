using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryDemo.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace LibraryDemo.TagHelpers
{
    [HtmlTargetElement("LoginType")]
    public class LoginTypeTagHelper:TagHelper
    {
        public string[] LoginType { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            foreach (var loginType in LoginType)
            {
                switch (loginType)
                {
                    case "UserName": output.Content.AppendHtml($"<option selected=\"selected/\" value=\"{loginType}\">账号</option>");
                        break;
                    case "Email": output.Content.AppendHtml(GetOption(loginType, "邮箱"));
                        break;
                    case "Phone": output.Content.AppendHtml(GetOption(loginType, "手机号码"));
                        break;
                    default: break;
                }                
            }            
            return Task.CompletedTask;
        }

        private static string GetOption(string loginType,string innerText)
        {
            return $"<option value=\"{loginType}\">{innerText}</option>";
        }
    }
}
