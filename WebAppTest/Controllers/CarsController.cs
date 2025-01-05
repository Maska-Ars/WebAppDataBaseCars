using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using WebAppTest.ViewModels;

namespace WebAppTest.Controllers
{
    public class CarsController : Controller
    {
        private readonly ILogger<CarsController> _logger;

        private CarsContext _context;

        public CarsController(ILogger<CarsController> logger)
        {
            _logger = logger;
            _context = new CarsContext();
        }

        public IActionResult Index()
        {
            return View(new CarsViewModel()
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

            return View("Index", new CarsViewModel()
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
            return View("Index", new CarsViewModel()
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

            var items = _context.Cars.FromSql($"SELECT * FROM car");

            if (searchNumber != null)
                items = from Car car in items
                        where car.Number == searchNumber
                        select car;
            if (searchBrand != null)
                items = from Car car in items
                        where car.Brand == searchBrand
                        select car;
            if (searchModel != null)
                items = from Car car in items
                        where car.Model == searchModel
                        select car;
            if (searchColor != null)
                items = from Car car in items
                        where car.Color == searchColor
                        select car;

            return View("Index", new CarsViewModel()
            {
                Cars = items.ToList(),
                Brands = _context.Brands.ToList()
            });
        }

        [HttpGet]
        public IActionResult Sort(int? idColumn, int? up)
        {
            if (idColumn == null || idColumn < 0 || idColumn > 3)
            {
                return View("Index", new CarsViewModel()
                {
                    Cars = _context.Cars.ToList(),
                    Brands = _context.Brands.ToList()
                });
            }
            if (up == null || up < -1 || up > 1)
            {
                return View("Index", new CarsViewModel()
                {
                    Cars = _context.Cars.ToList(),
                    Brands = _context.Brands.ToList()
                });
            }


            var cars = _context.Cars;
            return View("Index", new CarsViewModel()
            {
                Cars = cars.ToList(),
                Brands = _context.Brands.ToList()
            });
        }

        [HttpGet]
        public IActionResult Add(string? number, string? brand, string? model, string? color)
        {
            if (number == null || brand == null || model == null || color == null)
            {
                return View("Index", new CarsViewModel()
                {
                    Cars = _context.Cars.ToList(),
                    Brands = _context.Brands.ToList()
                });
            }

            Console.WriteLine("Добавление");
            Console.WriteLine($"{number}, {brand}, {model}, {color}");

            _context.Cars.Add(new Car
            {
                Number = number,
                Brand = brand,
                Model = model,
                Color = color

            });
            _context.SaveChanges();


            return View("Index", new CarsViewModel()
            {
                Cars = _context.Cars.ToList(),
                Brands = _context.Brands.ToList()
            });
        }


    }
}
