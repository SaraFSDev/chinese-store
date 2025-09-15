using project.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using project.Models.ModelsDTO;

namespace project.Repository.IRepository
{
    public interface IUserRepository
    {
        Task UpdateUserAsync(User user, int quantity);
        Task AddToBasketAsync(User user, Gift gift,int quantity);
        Task RemoveGiftFromUserAsync(string userId, int giftId);
        Task<User> GetByUserNameAndEmailAsync(string userName, string email);
        Task<IEnumerable<UserGiftDTO>> GetBasketUserAsync(string userId);
        Task<IEnumerable<UserGift>> GetBasketByUserAsync(string userId);
        Task ConfirmPurchaseAsync(string userId);
        Task<IEnumerable<UserGiftDTO>> GetBuyersByGiftIdAsync(int giftId);

    }
}
