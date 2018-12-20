using System;
using System.Runtime.Serialization;

namespace FileWatcher.Exceptions
{
    public class FileWasNotDeletedException : Exception
    {
        public FileWasNotDeletedException()
        {
        }

        public FileWasNotDeletedException(string message) : base(message)
        {
        }

        public FileWasNotDeletedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected FileWasNotDeletedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
