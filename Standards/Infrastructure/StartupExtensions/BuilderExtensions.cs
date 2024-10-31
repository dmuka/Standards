namespace Standards.Infrastructure.StartupExtensions;

public static class BuilderExtensions
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddDbConnection(builder.Configuration)
            .AddJwtAuth(builder.Configuration)
            .AddMediatrAutoRegister()
            .AddMediatrManualRegister()
            .AddMediatrPipelineBehaviors()
            .AddValidators<Program>()
            .AddAppServices()
            .AddCache()
            .RegisterQueryBuilder()
            .AddControllersServices();

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddSwaggerGen();
        }

        return builder;
    }
}