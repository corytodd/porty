using System;
using System.Runtime.InteropServices;

namespace Porty
{
    internal static class UsbNotification
    {
        // https://msdn.microsoft.com/en-us/library/windows/desktop/aa363480%28v=vs.85%29.aspx
        public const int DBT_DEVICEARRIVAL = 0x8000;       // system detected a new device
        public const int DBT_DEVICEREMOVALCOMPLETE = 0x8004;  // device is gone      
        
        // https://www.autoitscript.com/autoit3/docs/appendix/WinMsgCodes.htm
        public const int WM_DEVICECHANGE = 0x0219;


        public const int DBT_DEVTYP_PORT = 0x00000003;     // serial, parallel

        private const int DbtDevtypDeviceinterface = 5;
        private static readonly Guid GuidDevinterfaceUSBDevice0 = new Guid(0x25dbce51, 0x6c8f, 0x4a72,
                      0x8a, 0x6d, 0xb5, 0x4c, 0x2b, 0x4f, 0xc8, 0x35);//  
        // = new Guid("86E0D1E0-8089-11D0-9CE4-08003E301F73");
        //"A5DCBF10-6530-11D2-901F-00C04FB951ED"); // USB devices
        private static readonly Guid GuidDevinterfaceUSBDevice1 = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED");
        private static IntPtr notificationHandle;

        /// <summary>
        /// Registers a window to receive notifications when USB devices are plugged or unplugged.
        /// </summary>
        /// <param name="windowHandle">Handle to the window receiving notifications.</param>
        public static void RegisterUsbDeviceNotification(IntPtr windowHandle)
        {
            DevBroadcastDeviceinterface dbi0 = new DevBroadcastDeviceinterface
            {
                DeviceType = DbtDevtypDeviceinterface,
                Reserved = 0,
                ClassGuid = GuidDevinterfaceUSBDevice0,
                Name = 0
            };

            DevBroadcastDeviceinterface dbi1 = new DevBroadcastDeviceinterface
            {
                DeviceType = DbtDevtypDeviceinterface,
                Reserved = 0,
                ClassGuid = GuidDevinterfaceUSBDevice1,
                Name = 0
            };

            dbi0.Size = Marshal.SizeOf(dbi0);
            IntPtr buffer0 = Marshal.AllocHGlobal(dbi0.Size);
            Marshal.StructureToPtr(dbi0, buffer0, true);

            notificationHandle = RegisterDeviceNotification(windowHandle, buffer0, 0);

            dbi1.Size = Marshal.SizeOf(dbi1);
            IntPtr buffer1 = Marshal.AllocHGlobal(dbi1.Size);
            Marshal.StructureToPtr(dbi1, buffer1, true);

            notificationHandle = RegisterDeviceNotification(windowHandle, buffer1, 0);
        }

        /// <summary>
        /// Unregisters the window for USB device notifications
        /// </summary>
        public static void UnregisterUsbDeviceNotification()
        {
            UnregisterDeviceNotification(notificationHandle);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr RegisterDeviceNotification(IntPtr recipient, IntPtr notificationFilter, int flags);

        [DllImport("user32.dll")]
        private static extern bool UnregisterDeviceNotification(IntPtr handle);

        [StructLayout(LayoutKind.Sequential)]
        private struct DevBroadcastDeviceinterface
        {
            internal int Size;
            internal int DeviceType;
            internal int Reserved;
            internal Guid ClassGuid;
            internal short Name;
        }
    }
}
