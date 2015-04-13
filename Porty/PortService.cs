using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace Porty
{
    class PortyService : System.ServiceProcess.ServiceBase
    {
        
        private System.Diagnostics.EventLog eventLog1;
        private MainWindow mw;

        public PortyService()
        {          
            InitializeComponent();
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("PortyService"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "PortyService", "Porty");
            }
            eventLog1.Source = "PortyService";
            eventLog1.Log = "Porty";
        }  

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("PortyService Started");

            mw = new MainWindow();
            mw.Show();
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("PortyService Stoped.");

            mw = new MainWindow();
        }

        private void InitializeComponent()
        {
            // 
            // PortyService
            // 
            this.AutoLog = false;
            this.ServiceName = "PortyService";
        }

        // If a serial port device is detected, attempt to talk to it.
        private void notificate(string port)
        {
            PortEventNotice pen = new PortEventNotice();
            pen.Show();
        }
    }
}
