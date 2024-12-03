namespace Application.Abstractions.Configuration;

public interface IConfigService
{
    public T GetValue<T>(string valuePath);
}