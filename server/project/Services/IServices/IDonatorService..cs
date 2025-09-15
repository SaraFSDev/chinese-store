using project.Models;

namespace project.Services.IServices
{
    public interface IDonatorService
    {
        Task<IEnumerable<Donator>> GetDonatorAsync(); 
        Task<Donator?> GetByIdAsync(int id);
        Task<IEnumerable<DonatorDto>> GetDonatorsAsync();
        Task CreateDonatorAsync(Donator donator);
        Task UpdateDonatorAsync(Donator donator);
        Task DeleteDonatorAsync(int id);
        Task<IEnumerable<Donator>> FilterByNameAsync();
        Task<IEnumerable<Donator>> FilterByEmailAsync();
        //Task<IEnumerable<Gift>> GetDonatorGiftsAsync(int donatorId);
        //Task<object> GetGiftsByDonatorAsync(int donatorId);

        Task<object> GetDonatorWithGiftsAsync(int donatorId);
        //Task<object> GetDonatorGiftsAsync(int donatorId);  // הוספת הפונקציה הזאת
        //Task<object> GetGiftsByDonatorAsync(int donatorId); // הוספת הפונקציה הזאת


    }
}
