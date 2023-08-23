using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPTBOOK_STORE.Models;
using FPTBOOK_STORE.Controllers;
using Microsoft.Extensions.Caching.Memory;
using FPTBOOK_STORE.Utils;
using FPTBOOK_STORE.Areas.Identity.Data;
namespace FPTBOOK_STORE.Controllers{

public class HomeController : Controller
{
        private readonly ILogger<HomeController> _logger;
        private readonly FPTBOOK_STOREIdentityDbContext _context;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
}