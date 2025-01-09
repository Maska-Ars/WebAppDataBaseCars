﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
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

        [HttpPost]
        public IActionResult Edit(string? id, string? title, string? fullTitle, string? country)
        {
            string? error = null;
            if (id == null)
                error = "Элемент не выбран";
            else
            {
                Brand? e = _context.Brands.Where(e => e.Title == id).FirstOrDefault();
                if (e == null)
                    error = $"Выбранного элемента не существует";
                else
                {
                    if (title != null) { e.Title = title; }
                    else if (fullTitle != null) { e.FullTitle = fullTitle; }
                    else if (country != null) { e.Country = country; }

                    try
                    {
                        _context.Brands.Update(e);
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        error = $"Произошла непредвиденная ошибка";
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return View("Index", new BrandsViewModel
            {
                Brands = _context.Brands.ToList()
            });
        }

        [HttpPost]
        public IActionResult Delete(string? id)
        {
            string? error = null;
            if (id == null)
                error = "Элемент не выбран";
            else
            {
                Brand? e = _context.Brands.Where(e => e.Title == id).FirstOrDefault();
                if (e == null)
                    error = $"Выбранного элемента не существует";
                else
                {
                    try
                    {
                        _context.Brands.Remove(e);
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        error = $"Произошла непредвиденная ошибка";
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return View("Index", new BrandsViewModel
            {
                Brands = _context.Brands.ToList(),
                Error = error
            });
        }

        [HttpPost]
        public IActionResult Add(string? title, string? fullTitle, string? country)
        {
            string? error = null;
            if (title == null || fullTitle == null || country == null)
                error = "Не уаказан один из параметров";
            else
            {
                try
                {
                    _context.Brands.Add(new Brand
                    {
                        Title = title,
                        FullTitle = fullTitle,
                        Country = country
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
            

            return View("Index", new BrandsViewModel
            {
                Brands = _context.Brands.ToList(),
                Error = error
            });
        }

        [HttpGet]
        public IActionResult Filter(string? title, string? fullTitle, string? country)
        {
            var items = _context.Brands.Where(e => e == e);

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
