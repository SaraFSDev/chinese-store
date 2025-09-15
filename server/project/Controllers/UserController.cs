using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project.Models;
using project.Services.IServices;
using project.Repositories;
using project.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using project.Models.ModelsDTO;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;




namespace project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IGiftRepository _giftRepository;
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, IUserRepository userRepository, IGiftRepository giftRepository, ILogger<UserController> logger)
        {
            _userService = userService;
            _userRepository = userRepository;
            _giftRepository = giftRepository;
            _logger = logger;
        }

        private async Task<User> GetUserByTokenAsync(string token)
        {
            _logger.LogInformation("Received request to fetch user by token.");

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Authorization token is missing.");
                throw new Exception("Authorization token is missing.");
            }

            try
            {
                _logger.LogInformation("Parsing token...");
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token.Replace("Bearer ", "")) as JwtSecurityToken;

                if (jsonToken == null)
                {
                    _logger.LogError("Invalid token format.");
                    throw new Exception("Invalid token format.");
                }

                var userName = jsonToken?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                if (string.IsNullOrEmpty(userName))
                {
                    _logger.LogError("User name not found in the token.");
                    throw new Exception("User name not found in the token.");
                }

                var email = jsonToken?.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogError("Email not found in the token.");
                    throw new Exception("Email not found in the token.");
                }

                userName = userName.Trim();
                email = email.Trim().ToLower();

                _logger.LogInformation("Attempting to retrieve user with username: {UserName} and email: {Email}", userName, email);

                User user = await _userRepository.GetByUserNameAndEmailAsync(userName, email);

                if (user == null)
                {
                    _logger.LogError("User not found with username: {UserName} and email: {Email}", userName, email);
                    throw new Exception("User not found.");
                }

                _logger.LogInformation("User successfully found: {UserName}", userName);

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user by token.");
                throw;
            }
        }

        [HttpPut]
        public async Task UpdateUserAsync(User user, int quantity)
        {
            _logger.LogInformation("Starting UpdateUserAsync with User: {UserId} and Quantity: {Quantity}", user.Id, quantity);
            await _userService.UpdateUserAsync(user, quantity);
            _logger.LogInformation("Completed UpdateUserAsync for User: {UserId}", user.Id);
        }

        [HttpGet("getWithEmail")]
        public async Task<User> GetByUserNameAndEmailAsync(string userName, string email)
        {
            _logger.LogInformation("Starting GetByUserNameAndEmailAsync with UserName: {UserName} and Email: {Email}", userName, email);
            var user = await _userRepository.GetByUserNameAndEmailAsync(userName, email);
            _logger.LogInformation("Completed GetByUserNameAndEmailAsync for User: {UserName}", user?.UserName);
            return user;
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToBasketAsync(int giftId, int quantity = 1)
        {
            _logger.LogInformation("Starting AddToBasketAsync with GiftId: {GiftId} and Quantity: {Quantity}", giftId, quantity);
            User user = null;

            try
            {
                var token = Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("Authorization token is missing in the request.");
                    return BadRequest(new { message = "Authorization token is required." });
                }

                _logger.LogInformation("Authorization token received, attempting to retrieve user.");


                user = await GetUserByTokenAsync(token);
                if (user == null)
                {
                    _logger.LogWarning("User not found for the provided token.");
                    return Unauthorized(new { message = "User not found." });
                }

                _logger.LogInformation("User with Id: {UserId} successfully retrieved.", user.Id);

                Gift gift = await _giftRepository.GetByIdAsync(giftId);
                if (gift == null)
                {
                    _logger.LogWarning("Gift with Id: {GiftId} not found.", giftId);
                    return NotFound(new { message = "Gift not found." });
                }

                _logger.LogInformation("Gift with Id: {GiftId} successfully retrieved.", giftId);

                await _userService.AddToBasketAsync(user, gift, quantity);
                _logger.LogInformation("Completed AddToBasketAsync for User: {UserId} and GiftId: {GiftId} with Quantity: {Quantity}", user.Id, giftId, quantity);

                return Ok(new { message = "Gift successfully added to basket." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding Gift {GiftId} to User {UserId}'s basket", giftId, user?.Id);
                return StatusCode(500, new { message = "Internal server error." });

            }
        }

        [HttpDelete]
        [Authorize]

        public async Task RemoveGiftFromUserAsync(int giftId)
        {
            try
            {
                _logger.LogInformation("Starting RemoveGiftFromUserAsync for GiftId: {GiftId}", giftId);

                var userId = User.FindFirst("UserId")?.Value;
                if (userId == null)
                {
                    _logger.LogWarning("UserId is missing. Unable to remove gift for GiftId: {GiftId}", giftId);
                    throw new InvalidOperationException("UserId is missing.");
                }

                await _userService.RemoveGiftFromUserAsync(userId, giftId);

                _logger.LogInformation("Completed RemoveGiftFromUserAsync for GiftId: {GiftId}", giftId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot remove gift with ID {giftId} from user {userId} because it is purchased.", giftId);
                throw new HttpRequestException("Cannot remove gift because it has already been purchased.");
            }
        }


        [HttpGet("basket")]
        [Authorize]
        public async Task<IActionResult> GetBasketByUserAsync()
        {
            _logger.LogInformation("Starting GetBasketByUserAsync");

            string userId = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("User is not authenticated. Cannot retrieve basket for user: {UserId}", userId);
                return Unauthorized(new { message = "You need to log in to add items to the basket." });
            }

            var basket = await _userService.GetBasketByUserAsync(userId);

            if (basket == null || !basket.Any())
            {
                return Ok(new { message = "No gifts found in the basket." });
            }

            _logger.LogInformation("Completed GetBasketByUserAsync for User: {UserId}", userId);
            return Ok(basket);
        }


        [HttpPost("confirmPurchase")]
        [Authorize]
        public async Task ConfirmPurchaseAsync()
        {
            string userId = User.FindFirst("UserId")?.Value;
            _logger.LogInformation("Starting ConfirmPurchaseAsync for User: {UserId}", userId);
            try
            {
                await _userService.ConfirmPurchaseAsync(userId);
                _logger.LogInformation("Completed ConfirmPurchaseAsync for User: {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ConfirmPurchaseAsync for User: {UserId}", userId);
                throw new InvalidOperationException("An error occurred while confirming the purchase.", ex);
            }
        }


        [HttpGet("userBasket")]
        [Authorize]
        public async Task<IEnumerable<UserGiftDTO>> GetUserGifts()
        {
            string userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User ID not found in claims.");
            }

            return await _userService.GetBasketUserAsync(userId);
        }


        [HttpGet("{giftId}/buyers")]
        public async Task<IEnumerable<UserGiftDTO>> GetGiftBuyers(int giftId)
        {
            var buyers = await _userService.GetBuyersByGiftIdAsync(giftId);

            if (buyers == null || !buyers.Any())
            {
                return new List<UserGiftDTO>();
            }
            return buyers;
        }


    }
}
