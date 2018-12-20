using System;
using System.IO;
using System.Threading.Tasks;

using FileWatcher.Base;
using FileWatcher.Exceptions;
using FileWatcher.Info;

namespace FileWatcher
{
    public class EmailNotificationSenderWithRetries : INotificationSender
    {
        private readonly INotificationSender _sender;

        private readonly FileAttachInfo _attachInfo;

        public EmailNotificationSenderWithRetries(INotificationSender sender, FileAttachInfo attachInfo)
        {
            this._sender = sender ?? throw new ArgumentNullException(nameof(sender));
            this._attachInfo = attachInfo ?? throw new ArgumentNullException(nameof(attachInfo));
        }

        public async Task SendFileAsync(string filePath)
        {
            for (int i = 0; i < this._attachInfo.MaxRetries; i++)
            {
                try
                {
                    await this._sender.SendFileAsync(filePath);

                    return;
                }
                catch (IOException ex) // File is in use.
                {
                    if (i == this._attachInfo.MaxRetries - 1)
                    {
                        throw new FileWasNotAttachedException(
                            $"File {filePath} was not sent after {this._attachInfo.MaxRetries} retries" + 
                            $" with {this._attachInfo.RetryDelay} retry delay.", 
                            ex);
                    }
                }

                await Task.Delay(this._attachInfo.RetryDelay);
            }

            throw new FileWasNotAttachedException($"File {filePath} was not sent after {this._attachInfo.MaxRetries} retries " + 
                                                  $"with {this._attachInfo.RetryDelay} retry delay.");

        }
    }
}