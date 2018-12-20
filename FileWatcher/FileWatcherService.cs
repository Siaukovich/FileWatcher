using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using FileWatcher.Base;
using FileWatcher.Exceptions;
using FileWatcher.Info;

namespace FileWatcher
{ 
    public sealed class FileWatcherService : IFileWatcherService
    {
        private readonly INotificationSender _notificationSender;

        private readonly FileDeleteInfo _fileDeleteInfo;

        private readonly FileSystemWatcher _watcher;

        public event EventHandler<ErrorEventArgs> ErrorOccured;

        public event EventHandler<FileInfoEventArgs> FileFound;

        public event EventHandler<FileInfoEventArgs> MailSent;

        public event EventHandler<FileInfoEventArgs> FileRemoved;

        public FileWatcherService(string directory, string fileExtension, INotificationSender notificationSender, FileDeleteInfo fileDeleteInfo)
        {
            if (directory == null)
            {
                throw new ArgumentNullException(nameof(directory));
            }

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (string.IsNullOrEmpty(fileExtension))
            {
                throw new ArgumentException("File extension must not be null or empty.", nameof(fileExtension));
            }

            _notificationSender = notificationSender ?? throw new ArgumentNullException(nameof(notificationSender));
            _fileDeleteInfo = fileDeleteInfo ?? throw new ArgumentNullException(nameof(notificationSender));

            _watcher = new FileSystemWatcher(directory, $"*.{fileExtension}");
        }

        public void BeginWork()
        {
            _watcher.Created += CreatedHandler;
            _watcher.EnableRaisingEvents = true;
        }

        public void EndWork()
        {
            _watcher.Created -= CreatedHandler;
        }

        // Using async void isn't nice,
        // but I didn't found a way to make handler, that returns a Task.
        private async void CreatedHandler(object sender, FileSystemEventArgs args)
        {
            OnFileFound(args.Name);

            try
            {
                await _notificationSender.SendFileAsync(args.FullPath);
                OnEmailSent(args.Name);

                await RemoveFile(args.FullPath);
                OnFileRemoved(args.Name);
            }
            catch (Exception e)
            {
                this.OnErrorOccured(e);
            }
        }

        private void OnEmailSent(string fileName) => MailSent?.Invoke(this, new FileInfoEventArgs(fileName));

        private void OnErrorOccured(Exception e) => ErrorOccured?.Invoke(this, new ErrorEventArgs(e));

        private void OnFileRemoved(string fileName) => FileRemoved?.Invoke(this, new FileInfoEventArgs(fileName));

        private void OnFileFound(string fileName) => FileFound?.Invoke(this, new FileInfoEventArgs(fileName));

        private async Task RemoveFile(string fullPath)
        {
            if (fullPath == null)
            {
                throw new ArgumentNullException(nameof(fullPath));
            }

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"File '{fullPath}' not found.");
            }

            await Task.Run(() => TryDeleteFile(fullPath));
        }

        private void TryDeleteFile(string fullPath)
        {
            for (int i = 0; i < _fileDeleteInfo.MaxRetries; i++)
            {
                try
                {
                    File.Delete(fullPath);
                    return;
                }
                catch (IOException e) // File is in use.
                {
                    if (i == _fileDeleteInfo.MaxRetries - 1)
                    {
                        throw new FileWasNotDeletedException("File was not sent.", e);
                    }
                }

                Thread.Sleep(_fileDeleteInfo.RetryDelay);
            }
        }
    }
}
