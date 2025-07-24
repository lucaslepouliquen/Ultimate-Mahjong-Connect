using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.Text;
using Ultimate_Mahjong_Connect.Configurations;
using UltimateMahjongConnect.Application.Interface;
using UltimateMahjongConnect.Application.Profiles;
using UltimateMahjongConnect.Application.Services;
using UltimateMahjongConnect.Domain.Interfaces;
using UltimateMahjongConnect.Domain.Models;
using UltimateMahjongConnect.Infrastructure.Models;
using UltimateMahjongConnect.Infrastructure.Profiles;
using UltimateMahjongConnect.Infrastructure.Repositories;
using UltimateMahjongConnect.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);   

builder.Services.AddDbContext<ApplicationDbSQLContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
    .EnableSensitiveDataLogging()
    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(options => {
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

var authBuilder = builder.Services.AddAuthentication();

var googleClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID") ?? 
                    builder.Configuration["Authentication:Google:ClientId"] ?? "";
var googleClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET") ?? 
                        builder.Configuration["Authentication:Google:ClientSecret"] ?? "";

if (!string.IsNullOrEmpty(googleClientId) && !string.IsNullOrEmpty(googleClientSecret))
{
    authBuilder.AddGoogle(options =>
    {
        options.ClientId = googleClientId;
        options.ClientSecret = googleClientSecret;
        options.CallbackPath = "/auth/google/callback";
    });
}

var facebookAppId = Environment.GetEnvironmentVariable("FACEBOOK_APP_ID") ?? 
                   builder.Configuration["Authentication:Facebook:AppId"] ?? "";
var facebookAppSecret = Environment.GetEnvironmentVariable("FACEBOOK_APP_SECRET") ?? 
                       builder.Configuration["Authentication:Facebook:AppSecret"] ?? "";

if (!string.IsNullOrEmpty(facebookAppId) && !string.IsNullOrEmpty(facebookAppSecret))
{
    authBuilder.AddFacebook(options =>
    {
        options.AppId = facebookAppId;
        options.AppSecret = facebookAppSecret;
        options.CallbackPath = "/auth/facebook/callback";
    });
}

builder.Services.AddIdentityCore<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbSQLContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddApiVersioningService().AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
})
    .ConfigureOptions<SwaggerGenConfiguration>();

builder.Services.AddTransient<IApiVersionDescriptionProvider, DefaultApiVersionDescriptionProvider>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<GamerDTOProfile>();
    cfg.AddProfile<GamerEntityProfile>();
}, typeof(Program).Assembly);

builder.Services.AddTransient<IBoardRepository, SessionBoardRepository>();
builder.Services.AddTransient<IGamerRepository,GamerRepository>();
builder.Services.AddTransient<IGamerService,GamerService>();
builder.Services.AddTransient<IMahjongTile, MahjongTile>();
builder.Services.AddScoped<IMahjongBoard, MahjongBoard>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IBoardService, BoardService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "MahjongConnect.Session";
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.Path = "/";  
});

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalDevelopmentPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200", "http://127.0.0.1:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()
              .SetIsOriginAllowedToAllowWildcardSubdomains();
    });
    options.AddPolicy("RaspberryDevelopmentPolicy", policy =>
    {
        policy.WithOrigins(
            "http://192.168.1.186:31328", 
            "https://192.168.1.186:31328", 
            "http://mahjong-connect.local",
            "https://mahjong-connect.local")
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials();
    });
    options.AddPolicy("ProductionPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Utiliser un logger temporaire pour le debug initial
using (var tempScope = app.Services.CreateScope())
{
    var tempLogger = tempScope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    // DEBUG IMPORTANT
    tempLogger.LogInformation("ASPNETCORE_RASPBERRY = '{RaspberryEnv}'", Environment.GetEnvironmentVariable("ASPNETCORE_RASPBERRY"));
    tempLogger.LogInformation("IsRaspberryEnvironment config = '{RaspberryConfig}'", builder.Configuration.GetValue<bool>("IsRaspberryEnvironment"));
    tempLogger.LogInformation("Environment.EnvironmentName = '{EnvName}'", app.Environment.EnvironmentName);
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbSQLContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>(); 
    
    try
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") ??
                                builder.Configuration.GetConnectionString("DefaultConnection") ??
                              "Data Source=/app/data/app.db";
        logger.LogInformation("Database setup - Connection string: {ConnectionString}", connectionString);
        logger.LogInformation("Environment: {Environment}", app.Environment.EnvironmentName);
        logger.LogInformation("Current directory: {CurrentDirectory}", Directory.GetCurrentDirectory());
        
        if (!string.IsNullOrEmpty(connectionString) && connectionString.Contains("Data Source="))
        {
            var dbPath = connectionString.Split("Data Source=")[1].Split(';')[0];
            var dbDir = Path.GetDirectoryName(dbPath);
            
            logger.LogInformation("Database path: {DbPath}", dbPath);
            logger.LogInformation("Database directory: {DbDir}", dbDir);
            
            if (!string.IsNullOrEmpty(dbDir) && !Directory.Exists(dbDir))
            {
                logger.LogInformation("Creating database directory...");
                Directory.CreateDirectory(dbDir);
                logger.LogInformation("Database directory created successfully");
            }
            else if (!string.IsNullOrEmpty(dbDir))
            {
                logger.LogInformation("Database directory already exists");
                logger.LogInformation("Directory permissions: {Permissions}", new DirectoryInfo(dbDir).Attributes);
            }
        }
        
        logger.LogInformation("Ensuring database is created...");
        
        if (dbContext.Database.CanConnect())
        {
            logger.LogInformation("Database connection test successful");
        }
        else
        {
            logger.LogWarning("Database connection test failed, attempting to create...");
        }
        
        dbContext.Database.EnsureCreated();
        logger.LogInformation("Database EnsureCreated completed successfully");
        
        // Verify tables exist
        try
        {
            var tablesExist = await dbContext.Database.CanConnectAsync();
            logger.LogInformation("Database tables verification: SUCCESS");
        }
        catch (Exception ex)
        {
            logger.LogWarning("Database tables verification: FAILED - {Error}", ex.Message);
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Database initialization failed: {ErrorMessage}", ex.Message);
        throw;
    }
}

// Configure Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        
        // Configure Swagger UI for each API version
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", 
                                   $"Ultimate Mahjong Connect API {description.GroupName.ToUpperInvariant()}");
        }
        
        options.RoutePrefix = "swagger";
        options.DocumentTitle = "Ultimate Mahjong Connect API";
    });
}

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    var isRaspberry = Environment.GetEnvironmentVariable("ASPNETCORE_RASPBERRY") == "true" ||
                      builder.Configuration.GetValue<bool>("IsRaspberryEnvironment");
    
    // Utiliser Console.WriteLine pour éviter les conflits de scope
    Console.WriteLine($"IsRaspberry decision = {isRaspberry}");
    
    if (isRaspberry)
    {
        Console.WriteLine("Using RaspberryDevelopmentPolicy");
        app.UseCors("RaspberryDevelopmentPolicy");
    }
    else
    {
        Console.WriteLine("Using LocalDevelopmentPolicy");
        app.UseCors("LocalDevelopmentPolicy");
    }
}
else
{
    app.UseCors("ProductionPolicy");
}

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
