using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.Models;
using project.Models.ModelsDTO;

namespace project.Repositories.IRepositories
{
    public interface IWinningRepository
    {
        Task<List<Gift>> GetWinnersWithGiftsAsync();
        Task<(List<GiftRevenueDTO>, decimal)> GetGiftRevenuesAsync();
         Task<Winning?> GetByIdAsync(int id);
        Task DeleteWinningAsync(int id);
        Task<IEnumerable<Winning>> GetWinningAsync();
    }
}



