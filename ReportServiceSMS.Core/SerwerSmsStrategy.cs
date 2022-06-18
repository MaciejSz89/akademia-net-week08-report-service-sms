using NLog;
using serwersms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Core
{
    public class SerwerSmsStrategy : ISmsSenderStrategy
    {

		private string _username;
		private string _password;
		private Logger _logger;

        public SerwerSmsStrategy(string username, string password, Logger logger)
        {
			_username = username;
			_password = password;
			_logger = logger;
		}
        public void SendSms(string receiver, string messageText)
        {
			try
			{
				var serwerssms = new SerwerSMS(_username, _password);
				var data = new Dictionary<string, string>();

				String sender = null;
				var response = serwerssms.messages.sendSms(receiver, messageText, sender, data).ToString();
				_logger.Info(response);


			}
			catch (Exception e)
			{
				_logger.Error(e.Message);
			};
		}
    }
}
