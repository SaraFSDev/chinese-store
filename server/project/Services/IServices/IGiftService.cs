using project.Models;
using project.Models.ModelsDto;

namespace project.Services.IServices
{
    public interface IGiftService
    {
        Task<IEnumerable<Gift>> GetGiftsAsync();
        Task<Gift?> GetByIdAsync(int id);
        Task<IEnumerable<GiftDto>> GetGiftsWithDonatorAndCategory();
        Task<Gift> CreateGiftAsync(GiftDto gift);
        Task UpdateGiftAsync(GiftDto giftDto);
        Task DeleteGiftAsync(int id);
        Task<IEnumerable<GiftDto>> SearchAsync(string text);
        Task<IEnumerable<GiftDto>> GetGiftsBySortAsync(string? sortBy, string? order);
        Task<IEnumerable<GiftDto>> GetGiftsWithPurchasesAsync();
        Task<User> LotteryAsync(int giftId);
        Task CreateDonatorAsync(Donator donator);

    }


}


