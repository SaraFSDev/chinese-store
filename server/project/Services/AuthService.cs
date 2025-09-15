using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using project.Models;
using project.Models.ModelsDTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using project.Repository.IRepository;
using project.Services.IServices;
using project.Repositories;
using project.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;


namespace project.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ProjectDbContext _dBContext;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration, ProjectDbContext dBContext, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _dBContext = dBContext;
            _logger = logger;
        }
        public async Task RegisterAsync(Register model)
        {
            try
            {
                var existingUserByUsername = await _userManager.FindByNameAsync(model.Username);
                if (existingUserByUsername != null)
                {
                    _logger.LogWarning("Registration failed: Username '{Username}' is already taken.", model.Username);
                    throw new ApplicationException("Username is already taken");
                }

                var existingUserByEmail = await _userManager.FindByEmailAsync(model.Email);
                if (existingUserByEmail != null)
                {
                    _logger.LogWarning("Registration failed: Email '{Email}' is already registered.", model.Email);
                    throw new ApplicationException("Email is already registered");
                }

                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    _logger.LogError("Registration failed: {Errors}", string.Join(", ", errors));
                    throw new ApplicationException($"Registration failed: {string.Join(", ", errors)}");
                }

                _logger.LogInformation("User registered successfully: {Username}", model.Username);
            }
            catch (ApplicationException ex)
            {
                throw new HttpRequestException(ex.Message, ex, System.Net.HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration for user {Username}.", model.Username);
                throw new HttpRequestException("An unexpected error occurred during registration.", ex, System.Net.HttpStatusCode.InternalServerError);
            }
        }


        public async Task<IActionResult> LoginAsync(Login model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return new ObjectResult("Invalid email or password.") { StatusCode = 401 }; 
            }

            string token = await GenerateJwtToken(user);
            var isAdmin = await _userManager.IsInRoleAsync(user, "manager");
            
            return new ObjectResult(new { token = token, isAdmin = isAdmin }) { StatusCode = 200 }; 
        }

        private async Task<string> GenerateJwtToken(IdentityUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("UserId", user.Id),
            new Claim(ClaimTypes.Name, user.UserName),

            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", user.Email)
        };
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }







    }
}
