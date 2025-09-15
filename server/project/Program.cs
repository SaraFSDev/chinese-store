using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using project.Repository;
using project.Repositories;
using project.Repository.IRepository;
using project.Repositories.IRepositories;
using project.Services;
using project.Services.IServices;
using project.Controllers.Middlewares;
using System.Text;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day) 
    .Enrich.FromLogContext() 
    .CreateLogger();

try
{
    Log.Information("Starting up the application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

    builder.Services.AddDbContext<ProjectDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

    builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<ProjectDbContext>()
        .AddDefaultTokenProviders();

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
        };
    });

    builder.Services.AddScoped<IGiftRepository, GiftRepository>();
    builder.Services.AddScoped<IGiftService, GiftService>();
    builder.Services.AddScoped<IDonatorRepository, DonatorRepository>();
    builder.Services.AddScoped<IDonatorService, DonatorService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<ICategoryService, CategoryService>();
    builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
    builder.Services.AddScoped<IReportService, ReportService>();
    builder.Services.AddScoped<IWinningService, WinningService>();
    builder.Services.AddScoped<IWinningRepository, WinningRepository>();

    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
    });

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Project API",
            Description = "A simple example ASP.NET Core API to manage project",
            Contact = new OpenApiContact
            {
                Name = "Your Name",
                Email = "your.email@example.com",
                Url = new Uri("https://yourwebsite.com"),
            }
        });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter 'Bearer {your_token}'"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();

        app.UseSwagger();

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Project API V1");
            c.RoutePrefix = string.Empty;
        });
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        await DbInitializer.InitializeAsync(services);
    }

    app.UseCors("CorsPolicy");

    app.UseHttpsRedirection();
    app.UseRouting();

    app.UseMiddleware<OurMiddleware>();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
