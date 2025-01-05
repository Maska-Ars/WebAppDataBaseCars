using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppTest.ViewModels;

namespace WebAppTest.Controllers
{
    public class AccidentsController : Controller
    {
        private readonly ILogger<AccidentsController> _logger;

        private CarsContext _context;

        public AccidentsController(ILogger<AccidentsController> logger)
        {
            _logger = logger;
            _context = new CarsContext();
        }

        public IActionResult Index()
        {
            return View(new AccidentsViewModel 
            {
                Accidents = _context.Accidents.ToList(),
            });
        }

        [HttpGet]
        public IActionResult Edit(int? id, 
            string? number, DateOnly? date, string? type, string? region)
        {
            if (id == null)
            {
                Console.WriteLine("Пришел запрос без id записи");
            }
            else
            {
                Accident? e = _context.Accidents.Where(e => e.Id == id).FirstOrDefault();
                if (e == null)
                {
                    Console.WriteLine($"Элемент с id: {id} был удален");
                }
                else
                {
                    if (number != null)
                    {
                        e.Number = number;
                        _context.Accidents.Update(e);
                        _context.SaveChanges();
                    }
                    else if (date != null)
                    {
                        e.Date = (DateOnly)date;
                        _context.Accidents.Update(e);
                        _context.SaveChanges();
                    }
                    else if (type != null)
                    {
                        e.Type = type;
                        _context.Accidents.Update(e);
                        _context.SaveChanges();
                    }
                    else if (region != null)
                    {
                        e.Region = region;
                        _context.Accidents.Update(e);
                        _context.SaveChanges();
                    }
                }
            }
            return View("Index", new AccidentsViewModel
            {
                Accidents = _context.Accidents.ToList(),
            });
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                Accident? e = _context.Accidents.Where(e => e.Id == id).FirstOrDefault();
                if (e == null)
                {
                    Console.WriteLine($"Элемент с id: {id} уже был удален");
                }
                else
                {
                    _context.Accidents.Remove(e);
                    _context.SaveChanges();
                    Console.WriteLine($"Элемент с id: {id} был удален");
                }
            }
            return View("Index", new AccidentsViewModel
            {
                Accidents = _context.Accidents.ToList(),
            });
        }

        [HttpGet]
        public IActionResult Add(string? number, DateOnly? date, string? type, string? region)
        {
            if (number == null || date == null || type == null || region == null)
            {
                return View("Index", new AccidentsViewModel
                {
                    Accidents = _context.Accidents.ToList(),
                });
            }

            _context.Accidents.Add(new Accident
            {
                Number = number,
                Date = (DateOnly)date,
                Type = type,
                Region = region
            });
            _context.SaveChanges();

            return View("Index", new AccidentsViewModel
            {
                Accidents = _context.Accidents.ToList(),
            });
        }

        [HttpGet]
        public IActionResult Filter(string? number, DateOnly? date, string? type, string? region)
        {
            var items = _context.Accidents.FromSql($"SELECT * FROM accident");

            if (number != null)
                items = from Accident i in items
                        where i.Number == number
                        select i;
            if (date != null)
                items = from Accident i in items
                        where i.Date == date
                        select i;
            if (type != null)
                items = from Accident i in items
                        where i.Type == type
                        select i;
            if (region != null)
                items = from Accident i in items
                        where i.Region == region
                        select i;

            return View("Index", new AccidentsViewModel
            {
                Accidents = items.ToList(),
            });
        }
    }
}
