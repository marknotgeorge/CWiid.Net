using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CWiid.Net.Bluetooth;
using static CWiid.Net.Wiimote;

namespace CWiid.Net
{
    public interface IWrapper
    {
        IntPtr OpenTimeout(ref BluetoothDeviceAddress bdaddr, OpenFlags flags, int timeout);

        int Close(IntPtr wiimote);

        int SetRumble(IntPtr wiimote, bool rumble);

        int SetLED(IntPtr wiimote, LEDFlags flags);

        int GetState(IntPtr wiimote, ref WiimoteState state);

        int SetReportingMode(IntPtr wiimote, ReportModeFlags flags);
    }
}