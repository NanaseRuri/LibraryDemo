using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryDemo.Models.DomainModels;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace LibraryDemo.TagHelpers
{
    public class BookInfoTagHelper:TagHelper
    {
        public Book Book { get; set; }
        public bool? IsBookingBook { get; set; }
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (IsBookingBook.Value)
            {
                output.Content.AppendHtml($"<tr>" +
                                          $"<td>{Book.Name}</td>" +
                                          $"<td>{Book.BarCode}</td>" +
                                          $"<td>{GetState(Book.State)}</td>" +
                                          $"<td>{Book.AppointedLatestTime.Value.ToString("yyyy/M/dd")}"+
                                          $"<td>{Book.FetchBookNumber}</td>" +
                                          $"</tr>");
            }
            else
            {
                output.Content.AppendHtml($"<tr>" +
                                          $"<td></td>" +
                                          $"<td>{Book.Name}</td>" +
                                          $"<td>{Book.BarCode}</td>" +
                                          $"<td>{GetState(Book.State)}</td>" +
                                          $"<td>{Book.MatureTime.Value.ToString("yyyy/M/dd")}</td>" +
                                          $"<td>{Book.FetchBookNumber}</td>" +
                                          $"</tr>");
            }
            return Task.CompletedTask;
        }

        public string GetState(BookState state)
        {
            switch (state)
            {
                case BookState.Normal:
                    return "可借阅";
                case BookState.Readonly:
                    return "馆内阅览";
                case BookState.Borrowed:
                    return "已借出";
                case BookState.ReBorrowed:
                    return "被续借";
                case BookState.Appointed:
                    return "被预约";
                default:
                    return "";
            }
        }
    }
}
