using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace project.Controllers.Middlewares
{
    public class OurMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<OurMiddleware> _logger;
        private readonly IConfiguration _configuration;

        public OurMiddleware(RequestDelegate next, ILogger<OurMiddleware> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {

            var Paths = new List<(string path, string method)>
         {
        ("/api/Auth/register", "any"),
        ("/api/Auth/login", "any"),
        ("/api/Gift", "any"),
        ("/api/Category","get")
         };

            var requestPath = context.Request.Path.Value;
            if (Paths.Any(entry => requestPath != null && requestPath.StartsWith(entry.path, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }

            _logger.LogInformation($"Incoming Request: {context.Request.Method} {context.Request.Path}");

            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                _logger.LogWarning("Missing Authorization Header");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Authorization Header is missing.");
                return;
            }

            var token = context.Request.Headers["Authorization"].ToString().Split(" ").Last();

            if (!ValidateJwtToken(token))
            {
                _logger.LogWarning("Invalid Token");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid or Expired Token.");
                return;
            }

            await _next(context);

            _logger.LogInformation($"Outgoing Response: {context.Response.StatusCode}");
        }


        private bool ValidateJwtToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["JwtSettings:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}



