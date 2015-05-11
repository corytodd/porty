using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Porty
{
    class HardwareEvents
    {
        // https://www.autoitscript.com/autoit3/docs/appendix/WinMsgCodes.htm
        public const int WM_DEVICECHANGE = 0x0219;
        // https://msdn.microsoft.com/en-us/library/windows/desktop/aa363480%28v=vs.85%29.aspx
        public const int DBT_DEVICEARRIVAL = 0x8000;       // system detected a new device
        public const int DBT_DEVTYP_PORT = 0x00000003;     // serial, parallel

        // https://msdn.microsoft.com/en-us/library/windows/desktop/aa363246%28v=vs.85%29.aspx
        [StructLayout(LayoutKind.Sequential)]
        public struct DEV_BROADCAST_HDR
        {
            public int dbch_size;
            public int dbch_devicetype;
            public int dbch_reserved;
            public char dbcp_name;
        }
    }
}
