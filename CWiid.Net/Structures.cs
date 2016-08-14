using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CWiid.Net
{
    public partial class Wiimote
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct WiimoteState
        {
            public ReportModeFlags ReportMode;
            public LEDFlags LEDs;

            [MarshalAs(UnmanagedType.U1)]
            public bool Rumble;

            public byte Battery;
            public ButtonFlags Buttons;
            public byte AccelerometerX;
            public byte AccelerometerY;
            public byte AccelerometerZ;

            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 4)]
            public IRSource[] IRSources;

            //CWIID_IR_SRC_COUNT = 4
            public ExtensionType ExtensionType;

            public ExtensionState ExtensionState;
            public Error Error;

            public override string ToString()
            {
                bool validSource = false;
                string returnString = "Report Mode: " + ReportMode.ToString() + "\n" + "Active LEDs: " + LEDs.ToString() + "\n";
                returnString += "Rumble: " + ((Rumble) ? "On" : "Off") + "\n" + "Battery: " + ((int)((float)Battery / MaxBattery)).ToString() + "\n" +
                "Buttons: " + Buttons.ToString() + "\n";
                returnString += string.Format("ACC: x={0} y={1} z={2}\n", AccelerometerX.ToString(), AccelerometerY.ToString(), AccelerometerZ.ToString()) + "IR: ";
                for (int i = 0; i < IRSourceCount; i++)
                {
                    IRSource irSrc = IRSources[i];
                    if (irSrc.IsValid)
                    {
                        validSource = true;
                        returnString += "(" + irSrc.X.ToString() + "," + irSrc.Y.ToString() + ")";
                    }
                }
                if (!validSource)
                {
                    returnString += "no sources detected";
                }
                returnString += "\n";

                switch (ExtensionType)
                {
                    case ExtensionType.None:
                        returnString += "No extension\n";
                        break;

                    case ExtensionType.Unknown:
                        returnString += "Unknown extension attached\n";
                        break;

                    case ExtensionType.Nunchuk:
                        returnString += string.Format("Nunchuk: buttons={0} stick=({1},{2}) acc.x={3} acc.y={4} " + "acc.z={5}\n",
                                                      ExtensionState.Nunchuk.Buttons.ToString(),
                                                      ExtensionState.Nunchuk.StickX.ToString(),
                                                      ExtensionState.Nunchuk.StickY.ToString(),
                                                      ExtensionState.Nunchuk.AccelerometerX.ToString(),
                                                      ExtensionState.Nunchuk.AccelerometerY.ToString(),
                                                      ExtensionState.Nunchuk.AccelerometerZ.ToString());
                        break;

                    case ExtensionType.Classic:
                        returnString += string.Format("Classic: btns={0} l_stick=({1},{2}) r_stick=({3},{4}) " + "l={5} r={5}\n",
                            ExtensionState.Classic.Buttons.ToString(),
                            ExtensionState.Classic.LeftStickX.ToString(),
                            ExtensionState.Classic.LeftStickY.ToString(),
                            ExtensionState.Classic.RightStickX.ToString(),
                            ExtensionState.Classic.RightStickY.ToString(),
                            ExtensionState.Classic.L.ToString(),
                            ExtensionState.Classic.R.ToString());
                        break;
                }

                return returnString;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IRSource
        {
            [MarshalAs(UnmanagedType.U1)]
            public bool IsValid;

            public ushort X;
            public ushort Y;
            public sbyte Size;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NunchukState
        {
            public byte StickX;
            public byte StickY;

            public byte AccelerometerX;
            public byte AccelerometerY;
            public byte AccelerometerZ;

            public NunchukButtonFlags Buttons;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ClassicState
        {
            public byte LeftStickX;
            public byte LeftStickY;

            public byte RightStickX;
            public byte RightStickY;

            public byte L;
            public byte R;
            public ClassicButtonFlags Buttons;
        }

        // union
        [StructLayout(LayoutKind.Explicit)]
        public struct ExtensionState
        {
            [FieldOffset(0)]
            public NunchukState Nunchuk;

            [FieldOffset(0)]
            public ClassicState Classic;
        }
    }
}