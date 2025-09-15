using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.Models;
using project.Models.ModelsDto;
using project.Services;
using project.Services.IServices;
using Microsoft.Extensions.Logging;

namespace project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Register model)
        {
            try
            {
                if (model == null)
                {
                    _logger.LogWarning("Registration attempted with invalid input (null).");
                    return BadRequest(new { message = "Invalid input data." });
                }

                await _authService.RegisterAsync(model);
                _logger.LogInformation("User registered successfully.");

                return Ok(new { message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed.");

                return StatusCode(500, new { Error = ex.Message });
            }
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(Login model)
        {
            if (model == null)
            {
                _logger.LogWarning("Login attempted with invalid input (null).");
                return BadRequest("Invalid input data.");
            }

            var result = await _authService.LoginAsync(model);
            if (result is ObjectResult objResult && objResult.StatusCode == 401)
            {
                _logger.LogWarning("Invalid login attempt: {Email}", model.Email);
            }
            return result;
        }
    }
}
