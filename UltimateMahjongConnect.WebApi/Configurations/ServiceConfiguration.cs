using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace RDLoreal.Microbio.Server.WebApi.Net.Configurations;

public static class ServiceConfiguration
{
    /// <summary>
    /// Add versionning api discovery
    /// </summary>
    public static IServiceCollection AddApiVersioningService(this IServiceCollection collection) =>
        collection.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;

            //header can call api versionning
            config.ReportApiVersions = true;
            config.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddVersionedApiExplorer(config =>
        {
            config.GroupNameFormat = "'v'VVV";
            config.SubstituteApiVersionInUrl = true;
        });
}
