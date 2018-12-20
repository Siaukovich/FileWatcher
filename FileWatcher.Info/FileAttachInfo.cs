using System;

namespace FileWatcher.Info
{
    public class FileAttachInfo
    {
        public FileAttachInfo(int maxRetries, int retryDelay)
        {
            if (maxRetries < 1)
            {
                var msg = $"{nameof(maxRetries)} must be a positive value, but was {maxRetries}.";
                throw new ArgumentOutOfRangeException(nameof(maxRetries), msg);
            }

            if (retryDelay < 1)
            {
                var msg = $"{nameof(retryDelay)} must be a positive value, but was {retryDelay}.";
                throw new ArgumentOutOfRangeException(nameof(retryDelay), msg);
            }

            this.MaxRetries = maxRetries;
            this.RetryDelay = retryDelay;
        }

        public int MaxRetries { get; }

        public int RetryDelay { get; }
    }
}
