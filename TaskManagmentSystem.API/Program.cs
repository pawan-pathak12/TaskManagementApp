// ============================================================
// Program.cs — Updated for Azure Deployment
// ============================================================

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagmentSystem.API.Data;
using TaskManagmentSystem.API.Interfaces.Repositories;
using TaskManagmentSystem.API.Interfaces.Service;
using TaskManagmentSystem.API.Repositories;
using TaskManagmentSystem.API.Repositories.EFCore;
using TaskManagmentSystem.API.Services;

var builder = WebApplication.CreateBuilder(args);

// ─────────────────────────────────────────
// 1. Controllers
// ─────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ─────────────────────────────────────────
// 2. Swagger with JWT support
// ─────────────────────────────────────────
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter your JWT token below",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
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
            Array.Empty<string>()
        }
    });
});

// ─────────────────────────────────────────
// 3. CORS — Updated to support both local
//    development AND Azure-deployed frontend
//    (We will add the Azure URL here later)
// ─────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                // ── Local Development ──────────────────
                "http://localhost:5500",
                "http://127.0.0.1:5500",
                "http://localhost:5173",

                // ── Azure Deployed Frontend ────────────
                // TODO: Replace with your real frontend URL
                // after frontend is deployed. Example:
                // "https://my-frontend-app.azurestaticapps.net"
                "https://placeholder-replace-later.com"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// ─────────────────────────────────────────
// 4. Database — using "DefaultConnection"
//    This name MUST match Azure App Service
//    Configuration exactly
// ─────────────────────────────────────────
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            // Retry on transient Azure SQL failures (important for Azure!)
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null
            );
        }
    )
);

// ─────────────────────────────────────────
// 5. AutoMapper
// ─────────────────────────────────────────
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// ─────────────────────────────────────────
// 6. Services
// ─────────────────────────────────────────
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// ─────────────────────────────────────────
// 7. JWT Authentication
//    Key, Issuer, Audience come from:
//    → Azure App Service Configuration (in production)
//    → appsettings.json (in local development)
// ─────────────────────────────────────────
var jwtSettings = builder.Configuration.GetSection("Jwt");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            // ✅ Tell JWT: "sub" claim IS the NameIdentifier
            NameClaimType = JwtRegisteredClaimNames.Sub,  // Change this
            RoleClaimType = ClaimTypes.Role,

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
         Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
     )
        };
    });

// ─────────────────────────────────────────
// 8. Build the app
// ─────────────────────────────────────────
var app = builder.Build();

// ─────────────────────────────────────────
// 9. Auto-run migrations on startup
//    This creates/updates your Azure SQL
//    database tables automatically!
// ─────────────────────────────────────────
/*using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate(); // Safe to run — skips already-applied migrations
}
*/
// ─────────────────────────────────────────
// 10. Middleware Pipeline
//     ORDER MATTERS — do not rearrange!
// ─────────────────────────────────────────

// Swagger — available in all environments for now
// (you can restrict to Development only later)
app.UseSwagger();
app.UseSwaggerUI();

// CORS must come BEFORE authentication
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthentication();  // ← Must be before UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();

// Needed for integration testing
public partial class Program { }