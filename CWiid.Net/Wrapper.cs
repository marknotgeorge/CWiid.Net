using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static CWiid.Net.Bluetooth;
using static CWiid.Net.Wiimote;

namespace CWiid.Net
{
    public class Wrapper : IWrapper
    {
        [DllImport("libcwiid.so")]
        private static extern IntPtr cwiid_open_timeout(ref BluetoothDeviceAddress bdaddr, OpenFlags flags, int timeout);

        [DllImport("libcwiid.so")]
        private static extern int cwiid_close(IntPtr wiimote);

        [DllImport("libcwiid.so")]
        private static extern int cwiid_set_rumble(IntPtr wiimote, byte rumble);

        [DllImport("libcwiid.so")]
        public static extern int cwiid_set_led(IntPtr wiimote, LEDFlags ledFlags);

        [DllImport("libcwiid.so")]
        public static extern int cwiid_get_state(IntPtr wiimote, ref WiimoteState state);

        // int cwiid_set_rpt_mode(cwiid_wiimote_t *wiimote, uint8_t rpt_mode);
        [DllImport("libcwiid.so")]
        public static extern int cwiid_set_rpt_mode(IntPtr wiimote, ReportModeFlags rpt_mode);

        public IntPtr OpenTimeout(ref BluetoothDeviceAddress bdaddr, OpenFlags flags, int timeout)
        {
            return cwiid_open_timeout(ref bdaddr, flags, timeout);
        }

        public int Close(IntPtr wiimote)
        {
            return cwiid_close(wiimote);
        }

        public int SetRumble(IntPtr wiimote, bool rumble)
        {
            return cwiid_set_rumble(wiimote, Convert.ToByte(rumble));
        }

        public int SetLED(IntPtr wiimote, LEDFlags flags)
        {
            return cwiid_set_led(wiimote, flags);
        }

        public int GetState(IntPtr wiimote, ref WiimoteState state)
        {
            return cwiid_get_state(wiimote, ref state);
        }

        public int SetReportingMode(IntPtr wiimote, ReportModeFlags flags)
        {
            return cwiid_set_rpt_mode(wiimote, flags);
        }
    }
}