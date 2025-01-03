using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using WebAppTest.ViewModels;

namespace WebAppTest.Controllers
{
    public class CarsController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private CarsContext _context;

        public CarsController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = new CarsContext();
        }

        public IActionResult Index()
        {
            return View(new ViewModel() 
            { 
                Cars = _context.Cars.ToList(), 
                Brands = _context.Brands.ToList() 
            });
        }

        [HttpGet]
        public IActionResult Edit(string? number, string? newNumber, string? brand, string? model, string? color)
        {
            Console.WriteLine($"{number}, {newNumber}, {brand}, {model}, {color}");
            if (number == null)
            {
                Console.WriteLine("Пришел запрос без номера машины");
            }
            else
            {
                if (newNumber != null)
                {
                    Console.WriteLine("Редактируем базу данных");
                }
                else if (brand != null)
                {
                    Console.WriteLine("Редактируем базу данных");
                }
                else if (model != null)
                {
                    Console.WriteLine("Редактируем базу данных");
                }
                else if (color != null)
                {
                    Console.WriteLine("Редактируем базу данных");
                }
            }

            return View("Index", new ViewModel()
            {
                Cars = _context.Cars.ToList(),
                Brands = _context.Brands.ToList()
            });
        }

        [HttpGet]
        public IActionResult Delete(string? number) 
        {
            if (number != null)
            {
                Console.WriteLine($"Удаляем элемент с номером: {number}");
            }
            return View("Index", new ViewModel()
            {
                Cars = _context.Cars.ToList(),
                Brands = _context.Brands.ToList()
            });
        }

        [HttpGet]
        public IActionResult Filter(string? searchNumber, string? searchBrand, string? searchModel, string? searchColor)
        {
            Console.WriteLine("Фильтр");
            Console.WriteLine($"{searchNumber}, {searchBrand}, {searchModel}, {searchColor}");

            var cars = _context.Cars.FromSql($"SELECT * FROM car");

            if (searchNumber != null)
                cars = from Car car in cars
                       where car.Number == searchNumber
                       select car;
            if (searchBrand != null)
                cars = from Car car in cars
                       where car.Brand == searchBrand
                       select car;
            if (searchModel != null)
                cars = from Car car in cars
                       where car.Model == searchModel
                       select car;
            if (searchColor != null)
                cars = from Car car in cars
                       where car.Color == searchColor
                       select car;

            return View("Index", new ViewModel() 
            { 
                Cars = cars.ToList(), 
                Brands = _context.Brands.ToList() 
            });
        }

        [HttpGet]
        public IActionResult Add(string? number, string? brand, string? model, string? color)
        {
            Console.WriteLine("Добавление");
            Console.WriteLine($"{number}, {brand}, {model}, {color}");
            return View("Index", new ViewModel()
            {
                Cars = _context.Cars.ToList(),
                Brands = _context.Brands.ToList()
            });
        }


    }
}
