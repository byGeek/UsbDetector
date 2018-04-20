using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UsbDetectorLib;

namespace UsbDetectorWindowsForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.Load += (s, e) =>
            {
                UsbDetector.Instance.StateChanged += Instance_StateChanged;
                RegisterNotification();
            };

            this.FormClosed += (s, se) =>
            {
                UnRegisterNotification();
            };

        }

        void Instance_StateChanged(bool arrival)
        {
            MessageBox.Show("Use state: " + arrival);
        }

        private void RegisterNotification()
        {
            UsbDetector.Instance.RegisterNotificationFor(this.Handle, Native.DeviceType.DBT_DEVTYP_DEVICEINTERFACE);
        }

        private void UnRegisterNotification()
        {
            UsbDetector.Instance.UnRegister();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            //device change message
            if (m.Msg == Native.WM_DEVICECHANGE)
            {
                UsbDetector.Instance.ProcessWinMessage(m.Msg, m.WParam, m.LParam);
            }
        }
    }
}
