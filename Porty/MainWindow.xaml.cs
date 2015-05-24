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
        private PortEventModel currentAction = new PortEventModel();
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            source = PresentationSource.FromVisual(this) as HwndSource;
            var windowHandle = source.Handle;
            source.AddHook(WndProc);
            UsbNotification.RegisterUsbDeviceNotification(windowHandle);
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
            if (msg == UsbNotification.WM_DEVICECHANGE)
            {
                if (Marshal.ReadInt32(lParam, 4) == UsbNotification.DBT_DEVTYP_PORT)
                {
                    currentAction.PortName = Marshal.PtrToStringAuto((IntPtr)(long)lParam + 12);
                }
                else
                {
                    currentAction.ExtractPIDVID(Marshal.PtrToStringAuto((IntPtr)(long)lParam + 28));
                }

                switch ((int)wParam)
                {
                    case UsbNotification.DBT_DEVICEREMOVALCOMPLETE:
                        currentAction.Attached = false;
                        break;
                    case UsbNotification.DBT_DEVICEARRIVAL:
                        currentAction.Attached = true;
                        break;
                }

                if (currentAction.isComplete)
                {
                    generatePopup(currentAction);
                }

            }

            handled = false;
            return IntPtr.Zero;
        }

        /// <summary>
        /// Generates a PortEventNotice with the given string
        /// </summary>
        /// <param name="message">Message to display in popup</param>
        private void generatePopup(PortEventModel data)
        {
            PortEventNotice pen = new PortEventNotice();
            pen.PortName = data.PortName;
            pen.PidVid = String.Format("PID: {0}\nVID: {1}", data.PID, data.VID);

            if (data.Attached)
                pen.StateString = "VCP Device Connected";
            else
                pen.StateString = "VCP Device Disconnected";

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
