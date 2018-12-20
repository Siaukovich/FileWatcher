using System;
using System.Runtime.Serialization;

namespace FileWatcher.Exceptions
{
    public class FileWasNotAttachedException : Exception
    {
        public FileWasNotAttachedException()
        {
        }

        public FileWasNotAttachedException(string message) : base(message)
        {
        }

        public FileWasNotAttachedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected FileWasNotAttachedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
