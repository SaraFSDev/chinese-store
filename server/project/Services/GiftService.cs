using AutoMapper;
using project.Models;
using project.Models.ModelsDto;
using project.Repository.IRepository;
using project.Services.IServices;
using Microsoft.Extensions.Logging;

namespace project.Services
{
    public class GiftService : IGiftService
    {
        private readonly IGiftRepository _giftRepository;
        private readonly IDonatorRepository _donatorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GiftService> _logger;
        

        public GiftService(IGiftRepository giftRepository, IDonatorRepository donatorRepository, ICategoryRepository categoryRepository, IMapper mapper, ILogger<GiftService> logger)
        {
            _giftRepository = giftRepository;
            _donatorRepository = donatorRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Gift> CreateGiftAsync(GiftDto giftDto)
        {
            _logger.LogInformation("Starting CreateGiftAsync for GiftDto: {GiftDto} at {Time}", giftDto.Name, DateTime.UtcNow);
            var donator = await _donatorRepository.GetDonatorByNameAsync(giftDto.DonatorName);

            if (donator == null)
            {
                throw new ArgumentException("Donator not found.");
            }
            var category = await _categoryRepository.GetCategoryByNameAsync(giftDto.Category);

            if (category == null)
            {
                throw new ArgumentException("Category not found.");
            }
            if (await _giftRepository.ExistsByNameAsync(giftDto.Name))
            {
                throw new ArgumentException($"Gift with the name '{giftDto.Name}' already exists.");
            }
            try
            {
                var gift = _mapper.Map<Gift>(giftDto);
                gift.DonatorId = donator.Id;
                gift.CategoryId = category.Id;
                await _giftRepository.CreateGiftAsync(gift);
                _logger.LogInformation("CreateGiftAsync completed successfully for Gift: {GiftDto} at {Time}", giftDto.Name, DateTime.UtcNow);
                return gift;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error in CreateGiftAsync at {Time}", DateTime.UtcNow);
                throw new Exception("An error occurred while creating the gift.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in CreateGiftAsync at {Time}", DateTime.UtcNow);
                throw new Exception("An error occurred while creating the gift.");
            }
        }



        public async Task<IEnumerable<Gift>> GetGiftsAsync()
        {
            try
            {
                var gifts = await _giftRepository.GetGiftsAsync();
                _logger.LogInformation("GetGiftsAsync completed successfully at {Time}", DateTime.UtcNow);
                return gifts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetGiftsAsync at {Time}", DateTime.UtcNow);
                throw new Exception("An error occurred while retrieving gifts.");
            }
        }

        public async Task<Gift?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Starting GetByIdAsync for Gift ID {Id} at {Time}", id, DateTime.UtcNow);

            try
            {
                var gift = await _giftRepository.GetByIdAsync(id);
                if (gift == null)
                {
                    _logger.LogWarning("Gift with ID {Id} not found at {Time}", id, DateTime.UtcNow);
                    return null; 
                }

                _logger.LogInformation("GetByIdAsync completed successfully for Gift ID {Id} at {Time}", id, DateTime.UtcNow);
                return gift;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetByIdAsync for Gift ID {Id} at {Time}", id, DateTime.UtcNow);
                throw new Exception("An error occurred while retrieving the gift.");
            }
        }

        public async Task<IEnumerable<GiftDto>> GetGiftsWithDonatorAndCategory()
        {
            _logger.LogInformation("Starting GetGiftsWithDonatorAndCategory at {Time}", DateTime.UtcNow);

            try
            {
                var gifts = await _giftRepository.GetGiftsWithDonatorAndCategory();
                _logger.LogInformation("GetGiftsWithDonatorAndCategory completed successfully at {Time}", DateTime.UtcNow);
                return gifts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetGiftsWithDonatorAndCategory at {Time}", DateTime.UtcNow);
                throw new Exception("An error occurred while retrieving gifts with donators and categories.");
            }
        }
        public async Task CreateDonatorAsync(Donator donator)
        {
            try
            {
                _logger.LogInformation($"Creating donator: {donator.Name}");
                await _donatorRepository.CreateDonatorAsync(donator);
                _logger.LogInformation($"Successfully created donator: {donator.Name}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating donator: {donator.Name}");
                throw; 
            }
        }

        public async Task UpdateGiftAsync(GiftDto giftDto)
        {
            _logger.LogInformation("Starting UpdateGiftAsync for Gift ID {Id} at {Time}", giftDto.Id, DateTime.UtcNow);
           
             var category = await _categoryRepository.GetCategoryByNameAsync(giftDto.Category);

            if (category == null)
            {
                throw new ArgumentException("Category not found.");
            }
            try
            {
                var gift = _mapper.Map<Gift>(giftDto);

                gift.CategoryId = category.Id;
                await _giftRepository.UpdateGiftAsync(gift);
                _logger.LogInformation("UpdateGiftAsync completed successfully for Gift ID {Id} at {Time}", giftDto.Id, DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in UpdateGiftAsync for Gift ID {Id} at {Time}", giftDto.Id, DateTime.UtcNow);
                throw new Exception("An error occurred while updating the gift.");
            }
        }

        public async Task DeleteGiftAsync(int id)
        {
            _logger.LogInformation("Starting DeleteGiftAsync for Gift ID {Id} at {Time}", id, DateTime.UtcNow);

            try
            {
                await _giftRepository.DeleteGiftAsync(id);
                _logger.LogInformation("DeleteGiftAsync completed successfully for Gift ID {Id} at {Time}", id, DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in DeleteGiftAsync for Gift ID {Id} at {Time}", id, DateTime.UtcNow);
                throw new Exception("An error occurred while deleting the gift.");
            }
        }

        public async Task<IEnumerable<GiftDto>> SearchAsync(string text)
        {
            _logger.LogInformation("Starting SearchAsync with text '{Text}' at {Time}", text, DateTime.UtcNow);

            try
            {
                var gifts = await _giftRepository.SearchAsync(text);
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

            try
            {
                var gifts = await _giftRepository.GetGiftsBySortAsync(sortBy, order);
                return gifts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetGiftsAsync at {Time}", DateTime.UtcNow);
                throw new Exception("An error occurred while retrieving the gifts.");
            }
        }

        public async Task<IEnumerable<GiftDto>> GetGiftsWithPurchasesAsync()
        {
            _logger.LogInformation("Starting GetGiftsWithPurchasesAsync at {Time}", DateTime.UtcNow);

            try
            {
                var gifts = await _giftRepository.GetGiftsWithPurchasesAsync();
                _logger.LogInformation("GetGiftsWithPurchasesAsync completed successfully at {Time}", DateTime.UtcNow);
                return gifts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetGiftsWithPurchasesAsync at {Time}", DateTime.UtcNow);
                throw new Exception("An error occurred while retrieving gifts with purchases.");
            }
        }

        public async Task<User?> LotteryAsync(int giftId)
        {
            _logger.LogInformation("Starting LotteryAsync in Service for Gift ID {GiftId} at {Time}", giftId, DateTime.UtcNow);

            try
            {
                var gift = await _giftRepository.GetGiftWithParticipantsAsync(giftId);
                if (gift == null || gift.UserGifts == null || !gift.UserGifts.Any())
                {
                    _logger.LogWarning("No participants found for Gift ID {GiftId} at {Time}", giftId, DateTime.UtcNow);
                    throw new ApplicationException("No participants found for this gift.");
                }

                gift.IsLotteryCompleted = true;
                await _giftRepository.UpdateGiftAsync(gift);

                var userList = new List<User>();
                var userGiftList = gift.UserGifts.Where(x => x.GiftStatus == "Purchased");
                foreach (var link in userGiftList)
                {
                    for (int i = 0; i < link.Quantity; i++)
                    {
                        userList.Add(link.User);
                    }
                }

                if (!userList.Any())
                {
                    _logger.LogWarning("User list is empty for Gift ID {GiftId} at {Time}", giftId, DateTime.UtcNow);
                    throw new ApplicationException("No participants found for this gift.");
                }

                var random = new Random();
                var randomIndex = random.Next(userList.Count);
                var winner = userList[randomIndex];

                await _giftRepository.LotteryAsync(gift, winner);

                return winner;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in LotteryAsync Service for Gift ID {GiftId} at {Time}", giftId, DateTime.UtcNow);
                throw new HttpRequestException("An error occurred during the lottery process.", ex, System.Net.HttpStatusCode.InternalServerError);
            }
        }

    }
}

