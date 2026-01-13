using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using FinanceManagement.Infrastructure.Data;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Infrastructure.Repositories;
using FinanceManagement.Infrastructure.Services;
using FinanceManagement.API.Middleware;
using System.Security.Claims;
using FluentValidation.AspNetCore;
using FluentValidation;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/finance-api-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();

// 2. Enable Automatic Validation (This replaces the old AddFluentValidation)
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// 3. Register the validators from the assembly
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();

builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter JWT token like: Bearer {your token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddDbContext<FinanceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPartnerRepository, PartnerRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IBankTransactionRepository, BankTransactionRepository>();
builder.Services.AddScoped<IFinancialRepository, FinancialRepository>();

builder.Services.AddScoped<IFinancialService, FinancialService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET")
    ?? throw new Exception("JWT_SECRET not configured");

var key = Encoding.UTF8.GetBytes(secretKey);


builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
   {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        ClockSkew = TimeSpan.Zero,
        RoleClaimType = ClaimTypes.Role
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly",
        policy => policy.RequireRole("Admin"));

    options.AddPolicy("PartnerOnly",
        policy => policy.RequireRole("Partner" , "Admin"));

    options.AddPolicy("AdminOrPartner",
        policy => policy.RequireRole("Admin", "Partner"));
});


var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
                     ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    
    options.AddPolicy("DevelopmentPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000",
    "http://localhost:5173", "http://localhost:5178")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });

    options.AddPolicy("ProductionPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
// -----------------------------------------------------------------------

var app = builder.Build();

// 8. Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ✅ TASK-004: Apply Correct CORS Policy
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevelopmentPolicy");
}
else
{
    app.UseCors("ProductionPolicy");
}
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FinanceDbContext>();
    await context.Database.MigrateAsync();
    await SeedData(context);
}

app.Run();

static async Task SeedData(FinanceDbContext context)
{
    if (!context.Users.Any())
    {
        var users = new[]
        {
            new FinanceManagement.Domain.Entities.User
            {
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@company.com",
                //PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                //Role = FinanceManagement.Domain.Enums.UserRole.Admin,
                IsActive = true
            },
            new FinanceManagement.Domain.Entities.User
            {
                FirstName = "John",
                LastName = "Partner",
                Email = "partner1@company.com",
                //PasswordHash = BCrypt.Net.BCrypt.HashPassword("Partner123!"),
                Role = FinanceManagement.Domain.Enums.UserRole.Partner,
                IsActive = true
            }
        };

        context.Users.AddRange(users);
        await context.SaveChangesAsync();
    }

    // 2. Seed Partners (THIS WAS MISSING!)
    if (!context.Partners.Any())
    {
        // Find the user we just created
        var partnerUser = context.Users.FirstOrDefault(u => u.Email == "partner1@company.com");

        if (partnerUser != null)
        {
            var partner = new FinanceManagement.Domain.Entities.Partner
            {
                UserId = partnerUser.Id,
                PartnershipType = "Founding Partner",
                SharePercentage = 50.00m, // 50% Share
                IsMainPartner = true
            };

            context.Partners.Add(partner);
            await context.SaveChangesAsync();
        }
    }
}