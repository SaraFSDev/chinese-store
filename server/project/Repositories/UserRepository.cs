using project.Models;
using project.Models.ModelsDTO;
using project.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;


namespace project.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ProjectDbContext _dBContext;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ProjectDbContext dBContext, ILogger<UserRepository> logger)
        {
            _dBContext = dBContext;
            _logger = logger;
        }

        public async Task UpdateUserAsync(User user, int quantity)
        {
            _dBContext.Users.Update(user);
            await _dBContext.SaveChangesAsync();
        }
      
        public async Task AddToBasketAsync(User user, Gift gift, int quantity)
        {
            _logger.LogInformation("Starting to add Gift {GiftId} (Quantity: {Quantity}) to User {UserId}'s basket", gift.Id, quantity, user.Id);

            var userGift = await _dBContext.UserGifts.FirstOrDefaultAsync(ug => ug.UserId == user.Id && ug.GiftId == gift.Id);

            if (userGift != null)
            {
                userGift.Quantity = quantity;
                _logger.LogInformation("Updated existing UserGift entry for Gift {GiftId} (New Quantity: {Quantity}) and User {UserId}", gift.Id, quantity, user.Id);
            }
            else
            {
                userGift = new UserGift { UserId = user.Id, GiftId = gift.Id, Quantity = quantity };
                await _dBContext.UserGifts.AddAsync(userGift);
                _logger.LogInformation("Created new UserGift entry for Gift {GiftId} (Quantity: {Quantity}) and User {UserId}", gift.Id, quantity, user.Id);
            }

            try
            {
                await _dBContext.SaveChangesAsync();
                _logger.LogInformation("Successfully saved changes to database for Gift {GiftId} and User {UserId} (Quantity: {Quantity})", gift.Id, user.Id, quantity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving changes to database for Gift {GiftId} and User {UserId}", gift.Id, user.Id);
                throw;
            }
        }


        public async Task<User?> GetByUserNameAndEmailAsync(string userName, string email)
        {
            Console.WriteLine($"NormalizedUserName: {userName.ToUpper()}, Email: {email.ToLower()}");
            try
            {
                var user = await _dBContext.Users
                    .FromSqlRaw("SELECT * FROM AspNetUsers WHERE NormalizedUserName = @p0 AND Email = @p1 AND Discriminator = 'User'",
                                userName.ToUpper(),
                                email.ToLower())
                    .FirstOrDefaultAsync();

                Console.WriteLine($"User from raw SQL: UserName={user?.UserName}, Email={user?.Email}");
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing SQL: {ex.Message} \n{ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<UserGiftDTO>> GetBasketUserAsync(string userId)
        {
            _logger.LogInformation("Start fetching user gifts for userId: {UserId}", userId);

            try
            {
                var userGifts = await _dBContext.UserGifts
                    .Where(ug => ug.UserId == userId)
                    .Include(ug => ug.Gift)
                    .ToListAsync();

                if (userGifts == null || !userGifts.Any())
                {
                    _logger.LogWarning("No gifts found for userId: {UserId}", userId);
                    return new List<UserGiftDTO>();
                }

                var result = userGifts.Select(ug => new UserGiftDTO
                {
                    GiftId = ug.GiftId,
                    GiftName = ug.Gift.Name,
                    GiftPrice = ug.Gift.Price,
                    GiftImage = ug.Gift.image,
                    Quantity = ug.Quantity
                }).ToList();

                _logger.LogInformation("Successfully fetched gifts for userId: {UserId}, result count: {ResultCount}", userId, result.Count);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching gifts for userId: {UserId}", userId);
                throw;
            }
        }
        public async Task RemoveGiftFromUserAsync(string userId, int giftId)
        {
            _logger.LogInformation("Removing Gift {GiftId} for User {UserId} in database", giftId, userId);
            var userGift = await _dBContext.UserGifts.FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GiftId == giftId);


            if (userGift == null)
            {
                _logger.LogWarning("No UserGift entry found for Gift {GiftId} and User {UserId}", giftId, userId);
                return;
            }
            var isGiftPurchased = await _dBContext.UserGifts
               .AnyAsync(ug => ug.GiftId == giftId && ug.GiftStatus == "Purchased");
            if (isGiftPurchased)
            {
                throw new InvalidOperationException($"Cannot remove gift with ID {giftId} from user {userId} because it is purchased.");
            }

            _dBContext.UserGifts.Remove(userGift);
            await _dBContext.SaveChangesAsync();
            _logger.LogInformation("Removed Gift {GiftId} for User {UserId} successfully", giftId, userId);
        }
        public async Task<IEnumerable<UserGift>> GetBasketByUserAsync(string userId)
        {
            _logger.LogInformation("Starting to fetch basket for userId: {UserId}", userId);
   

            try
            {
                var userGifts = await _dBContext.UserGifts
                    .Where(ug => ug.UserId == userId)
                    .Include(ug => ug.Gift)
                    .ToListAsync();

                if (userGifts == null || !userGifts.Any())
                {
                    _logger.LogWarning("No gifts found for userId: {UserId}", userId);
                    return Enumerable.Empty<UserGift>();
                }

                _logger.LogInformation("Successfully fetched {GiftCount} gifts for userId: {UserId}", userGifts.Count, userId);
                return userGifts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching basket for userId: {UserId}", userId);
                throw; 
            }
        }
        public async Task ConfirmPurchaseAsync(string userId)
        {
            _logger.LogInformation("Fetching UserGifts for User {UserId} to confirm purchase", userId);
            var userGifts = await _dBContext.UserGifts
                .Where(ug => ug.UserId == userId && ug.Gift.Status == "Draft")
                .Include(ug => ug.Gift)
                .ToListAsync();

            if (userGifts == null || !userGifts.Any())
            {
                _logger.LogWarning("No draft gifts found for User {UserId}", userId);
                return;
            }

            foreach (var userGift in userGifts)
            {
                userGift.GiftStatus = "Purchased";
                _logger.LogInformation("Updated Gift {GiftId} to Purchased for User {UserId}", userGift.GiftId, userId);
            }

            await _dBContext.SaveChangesAsync();
            _logger.LogInformation("Purchase confirmed for User {UserId}", userId);
        }

        public async Task<IEnumerable<UserGiftDTO>> GetBuyersByGiftIdAsync(int giftId)
        {
            var buyers = await _dBContext.UserGifts
            .Where(ug => ug.GiftId == giftId && ug.GiftStatus == "Purchased")
            .Select(ug => new UserGiftDTO
            {
                GiftId = ug.GiftId,
                GiftName = ug.Gift.Name,
                GiftImage = ug.Gift.image,
                GiftPrice = ug.Gift.Price,
                UserId = ug.UserId,
                UserName = ug.User.UserName,
                UserEmail = ug.User.Email,
                Quantity = ug.Quantity
            })
            .ToListAsync();

            return buyers.Any() ? buyers : null;
        }


    }
}
