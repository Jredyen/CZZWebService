using CZZ.User.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

namespace CZZ.User.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        
        //public IActionResult AllFiles()
        //{
        //    return View();
        //}

        [HttpGet]
        public IActionResult AllFiles(string? id)
        {
            if (id != null)
            {
                return RedirectToAction("ObjectList", "Home", id);
            }
            else
            {
                return View();
            }
        }

        public IActionResult ObjectList(string? id)
        {
            ViewBag.Date = id;
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}