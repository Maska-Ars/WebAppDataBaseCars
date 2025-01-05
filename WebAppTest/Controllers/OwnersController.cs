using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppTest.ViewModels;

namespace WebAppTest.Controllers
{
    public class OwnersController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private CarsContext _context;

        public OwnersController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = new CarsContext();
        }

        public IActionResult Index()
        {
            return View(new OwnersViewModel()
            {
                Owners = _context.Owners.ToList(),
            });
        }

        [HttpGet]
        public IActionResult Edit(int? id, 
            string? newNumber, string? secondName, string? name, string? surname)
        {
            Console.WriteLine($"{id}, {newNumber}, {secondName}, {name}, {surname}");
            if (id == null)
            {
                Console.WriteLine("Пришел запрос без id владельца");
            }
            else
            {
                Owner? owner = _context.Owners.Where(e => e.Id == id).FirstOrDefault();
                if (owner == null) 
                {
                    Console.WriteLine($"Элемент с id: {id} был удален");
                }
                else
                {
                    if (newNumber != null)
                    {
                        Console.WriteLine("Редактируем базу данных");
                        owner.Number = newNumber;
                        _context.Owners.Update(owner);
                        _context.SaveChanges();
                    }
                    else if (secondName != null)
                    {
                        Console.WriteLine("Редактируем базу данных"); 
                        owner.SecondName = secondName;
                        _context.Owners.Update(owner);
                        _context.SaveChanges();
                    }
                    else if (name != null)
                    {
                        Console.WriteLine("Редактируем базу данных");
                        owner.Name = name;
                        _context.Owners.Update(owner);
                        _context.SaveChanges();

                    }
                    else if (surname != null)
                    {
                        Console.WriteLine("Редактируем базу данных");
                        owner.Surname = surname;
                        _context.Owners.Update(owner);
                        _context.SaveChanges();
                    }
                }

                
            }

            return View("Index", new OwnersViewModel()
            {
                Owners = _context.Owners.ToList(),
            });
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                Owner? owner = _context.Owners.Where(e => e.Id == id).FirstOrDefault();
                if (owner == null)
                {
                    Console.WriteLine($"Элемент с id: {id} уже был удален");
                }
                else
                {
                    _context.Owners.Remove(owner);
                    _context.SaveChanges();
                    Console.WriteLine($"Элемент с id: {id} был удален");
                }
            }
            return View("Index", new OwnersViewModel()
            {
                Owners = _context.Owners.ToList(),
            });
        }

        [HttpGet]
        public IActionResult Filter(string? searchNumber, string? searchSecondName, string? searchName, string? searchSurname)
        {
            Console.WriteLine("Фильтр");
            Console.WriteLine($"{searchNumber}, {searchSecondName}, {searchName}, {searchSurname}");

            var items = _context.Owners.FromSql($"SELECT * FROM owner");

            if (searchNumber != null)
                items = from Owner i in items
                        where i.Number == searchNumber
                        select i;
            if (searchSecondName != null)
                items = from Owner i in items
                        where i.SecondName == searchSecondName
                        select i;
            if (searchName != null)
                items = from Owner i in items
                        where i.Name == searchName
                        select i;
            if (searchSurname != null)
                items = from Owner i in items
                        where i.Surname == searchSurname
                        select i;

            return View("Index", new OwnersViewModel()
            {
                Owners = items.ToList(),
            });
        }

        [HttpGet]
        public IActionResult Sort(int? idColumn, int? up)
        {
            if (idColumn == null || idColumn < 0 || idColumn > 3)
            {
                return View("Index", new OwnersViewModel()
                {
                    Owners = _context.Owners.ToList(),
                });
            }
            if (up == null || up < -1 || up > 1)
            {
                return View("Index", new OwnersViewModel()
                {
                    Owners = _context.Owners.ToList(),
                });
            }


            var cars = _context.Cars;
            return View("Index", new OwnersViewModel()
            {
                Owners = _context.Owners.ToList(),
            });
        }

        [HttpGet]
        public IActionResult Add(string? number, string? secondName, string? name, string? surname)
        {
            if (number == null || secondName == null || name == null || surname == null)
            {
                return View("Index", new OwnersViewModel()
                {
                    Owners = _context.Owners.ToList(),
                });
            }

            Console.WriteLine("Добавление");
            Console.WriteLine($"{number}, {secondName}, {name}, {surname}");
            
            _context.Owners.Add(new Owner 
            { 
                Number = number, 
                SecondName = secondName, 
                Name = name, 
                Surname = surname 
            
            });
            _context.SaveChanges();

            return View("Index", new OwnersViewModel()
            {
                Owners = _context.Owners.ToList(),
            });
        }


    }
}
