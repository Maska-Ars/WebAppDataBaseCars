using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppTest.ViewModels;

namespace WebAppTest.Controllers
{
    public class MileagesController : Controller
    {
        private readonly ILogger<MileagesController> _logger;

        private CarsContext _context;

        public MileagesController(ILogger<MileagesController> logger)
        {
            _logger = logger;
            _context = new CarsContext();
        }


        public IActionResult Index()
        {
            return View(new MileagesViewModel 
            { 
                Mileages = _context.Mileages.ToList()            
            });
        }

        [HttpGet]
        public IActionResult Edit(int? id, string? newNumber, DateOnly? date, long? value)
        {
            Console.WriteLine($"{id}, {newNumber}, {date}, {value}");
            if (id == null)
            {
                Console.WriteLine("Пришел запрос без id записи");
            }
            else
            {
                Mileage? e = _context.Mileages.Where(e => e.Id == id).FirstOrDefault();
                if (e == null)
                {
                    Console.WriteLine($"Элемент с id: {id} был удален");
                }
                else
                {
                    if (newNumber != null)
                    {
                        Console.WriteLine("Редактируем базу данных");
                        e.Number = newNumber;
                        _context.Mileages.Update(e);
                        _context.SaveChanges();
                    }
                    else if (date != null)
                    {
                        Console.WriteLine("Редактируем базу данных");
                        e.FixationDate = (DateOnly)date;
                        _context.Mileages.Update(e);
                        _context.SaveChanges();
                    }
                    else if (value != null)
                    {
                        Console.WriteLine("Редактируем базу данных");
                        e.FixationValue = (long)value;
                        _context.Mileages.Update(e);
                        _context.SaveChanges();

                    }
                }
            }
            return View("Index", new MileagesViewModel
            {
                Mileages = _context.Mileages.ToList(),
            });
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                Mileage? e = _context.Mileages.Where(e => e.Id == id).FirstOrDefault();
                if (e == null)
                {
                    Console.WriteLine($"Элемент с id: {id} уже был удален");
                }
                else
                {
                    _context.Mileages.Remove(e);
                    _context.SaveChanges();
                    Console.WriteLine($"Элемент с id: {id} был удален");
                }
            }
            return View("Index", new MileagesViewModel()
            {
                Mileages = _context.Mileages.ToList(),
            });
        }

        [HttpGet]
        public IActionResult Add(string? number, DateOnly? date, long? value)
        {
            if (number == null || date == null || value == null)
            {
                return View("Index", new MileagesViewModel()
                {
                    Mileages = _context.Mileages.ToList(),
                });
            }

            _context.Mileages.Add(new Mileage
            {
                Number = number,
                FixationDate = (DateOnly)date,
                FixationValue = (long)value
            });
            _context.SaveChanges();

            return View("Index", new MileagesViewModel()
            {
                Mileages = _context.Mileages.ToList(),
            });
        }

        [HttpGet]
        public IActionResult Filter(string? number, DateOnly? date, long? value)
        {
            var items = _context.Mileages.FromSql($"SELECT * FROM mileage");

            if (number != null)
                items = from Mileage i in items
                        where i.Number == number
                        select i;
            if (date != null)
                items = from Mileage i in items
                        where i.FixationDate == date
                        select i;
            if (value != null)
                items = from Mileage i in items
                        where i.FixationValue == value
                        select i;

            return View("Index", new MileagesViewModel
            {
                Mileages = items.ToList()
            });
        }
    }
}
