using CarRent.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRent.Services.Interfaces
{
    public interface IBookingService
    {
        Task<Order> GetBookingByIdAsync(int id);
        Task<List<Order>> GetAllBookingsAsync();
        Task<bool> AddBookingAsync(Order order);
        Task UpdateBookingAsync(Order order);
        Task DeleteBookingAsync(int id);
        // Poți adăuga aici alte metode specifice gestionării rezervărilor
    }
}