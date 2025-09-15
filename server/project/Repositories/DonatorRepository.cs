using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using project.Models;
using project.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.Repository
{
    public class DonatorRepository : IDonatorRepository
    {
        private readonly ProjectDbContext _dBContext;
        private readonly IMapper _mapper;
        private readonly ILogger<DonatorRepository> _logger;

        public DonatorRepository(ProjectDbContext dBContext, IMapper mapper, ILogger<DonatorRepository> logger)
        {
            _dBContext = dBContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Donator> GetDonatorByNameAsync(string name)
        {
            try
            {
                _logger.LogInformation($"Fetching donator by name: {name}");
                var donator = await _dBContext.Donators.FirstOrDefaultAsync(d => d.Name == name);

                if (donator == null)
                {
                    _logger.LogWarning($"Donator with name {name} not found.");
                }

                return donator;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching donator by name {name}: {ex.Message}");
                throw new InvalidOperationException("Error occurred while fetching the donator by name.", ex);
            }
        }

        public async Task CreateDonatorAsync(Donator donator)
        {
            try
            {
                _logger.LogInformation($"Adding donator with name: {donator.Name}");
                await _dBContext.Donators.AddAsync(donator);
                await _dBContext.SaveChangesAsync();
                _logger.LogInformation($"Successfully added donator: {donator.Name}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding donator {donator.Name}: {ex.Message}");
                throw new InvalidOperationException("Error occurred while adding the donator.", ex);
            }
        }

        public async Task DeleteDonatorAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Attempting to delete donator with ID: {id}");
                var donator = await GetByIdAsync(id);

                if (donator == null)
                {
                    _logger.LogWarning($"Donator with ID {id} not found. Cannot delete.");
                    throw new KeyNotFoundException($"Donator with ID {id} not found.");
                }
                
                    _dBContext.Donators.Remove(donator);
                    await _dBContext.SaveChangesAsync();
                    _logger.LogInformation($"Successfully deleted donator with ID: {id}");
                
            
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting donator with ID {id}: {ex.Message}");
                throw new InvalidOperationException("Error occurred while deleting the donator.", ex);
            }
        }

        public async Task<Donator?> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching donator with ID: {id}");
                var donator = await _dBContext.Donators.FirstOrDefaultAsync(x => x.Id == id);

                if (donator == null)
                {
                    _logger.LogWarning($"Donator with ID {id} not found.");
                }

                return donator;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching donator with ID {id}: {ex.Message}");
                throw new InvalidOperationException("Error occurred while fetching the donator by ID.", ex);
            }
        }

        public async Task<IEnumerable<Donator>> GetDonatorsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all donators.");
                var donators = await _dBContext.Donators.ToListAsync();
                _logger.LogInformation($"Successfully fetched {donators.Count} donators.");
                return donators;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching donators: {ex.Message}");
                throw new InvalidOperationException("Error occurred while fetching all donators.", ex);
            }
        }

        public async Task<IEnumerable<DonatorDto>> GetDonators()
        {
            try
            {
                _logger.LogInformation("Fetching donators with DTO mapping.");
                var donators = await _dBContext.Donators.ToListAsync();
                var donatorDtos = _mapper.Map<IEnumerable<DonatorDto>>(donators);
                _logger.LogInformation($"Successfully mapped and fetched {donatorDtos.Count()} donators.");
                return donatorDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching donators with DTO mapping: {ex.Message}");
                throw new InvalidOperationException("Error occurred while fetching donators with DTO mapping.", ex);
            }
        }

        public async Task UpdateDonatorAsync(Donator donator)
        {
            try
            {
                _logger.LogInformation($"Updating donator with ID: {donator.Id}");
                _dBContext.Donators.Update(donator);
                await _dBContext.SaveChangesAsync();
                _logger.LogInformation($"Successfully updated donator with ID: {donator.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating donator with ID {donator.Id}: {ex.Message}");
                throw new InvalidOperationException("Error occurred while updating the donator.", ex);
            }
        }

        public async Task<IEnumerable<Gift>> GetGiftsByDonatorAsync(int donatorId)
        {
            try
            {
                _logger.LogInformation($"Fetching gifts for donator ID: {donatorId}");
                var gifts = await _dBContext.Gifts.Where(g => g.DonatorId == donatorId).ToListAsync();

                if (!gifts.Any())
                {
                    _logger.LogWarning($"No gifts found for donator ID: {donatorId}");
                }

                _logger.LogInformation($"Successfully fetched {gifts.Count} gifts for donator ID: {donatorId}");
                return gifts;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching gifts for donator ID {donatorId}: {ex.Message}");
                throw new InvalidOperationException("Error occurred while fetching gifts for the donator.", ex);
            }
        }
    }
}
