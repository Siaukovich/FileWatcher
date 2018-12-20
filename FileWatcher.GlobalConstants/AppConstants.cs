using System.Configuration;
using FileWatcher.Info;

namespace FileWatcher.GlobalConstants
{
    public static class AppConstants
    {
        public static string TrackedFileExtension { get; set; }

        public static string Directory { get; }
        
        public static EmailMessageInfo EmailMessageInfo { get; }

        public static FileAttachInfo FileAttachInfo { get; }

        public static FileDeleteInfo FileDeleteInfo { get; }

        public static SmtpConnectionInfo SmtpConnectionInfo { get; }

        static AppConstants()
        {
            TrackedFileExtension = GetValueFromConfig("trackedFileExtension");
            Directory = GetValueFromConfig("directory");

            var messageBody = GetValueFromConfig("messageBody");
            var subject = GetValueFromConfig("subject");
            var emailTo = GetValueFromConfig("emailTo");
            var emailFrom = GetValueFromConfig("emailFrom");
            var emailFromPassword = GetValueFromConfig("emailFromPassword");
            EmailMessageInfo = new EmailMessageInfo(messageBody, subject, emailTo, emailFrom, emailFromPassword);

            var attachMaxRetries = GetIntValueFromConfig("fileAttachmentMaxRetries");
            var attachRetryDelay = GetIntValueFromConfig("fileAttachmentRetryDelay");
            FileAttachInfo = new FileAttachInfo(attachMaxRetries, attachRetryDelay);

            var deletionMaxRetries = GetIntValueFromConfig("fileDeletionMaxRetries");
            var deletionRetryDelay = GetIntValueFromConfig("fileDeletionRetryDelay");
            FileDeleteInfo = new FileDeleteInfo(deletionMaxRetries, deletionRetryDelay);

            var smtpHost = GetValueFromConfig("smtpHost");
            var smtpPort = GetIntValueFromConfig("smtpPort");
            var enableSsl = GetBoolValueFromConfig("enableSsl");
            var timeout = GetIntValueFromConfig("timeout");
            SmtpConnectionInfo = new SmtpConnectionInfo(smtpHost, smtpPort, enableSsl, timeout);
        }

        private static int GetIntValueFromConfig(string key) => int.Parse(GetValueFromConfig(key));

        private static bool GetBoolValueFromConfig(string key) => bool.Parse(GetValueFromConfig(key));

        private static string GetValueFromConfig(string key) => ConfigurationManager.AppSettings[key];
    }
}