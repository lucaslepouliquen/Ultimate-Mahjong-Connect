using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
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
app.UseAuthentication();
app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
app.UseAuthorization();

app.MapControllers();

app.Run();
