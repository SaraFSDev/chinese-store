
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project.Models;
using project.Models.ModelsDto;
using project.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;


namespace project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GiftController : ControllerBase
    {
        private readonly IGiftService _giftService;
        private readonly IMapper _mapper;
        private readonly ILogger<GiftController> _logger;

        public GiftController(IGiftService giftService, IMapper mapper, ILogger<GiftController> logger)
        {
            _giftService = giftService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        
        public async Task<IEnumerable<Gift>> GetGiftsAsync()
        {
            _logger.LogInformation("Starting GetGiftsAsync at {Time}", DateTime.UtcNow);

            try
            {
                var gifts = await _giftService.GetGiftsAsync();
                _logger.LogInformation("GetGiftsAsync completed successfully at {Time}", DateTime.UtcNow);
                return gifts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetGiftsAsync at {Time}", DateTime.UtcNow);
                throw new Exception("An error occurred while retrieving the gifts.");
            }
        }

        [HttpGet("sortBy")]
        [Authorize]
        public async Task<IActionResult> GetGiftsBySortAsync(string? sortBy, string? order)
        {
            _logger.LogInformation("Received request for GetGiftsBySortAsync with parameters sortBy: {SortBy}, order: {Order}", sortBy, order);

            try
            {

                if (string.IsNullOrEmpty(sortBy) || string.IsNullOrEmpty(order))
                {
                    _logger.LogWarning("Invalid parameters. sortBy or order is null or empty.");
                    return BadRequest("Both 'sortBy' and 'order' parameters are required.");
                }

                var result = await _giftService.GetGiftsBySortAsync(sortBy, order);

                if (result == null || !result.Any())
                {
                    _logger.LogInformation("No gifts found for the provided sort criteria.");
                    return NotFound(new { message = "No gifts found." });
                }

                _logger.LogInformation("Successfully retrieved {Count} gifts.", result.Count());
                return Ok(result );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing GetGiftsBySortAsync.");
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }


        [HttpGet("giftsWithDonatorAndCategory")]
        [Authorize]

        public async Task<IEnumerable<GiftDto>> GetGiftsWithDonatorAndCategory()
        {
            _logger.LogInformation("Starting GetGiftsWithDonatorAndCategory at {Time}", DateTime.UtcNow);

            try
            {
                var gifts = await _giftService.GetGiftsWithDonatorAndCategory();
                _logger.LogInformation("GetGiftsWithDonatorAndCategory completed successfully at {Time}", DateTime.UtcNow);
                return gifts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetGiftsWithDonatorAndCategory at {Time}", DateTime.UtcNow);
                throw new Exception("An error occurred while retrieving the gifts with donators and categories.",ex);
            }
        }

        [HttpGet("{id}")]
        [Authorize]

        public async Task<ActionResult<Gift>> GetByIdAsync(int id)
        {
            _logger.LogInformation("Starting GetByIdAsync with ID {Id} at {Time}", id, DateTime.UtcNow);

            try
            {
                var gift = await _giftService.GetByIdAsync(id);
                if (gift == null)
                {
                    _logger.LogWarning("Gift with ID {Id} not found at {Time}", id, DateTime.UtcNow);
                    return NotFound($"Gift with ID {id} not found.");
                }

                _logger.LogInformation("GetByIdAsync completed successfully with ID {Id} at {Time}", id, DateTime.UtcNow);
                return Ok(gift);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetByIdAsync with ID {Id} at {Time}", id, DateTime.UtcNow);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the gift.");
            }
        }

        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> CreateGiftAsync([FromBody] GiftDto gift)
        {
            _logger.LogInformation("Starting CreateGiftAsync at {Time} with payload: {Payload}", DateTime.UtcNow, gift);

            if (gift == null)
            {
                _logger.LogWarning("Received null payload for CreateGiftAsync at {Time}", DateTime.UtcNow);
                //return BadRequest("Gift data is required.");
                return BadRequest(new { message = "Gift data is required." });
            }

            try
            {
                await _giftService.CreateGiftAsync(gift);
                _logger.LogInformation("CreateGiftAsync completed successfully at {Time}", DateTime.UtcNow);
                //return Ok("Gift created successfully.");
                return Ok(new { message = "Gift created successfully." });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error in CreateGiftAsync at {Time}", DateTime.UtcNow);
                //return BadRequest(ex.Message);
                return BadRequest(new { message = ex.Message });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in CreateGiftAsync at {Time}", DateTime.UtcNow);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the gift.");
            }
        }


        [HttpPut]
        [Authorize(Roles = "manager")]

        public async Task<IActionResult> UpdateGiftAsync(GiftDto gift)
        {
            _logger.LogInformation("Starting UpdateGiftAsync at {Time} with payload: {Payload}", DateTime.UtcNow, gift);

            if (gift == null)
            {
                _logger.LogWarning("Received null payload for UpdateGiftAsync at {Time}", DateTime.UtcNow);
                return BadRequest(new { message = "Gift data is required." });
            }
            try
            {
                await _giftService.UpdateGiftAsync(gift);
                _logger.LogInformation("UpdateGiftAsync completed successfully at {Time}", DateTime.UtcNow);
                return Ok(new { message = "Gift updated successfully." });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error in UpdateGiftAsync at {Time}", DateTime.UtcNow);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in UpdateGiftAsync at {Time}", DateTime.UtcNow);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the gift.");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "manager")]

        public async Task<IActionResult> DeleteGiftAsync(int id)
        {
            _logger.LogInformation("Starting DeleteGiftAsync with ID {Id} at {Time}", id, DateTime.UtcNow);

            try
            {
                await _giftService.DeleteGiftAsync(id);
                _logger.LogInformation("DeleteGiftAsync completed successfully with ID {Id} at {Time}", id, DateTime.UtcNow);
                return Ok(new { message = "Gift deleted successfully." });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error occurred in DeleteGiftAsync with ID {Id} at {Time}", id, DateTime.UtcNow);
                return NotFound(new { message = $"Gift with ID {id} not found." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in DeleteGiftAsync with ID {Id} at {Time}", id, DateTime.UtcNow);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the gift.");
            }
        }

        [HttpPost("lottery")]
        [Authorize(Roles = "manager")]

        public async Task<IActionResult> LotteryAsync([FromBody] int giftId)
        {
            _logger.LogInformation("Starting LotteryAsync for Gift ID {GiftId} at {Time}", giftId, DateTime.UtcNow);

            try
            {
                var winner = await _giftService.LotteryAsync(giftId);
                if (winner == null)
                {
                    _logger.LogWarning("No winner found for Gift ID {GiftId} at {Time}", giftId, DateTime.UtcNow);
                    return NotFound(new { message = "No winner found for this gift." });
                }

                _logger.LogInformation("LotteryAsync completed successfully for Gift ID {GiftId} at {Time}", giftId, DateTime.UtcNow);
                return Ok(new { winner = winner.UserName });
            }
            catch (HttpRequestException ex) 
            {
                _logger.LogError(ex, "Error occurred in LotteryAsync for Gift ID {GiftId} at {Time}", giftId, DateTime.UtcNow);
                return StatusCode(500, new { Error = ex.Message });
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Unexpected error in LotteryAsync for Gift ID {GiftId} at {Time}", giftId, DateTime.UtcNow);
                return StatusCode(500, new { Error = "An unexpected error occurred." });
            }
        }

        [HttpGet("search")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> SearchAsync(string text)
        {
            _logger.LogInformation("Starting SearchAsync with text '{Text}' at {Time}", text, DateTime.UtcNow);

            try
            {
                
                if (string.IsNullOrWhiteSpace(text))
                {
                    _logger.LogWarning("Search attempted with an empty or null text at {Time}", DateTime.UtcNow);
                    return BadRequest(new { message = "Search field cannot be empty. Please enter a keyword to search." });
                }

                var gifts = await _giftService.SearchAsync(text);
                _logger.LogInformation("SearchAsync completed successfully with text '{Text}' at {Time}", text, DateTime.UtcNow);
                return Ok(gifts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in SearchAsync with text '{Text}' at {Time}", text, DateTime.UtcNow);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while searching for gifts." });
            }
        }

        [Authorize(Roles = "manager")]
        [HttpGet("giftsWithPurchases")]
        public async Task<ActionResult<IEnumerable<GiftDto>>> GetGiftsWithPurchases()
        {
            _logger.LogInformation("Received request to get gifts with purchases.");

            try
            {
                var gifts = await _giftService.GetGiftsWithPurchasesAsync();

                if (gifts == null || !gifts.Any())
                {
                    _logger.LogWarning("No gifts with purchases found.");
                    return NotFound("No gifts with purchases found.");
                }

                _logger.LogInformation("Successfully retrieved {GiftCount} gifts with purchases.", gifts.Count());
                return Ok(gifts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving gifts with purchases.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

    }

}