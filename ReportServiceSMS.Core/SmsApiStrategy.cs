using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Core
{
    public class SmsApiStrategy : ISmsSenderStrategy
    {
        private string _authorizationToken;
        private string _senderName;
        private Logger _logger;
        public SmsApiStrategy(string authorizationToken, string senderName, Logger logger)
        {
            _authorizationToken = authorizationToken;
            _senderName = senderName;
            _logger = logger;
        }
        public void SendSms(string receiver, string messageText)
        {
            SMSApi.Api.IClient client = new SMSApi.Api.ClientOAuth(_authorizationToken);

            var smsApi = new SMSApi.Api.SMSFactory(client);
            // for SMSAPI.com clients:
            // var smsApi = new SMSApi.Api.SMSFactory(client, ProxyAddress.SmsApiCom);

            var result =
                smsApi.ActionSend()
                    .SetText(messageText)
                    .SetTo(receiver)
                    .SetSender(_senderName) //Sender name
                    .Execute();

            _logger.Info("Send: " + result.Count);

            string[] ids = new string[result.Count];

            for (int i = 0, l = 0; i < result.List.Count; i++)
            {
                if (!result.List[i].isError())
                {
                    if (!result.List[i].isFinal())
                    {
                        ids[l] = result.List[i].ID;
                        l++;
                    }
                }
            }

            _logger.Info("Get:");
            result =
                smsApi.ActionGet()
                    .Ids(ids)
                    .Execute();

            foreach (var status in result.List)
            {
                _logger.Info("ID: " + status.ID + " NUmber: " + status.Number + " Points:" + status.Points + " Status:" + status.Status + " IDx: " + status.IDx);
            }
        }
    }
}
