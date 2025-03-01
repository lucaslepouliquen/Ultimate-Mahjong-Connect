using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Ultimate_Mahjong_Connect.Configurations;
using UltimateMahjongConnect.Core.Net.Interfaces;
using UltimateMahjongConnect.Core.Net.Models;
using UltimateMahjongConnect.Infrastructure.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbSQLContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
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
builder.Services.AddTransient<IMahjongTile, MahjongTile>();
builder.Services.AddTransient<IMahjongBoard, MahjongBoard>();

var app = builder.Build();

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
app.UseAuthorization();

app.MapControllers();

app.Run();
