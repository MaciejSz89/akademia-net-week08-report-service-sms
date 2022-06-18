using Cipher;
using NLog;
using ReportService.Core;
using ReportService.Core.Models.Converters;
using ReportService.Core.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ReportServiceSMS
{
    public partial class ReportServiceSMS : ServiceBase
    {

        private static readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private Timer _timer;
        private readonly int _sendHour;
        private readonly int _intervalInMinutes;
        private readonly bool _sendingReportsEnable;
        private readonly string _smsReceiver;
        private readonly SmsSender _smsSender;
        private ErrorRepository _errorRepository = new ErrorRepository();
        private ReportRepository _reportRepository = new ReportRepository();
        private StringCipher _stringCipher = new StringCipher("BA7D49D2-3769-4A59-9C93-08389D129CEF");
        private const string NotEncryptedPasswordPrefix = "encrypt:";

        public ReportServiceSMS()
        {
            InitializeComponent();

            try
            {
                _sendHour = int.Parse(ConfigurationManager.AppSettings["SendHour"]);
                _intervalInMinutes = int.Parse(ConfigurationManager.AppSettings["IntervalInMinutes"]);
                _sendingReportsEnable = bool.Parse(ConfigurationManager.AppSettings["SendingReportsEnable"]);
   
                _smsReceiver = ConfigurationManager.AppSettings["SmsReceiver"];
                var smsSenderName = ConfigurationManager.AppSettings["SmsSenderName"];
                var smsServiceVendor = ConfigurationManager.AppSettings["ServiceVendor"];
                ISmsSenderStrategy smsSenderStrategy;


                switch (smsServiceVendor)
                {
                    case "SMSApi":
                        var smsAuthorizationToken = DecryptSetting("SmsAuthorizationToken");
                        smsSenderStrategy = new SmsApiStrategy(smsAuthorizationToken, smsSenderName, _logger);
                        break;
                    case "SerwerSMS":
                        var username = ConfigurationManager.AppSettings["Username"];
                        var password = DecryptSetting("Password");
                        smsSenderStrategy = new SerwerSmsStrategy(username, password, _logger);
                        break;
                    default:
                        var ex = new Exception("No valid vendor chosen...");
                        _logger.Error(ex.Message);
                        throw ex;
                }            

                _smsSender = new SmsSender(smsSenderStrategy);

                _timer = new Timer(_intervalInMinutes * 60000);
            }
            catch (Exception ex)
            {

                _logger.Error(ex, ex.Message);
                throw ex;
            }
        }

        protected override void OnStart(string[] args)
        {
            _logger.Info("Service started.");

            _timer.Elapsed += DoWork;
            _timer.Start();
        }

        private void DoWork(object sender, ElapsedEventArgs e)
        {
            try
            {
                SendError();
                SendReport();
            }
            catch (Exception ex)
            {

                _logger.Error(ex, ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private void SendError()
        {
            var errors = _errorRepository.GetLastErrors(_intervalInMinutes);

            if (errors == null || !errors.Any())
                return;

            _smsSender.SendSMS(_smsReceiver, errors.ToSmsContent());


            _logger.Info("Error sent.");
        }



        private void SendReport()
        {
            if (!_sendingReportsEnable)
                return;

            var actualHour = DateTime.Now.Hour;

            if (actualHour < _sendHour)
                return;

            var report = _reportRepository.GetLastNotSentReport();

            if (report == null)
                return;

            _smsSender.SendSMS(_smsReceiver, report.ToSmsContent());

            _reportRepository.ReportSent(report);


            _logger.Info("Report sent.");
        }

        private string DecryptSetting(string settingName)
        {

            var encryptedToken = ConfigurationManager.AppSettings[settingName];

            if (encryptedToken.StartsWith(NotEncryptedPasswordPrefix))
            {
                encryptedToken = _stringCipher.Encrypt(encryptedToken.Replace(NotEncryptedPasswordPrefix, ""));

                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                configFile.AppSettings.Settings[settingName].Value = encryptedToken;
                configFile.Save();
            }
            return _stringCipher.Decrypt(encryptedToken);
        }

        protected override void OnStop()
        {
            _logger.Info("Service stopped.");
        }


    }
}
