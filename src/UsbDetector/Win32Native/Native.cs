using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UsbDetectorLib
{
    public static class Native
    {
        #region native method

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr RegisterDeviceNotification(IntPtr intPtr, IntPtr notificationFilter, uint flags);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint UnregisterDeviceNotification(IntPtr hHandle);

        #endregion



        public const int DEVICE_NOTIFY_SERVICE_HANDLE = 1;
        public const int DEVICE_NOTIFY_WINDOW_HANDLE = 0;
        public const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 4;

        public const int SERVICE_CONTROL_STOP = 0x00000001;
        public const int SERVICE_CONTROL_DEVICEEVENT = 0x00000011;
        public const int SERVICE_CONTROL_SHUTDOWN = 0x00000005;

        public enum DeviceType
        {
            /*device type
             * https://msdn.microsoft.com/en-us/library/windows/desktop/aa363246(v=vs.85).aspx
             */

            DBT_DEVTYP_DEVICEINTERFACE = 0x00000005,
            DBT_DEVTYP_HANDLE = 0x00000006,
            DBT_DEVTYP_OEM = 0x00000000,
            DBT_DEVTYP_PORT = 0x00000003,
            DBT_DEVTYP_VOLUME = 0x00000002
        }

        public enum WM_DEVICECHANGE_WPPARAMS
        {
            /*
             * WM_DEVICECHANGE message WParam
             * https://msdn.microsoft.com/en-us/library/windows/desktop/aa363480(v=vs.85).aspx
             * */

            DBT_DEVICEARRIVAL = 0x8000,
            DBT_DEVICEQUERYREMOVE = 0x8001,
            DBT_DEVICEREMOVECOMPLETE = 0x8004,
            DBT_CONFIGCHANGECANCELED = 0x19,
            DBT_CONFIGCHANGED = 0x18,
            DBT_CUSTOMEVENT = 0x8006,
            DBT_DEVICEQUERYREMOVEFAILED = 0x8002,
            DBT_DEVICEREMOVEPENDING = 0x8003,
            DBT_DEVICETYPESPECIFIC = 0x8005,
            DBT_DEVNODES_CHANGED = 0x7,
            DBT_QUERYCHANGECONFIG = 0x17,
            DBT_USERDEFINED = 0xFFFF
        }

        /*WM_DEVICECHANGE message
         *
         * https://msdn.microsoft.com/en-us/library/windows/desktop/aa363480%28v=vs.85%29.aspx?f=255&MSPPError=-2147217396
         * 
         * 
         * Device management event
         * 
         * https://msdn.microsoft.com/en-us/library/windows/desktop/aa363232%28v=vs.85%29.aspx?f=255&MSPPError=-2147217396
         * 
         */
        public const int WM_DEVICECHANGE = 0x0219; // device state change
        public const int DBT_DEVICEARRIVAL = 0x8000; // detected a new device
        public const int DBT_DEVICEQUERYREMOVE = 0x8001; // preparing to remove
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004; // removed
        public const int DBT_DEVNODES_CHANGED = 0x0007; //A device has been added to or removed from the system.

        [Flags]
        public enum DEVICE_NOTIFY : uint
        {
            /*
             * https://msdn.microsoft.com/en-us/library/windows/desktop/aa363431(v=vs.85).aspx
             * */

            DEVICE_NOTIFY_WINDOW_HANDLE = 0x00000000,  //The hRecipient parameter is a window handle.
            DEVICE_NOTIFY_SERVICE_HANDLE = 0x00000001, //The hRecipient parameter is a service status handle.

            /*
             * Notifies the recipient of device interface events for all device interface classes. (The dbcc_classguid member is ignored.)
             * This value can be used only if the dbch_devicetype member is DBT_DEVTYP_DEVICEINTERFACE.
             * */
            DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 0x00000004
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DEV_BROADCAST_DEVICEINTERFACE
        {
            /*
             * https://msdn.microsoft.com/en-us/library/windows/desktop/aa363244%28v=vs.85%29.aspx?f=255&MSPPError=-2147217396
             * */

            public UInt32 dbcc_size;

            public UInt32 dbcc_devicetype;

            public UInt32 dbcc_reserved;

            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 16)]
            public byte[] dbcc_classguid;  //https://msdn.microsoft.com/en-us/library/windows/desktop/aa373931(v=vs.85).aspx

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public Char[] dbcc_name;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DEV_BROADCAST_HDR
        {
            /*
             * https://msdn.microsoft.com/en-us/library/windows/desktop/aa363246(v=vs.85).aspx
             * */

            public UInt32 dbcc_size;
            public UInt32 dbcc_devicetype;
            public UInt32 dbcc_reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DEV_BROADCAST_HANDLE
        {
            /*
             * https://msdn.microsoft.com/en-us/library/windows/desktop/aa363245%28v=vs.85%29.aspx?f=255&MSPPError=-2147217396
            */

            public UInt32 dbch_size;
            public UInt32 dbch_devicetype;
            public UInt32 dbch_reserved;
            public IntPtr dbch_handle;
            public IntPtr dbch_hdevnotify;

            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 16)]
            public byte[] dbch_eventguid;

            public Int32 dbch_nameoffset;
            public Byte dbch_data;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DEV_BROADCAST_OEM
        {
            /*
             * https://msdn.microsoft.com/en-us/library/windows/desktop/aa363247(v=vs.85).aspx
             * */

            public UInt32 dbco_size;
            public UInt32 dbco_devicetype;
            public UInt32 dbco_reserved;
            public UInt32 dbco_identifier;
            public UInt32 dbco_suppfunc;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DEV_BROADCAST_PORT
        {
            /*
             * https://msdn.microsoft.com/en-us/library/windows/desktop/aa363248%28v=vs.85%29.aspx?f=255&MSPPError=-2147217396
             * */

            public UInt32 dbcp_size;
            public UInt32 dbcp_devicetype;
            public UInt32 dbcp_reserved;
            char dbcp_name;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DEV_BROADCAST_VOLUME
        {
            /*
             * https://msdn.microsoft.com/en-us/library/windows/desktop/aa363249(v=vs.85).aspx
             * */

            public UInt32 dbcv_size;
            public UInt32 dbcv_devicetype;
            public UInt32 dbcv_reserved;
            public UInt32 dbcv_unitmask;
            public UInt16 dbcv_flags;
        }

    }
}
