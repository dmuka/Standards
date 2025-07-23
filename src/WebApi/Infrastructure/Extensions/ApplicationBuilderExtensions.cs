using Swashbuckle.AspNetCore.SwaggerUI;

namespace WebApi.Infrastructure.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSwaggerWithUi(this IApplicationBuilder builder)
    {
        builder.UseSwagger()
            .UseSwaggerUI(options =>
            {
                options.ConfigObject = new ConfigObject
                {
                    DisplayRequestDuration = true,
                    DocExpansion = DocExpansion.List,
                    TryItOutEnabled = false
                };
            });

        return builder;
    }
}