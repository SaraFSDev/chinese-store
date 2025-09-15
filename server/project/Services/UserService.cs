using project.Models;
using project.Repository.IRepository;
using project.Services.IServices;
using project.Models.ModelsDTO;
using System.Text.Json;
using System.Text.Json.Serialization;
using project.Repository;

namespace project.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGiftRepository _giftRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IGiftRepository giftRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _giftRepository = giftRepository;

            _logger = logger;
        }



        public async Task UpdateUserAsync(User user, int quantity)
        {
            _logger.LogInformation("Updating user {UserId} with Quantity: {Quantity}", user.Id, quantity);
            await _userRepository.UpdateUserAsync(user, quantity);
            _logger.LogInformation("Updated user {UserId} successfully", user.Id);
        }

        public async Task AddToBasketAsync(User user, Gift gift, int quantity)
        {
            _logger.LogInformation("Adding Gift {GiftId} to User {UserId}'s basket with Quantity: {Quantity}", gift.Id, user.Id, quantity);
            if (gift.IsLotteryCompleted)
            {
                _logger.LogWarning("Gift {GiftId} cannot be added to User {UserId}'s basket. Lottery is completed.", gift.Id, user.Id);
                throw new Exception("Cannot purchase gift, lottery has already been completed.");
            }
            await _userRepository.AddToBasketAsync(user, gift, quantity);
            _logger.LogInformation("Added Gift {GiftId} to User {UserId}'s basket successfully", gift.Id, user.Id);
        }

        public async Task<User> GetByUserNameAndEmailAsync(string userName, string email)
        {
            _logger.LogInformation("Received request to get user by username: {UserName} and email: {Email}", userName, email);

            try
            {
                var user = await _userRepository.GetByUserNameAndEmailAsync(userName, email);

                if (user == null)
                {
                    _logger.LogInformation("No user found for username: {UserName} and email: {Email}", userName, email);
                }
                else
                {
                    _logger.LogInformation("Successfully retrieved user: {UserName} with email: {Email}", userName, email);
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user by username: {UserName} and email: {Email}", userName, email);
                throw;
            }
        }

        public async Task RemoveGiftFromUserAsync(string userId, int giftId)
        {
            _logger.LogInformation("Removing Gift {GiftId} from User {UserId}'s basket", giftId, userId);
            await _userRepository.RemoveGiftFromUserAsync(userId, giftId);
            _logger.LogInformation("Removed Gift {GiftId} from User {UserId}'s basket successfully", giftId, userId);
        }


        public async Task<IEnumerable<UserGift>> GetBasketByUserAsync(string userId)
        {

            _logger.LogInformation("Received request to get basket for user: {UserId}", userId);
            try
            {
                var userGifts = await _userRepository.GetBasketByUserAsync(userId);

                if (userGifts == null || !userGifts.Any())
                {
                    _logger.LogInformation("No gifts found for user: {UserId}", userId);

                    return Enumerable.Empty<UserGift>();
                }

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true
                };

                var jsonResponse = JsonSerializer.Serialize(userGifts, options);

                _logger.LogInformation("Serialized response: {JsonResponse}", jsonResponse);

                return userGifts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching basket for user: {UserId}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<UserGiftDTO>> GetBasketUserAsync(string userId)
        {
            _logger.LogInformation("Received request to get basket for user: {UserId}", userId);

            try
            {


                var userGifts = await _userRepository.GetBasketUserAsync(userId);

                if (userGifts == null || !userGifts.Any())
                {
                    _logger.LogInformation("No gifts found for user: {UserId}", userId);
                    return Enumerable.Empty<UserGiftDTO>();
                }

                _logger.LogInformation("Successfully retrieved {GiftCount} gifts for user: {UserId}", userGifts.Count(), userId);
                return userGifts;
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access for user: {UserId}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching basket for user: {UserId}", userId);
                throw;
            }
        }

        public async Task ConfirmPurchaseAsync(string userId)
        {
            _logger.LogInformation("Confirming purchase for User {UserId}", userId);
            await _userRepository.ConfirmPurchaseAsync(userId);
            _logger.LogInformation("Purchase confirmed for User {UserId}", userId);
        }

        public async Task<IEnumerable<UserGiftDTO>> GetBuyersByGiftIdAsync(int giftId)
        {
            return await _userRepository.GetBuyersByGiftIdAsync(giftId);
        }


    }
}
