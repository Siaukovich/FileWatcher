using System;

namespace FileWatcher.Info
{
    public class SmtpConnectionInfo
    {
        public SmtpConnectionInfo(string host, int port, bool enableSsl, int timeout)
        {
            this.Host = host ?? throw new ArgumentNullException(nameof(host));

            const int PORT_MIN = 0;
            const int PORT_MAX = 65535;
            if (port < PORT_MIN || PORT_MAX < port)
            {
                throw new ArgumentOutOfRangeException(nameof(port), $"Port must be in range {PORT_MIN}-{PORT_MAX}, but was {port}.");
            }

            if (timeout < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(timeout), $"Timeout must not be a positive value, but was {timeout}");
            }

            this.Port = port;
            this.EnableSsl = enableSsl;

            this.Timeout = timeout;
        }

        public string Host { get; set; }

        public int Port { get; set; }

        public bool EnableSsl { get; set; }

        public int Timeout { get; set; }
    }
}