using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project.Models;
using project.Services.IServices;

namespace project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonatorController : ControllerBase
    {
        private readonly IDonatorService _donatorService;
        private readonly ILogger<DonatorController> _logger;

        public DonatorController(IDonatorService donatorService, ILogger<DonatorController> logger)
        {
            _donatorService = donatorService;
            _logger = logger;
        }

        [Authorize(Roles = "manager")]
        [HttpGet]
        public async Task<IActionResult> GetDonatorsAsync()
        {
            try
            {
                var donators = await _donatorService.GetDonatorAsync();
                return Ok(donators);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching donators: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        

        [HttpGet("{id}")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var donator = await _donatorService.GetByIdAsync(id);
                if (donator == null)
                {
                    _logger.LogWarning($"Donator with ID {id} not found.");
                    return NotFound($"Donator with ID {id} not found.");
                }
                return Ok(donator);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching donator with ID {id}: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPut]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> UpdateDonatorAsync(Donator donator)
        {
            try
            {
                await _donatorService.UpdateDonatorAsync(donator);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating donator with ID {donator.Id}: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteDonatorAsync(int id)
        {
            try
            {
                await _donatorService.DeleteDonatorAsync(id);
                _logger.LogInformation("DeleteGiftAsync completed successfully with ID {Id} at {Time}", id, DateTime.UtcNow);
                return Ok(new { message = "Gift deleted successfully." });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error occurred in DeleteGiftAsync with ID {Id} at {Time}", id, DateTime.UtcNow);
                return NotFound(new { message = $"Donator with ID {id} not found." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting donator with ID {id}: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpGet("filterByEmail")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> FilterByEmailAsync()
        {
            try
            {
                var donators = await _donatorService.FilterByEmailAsync();
                return Ok(donators);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error filtering donators by email: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("filterByName")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> FilterByNameAsync()
        {
            try
            {
                var donators = await _donatorService.FilterByNameAsync();
                return Ok(donators);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error filtering donators by name: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> CreateDonatorAsync(Donator donator)
        {
            _logger.LogInformation($"Received request to create donator: {donator.Name}");

            try
            {
                await _donatorService.CreateDonatorAsync(donator);
                _logger.LogInformation($"Successfully created donator: {donator.Name}");
                return Ok(new { message = "Donator created successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while creating donator: {donator.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }


        [HttpGet("giftsByDonator/{donatorId}")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> GetDonatorWithGiftsAsync(int donatorId)
        {
            try
            {
                var result = await _donatorService.GetDonatorWithGiftsAsync(donatorId);
                if (result == null)
                {
                    _logger.LogWarning($"No gifts found for donator with ID {donatorId}");
                    return NotFound($"No gifts found for donator with ID {donatorId}");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching gifts for donator with ID {donatorId}: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }
    }


}
