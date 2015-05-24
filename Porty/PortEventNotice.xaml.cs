using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Porty
{
    /// <summary>
    /// Interaction logic for PortEventNotice.xaml
    /// </summary>
    public partial class PortEventNotice : Window, INotifyPropertyChanged
    {
        #region Fields
        private string _portName = string.Empty;
        private string _pidvid = string.Empty;
        private string _stateStr = string.Empty;
        #endregion

        public PortEventNotice()
            : base()
        {
            this.InitializeComponent();
            this.DataContext = this;
            this.Closed += this.NotificationWindowClosed;
        }

        /// <summary>
        /// Gets or set the summary message on the popup
        /// </summary>
        public string PortName
        {
            get { return _portName; }
            set
            {
                _portName = value;
                OnPropertyChanged("PortName");
            }
        }

        public string StateString
        {
            get { return _stateStr; }
            set 
            {
                _stateStr = value;
                OnPropertyChanged("StateString");
            }
        }

        /// <summary>
        /// Gets or set the summary message on the popup
        /// </summary>
        public string PidVid
        {
            get { return _pidvid; }
            set
            {
                _pidvid = value;
                OnPropertyChanged("PidVid");
            }
        }

        /// <summary>
        /// Make popup visible and begin animation events.
        /// </summary>
        public new void Show()
        {
            this.Topmost = true;
            base.Show();

            this.Owner = System.Windows.Application.Current.MainWindow;
            this.Closed += this.NotificationWindowClosed;
            var workingArea = System.Windows.SystemParameters.WorkArea;

            this.Left = workingArea.Right - this.ActualWidth;
            double top = workingArea.Bottom - this.ActualHeight;

            foreach (Window window in System.Windows.Application.Current.Windows)
            {
                string windowName = window.GetType().Name;

                if (windowName.Equals("NotificationWindow") && window != this)
                {
                    window.Topmost = true;
                    top = window.Top - window.ActualHeight;
                }
            }

            this.Top = top;
        }

        /// <summary>
        /// Close the window if user clicks the x
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageMouseUp(object sender,
            System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Close ourselves at the end of the animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoubleAnimationCompleted(object sender, EventArgs e)
        {
            if (!this.IsMouseOver)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Ensure that our windows stack nicely instead overlapping.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotificationWindowClosed(object sender, EventArgs e)
        {
            foreach (Window window in System.Windows.Application.Current.Windows)
            {
                string windowName = window.GetType().Name;

                if (windowName.Equals("NotificationWindow") && window != this)
                {
                    // Adjust any windows that were above this one to drop down
                    if (window.Top < this.Top)
                    {
                        window.Top = window.Top + this.ActualHeight;
                    }
                }
            }
        }

        /// <summary>
        /// Boiler plate property change stuff
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}
