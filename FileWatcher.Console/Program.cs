using System;
using System.IO;

using DependencyResolver;

using FileWatcher.Base;

using Ninject;

namespace FileWatcher.ConsolePL
{
    using System.Diagnostics;

    using NLog;

    class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            using (var kernel = new StandardKernel())
            {
                kernel.ResolveDependencies();

                var service = kernel.Get<IFileWatcherService>();

                SetEventHandlers(service);

                var sw = Stopwatch.StartNew();
                service.BeginWork();

                Console.WriteLine("Press any button to stop.");

                Console.ReadKey();
                Console.WriteLine(sw.ElapsedMilliseconds / 1000f);

                Console.ReadKey();
            }
        }

        private static void SetEventHandlers(IFileWatcherService service)
        {
            service.FileFound += FileFoundHandler;
            service.MailSent += MailSentHandler;
            service.FileRemoved += FileRemovedHandler;
            service.ErrorOccured += ErrorOccuredHandler;
        }

        private static void FileRemovedHandler(object sender, FileInfoEventArgs args)
        {
            Console.WriteLine($"File {args.FileName} was removed.");
        }

        private static void ErrorOccuredHandler(object sender, ErrorEventArgs args)
        {
            Console.WriteLine("Error occured. Check log file for more info.");

            Logger.Error(args.GetException());
        }

        private static void MailSentHandler(object sender, FileInfoEventArgs args)
        {
            Console.WriteLine($"File {args.FileName} was sent.");
        }

        private static void FileFoundHandler(object sender, FileInfoEventArgs args)
        {
            Console.WriteLine($"File {args.FileName} was found.");
        }
    }
}
