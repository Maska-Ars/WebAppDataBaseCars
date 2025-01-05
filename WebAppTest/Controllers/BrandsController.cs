using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppTest.ViewModels;

namespace WebAppTest.Controllers
{
    public class BrandsController : Controller
    {
        private readonly ILogger<BrandsController> _logger;

        private CarsContext _context;

        public BrandsController(ILogger<BrandsController> logger)
        {
            _logger = logger;
            _context = new CarsContext();
        }

        public IActionResult Index()
        {
            return View(new BrandsViewModel
            {
                Brands = _context.Brands.ToList()
            });
        }

        [HttpGet]
        public IActionResult Edit(string? id, string? title, string? fullTitle, string? country)
        {
            if (id == null)
            {
                Console.WriteLine("Пришел запрос без id записи");
            }
            else
            {
                Brand? e = _context.Brands.Where(e => e.Title == id).FirstOrDefault();
                if (e == null)
                {
                    Console.WriteLine($"Элемент с id: {id} был удален");
                }
                else
                {
                    if (title != null)
                    {
                        e.Title = title;
                        _context.Brands.Update(e);
                        _context.SaveChanges();
                    }
                    else if (fullTitle != null)
                    {
                        e.FullTitle = fullTitle;
                        _context.Brands.Update(e);
                        _context.SaveChanges();
                    }
                    else if (country != null)
                    {
                        e.Country = country;
                        _context.Brands.Update(e);
                        _context.SaveChanges();

                    }
                }
            }
            return View("Index", new BrandsViewModel
            {
                Brands = _context.Brands.ToList()
            });
        }

        [HttpGet]
        public IActionResult Delete(string? id)
        {
            if (id != null)
            {
                Brand? e = _context.Brands.Where(e => e.Title == id).FirstOrDefault();
                if (e == null)
                {
                    Console.WriteLine($"Элемент с id: {id} уже был удален");
                }
                else
                {
                    _context.Brands.Remove(e);
                    _context.SaveChanges();
                    Console.WriteLine($"Элемент с id: {id} был удален");
                }
            }
            return View("Index", new BrandsViewModel
            {
                Brands = _context.Brands.ToList()
            });
        }

        [HttpGet]
        public IActionResult Add(string? title, string? fullTitle, string? country)
        {
            if (title == null || fullTitle == null || country == null)
            {
                return View("Index", new BrandsViewModel
                {
                    Brands = _context.Brands.ToList()
                });
            }

            _context.Brands.Add(new Brand
            {
                Title = title,
                FullTitle = fullTitle,
                Country = country
            });
            _context.SaveChanges();

            return View("Index", new BrandsViewModel
            {
                Brands = _context.Brands.ToList()
            });
        }

        [HttpGet]
        public IActionResult Filter(string? title, string? fullTitle, string? country)
        {
            var items = _context.Brands.FromSql($"SELECT * FROM brands");

            if (title != null)
                items = from Brand i in items
                        where i.Title == title
                        select i;
            if (fullTitle != null)
                items = from Brand i in items
                        where i.FullTitle == fullTitle
                        select i;
            if (country != null)
                items = from Brand i in items
                        where i.Country == country
                        select i;

            return View("Index", new BrandsViewModel
            {
                Brands = items.ToList()
            });
        }
    }
}
