using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CWiid.Net.Bluetooth;

namespace CWiid.Net
{
    public partial class Wiimote
    {
        private BluetoothDeviceAddress m_deviceAddress;
        private IntPtr m_wiimote;

        private bool _isConnected = false;
        private IWrapper _wrapper;

        public bool IsConnected
        {
            get { return _isConnected; }
            set { _isConnected = value; }
        }

        public Wiimote(BluetoothDeviceAddress deviceAddress, IWrapper wrapper = null)
        {
            m_deviceAddress = deviceAddress;
            if (wrapper == null)
                _wrapper = new Wrapper();
            else
                _wrapper = wrapper;
        }

        public Wiimote(IWrapper wrapper = null)
        {
            m_deviceAddress = BluetoothDeviceAddressAny;
            if (wrapper == null)
                _wrapper = new Wrapper();
            else
                _wrapper = wrapper;
        }

        public Wiimote(string deviceAddressString, IWrapper wrapper = null)
        {
            m_deviceAddress = new BluetoothDeviceAddress(deviceAddressString);
            if (wrapper == null)
                _wrapper = new Wrapper();
            else
                _wrapper = wrapper;
        }

        public void Connect(OpenFlags flags = OpenFlags.Null, int timeout = 5)
        {
            if (!IsConnected)
            {
                m_wiimote = _wrapper.OpenTimeout(ref m_deviceAddress, flags, timeout);

                if (m_wiimote != null)
                {
                    IsConnected = true;
                    var args = new ConnectOrDisconnectEventArgs()
                    {
                        Success = IsConnected,
                        Message = "Connected",
                        Id = "WIIMOTE_ID_HERE"
                    };
                    OnConnect(args);
                }
                else
                {
                    IsConnected = false;
                    var args = new ConnectOrDisconnectEventArgs()
                    {
                        Success = IsConnected,
                        Message = "Connection Failure!",
                        Id = "WIIMOTE_ID_HERE"
                    };
                    OnConnect(args);
                }
            }
        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                IsConnected = false;

                var returnVal = _wrapper.Close(m_wiimote);

                var returnMessage = (returnVal == 0) ? "Disconnected" : "Error in disconnecting";

                var args = new ConnectOrDisconnectEventArgs()
                {
                    Success = IsConnected,
                    Message = returnMessage,
                    Id = "WIIMOTE_ID_HERE"
                };
                OnDisconnect(args);
            }
        }

        private LEDFlags _LEDs;

        public LEDFlags LEDs
        {
            get
            {
                return _LEDs;
            }
            set
            {
                _LEDs = value;
                _wrapper.SetLED(m_wiimote, value);
            }
        }

        private bool _rumble;

        public bool Rumble
        {
            get
            {
                return _rumble;
            }
            set
            {
                _rumble = value;
                _wrapper.SetRumble(m_wiimote, value);
            }
        }

        private ReportModeFlags _reportingMode;

        public ReportModeFlags ReportingMode
        {
            get
            {
                return _reportingMode;
            }
            set
            {
                _reportingMode = value;
                _wrapper.SetReportingMode(m_wiimote, value);
            }
        }

        public WiimoteState? State
        {
            get
            {
                WiimoteState returnState = new WiimoteState();

                int result = _wrapper.GetState(m_wiimote, ref returnState);
                if (result > 0)
                    return null;
                else
                    return returnState;
            }
        }

        public event EventHandler<ConnectOrDisconnectEventArgs> Connected;

        protected virtual void OnConnect(ConnectOrDisconnectEventArgs e)
        {
            Connected?.Invoke(this, e);
        }

        public event EventHandler<ConnectOrDisconnectEventArgs> Disconnected;

        protected virtual void OnDisconnect(ConnectOrDisconnectEventArgs e)
        {
            Disconnected?.Invoke(this, e);
        }
    }

    public class ConnectOrDisconnectEventArgs : EventArgs
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Id { get; set; }
    }
}