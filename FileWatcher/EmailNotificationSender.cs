using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

using FileWatcher.Base;
using FileWatcher.Exceptions;
using FileWatcher.Info;

namespace FileWatcher
{
    public sealed class EmailNotificationSender : INotificationSender
    {
        private readonly EmailMessageInfo _emailInfo;

        private readonly SmtpConnectionInfo _connectionInfo;

        public EmailNotificationSender(EmailMessageInfo emailInfo, SmtpConnectionInfo connectionInfo)
        {
            this._emailInfo = emailInfo ?? throw new ArgumentNullException(nameof(emailInfo));
            this._connectionInfo = connectionInfo ?? throw new ArgumentNullException(nameof(connectionInfo));
        }

        public async Task SendFileAsync(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File '{filePath}' not found.");
            }

            using (var mail = CreateMail(filePath))
            {
                await TrySendMail(mail);
            }
        }

        private async Task TrySendMail(MailMessage mail)
        {
            try
            {
                var s = GetSmtpClient();

                // Don't understand why, but using SendMailAsync method without
                // await works faster than sync Send method.
                await s.SendMailAsync(mail);
            }
            catch (SmtpException e)
            {
                throw new EmailWasNotSentException("Email was not sent.", e);
            }
        }

        private MailMessage CreateMail(string filePath)
        {
            var mail = new MailMessage(this._emailInfo.EmailFrom, this._emailInfo.EmailTo)
            {
                Subject = this._emailInfo.Subject,
                Body = this._emailInfo.MessageBody
            };

            this.AttachFile(mail, filePath);

            return mail;
        }

        private void AttachFile(MailMessage mail, string filePath)
        {
            var fileAttachment = new Attachment(filePath);
            mail.Attachments.Add(fileAttachment);
        }

        private SmtpClient GetSmtpClient()
        {
            return new SmtpClient(this._connectionInfo.Host, this._connectionInfo.Port)
            {
                Credentials = new NetworkCredential(this._emailInfo.EmailFrom, this._emailInfo.EmailFromPassword),
                EnableSsl = true,
                Timeout = this._connectionInfo.Timeout
            };
        }
    }
}
