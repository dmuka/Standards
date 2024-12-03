using Infrastructure.Exceptions.Enum;

namespace Infrastructure.Exceptions
{
    public class StandardsBaseException : Exception
    {
        /// <summary>
        /// The constructor is designed to create an exception that will assign a displayed message to the user 
        /// and separately assign a message that will be displayed in the logs. Also assign initiator of the exception.
        /// If messages null, they will not be displayed either to the user or in the logs.
        /// </summary>
        /// <param name="error">The type of exception error that we will translate to status code.</param>
        /// <param name="messageForLog">The message that will be logged. If null, the message will not be shown in the logs.</param>
        /// <param name="messageForUser">The message that will be shown to the user. If null then the message will not be shown to the user.</param>
        /// <param name="innerException">Initiator of the exception.</param>
        protected StandardsBaseException(
            StatusCodeByError error,
            string messageForLog,
            string messageForUser,
            Exception innerException) : base(messageForLog, innerException)
        {
            Error = error;
            MessageForLog = messageForLog;
            MessageForUser = messageForUser;
        }

        /// <summary>
        /// The constructor is designed to create an exception that will assign a displayed message to the user 
        /// and separately assign a message that will be displayed in the logs. 
        /// If messages null, they will not be displayed either to the user or in the logs.
        /// </summary>
        /// <param name="error">The type of exception error that we will translate to status code.</param>
        /// <param name="messageForLog">The message that will be logged. If null, the message will not be shown in the logs.</param>
        /// <param name="messageForUser">The message that will be shown to the user. If null then the message will not be shown to the user.</param>
        protected StandardsBaseException(
            StatusCodeByError error,
            string messageForLog,
            string messageForUser) : base(messageForLog)
        {
            Error = error;
            MessageForLog = messageForLog;
            MessageForUser = messageForUser;
        }
        public StatusCodeByError Error { get; }

        public bool IsHandled { get; set; }

        public string MessageForUser { get; }

        public string MessageForLog { get; }
    }
}