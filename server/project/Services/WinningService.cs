using project.Models;
using project.Repository.IRepository;
using project.Services.IServices;
using project.Repositories.IRepositories;

namespace project.Services
{
    public class WinningService : IWinningService
    {
        private readonly IWinningRepository _winningRepository;
        private readonly ILogger<WinningService> _logger;

        public WinningService(IWinningRepository winningRepository, ILogger<WinningService> logger)
        {
            _winningRepository = winningRepository;
            _logger = logger;
        }

        public async Task DeleteWinningAsync(int id)
        {
            _logger.LogInformation("Request to delete winning with id: {Id} received.", id);
            await _winningRepository.DeleteWinningAsync(id);
            _logger.LogInformation("Winning with id: {Id} deleted successfully.", id);
        }

        public async Task<Winning?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching winning by id: {Id}.", id);
            var result = await _winningRepository.GetByIdAsync(id);

            if (result == null)
            {
                _logger.LogWarning("No winning found with id: {Id}.", id);
            }
            else
            {
                _logger.LogInformation("Successfully fetched winning with id: {Id}.", id);
            }

            return result;
        }

        public async Task<IEnumerable<Winning>> GetWinningAsync()
        {
            _logger.LogInformation("Fetching all winnings.");
            var winnings = await _winningRepository.GetWinningAsync();
            _logger.LogInformation("Successfully fetched {WinningCount} winnings.", winnings.Count());
            return winnings;
        }
    }

}
