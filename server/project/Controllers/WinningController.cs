using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project.Models;
using project.Services.IServices;

namespace project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WinningController : ControllerBase
    {
        private readonly IWinningService _winningService;
        private readonly ILogger<WinningController> _logger;

        public WinningController(IWinningService winningService, ILogger<WinningController> logger)
        {
            _winningService = winningService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "manager")]

        public async Task<IEnumerable<Winning>> GetWinningsAsync()
        {
            _logger.LogInformation("Received request to fetch all winnings.");
            var winnings = await _winningService.GetWinningAsync();
            _logger.LogInformation("Successfully retrieved {WinningCount} winnings.", winnings.Count());
            return winnings;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "manager")]

        public async Task<Winning?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Received request to fetch winning by id: {Id}", id);
            var winning = await _winningService.GetByIdAsync(id);

            if (winning == null)
            {
                _logger.LogWarning("No winning found with id: {Id}", id);
            }
            else
            {
                _logger.LogInformation("Successfully retrieved winning with id: {Id}", id);
            }

            return winning;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "manager")]

        public async Task DeleteWinningAsync(int id)
        {
            _logger.LogInformation("Received request to delete winning with id: {Id}", id);
            await _winningService.DeleteWinningAsync(id);
            _logger.LogInformation("Successfully deleted winning with id: {Id}", id);
        }
    }

}
