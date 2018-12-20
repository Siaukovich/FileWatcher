using System;

namespace FileWatcher.Info
{
    public class EmailMessageInfo
    {
        public string MessageBody { get; }

        public string Subject { get; }

        public string EmailTo { get; }

        public string EmailFrom { get; }

        public string EmailFromPassword { get; }

        public EmailMessageInfo(string messageBody, string subject, string emailTo, string emailFrom, string emailFromPassword)
        {
            this.MessageBody = messageBody ?? throw new ArgumentNullException(nameof(messageBody));
            this.Subject = subject ?? throw new ArgumentNullException(nameof(subject));
            this.EmailTo = emailTo ?? throw new ArgumentNullException(nameof(emailTo));
            this.EmailFrom = emailFrom ?? throw new ArgumentNullException(nameof(emailFrom));
            this.EmailFromPassword = emailFromPassword ?? throw new ArgumentNullException(nameof(emailFromPassword));
        }
    }
}
