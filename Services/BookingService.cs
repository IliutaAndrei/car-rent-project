using CarRent.Data;
using CarRent.Models;
using CarRent.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRent.Services
{
    public class BookingService : IBookingService
    {
        private readonly CarRentContext _context;

        public BookingService(CarRentContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetBookingByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<List<Order>> GetAllBookingsAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<bool> AddBookingAsync(Order order)
        {
            _context.Orders.Add(order);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task UpdateBookingAsync(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookingAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}