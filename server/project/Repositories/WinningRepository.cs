using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.Repository;
using project.Repositories.IRepositories;
using project.Models;
using Microsoft.EntityFrameworkCore;
using project.Models.ModelsDTO;

namespace project.Repositories
{
    public class WinningRepository : IWinningRepository
    {
        private readonly ProjectDbContext _dBContext;
        private readonly ILogger<WinningRepository> _logger;

        public WinningRepository(ProjectDbContext dBContext, ILogger<WinningRepository> logger)
        {
            _dBContext = dBContext;
            _logger = logger;
        }

        public async Task<IEnumerable<Winning>> GetWinningAsync()
        {
            _logger.LogInformation("Fetching all winnings from database.");
            var winnings = await _dBContext.Winnings.ToListAsync();
            _logger.LogInformation("Successfully fetched {WinningCount} winnings.", winnings.Count());
            return winnings;
        }

        public async Task<List<Gift>> GetWinnersWithGiftsAsync()
        {
            _logger.LogInformation("Starting to fetch winners with their gifts.");

            try
            {
                var gifts = await _dBContext.Gifts
                    .Where(g => g.WinningId.HasValue)  
                    .Include(g => g.Winning)          
                    .ThenInclude(w => w.LinkedUser)    
                    .ToListAsync();

                if (gifts == null || !gifts.Any())
                {
                    _logger.LogWarning("No winners found with associated gifts.");
                    return new List<Gift>();
                }

                _logger.LogInformation("Successfully fetched {GiftCount} gifts for {WinnerCount} winners.", gifts.Count, gifts.Select(g => g.Winning?.LinkedUser?.Id).Distinct().Count());
                return gifts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching winners with gifts.");
                throw; 
            }
        }
            public async Task<Winning?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching winning by id: {Id} from database.", id);
            var winning = await _dBContext.Winnings.FirstOrDefaultAsync(x => x.Id == id);

            if (winning == null)
            {
                _logger.LogWarning("No winning found with id: {Id}.", id);
            }
            else
            {
                _logger.LogInformation("Successfully fetched winning with id: {Id}.", id);
            }

            return winning;
        }

        public async Task DeleteWinningAsync(int id)
        {
            _logger.LogInformation("Request to delete winning with id: {Id} received at repository level.", id);
            var winning = await GetByIdAsync(id);

            if (winning == null)
            {
                _logger.LogError("No winning found with id: {Id} to delete.", id);
                throw new ArgumentException("Winning not found");
            }

            _dBContext.Winnings.Remove(winning);
            await _dBContext.SaveChangesAsync();
            _logger.LogInformation("Successfully deleted winning with id: {Id}.", id);
        }

        public async Task<(List<GiftRevenueDTO>, decimal)> GetGiftRevenuesAsync()
        {
            _logger.LogInformation("Calculating gift revenues.");
            var giftRevenues = await (from g in _dBContext.Gifts
                                      join ug in _dBContext.UserGifts
                                      .Where(x=>x.GiftStatus== "Purchased")
                                      on g.Id equals ug.GiftId    
                                      group new { g, ug } by g.Id into gGroup
                                     
                                      select new GiftRevenueDTO
                                      {
                                          GiftName = gGroup.FirstOrDefault().g.Name,
                                          Revenue = gGroup.Sum(x => x.ug.Quantity * x.g.Price)
                                      }).ToListAsync();

            decimal totalRevenue = giftRevenues.Sum(gr => gr.Revenue);
            _logger.LogInformation("Total revenue calculated: {TotalRevenue}.", totalRevenue);
            return (giftRevenues, totalRevenue);
        }
    }

}


