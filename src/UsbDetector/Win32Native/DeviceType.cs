using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsbDetectorLib
{
    /// <summary>
    /// device type
    /// 
    /// <ref>https://msdn.microsoft.com/en-us/library/windows/desktop/aa363246(v=vs.85).aspx</ref>
    /// </summary>
    public static class DeviceType
    {
        /// <summary>
        /// Class of devices. This structure is a DEV_BROADCAST_DEVICEINTERFACE structure.
        /// </summary>
        public const int DBT_DEVTYP_DEVICEINTERFACE = 0x00000005;

        /// <summary>
        /// File system handle. This structure is a DEV_BROADCAST_HANDLE structure.
        /// </summary>
        public const int DBT_DEVTYP_HANDLE = 0x00000006;

        /// <summary>
        /// OEM- or IHV-defined device type. This structure is a DEV_BROADCAST_OEM structure.
        /// </summary>
        public const int DBT_DEVTYP_OEM = 0x00000000;

        /// <summary>
        /// Port device (serial or parallel). This structure is a DEV_BROADCAST_PORT structure.
        /// </summary>
        public const int DBT_DEVTYP_PORT = 0x00000003;

        /// <summary>
        /// ogical volume. This structure is a DEV_BROADCAST_VOLUME structure.
        /// </summary>
        public const int DBT_DEVTYP_VOLUME = 0x00000002;

    }
}
