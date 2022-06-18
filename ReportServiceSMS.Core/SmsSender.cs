using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Core
{
    public class SmsSender
    {
        private ISmsSenderStrategy _senderStrategy;

        public SmsSender(ISmsSenderStrategy senderStrategy)
        {
            _senderStrategy = senderStrategy;
        }
        public void SendSMS(string receiver, string messageText)
        {
            _senderStrategy.SendSms(receiver, messageText);           

        }
    }
}
