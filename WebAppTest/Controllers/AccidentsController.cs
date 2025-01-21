using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using WebAppTest.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebAppTest.Controllers
{
    public class AccidentsController : Controller
    {

        private CarsContext _context;

        public AccidentsController(CarsContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(new AccidentsViewModel 
            {
                Accidents = _context.Accidents.ToList(),
            });
        }

        [HttpPost]
        public IActionResult Edit(int? id, 
            string? number, DateOnly? date, string? type, string? region)
        {
            string? error = null;
            if (id == null)
            {
                error = $"Не укзан id";
            }
            else
            {
                Accident? e = _context.Accidents.Where(e => e.Id == id).FirstOrDefault();
                if (e == null)
                {
                    error = $"Выбранного элемента не существует";
                }
                else
                {
                    if (number != null) { e.Number = number; }
                    else if (date != null) { e.Date = (DateOnly)date; }
                    else if (type != null) { e.Type = type; }
                    else if (region != null) { e.Region = region; }

                    try
                    {
                        _context.Accidents.Update(e);
                        _context.SaveChanges();
                    }
                    catch (DbUpdateException ex)
                    {
                        if (ex.InnerException != null)
                        {
                            PostgresException inner = (PostgresException)ex.InnerException;
                            if (inner.SqlState == "23503")
                                error = $"Нельзя добавить запись с номером машины, " +
                                    $"отсуствующем в таблице машины";
                            else if (inner.SqlState == "23514" || inner.SqlState == "22001")
                                error = $"Неправильный формат номера машины";
                            else
                                Console.WriteLine(inner.Message);
                        }
                        else
                        {
                            error = "Произошла непредвиденная ошибка";
                            Console.WriteLine(ex);
                        }
                    }
                    catch (Exception ex)
                    {
                        error = $"{ex.Message}";
                    }

                }
            }
            return View("Index", new AccidentsViewModel
            {
                Accidents = _context.Accidents.ToList(),
                Error = error
            });
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            string? error = null;
            if (id == null)
                error = "Элемент не выбран";
            else
            {
                Accident? e = _context.Accidents.Where(e => e.Id == id).FirstOrDefault();
                if (e == null)
                    error = $"Выбранного элемента не существует";
                else
                {
                    try
                    {
                        _context.Accidents.Remove(e);
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        error = $"Произошла непредвиденная ошибка";
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return View("Index", new AccidentsViewModel
            {
                Accidents = _context.Accidents.ToList(),
                Error = error
            });
        }

        [HttpPost]
        public IActionResult Add(string? number, DateOnly? date, string? type, string? region)
        {
            string? error = null;
            if (number == null || date == null || type == null || region == null)
                error = "Не уаказан один из параметров";
            else
            {
                try
                {
                    _context.Accidents.Add(new Accident
                    {
                        Number = number,
                        Date = (DateOnly)date,
                        Type = type,
                        Region = region
                    });
                    _context.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null)
                    {
                        PostgresException e = (PostgresException)ex.InnerException;
                        if (e.SqlState == "23503")
                            error = $"Нельзя добавить запись с номером машины, " +
                                $"отсуствующем в таблице машины";
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
            return View("Index", new AccidentsViewModel
            {
                Accidents = _context.Accidents.ToList(),
                Error = error
            });
        }

        [HttpGet]
        public IActionResult Filter(string? number, DateOnly? date, string? type, string? region)
        {
            var items = _context.Accidents.Where(e => e == e);

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
