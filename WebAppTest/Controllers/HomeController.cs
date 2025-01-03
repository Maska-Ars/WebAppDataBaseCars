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
            return View(_context.Cars);
        }

        [HttpGet]
        public IActionResult Cars(string? number, string? newNumber, string? brand, string? model, string? color)
        {
            Console.WriteLine($"{number}, {newNumber}, {brand}, {model}, {color}");

            if (number == null ) 
            {
                Console.WriteLine("������ ������ ��� ������ ������");
            }
            else
            {
                if (newNumber != null)
                {
                    Console.WriteLine("����������� ���� ������");
                }
                if (brand != null)
                {
                    Console.WriteLine("����������� ���� ������");
                }
                if (model != null)
                {
                    Console.WriteLine("����������� ���� ������");
                }
                if (color != null)
                {
                    Console.WriteLine("����������� ���� ������");
                }
            }

            return View(_context.Cars);
        }

        public IActionResult Owners()
        {
            return View(_context.Owners);
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
