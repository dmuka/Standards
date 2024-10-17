using Standards.Infrastructure.Exceptions.Enum;

namespace Standards.Infrastructure.Exceptions;

public class StandardsConfigValueNotFoundException : StandardsBaseException
{
    public StandardsConfigValueNotFoundException(
        StatusCodeByError error, 
        string messageForLog, 
        string messageForUser) : base(error, messageForLog, messageForUser)
    {

    }

    public StandardsConfigValueNotFoundException(
        StatusCodeByError error,
        string messageForLog,
        string messageForUser,
        Exception exception) : base(error, messageForLog, messageForUser, exception)
    {

    }

    public StandardsConfigValueNotFoundException(
        StatusCodeByError error,
        string messageForLog,
        string messageForUser,
        bool isHandled) : base(error, messageForLog, messageForUser)
    {
        IsHandled = isHandled;
    }
}