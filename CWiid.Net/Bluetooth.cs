using System;
using System.Runtime.InteropServices;

namespace CWiid.Net
{
    public class Bluetooth
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct BluetoothDeviceAddress
        {
            public byte b0;
            public byte b1;
            public byte b2;
            public byte b3;
            public byte b4;
            public byte b5;

            public BluetoothDeviceAddress(byte bd5, byte bd4, byte bd3, byte bd2, byte bd1, byte bd0)
            {
                this.b0 = bd0;
                this.b1 = bd1;
                this.b2 = bd2;
                this.b3 = bd3;
                this.b4 = bd4;
                this.b5 = bd5;
            }

            public BluetoothDeviceAddress(string addressString)
            {
                var addressSegments = addressString.Split(':');

                if (addressSegments.Length != 6)
                {
                    throw new ArgumentException("addressString not a valid format");
                }
                else
                {
                    this.b5 = byte.Parse(addressSegments[0]);
                    this.b4 = byte.Parse(addressSegments[1]);
                    this.b3 = byte.Parse(addressSegments[2]);
                    this.b2 = byte.Parse(addressSegments[3]);
                    this.b1 = byte.Parse(addressSegments[4]);
                    this.b0 = byte.Parse(addressSegments[5]);
                }
            }

            public override string ToString()
            {
                return b5.ToString("X2") + ":" + b4.ToString("X2") + ":" + b3.ToString("X2") + ":" + b2.ToString("X2") + ":" + b1.ToString("X2") + ":" + b0.ToString("X2");
            }
        }

        public static BluetoothDeviceAddress BluetoothDeviceAddressAny = new BluetoothDeviceAddress();
        public static BluetoothDeviceAddress BluetoothDeviceAddressAll = new BluetoothDeviceAddress(0xff, 0xff, 0xff, 0xff, 0xff, 0xff);
        public static BluetoothDeviceAddress BluetoothDeviceAddressLocal = new BluetoothDeviceAddress(0, 0, 0, 0xff, 0xff, 0xff);
    }
}