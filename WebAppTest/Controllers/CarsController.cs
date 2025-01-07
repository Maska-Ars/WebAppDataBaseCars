using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Npgsql;
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
        public IActionResult Edit(string? id, string? newNumber, string? brand, string? model, string? color)
        {
            string? error = null;
            if (id == null)
                error = "Элемент не выбран";
            else
            {
                Car? e = _context.Cars.Where(e => e.Number == id).FirstOrDefault();
                if(e == null)
                    error = $"Выбранного элемента не существует";
                else
                {
                    if (newNumber != null) { e.Number = newNumber; }
                    else if (brand != null) { e.Brand = brand; }
                    else if (model != null) { e.Model = model; }
                    else if (color != null) { e.Color = color; }

                    try
                    {
                        _context.Cars.Update(e);
                        _context.SaveChanges();
                    }
                    catch (DbUpdateException ex)
                    {
                        if (ex.InnerException != null)
                        {
                            PostgresException inner = (PostgresException)ex.InnerException;
                            if (inner.SqlState == "23503")
                                error = $"Нельзя добавить запись с брендом, " +
                                    $"отсуствующем в таблице Бренды";
                        }
                        else
                        {
                            error = "Произошла непредвиденная ошибка";
                            Console.WriteLine(ex);
                        }
                    }
                    catch (Exception ex)
                    {
                        error = $"Произошла непредвиденная ошибка";
                        Console.WriteLine(ex.Message);
                    }
                }
                
            }

            return View("Index", new CarsViewModel()
            {
                Cars = _context.Cars.ToList(),
                Brands = _context.Brands.ToList(),
                Error = error
            });
        }

        [HttpGet]
        public IActionResult Delete(string? id)
        {
            string? error = null;
            if (id == null)
                error = "Элемент не выбран";
            else
            {
                Car? e = _context.Cars.Where(e => e.Number == id).FirstOrDefault();
                if (e == null)
                    error = $"Выбранного элемента не существует";
                else
                {
                    try
                    {
                        _context.Cars.Remove(e);
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        error = $"Произошла непредвиденная ошибка";
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return View("Index", new CarsViewModel()
            {
                Cars = _context.Cars.ToList(),
                Brands = _context.Brands.ToList(),
                Error = error
            });
        }

        [HttpGet]
        public IActionResult Filter(string? searchNumber, string? searchBrand, string? searchModel, string? searchColor)
        {
            var items = _context.Cars.Where(e => e == e);

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
        public IActionResult Add(string? number, string? brand, string? model, string? color)
        {
            string? error = null;
            if (number == null || brand == null || model == null || color == null)
                error = "Не уаказан один из параметров";
            else
            {
                try
                {
                    _context.Cars.Add(new Car
                    {
                        Number = number,
                        Brand = brand,
                        Model = model,
                        Color = color

                    });
                    _context.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null)
                    {
                        PostgresException e = (PostgresException)ex.InnerException;
                        if (e.SqlState == "23503")
                            error = $"Нельзя добавить запись с брендом, " +
                                $"отсуствующем в таблице Бренды";
                    }
                    else
                    {
                        error = "Произошла непредвиденная ошибка";
                        Console.WriteLine(ex);
                    }
                }
                catch (Exception ex)
                {
                    error = $"Произошла непредвиденная ошибка";
                    Console.WriteLine(ex);
                }
            }
            return View("Index", new CarsViewModel()
            {
                Cars = _context.Cars.ToList(),
                Brands = _context.Brands.ToList(),
                Error = error
            });
        }


    }
}
