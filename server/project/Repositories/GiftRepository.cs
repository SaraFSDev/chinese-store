using Microsoft.EntityFrameworkCore.Metadata;
using project.Models;
using project.Models.ModelsDto;
using project.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using project.Models.ModelsDTO;

namespace project.Repository
{
    public class GiftRepository : IGiftRepository
    {
        private readonly ProjectDbContext _dBContext;
        private readonly ILogger<GiftRepository> _logger;

        public GiftRepository(ProjectDbContext dBContext, ILogger<GiftRepository> logger)
        {
            _dBContext = dBContext;
            _logger = logger;
        }

        public async Task CreateGiftAsync(Gift gift)
        {
            _logger.LogInformation("Starting CreateGiftAsync for Gift: {Gift} at {Time}", gift.Name, DateTime.UtcNow);

            try
            {
                await _dBContext.Gifts.AddAsync(gift);
                await _dBContext.SaveChangesAsync();
                _logger.LogInformation("CreateGiftAsync completed successfully for Gift: {Gift} at {Time}", gift.Name, DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in CreateGiftAsync for Gift: {Gift} at {Time}", gift.Name, DateTime.UtcNow);
                throw new Exception("An error occurred while creating the gift.");
            }
        }

        public async Task DeleteGiftAsync(int id)
        {
            _logger.LogInformation("Starting DeleteGiftAsync for Gift ID {Id} at {Time}", id, DateTime.UtcNow);

            try
            {
                Gift? gift = await GetByIdAsync(id);
                if (gift == null)
                {
                    _logger.LogWarning("Gift with ID {Id} not found at {Time}", id, DateTime.UtcNow);
                    throw new ArgumentException("Gift not found", nameof(id));
                }
              
                    _dBContext.Gifts.Remove(gift);
                    await _dBContext.SaveChangesAsync();
                    _logger.LogInformation("DeleteGiftAsync completed successfully for Gift ID {Id} at {Time}", id, DateTime.UtcNow);
               
     
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in DeleteGiftAsync for Gift ID {Id} at {Time}", id, DateTime.UtcNow);
                throw new Exception("An error occurred while deleting the gift.");
            }
        }

        public async Task<Gift?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Starting GetByIdAsync for Gift ID {Id} at {Time}", id, DateTime.UtcNow);

            try
            {
                var gift = await _dBContext.Gifts.FirstOrDefaultAsync(x => x.Id == id);
                if (gift == null)
                {
                    _logger.LogWarning("Gift with ID {Id} not found at {Time}", id, DateTime.UtcNow);
                }
                return gift;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetByIdAsync for Gift ID {Id} at {Time}", id, DateTime.UtcNow);
                throw new Exception("An error occurred while retrieving the gift.");
            }
        }

        public async Task<IEnumerable<Gift>> GetGiftsAsync()
        {
            //_logger.LogInformation("Starting GetGiftsAsync at {Time}", DateTime.UtcNow);

            try
            {
                var gifts = await _dBContext.Gifts.ToListAsync();
                _logger.LogInformation("GetGiftsAsync completed successfully at {Time}", DateTime.UtcNow);
                return gifts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetGiftsAsync at {Time}", DateTime.UtcNow);
                throw new Exception("An error occurred while retrieving the gifts.");
            }
        }

        public async Task<IEnumerable<GiftDto>> GetGiftsWithDonatorAndCategory()
        {
            _logger.LogInformation("Starting GetGiftsWithDonatorAndCategory at {Time}", DateTime.UtcNow);

            try
            {
                var gifts = await _dBContext.Gifts
                    .Include(g => g.Donator)
                    .Include(g => g.Category)
                    .Select(g => new GiftDto
                    {
                        Id = g.Id,
                        Name = g.Name,
                        Price = g.Price,
                        image = g.image,
                        IsLotteryCompleted = g.IsLotteryCompleted,
                        DonatorName = g.Donator != null ? g.Donator.Name : null,
                        Category = g.Category != null ? g.Category.name : null,
                        CategoryId = g.CategoryId,
                        Purchases = g.UserGifts.Count
                    })
                    .ToListAsync();




                _logger.LogInformation("GetGiftsWithDonatorAndCategory completed successfully at {Time}", DateTime.UtcNow);
                return gifts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetGiftsWithDonatorAndCategory at {Time}", DateTime.UtcNow);
                throw new Exception("An error occurred while retrieving the gifts with donators and categories.");
            }
        }


        public async Task CreateDonatorAsync(Donator donator)
        {
            try
            {
                _logger.LogInformation($"Starting to create donator: {donator.Name}");

                await _dBContext.Donators.AddAsync(donator);
                await _dBContext.SaveChangesAsync();

                _logger.LogInformation($"Successfully created donator: {donator.Name}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while adding donator: {donator.Name}");
                throw; 
            }
        }


        public async Task UpdateGiftAsync(Gift gift)
        {
            Gift? updateGift = await _dBContext.Gifts.FirstOrDefaultAsync(x => x.Id == gift.Id);
            if (updateGift != null)
            {
                updateGift.Name = gift.Name;
                updateGift.CategoryId = gift.CategoryId;
                updateGift.Price = gift.Price;
                updateGift.image = gift.image;
                updateGift.DonatorId = gift.DonatorId;
                await _dBContext.SaveChangesAsync();
            }
        }

        public async Task<Gift?> GetGiftWithParticipantsAsync(int giftId)
        {
            _logger.LogInformation("Fetching Gift with participants for Gift ID {GiftId} at {Time}", giftId, DateTime.UtcNow);

            return await _dBContext.Gifts
                .Include(g => g.UserGifts)
                .ThenInclude(ug => ug.User)
                .FirstOrDefaultAsync(g => g.Id == giftId);
        }

        public async Task<User> LotteryAsync(Gift gift, User winner)
        {
            _logger.LogInformation("Starting LotteryAsync in Repository for Gift ID {GiftId} at {Time}", gift.Id, DateTime.UtcNow);

            try
            {
                gift.Winning = new Winning
                {
                    Name = winner.UserName,
                    LinkedUserId = winner.Id,
                    LinkedUser = winner
                };
                gift.WinningId = gift.Winning.Id;

                await _dBContext.SaveChangesAsync();

                _logger.LogInformation("LotteryAsync completed successfully in Repository for Gift ID {GiftId} with Winner {Winner} at {Time}", gift.Id, winner.UserName, DateTime.UtcNow);
                return winner;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in LotteryAsync Repository for Gift ID {GiftId} at {Time}", gift.Id, DateTime.UtcNow);
                throw;
            }
        }


        public async Task<IEnumerable<GiftDto>> SearchAsync(string text)
        {
            _logger.LogInformation("Starting SearchAsync with text '{Text}' at {Time}", text, DateTime.UtcNow);

            try
            {
                var gifts = await _dBContext.Gifts
                    .Where(g => g.Name.Contains(text)
                    || (g.Donator != null && g.Donator.Name.Contains(text))
                    || g.UserGifts.Count.ToString() == text)
                    .Include(g => g.Donator)
                    .Include(g => g.Category)
                    .Select(g => new GiftDto
                    {
                        Id = g.Id,
                        Name = g.Name,
                        Price = g.Price,
                        DonatorName = g.Donator != null ? g.Donator.Name : null,
                        Category = g.Category != null ? g.Category.name : null,
                        CategoryId = g.CategoryId,
                        Purchases = g.UserGifts.Count
                    })
                    .ToListAsync();

                _logger.LogInformation("SearchAsync completed successfully with text '{Text}' at {Time}", text, DateTime.UtcNow);
                return gifts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in SearchAsync with text '{Text}' at {Time}", text, DateTime.UtcNow);
                throw new Exception("An error occurred while searching for gifts.");
            }
        }

        public async Task<IEnumerable<GiftDto>> GetGiftsBySortAsync(string? sortBy, string? order)
        {
            _logger.LogInformation("Starting GetGiftsAsync with sortBy: {SortBy}, order: {Order} at {Time}", sortBy, order, DateTime.UtcNow);

            try
            {
                var query = _dBContext.Gifts.AsQueryable();

                if (!string.IsNullOrEmpty(sortBy))
                {
                    if (sortBy == "Price")
                    {
                        query = order == "desc" ? query.OrderByDescending(g => g.Price) : query.OrderBy(g => g.Price);
                    }
                    else if (sortBy == "purchases")
                    {
                        query = order == "desc"
                            ? query.OrderByDescending(g => g.UserGifts.Sum(ug => ug.Quantity))
                            : query.OrderBy(g => g.UserGifts.Sum(ug => ug.Quantity));
                    }
                    else if (sortBy == "category")
                    {
                        query = order == "desc"
                                    ? query.Include(g => g.Category).OrderByDescending(g => g.Category.name)
                                    : query.Include(g => g.Category).OrderBy(g => g.Category.name);
                    }

                }

                var gifts = await query
                .Select(g => new GiftDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    Price = g.Price,
                    image = g.image,
                    Purchases = g.UserGifts
                         .Where(ug => ug.GiftStatus == "Purchased")
                         .Sum(ug => ug.Quantity),
                    Category = g.Category.name
                })
                .ToListAsync();
                _logger.LogInformation("GetGiftsAsync completed successfully at {Time}", DateTime.UtcNow);
                return gifts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetGiftsAsync with sortBy: {SortBy}, order: {Order} at {Time}", sortBy, order, DateTime.UtcNow);
                throw new Exception("An error occurred while retrieving the gifts.");
            }
        }

        public async Task<IEnumerable<GiftDto>> GetGiftsWithPurchasesAsync()
        {
            _logger.LogInformation("Starting GetGiftsWithPurchasesAsync at {Time}", DateTime.UtcNow);

            try
            {
                var gifts = await _dBContext.Gifts
                 .Select(g => new GiftDto
                 {
                     Id = g.Id,
                     Name = g.Name,
                     Price = g.Price,
                     image = g.image,
                     Purchases = g.UserGifts
                         .Where(ug => ug.GiftStatus == "Purchased")
                         .Sum(ug => ug.Quantity)
                 })
                 .ToListAsync();

                _logger.LogInformation("GetGiftsWithPurchasesAsync completed successfully at {Time}", DateTime.UtcNow);
                return gifts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetGiftsWithPurchasesAsync at {Time}", DateTime.UtcNow);
                throw new Exception("An error occurred while retrieving gifts with purchases.");
            }
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _dBContext.Gifts.AnyAsync(g => g.Name == name);
        }



    }
}
