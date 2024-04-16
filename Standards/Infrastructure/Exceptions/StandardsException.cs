using Standards.Infrastructure.Exceptions.Enum;

namespace Standards.Infrastructure.Exceptions
{
    public class StandardsException : StandardsBaseException
    {
        public StandardsException(
            StatusCodeByError error, 
            string messageForLog, 
            string messageForUser) : base(error, messageForLog, messageForUser)
        {

        }

        public StandardsException(
            StatusCodeByError typeError,
            string messageForLog,
            string messageForUser,
            Exception exception) : base(typeError, messageForLog, messageForUser, exception)
        {

        }

        public StandardsException(
            StatusCodeByError typeError,
            string messageForLog,
            string messageForUser,
            bool isHandled) : base(typeError, messageForLog, messageForUser)
        {
            IsHandled = isHandled;
        }

    }
}
