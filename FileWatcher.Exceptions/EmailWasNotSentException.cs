using System;
using System.Runtime.Serialization;

namespace FileWatcher.Exceptions
{
    public class EmailWasNotSentException : Exception
    {
        public EmailWasNotSentException()
        {
        }

        public EmailWasNotSentException(string message) : base(message)
        {
        }

        public EmailWasNotSentException(string message, Exception inner) : base(message, inner)
        {
        }

        protected EmailWasNotSentException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}