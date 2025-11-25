using CarRent.Models;

namespace CarRent.Services.Interfaces
{
    public interface ICarService
    {
        Task<Car?> GetCarByIdAsync(int id);
        Task<List<Car>> GetAllCarsAsync();
        Task<bool> AddCarAsync(Car car);
        Task UpdateCarAsync(Car car);
        Task<bool> DeleteCarAsync(int id);
        Task<bool> RentCarAsync(int id);


        Task<List<Car>> SearchCarsAsync(
            string make,
            string model,
            int? year,
            FuelType? fuelType, 
            TransmissionType? transmissionType, 
            bool? isAvailable,
            decimal? minPrice,
            decimal? maxPrice);

        Task<int> CountAllCarsAsync();
    }
}
