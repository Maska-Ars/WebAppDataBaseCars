using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using WebAppDataBaseCars.ViewModels;
using WebAppDataBaseCars.Models;


namespace WebAppDataBaseCars.Controllers;

public class OwnersController(CarsContext context) : Controller
{
    private readonly CarsContext _context = context;

    public IActionResult Index()
    {
        return View(new OwnersViewModel()
        {
            Owners = [.. _context.Owners],
        });
    }

    [HttpPost]
    public IActionResult Edit(int? id, 
        string? newNumber, string? secondName, string? name, string? surname)
    {
        string? error = null;
        if (id == null)
            error = $"Не укзан id";
        else
        {
            Owner? owner = _context.Owners.Where(e => e.Id == id).FirstOrDefault();
            if (owner == null)
                error = $"Выбранного элемента не существует";
            else
            {
                if (newNumber != null) { owner.Number = newNumber; }
                else if (secondName != null) { owner.SecondName = secondName; }
                else if (name != null) { owner.Name = name; }
                else if (surname != null) { owner.Surname = surname; }

                try
                {
                    _context.Owners.Update(owner);
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
                        else if (e.SqlState == "23514" || e.SqlState == "22001")
                            error = $"Неправильный формат номера машины";
                        else
                            Console.WriteLine(e.Message);
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

        return View("Index", new OwnersViewModel()
        {
            Owners = [.. _context.Owners],
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
            Owner? owner = _context.Owners.Where(e => e.Id == id).FirstOrDefault();
            if (owner == null)
                error = $"Выбранного элемента не существует";
            else
            {
                try
                {
                    _context.Owners.Remove(owner);
                    _context.SaveChanges();
                }
                catch (Exception ex) 
                {
                    error = $"Произошла непредвиденная ошибка";
                    Console.WriteLine(ex.Message);
                }

            }
        }
        return View("Index", new OwnersViewModel()
        {
            Owners = [.. _context.Owners],
            Error = error
        });
    }

    [HttpGet]
    public IActionResult Filter(string? searchNumber, string? searchSecondName, string? searchName, string? searchSurname)
    {
        var items = _context.Owners.Where(e => e == e);

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
            Owners = [.. items],
        });
    }

    [HttpPost]
    public IActionResult Add(string? number, string? secondName, string? name, string? surname)
    {
        string? error = null;
        if (number == null || name == null)
        {
            error = "Не уаказан один из параметров";
        }
        else
        {
            try
            {
                _context.Owners.Add(new Owner
                {
                    Number = number,
                    SecondName = secondName,
                    Name = name,
                    Surname = surname

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
            
        return View("Index", new OwnersViewModel()
        {
            Owners = [.. _context.Owners],
            Error = error
        });
    }
}
