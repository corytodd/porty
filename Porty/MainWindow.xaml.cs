using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

namespace Porty
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields
        private System.Windows.Forms.ContextMenu taskIconContextMenu = new System.Windows.Forms.ContextMenu();
        private System.Windows.Forms.NotifyIcon notifyIcon = null;
        private Dictionary<string, System.Drawing.Icon> IconHandles = null;
        private HwndSource source;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);
        }

        /// <summary>
        /// Ingest and parse messages as they arrive from the underlying message pump
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // A hardware change occured
            if (msg == HardwareEvents.WM_DEVICECHANGE)
            {
                // This was an insertion
                if (wParam.ToInt32() == HardwareEvents.DBT_DEVICEARRIVAL)
                {
                    // of a serial port device
                    if (Marshal.ReadInt32(lParam, 4) == HardwareEvents.DBT_DEVTYP_PORT)
                    {
                        // Cannot marshall variable data into a struct, just dig out the string manually.
                        string port = Marshal.PtrToStringAuto((IntPtr)((long)lParam + 12));
                        generatePopup(port);
                    }
                }
            }

            return IntPtr.Zero;
        }


        /// <summary>
        /// Generates a PortEventNotice with the given string
        /// </summary>
        /// <param name="message">Message to display in popup</param>
        private void generatePopup(string message)
        {
            PortEventNotice pen = new PortEventNotice();
            pen.Summary = message;
            pen.Show();
        }

        /// <summary>
        /// Close the application and stop listening to events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitApplication(object sender, EventArgs e)
        {
            source.RemoveHook(WndProc);
            Application.Current.Shutdown();
        }

        #region Events
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            notifyIcon.Visible = true;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            IconHandles = new Dictionary<string, System.Drawing.Icon>();
            IconHandles.Add("Stop Porty", Properties.Resources.goat);
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.BalloonTipText = "Porty Service";
            notifyIcon.BalloonTipTitle = "Porty Service";
            notifyIcon.Text = "Notification service for serial devices";
            notifyIcon.Icon = IconHandles["Stop Porty"];

            taskIconContextMenu.MenuItems.Add("Stop Porty", exitApplication);
            notifyIcon.ContextMenu = taskIconContextMenu;
        }
        #endregion
    }
}
