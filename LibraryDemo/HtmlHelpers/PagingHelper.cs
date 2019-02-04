using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using LibraryDemo.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace LibraryDemo.HtmlHelpers
{
    public static class PagingHelper
    {
        public static HtmlString PageLinks(this IHtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            StringWriter writer=new StringWriter();
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder tag=new TagBuilder("a");
                tag.MergeAttribute("href",pageUrl(i));
                tag.InnerHtml.AppendHtml(i.ToString());
                if (i==pagingInfo.CurrentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                tag.WriteTo(writer,HtmlEncoder.Default);
            }
            return new HtmlString(writer.ToString());
        }
    }
}
