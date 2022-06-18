namespace ReportServiceSMS
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.reportServiceSmsProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.reportServiceSmsInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // reportServiceSmsProcessInstaller
            // 
            this.reportServiceSmsProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.reportServiceSmsProcessInstaller.Password = null;
            this.reportServiceSmsProcessInstaller.Username = null;
            // 
            // reportServiceSmsInstaller
            // 
            this.reportServiceSmsInstaller.Description = "Wysyłanie raportów SMS";
            this.reportServiceSmsInstaller.DisplayName = "ReportServiceSMS";
            this.reportServiceSmsInstaller.ServiceName = "ReportServiceSMS";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.reportServiceSmsProcessInstaller,
            this.reportServiceSmsInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller reportServiceSmsProcessInstaller;
        private System.ServiceProcess.ServiceInstaller reportServiceSmsInstaller;
    }
}