using Infrastructure.Exceptions.Enum;

namespace Infrastructure.Exceptions
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
            StatusCodeByError error,
            string messageForLog,
            string messageForUser,
            Exception exception) : base(error, messageForLog, messageForUser, exception)
        {

        }

        public StandardsException(
            StatusCodeByError error,
            string messageForLog,
            string messageForUser,
            bool isHandled) : base(error, messageForLog, messageForUser)
        {
            IsHandled = isHandled;
        }
    }
}
