using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CarRent.Models;
using CarRent.Data;
using Microsoft.EntityFrameworkCore;
using CarRent.Services.Interfaces;
using CarRent.Services;

namespace CarRent.Controllers;

public class HomeController : Controller
{
    private readonly CarRentContext _context; 
    private readonly ILogger<HomeController> _logger;
    private readonly ICarService _carService;

    public HomeController(CarRentContext context, ILogger<HomeController> logger, ICarService carService)
    {
        _context = context;
        _logger = logger;
        _carService = carService;
    }

    public async Task<IActionResult> Index()
    {
        var allCars = await _context.Cars.ToListAsync();
        var carCount = await _carService.CountAllCarsAsync();

        ViewBag.TotalCarCount = carCount;
        return View(allCars);
    }

    [HttpGet("/cars/search")]
    public async Task<IActionResult> Search(string make, string model, int? year, FuelType? fuelType, TransmissionType? transmissionType, decimal? minPrice, decimal? maxPrice, bool isAvailable = false)
    {
        IQueryable<Car> query = _context.Cars;

        if (!string.IsNullOrEmpty(make))
        {
            query = query.Where(c => c.Make.Contains(make));
        }
        if (!string.IsNullOrEmpty(model))
        {
            query = query.Where(c => c.Model.Contains(model));
        }
        if (year.HasValue)
        {
            query = query.Where(c => c.YearOfFabrication == year.Value);
        }
        if (fuelType.HasValue)
        {
            query = query.Where(c => c.FuelType == fuelType.Value);
        }
        if (transmissionType.HasValue)
        {
            query = query.Where(c => c.TransmissionType == transmissionType.Value);
        }
        if (minPrice.HasValue)
        {
            query = query.Where(c => c.PricePerDay >= minPrice.Value);
        }
        if (maxPrice.HasValue)
        {
            query = query.Where(c => c.PricePerDay <= maxPrice.Value);
        }
        if (isAvailable)
        {
            query = query.Where(c => c.IsAvailable);
        }

        var searchResults = await query.ToListAsync();
        return View("SearchResults", searchResults);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var car = await _context.Cars
            .FirstOrDefaultAsync(m => m.Id == id);

        if (car == null)
        {
            return NotFound();
        }

        return View(car);
    }

    public IActionResult Rent(int id)
    {
        var car = _context.Cars.Find(id);
        if (car == null)
        {
            return NotFound();
        }
        ViewBag.Car = car;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
