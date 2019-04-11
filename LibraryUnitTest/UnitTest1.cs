using System;
using System.Threading.Tasks;
using LibraryDemo.Controllers;
using LibraryDemo.Data;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LibraryDemo.UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public async Task EditAction()
        {
            using (LendingInfoDbContext context = new LendingInfoDbContext(InMemoryDbContextOptionsFactory.Create<LendingInfoDbContext>()))
            {
                var controller = new BookInfoController(context);
                var result = await controller.EditBookDetails("123456");
                Assert.IsType<RedirectToActionResult>(result);
            }
        }
    }
}
