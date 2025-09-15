using project.Models;
using project.Models.ModelsDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace project.Repository.IRepository
{
    public interface IGiftRepository
    {
        Task CreateGiftAsync(Gift gift);
        Task DeleteGiftAsync(int id);
        Task<Gift?> GetByIdAsync(int id);
        Task<IEnumerable<GiftDto>> GetGiftsWithDonatorAndCategory();
        Task<IEnumerable<Gift>> GetGiftsAsync();
        Task UpdateGiftAsync(Gift gift);
        Task<User> LotteryAsync(Gift gift, User winner);
        Task<Gift?> GetGiftWithParticipantsAsync(int giftId);
        Task<IEnumerable<GiftDto>> SearchAsync(string text);
        Task<IEnumerable<GiftDto>> GetGiftsBySortAsync(string? sortBy, string? order);
        Task<IEnumerable<GiftDto>> GetGiftsWithPurchasesAsync();
        Task CreateDonatorAsync(Donator donator);
        Task<bool> ExistsByNameAsync(string name);
    }
}




