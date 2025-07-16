using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

// Facebook OAuth - only if credentials are provided
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
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
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

//Register Mahjong services
builder.Services.AddAutoMapper(
    typeof(Program).Assembly,
    typeof(GamerDTOProfile).Assembly,             
    typeof(GamerEntityProfile).Assembly
    );
builder.Services.AddTransient<IGamerRepository,GamerRepository>();
builder.Services.AddTransient<IGamerService,GamerService>();
builder.Services.AddTransient<IMahjongTile, MahjongTile>();
builder.Services.AddTransient<IMahjongBoard, MahjongBoard>();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

// CORS temporairement désactivé pour debug
// builder.Services.AddCors(options =>
// {
//     options.AddDefaultPolicy(policy =>
//     {
//         policy.WithOrigins(
//             "http://localhost:4200", 
//             "http://localhost:9876",
//             "http://localhost:8080",
//             "http://192.168.1.186:31328"
//         )
//         .AllowAnyMethod()
//         .AllowAnyHeader()
//         .AllowCredentials();
//     });
// });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbSQLContext>();
    
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (!string.IsNullOrEmpty(connectionString) && connectionString.Contains("Data Source="))
    {
        var dbPath = connectionString.Split("Data Source=")[1].Split(';')[0];
        var dbDir = Path.GetDirectoryName(dbPath);
        if (!string.IsNullOrEmpty(dbDir) && !Directory.Exists(dbDir))
        {
            Directory.CreateDirectory(dbDir);
        }
    }
    
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    _ = app.UseSwagger()
      .UseSwaggerUI(options =>
      {
          options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
          IApiVersionDescriptionProvider provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
          foreach (ApiVersionDescription item in provider.ApiVersionDescriptions)
          {
              options.SwaggerEndpoint($"/swagger/{item.GroupName}/swagger.json", item.ApiVersion.ToString());
          }
      });
}

// CORS temporairement désactivé pour debug
// app.UseCors();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
