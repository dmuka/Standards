namespace Standards.Infrastructure.Services.Interfaces;

public interface IConfigService
{
    public T GetValue<T>(string valuePath);
}