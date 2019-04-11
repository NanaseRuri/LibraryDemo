using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryDemo.Data;
using LibraryDemo.Models.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace LibraryDemo.Controllers
{
    public class CacheController : Controller
    {
        private IMemoryCache _cache;
        private LendingInfoDbContext _lendingInfoDbContext;

        public CacheController(IMemoryCache cache, LendingInfoDbContext lendingInfoDbContext)
        {
            _lendingInfoDbContext = lendingInfoDbContext;
            _cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            StudentInfo info=new StudentInfo();
            if (!_cache.TryGetValue($"student:{info.UserName}",out info))
            {
                info = await _lendingInfoDbContext.Students.FirstOrDefaultAsync(s => s.UserName == User.Identity.Name);
                _cache.Set($"student:{info.UserName}", info,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
            }
            return View();
        }
    }
}