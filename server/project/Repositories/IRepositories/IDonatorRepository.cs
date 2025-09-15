using project.Models;
using project.Models.ModelsDto;

namespace project.Repository.IRepository
{
    public interface IDonatorRepository
    {
        Task CreateDonatorAsync(Donator donator);
        Task DeleteDonatorAsync(int id);
        Task<Donator?> GetByIdAsync(int id);
        Task<IEnumerable<DonatorDto>> GetDonators();
        Task<IEnumerable<Donator>> GetDonatorsAsync();

        Task UpdateDonatorAsync(Donator donator);
        Task<IEnumerable<Gift>> GetGiftsByDonatorAsync(int donatorId);
        Task<Donator> GetDonatorByNameAsync(string name);


    }
}
