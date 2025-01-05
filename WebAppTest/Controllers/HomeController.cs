using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebAppTest.Models;

namespace WebAppTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private CarsContext _context;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = new CarsContext();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Cars()
        {
            return Redirect("~/Cars/");
        }

        public IActionResult Owners()
        {
            return Redirect("~/Owners/");
        }

        public IActionResult Mileages()
        {
            return Redirect("~/Mileages/");
        }

        public IActionResult Brands()
        {
            return Redirect("~/Brands/");
        }

        public IActionResult Accidents()
        {
            return Redirect("~/Accidents/");
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
