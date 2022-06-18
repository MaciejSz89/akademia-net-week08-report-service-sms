using NLog;

namespace ReportService.Core
{
    public interface ISmsSenderStrategy
    {
        void SendSms(string receiver, string messageText);
    }
}