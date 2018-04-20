using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UsbDetectorLib
{
    public delegate void UsbStateChangedEventHandler(bool arrival);

    public sealed class UsbDetector
    {
        #region singleton

        private UsbDetector() { }

        public static UsbDetector Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        class Nested
        {
            static Nested() { }
            internal static readonly UsbDetector instance = new UsbDetector();
        }

        #endregion



        public IntPtr HwndHandler(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            ProcessWinMessage(msg, wParam, lParam);
            return IntPtr.Zero;
        }

        public event UsbStateChangedEventHandler StateChanged;
        public IntPtr _hdNotify = IntPtr.Zero;


        public void ProcessWinMessage(int msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg == Native.WM_DEVICECHANGE)
            {
                switch (wParam.ToInt32())
                {
                    case (int)Native.WM_DEVICECHANGE_WPPARAMS.DBT_DEVICEARRIVAL:

                        if (IsDesiredDevice(lParam))
                        {
                            if (StateChanged != null)
                            {
                                StateChanged(true);
                            }
                        }

                        break;

                    case (int)Native.WM_DEVICECHANGE_WPPARAMS.DBT_DEVICEREMOVECOMPLETE:

                        if (IsDesiredDevice(lParam))
                        {
                            if (StateChanged != null)
                            {
                                StateChanged(false);
                            }
                        }
                        break;
                    /*
                     * this event will be fired when device been added and removed
                case Native.DBT_DEVNODES_CHANGED:
                        
                    if (StateChanged != null)
                    {
                        StateChanged(false);
                    }
                    break;
                     * */
                    default:
                        break;
                }
            }
        }

        private bool IsDesiredDevice(IntPtr lParam)
        {
            var hdr = (Native.DEV_BROADCAST_HDR)Marshal.PtrToStructure(lParam, typeof(Native.DEV_BROADCAST_HDR));
            if (hdr.dbcc_devicetype == (uint)_deviceType)
            {
                if (_deviceType == Native.DeviceType.DBT_DEVTYP_DEVICEINTERFACE)
                {
                    Native.DEV_BROADCAST_DEVICEINTERFACE deviceInterface =
                        (Native.DEV_BROADCAST_DEVICEINTERFACE)Marshal.PtrToStructure(lParam, typeof(Native.DEV_BROADCAST_DEVICEINTERFACE));

                    var str = Encoding.Default.GetString(deviceInterface.dbcc_classguid);
                    var guid = new Guid(deviceInterface.dbcc_classguid);
                    if (guid.ToString() == DEVICE_INTERFACE_GUID)
                    {
                        return true;
                    }
                }
                else
                {
                    //TODO: other device type
                }
            }
            return false;
        }

        // private const string USBClassID = "c671678c-82c1-43f3-d700-0049433e9a4b";
        // http://msdn.microsoft.com/en-us/library/ff545972.aspx
        /* GUID_DEVINTERFACE_USB_DEVICE : {A5DCBF10-6530-11D2-901F-00C04FB951ED}
         * GUID_DEVINTERFACE_USB_HOST_CONTROLLER : {3ABF6F2D-71C4-462A-8A92-1E6861E6AF27}
         * GUID_DEVINTERFACE_USB_HUB : {F18A0E88-C30C-11D0-8815-00A0C906BED8}
         * 
         */

        private const string DEVICE_INTERFACE_GUID = "cc22e4b4-7985-426a-87ea-6ee58f202136";

        public void UnRegister()
        {
            if (_hdNotify != IntPtr.Zero)
            {
                Native.UnregisterDeviceNotification(_hdNotify);
            }
            if (StateChanged != null)
            {
                var list = StateChanged.GetInvocationList();
                foreach (var item in list)
                {
                    StateChanged -= item as UsbStateChangedEventHandler;
                }
            }
        }

        private Native.DeviceType _deviceType;
        public void RegisterNotificationFor(IntPtr recipientHandle, Native.DeviceType deviceType)
        {
            _deviceType = deviceType;

            IntPtr notificationFilter = IntPtr.Zero;

            switch (_deviceType)
            {
                case Native.DeviceType.DBT_DEVTYP_DEVICEINTERFACE:
                    {
                        Native.DEV_BROADCAST_DEVICEINTERFACE di =
                            new Native.DEV_BROADCAST_DEVICEINTERFACE();

                        var size = Marshal.SizeOf(di);

                        di.dbcc_size = (uint)size;
                        di.dbcc_devicetype = (int)Native.DeviceType.DBT_DEVTYP_DEVICEINTERFACE;
                        di.dbcc_reserved = 0;
                        di.dbcc_classguid = new Guid(DEVICE_INTERFACE_GUID).ToByteArray();

                        notificationFilter = Marshal.AllocHGlobal(size);
                        Marshal.StructureToPtr(di, notificationFilter, true);
                        break;
                    }
                case Native.DeviceType.DBT_DEVTYP_HANDLE:
                    {
                        Native.DEV_BROADCAST_HANDLE di =
                            new Native.DEV_BROADCAST_HANDLE();

                        var size = Marshal.SizeOf(di);

                        di.dbch_size = (uint)size;
                        di.dbch_devicetype = (int)Native.DeviceType.DBT_DEVTYP_HANDLE;
                        di.dbch_reserved = 0;

                        //https://msdn.microsoft.com/en-us/library/windows/desktop/aa363217(v=vs.85).aspx
                        di.dbch_handle = IntPtr.Zero;  //Be sure to set the dbch_handle member to the device handle obtained from the CreateFile function. 
                        di.dbch_hdevnotify = IntPtr.Zero;

                        //TODO
                        //di.dbch_eventguid =
                        //di.dbch_nameoffset = 
                        //di.dbch_data =

                        break;
                    }
                case Native.DeviceType.DBT_DEVTYP_OEM:
                    {
                        //TODO

                        break;
                    }
                case Native.DeviceType.DBT_DEVTYP_PORT:
                    {
                        //TODO
                        break;
                    }
                case Native.DeviceType.DBT_DEVTYP_VOLUME:
                    {
                        //TODO
                        break;
                    }
                default:
                    break;
            }

            if (notificationFilter != IntPtr.Zero)
            {
                _hdNotify = Native.RegisterDeviceNotification(recipientHandle, notificationFilter, (uint)Native.DEVICE_NOTIFY.DEVICE_NOTIFY_WINDOW_HANDLE);
            }

        }
    }
}
