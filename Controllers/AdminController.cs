using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using CarRent.Models;
using CarRent.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CarRent.Data;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<IdentityUser> _userManager; 
    private readonly ICarService _carService;
    private readonly IBookingService _bookingService;
    private readonly CarRentContext _context;
    private readonly ILogger<AdminController> _logger;

    public AdminController(UserManager<IdentityUser> userManager, ICarService carService, IBookingService bookingService, CarRentContext context, ILogger<AdminController> logger) // Revert to IdentityUser
    {
        _userManager = userManager;
        _carService = carService;
        _bookingService = bookingService;
        _context = context;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult ManageUsers()
    {
        var users = _userManager.Users.ToList();
        return View(users);
    }

    public IActionResult AddCar()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddCar(Car car)
    {
        if (ModelState.IsValid)
        {
            var result = await _carService.AddCarAsync(car);
            if (result)
            {
                return RedirectToAction(nameof(ManageCars));
            }
            else
            {
                ModelState.AddModelError("", "Failed to add the car.");
                _logger.LogError("Failed to add car. CarService returned false.");
            }
        }
        _logger.LogWarning("Failed to add car. Invalid model state.");
        return View(car);
    }

    public async Task<IActionResult> ManageCars()
    {
        var cars = await _carService.GetAllCarsAsync();
        return View(cars);
    }

    public async Task<IActionResult> ManageBookings()
    {
        var bookings = await _bookingService.GetAllBookingsAsync();
        return View(bookings);
    }

    public async Task<IActionResult> BlockUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.UpdateAsync(user);
            _logger.LogInformation("User with ID {Id} blocked.", id);
        }
        else
        {
            _logger.LogWarning("Attempted to block non-existent user with ID {Id}.", id);
        }
        return RedirectToAction(nameof(ManageUsers));
    }

    public async Task<IActionResult> DeleteCar(int id)
    {
        try
        {
            await _carService.DeleteCarAsync(id);
            _logger.LogInformation("Car with ID {Id} deleted.", id);
            return RedirectToAction(nameof(ManageCars));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to delete car with ID {Id}.", id);
            return RedirectToAction(nameof(ManageCars));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting car with ID {Id}.", id);
            return RedirectToAction(nameof(ManageCars));
        }
    }

    // GET: Admin/EditCar/5
    public async Task<IActionResult> EditCar(int? id)
    {
        if (id == null)
        {
            _logger.LogWarning("EditCar: id is null.");
            return NotFound();
        }

        var car = await _context.Cars.FindAsync(id);
        if (car == null)
        {
            _logger.LogWarning("EditCar: Car with ID {Id} not found.", id);
            return NotFound();
        }
        _logger.LogInformation("EditCar: Displaying edit form for car with ID {Id}.", id);
        return View(car);
    }

    // POST: Admin/EditCar/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCar(int id, [Bind("Id,Make,Model,YearOfFabrication,FuelType,TransmissionType,IsAvailable,PricePerDay")] Car car)
    {
        _logger.LogInformation("EditCar: Attempting to update car with ID {Id}.", id);
        if (id != car.Id)
        {
            _logger.LogWarning("EditCar: ID mismatch - id: {Id}, car.Id: {CarId}.", id, car.Id);
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _logger.LogInformation("EditCar: Model state is valid.");
            try
            {
                _context.Update(car);
                await _context.SaveChangesAsync();
                _logger.LogInformation("EditCar: Car with ID {Id} updated successfully.", id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "EditCar: A concurrency error occurred while updating car with ID {Id}.", id);
                if (!CarExists(car.Id))
                {
                    _logger.LogWarning("EditCar: Car with ID {Id} not found.", id);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(ManageCars));
        }
        _logger.LogWarning("EditCar: Model state is invalid.");
        return View(car);
    }

    private bool CarExists(int id)
    {
        bool exists = _context.Cars.Any(e => e.Id == id);
        _logger.LogInformation("CarExists: Car with ID {Id} exists: {Exists}.", id, exists);
        return exists;
    }
}