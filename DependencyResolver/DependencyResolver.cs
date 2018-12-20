using FileWatcher;
using FileWatcher.Base;
using Ninject;
using FileWatcher.GlobalConstants;

namespace DependencyResolver
{
    public static class DependencyResolver
    {
        public static void ResolveDependencies(this IKernel kernel)
        {
            kernel.Bind<IFileWatcherService>()
                  .To<FileWatcherService>()
                  .WithConstructorArgument("directory", AppConstants.Directory)
                  .WithConstructorArgument("fileExtension", AppConstants.TrackedFileExtension)
                  .WithConstructorArgument("fileDeleteInfo", AppConstants.FileDeleteInfo);

            kernel.Bind<INotificationSender>()
                .To<EmailNotificationSenderWithRetries>()
                .WithConstructorArgument("attachInfo", AppConstants.FileAttachInfo);

            kernel.Bind<INotificationSender>()
                  .To<EmailNotificationSender>()
                  .WhenInjectedInto<EmailNotificationSenderWithRetries>()
                  .WithConstructorArgument("emailInfo", AppConstants.EmailMessageInfo)
                  .WithConstructorArgument("connectionInfo", AppConstants.SmtpConnectionInfo);
        }
    }
}
