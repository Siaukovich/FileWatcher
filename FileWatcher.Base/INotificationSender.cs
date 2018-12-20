using System.Threading.Tasks;

namespace FileWatcher.Base
{
    public interface INotificationSender
    {
        Task SendFileAsync(string filePath);
    }
}
