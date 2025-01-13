namespace WebApi.Infrastructure.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSwaggerWithUi(this IApplicationBuilder builder)
    {
        builder.UseSwagger()
            .UseSwaggerUI();

        return builder;
    }
}