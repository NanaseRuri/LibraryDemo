using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace ExternalLibrary.Controllers
{
    public class TestController:Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
