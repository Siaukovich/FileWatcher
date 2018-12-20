using System;
using System.IO;

namespace FileWatcher.Base
{
    public interface IFileWatcherService
    {
        event EventHandler<ErrorEventArgs> ErrorOccured;

        event EventHandler<FileInfoEventArgs> FileFound;

        event EventHandler<FileInfoEventArgs> MailSent;

        event EventHandler<FileInfoEventArgs> FileRemoved;

        void BeginWork();

        void EndWork();
    }
}