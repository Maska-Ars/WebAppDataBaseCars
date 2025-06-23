using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using WebAppDataBaseCars.ViewModels;
using WebAppDataBaseCars.Models;


namespace WebAppDataBaseCars.Controllers;

public class MileagesController(CarsContext context) : Controller
{
    private readonly CarsContext _context = context;

    public IActionResult Index()
    {
        return View(new MileagesViewModel 
        { 
            Mileages = [.. _context.Mileages]            
        });
    }

    [HttpPost]
    public IActionResult Edit(int? id, string? newNumber, DateOnly? date, long? value)
    {
        string? error = null;
        if (id == null)
            error = "Элемент не выбран";
        else
        {
            Mileage? e = _context.Mileages.Where(e => e.Id == id).FirstOrDefault();
            if (e == null)
                error = $"Выбранного элемента не существует";
            else
            {
                if (newNumber != null) { e.Number = newNumber; }
                else if (date != null) { e.FixationDate = (DateOnly)date; }
                else if (value != null) { e.FixationValue = (long)value; }

                try
                {
                    _context.Mileages.Update(e);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    error = $"Произошла непредвиденная ошибка";
                    Console.WriteLine(ex.Message);
                }
            }
        }
        return View("Index", new MileagesViewModel
        {
            Mileages = [.. _context.Mileages],
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
            Mileage? e = _context.Mileages.Where(e => e.Id == id).FirstOrDefault();
            if (e == null)
                error = $"Выбранного элемента не существует";
            else
            {
                try
                {
                    _context.Mileages.Remove(e);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    error = $"Произошла непредвиденная ошибка";
                    Console.WriteLine(ex.Message);
                }
            }
        }
        return View("Index", new MileagesViewModel()
        {
            Mileages = [.. _context.Mileages],
            Error = error
        });
    }

    [HttpPost]
    public IActionResult Add(string? number, DateOnly? date, long? value)
    {
        string? error = null;
        if (number == null || date == null || value == null)
            error = "Не уаказан один из параметров";
        else
        {
            try
            {
                _context.Mileages.Add(new Mileage
                {
                    Number = number,
                    FixationDate = (DateOnly)date,
                    FixationValue = (long)value
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
        return View("Index", new MileagesViewModel()
        {
            Mileages = [.. _context.Mileages],
            Error = error
        });
    }

    [HttpGet]
    public IActionResult Filter(string? number, DateOnly? date, long? value)
    {
        var items = _context.Mileages.Where(e => e == e);

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
            Mileages = [.. items]
        });
    }
}
