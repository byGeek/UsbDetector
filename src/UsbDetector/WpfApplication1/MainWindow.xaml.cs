using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UsbDetectorLib;
using System.Windows.Interop;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
            {
                UsbDetector.Instance.StateChanged += Instance_StateChanged;
                RegisterNotification();
            };

            this.Unloaded += (s, e) =>
            {
                UnRegisterNotification();
            };
        }

        private void RegisterNotification()
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            UsbDetector.Instance.RegisterNotificationFor(helper.Handle, Native.DeviceType.DBT_DEVTYP_DEVICEINTERFACE);
            HwndSource source = HwndSource.FromHwnd(helper.Handle);
            source.AddHook(new HwndSourceHook(UsbDetector.Instance.HwndHandler));
        }

        private void UnRegisterNotification()
        {
            UsbDetector.Instance.UnRegister();
        }

        void Instance_StateChanged(bool arrival)
        {
            MessageBox.Show("usb state: " + arrival);
        }
    }
}
