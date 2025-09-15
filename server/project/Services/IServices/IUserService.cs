using project.Models;
using project.Models.ModelsDTO;

namespace project.Services.IServices
{
    public interface IUserService
    {
     
        Task UpdateUserAsync(User user, int quantity);
        Task AddToBasketAsync(User user, Gift gift, int quantity);
        Task<User> GetByUserNameAndEmailAsync(string userName, string email);
        Task RemoveGiftFromUserAsync(string userId, int giftId);
        Task<IEnumerable<UserGiftDTO>> GetBasketUserAsync(string userId);
        Task<IEnumerable<UserGift>> GetBasketByUserAsync(string userId);
        Task ConfirmPurchaseAsync(string userId);
        Task<IEnumerable<UserGiftDTO>> GetBuyersByGiftIdAsync(int giftId);

    }
}
