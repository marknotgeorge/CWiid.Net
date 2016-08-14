using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CWiid.Net
{
    public partial class Wiimote
    {
        /// <summary>
        /// Option flags can be enabled by cwiid_open, or subsequently with cwiid_enable.
        /// </summary>
        [Flags]
        public enum OpenFlags
        {
            /// <summary>
            /// Null flag
            /// </summary>
            Null = 0x00,

            /// <summary>
            /// Enable the message based interfaces (message callback and cwiid_get_mesg)
            /// </summary>
            MessageInterface = 0x01,

            /// <summary>
            /// Enable continuous wiimote reports
            /// </summary>
            Continuous = 0x02,

            /// <summary>
            /// Deliver a button message for each button value received, even if it hasn't changed.
            /// </summary>
            RepeatButton = 0x04,

            /// <summary>
            /// Causes cwiid_get_mesg to fail instead of block if no messages are ready.
            /// </summary>
            NonBlock = 0x08
        }

        [Flags]
        public enum LEDFlags : byte
        {
            Led1 = 0x01,
            Led2 = 0x02,
            Led3 = 0x04,
            Led4 = 0x08
        }

        [Flags]
        public enum CommandFlags
        {
            StatusCommand,
            LEDCommand,
            RumbleCommand,
            ReportModeCommand
        }

        [Flags]
        public enum ReportModeFlags : byte
        {
            None = 0,
            Status = 0x01,
            Button = 0x02,
            Accelerometer = 0x04,
            IRCamera = 0x08,
            Nunchuk = 0x10,
            Classic = 0x20,
            Extension = (Nunchuk | Classic)
        }

        [Flags]
        public enum ButtonFlags : ushort
        {
            None = 0,
            Two = 0x0001,
            One = 0x0002,
            A = 0x0004,
            B = 0x0008,
            Minus = 0x0010,
            Home = 0x0080,
            Left = 0x0100,
            Right = 0x0200,
            Down = 0x0400,
            Up = 0x0800,
            Plus = 0x1000
        }

        public enum ExtensionType
        {
            None,
            Nunchuk,
            Classic,
            Unknown
        }

        [Flags]
        public enum NunchukButtonFlags : byte
        {
            None = 0,
            Z = 0x01,
            C = 0x02
        }

        [Flags]
        public enum ClassicButtonFlags : ushort
        {
            None = 0,
            Up = 0x0001,
            Left = 0x0002,
            ZRight = 0x0004,
            X = 0x0008,
            A = 0x0010,
            Y = 0x0020,
            B = 0x0040,
            ZLeft = 0x0080,
            R = 0x0200,
            Plus = 0x0400,
            Home = 0x0800,
            Minus = 0x1000,
            L = 0x2000,
            Down = 0x4000,
            Right = 0x800
        }

        public enum Error
        {
            None,
            Disconnect,
            Comm
        }
    }
}