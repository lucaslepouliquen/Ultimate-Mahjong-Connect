using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Ultimate_Mahjong_Connect.Configurations;
using UltimateMahjongConnect.Application.Interface;
using UltimateMahjongConnect.Application.Profiles;
using UltimateMahjongConnect.Application.Services;
using UltimateMahjongConnect.Domain.Interfaces;
using UltimateMahjongConnect.Domain.Models;
using UltimateMahjongConnect.Infrastructure.Models;
using UltimateMahjongConnect.Infrastructure.Profiles;
using UltimateMahjongConnect.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbSQLContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
    .EnableSensitiveDataLogging()
    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddAuthorization();


builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);

builder.Services.AddIdentityCore<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbSQLContext>()
    .AddApiEndpoints();
builder.Services.AddRazorPages();

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

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddApiVersioningService().AddSwaggerGen().ConfigureOptions<SwaggerGenConfiguration>();

builder.Services.AddTransient<IApiVersionDescriptionProvider, DefaultApiVersionDescriptionProvider>();

//Register Mahjong services
builder.Services.AddAutoMapper(
    typeof(Program).Assembly,
    typeof(GamerDTOProfile).Assembly,             
    typeof(GamerEntityProfile).Assembly
    );
builder.Services.AddTransient<IGamerRepository,GamerRepository>();
builder.Services.AddTransient<GamerService>();
builder.Services.AddTransient<IMahjongTile, MahjongTile>();
builder.Services.AddTransient<IMahjongBoard, MahjongBoard>();

builder.Services.AddCors();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbSQLContext>();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    _ = app.UseSwagger()
      .UseSwaggerUI(options =>
      {
          IApiVersionDescriptionProvider provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
          foreach (ApiVersionDescription item in provider.ApiVersionDescriptions)
          {
              options.SwaggerEndpoint($"/swagger/{item.GroupName}/swagger.json", item.ApiVersion.ToString());
          }
      });
}

app.UseHttpsRedirection();

app.UseCors(policy => policy
    .WithOrigins("http://localhost:4200")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
