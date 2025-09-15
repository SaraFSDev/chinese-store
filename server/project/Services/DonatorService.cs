using project.Models;
using project.Repository.IRepository;
using project.Services.IServices;
using project.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;  // הוספת לוגים

namespace project.Services
{
    public class DonatorService : IDonatorService
    {
        private readonly IDonatorRepository _donatorRepository;
        private readonly ILogger<DonatorService> _logger;

        public DonatorService(IDonatorRepository donatorRepository, ILogger<DonatorService> logger)
        {
            _donatorRepository = donatorRepository;
            _logger = logger;
        }

        public async Task CreateDonatorAsync(Donator donator)
        {
            try
            {
                _logger.LogInformation($"Creating donator with name: {donator.Name}");
                await _donatorRepository.CreateDonatorAsync(donator);
                _logger.LogInformation($"Donator {donator.Name} created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating donator {donator.Name}: {ex.Message}");
                throw new InvalidOperationException("Error creating donator.", ex);
            }
        }

        public async Task DeleteDonatorAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting donator with ID: {id}");
                await _donatorRepository.DeleteDonatorAsync(id);
                _logger.LogInformation($"Successfully deleted donator with ID: {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting donator with ID {id}: {ex.Message}");
                throw new InvalidOperationException($"Error deleting donator with ID {id}.", ex);
            }
        }

        public async Task<Donator?> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching donator with ID: {id}");
                var donator = await _donatorRepository.GetByIdAsync(id);
                if (donator == null)
                {
                    _logger.LogWarning($"Donator with ID {id} not found.");
                }
                return donator;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching donator with ID {id}: {ex.Message}");
                throw new InvalidOperationException($"Error fetching donator with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<Donator>> GetDonatorAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all donators.");
                var donators = await _donatorRepository.GetDonatorsAsync();
                return donators;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching donators: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateDonatorAsync(Donator donator)
        {
            try
            {
                _logger.LogInformation($"Updating donator with ID: {donator.Id}");
                await _donatorRepository.UpdateDonatorAsync(donator);
                _logger.LogInformation($"Donator with ID {donator.Id} updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating donator with ID {donator.Id}: {ex.Message}");
                throw new InvalidOperationException($"Error updating donator with ID {donator.Id}.", ex);
            }
        }

        public async Task<IEnumerable<Donator>> FilterByNameAsync()
        {
            try
            {
                _logger.LogInformation("Filtering donators by name.");
                var donators = await _donatorRepository.GetDonatorsAsync();
                var orderedDonators = donators.OrderBy(d => d.Name);
                return orderedDonators;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error filtering donators by name: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Donator>> FilterByEmailAsync()
        {
            try
            {
                _logger.LogInformation("Filtering donators by email.");
                var donators = await _donatorRepository.GetDonatorsAsync();
                var orderedDonators = donators.OrderBy(d => d.Email);
                return orderedDonators;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error filtering donators by email: {ex.Message}");
                throw;
            }
        }

        public async Task<object> GetDonatorWithGiftsAsync(int donatorId)
        {
            try
            {
                _logger.LogInformation($"Fetching donator with gifts for ID: {donatorId}");
                var donator = await _donatorRepository.GetByIdAsync(donatorId);
                if (donator == null)
                {
                    _logger.LogWarning($"Donator with ID {donatorId} not found.");
                    return null;
                }

                var gifts = await _donatorRepository.GetGiftsByDonatorAsync(donatorId);
                if (gifts == null || !gifts.Any())
                {
                    _logger.LogWarning($"No gifts found for donator with ID {donatorId}.");
                }

                return new
                {
                    DonatorName = donator.Name,
                    Gifts = gifts.Select(g => new { g.Name, g.image })
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching donator with gifts for ID {donatorId}: {ex.Message}");
                throw new InvalidOperationException($"Error fetching donator with gifts for ID {donatorId}.", ex);
            }
        }
        public async Task<IEnumerable<DonatorDto>> GetDonatorsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all donators with DTO mapping via repository.");
                var donatorDtos = await _donatorRepository.GetDonators();

                if (donatorDtos == null)
                {
                    _logger.LogWarning("No donators found in the repository.");
                    return new List<DonatorDto>();
                }

                _logger.LogInformation($"Successfully fetched {donatorDtos.Count()} donators from repository.");
                return donatorDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching donators: {ex.Message}");
                throw new InvalidOperationException("Error occurred while fetching donators.", ex);
            }
        }
    }
    

}
