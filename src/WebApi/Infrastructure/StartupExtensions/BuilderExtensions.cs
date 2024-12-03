namespace WebApi.Infrastructure.StartupExtensions;

public static class BuilderExtensions
{
    public static IApplicationBuilder UseSwaggerWithUi(this IApplicationBuilder builder)
    {
        builder.UseSwagger()
            .UseSwaggerUI();

        return builder;
    }
}