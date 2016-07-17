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
    }
}