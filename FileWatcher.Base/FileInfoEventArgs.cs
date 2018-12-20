using System;

namespace FileWatcher.Base
{
    public class FileInfoEventArgs : EventArgs
    {
        public string FileName { get;}

        public FileInfoEventArgs(string fileName)
        {
            FileName = fileName;
        }
    }
}