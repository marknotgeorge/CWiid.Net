using CWiid.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Example
{
    internal class Program
    {
        private static ManualResetEvent waitHandle = new ManualResetEvent(false);

        private static Wiimote wiimote;

        private static void Main(string[] args)
        {
            if (args.Length > 1)
                wiimote = new Wiimote(args[1]);
            else
                wiimote = new Wiimote();

            wiimote.Connected += wiimoteConnected;
            wiimote.Disconnected += wiimoteDisconnected;

            Console.WriteLine("Put your Wiimote in discoverable mode (press 1+2)...");
            wiimote.Connect();
            waitHandle.WaitOne();

            Console.WriteLine("Trying to close Wiimote...");
            wiimote.Disconnect();
            waitHandle.WaitOne();
        }

        private static void wiimoteDisconnected(object sender, ConnectOrDisconnectEventArgs e)
        {
            Console.WriteLine(e.Message);
            waitHandle.Set();
        }

        private static void wiimoteConnected(object sender, ConnectOrDisconnectEventArgs e)
        {
            if (e.Success)
            {
                Console.WriteLine("Wiimote successfully opened. Press return to rumble!");
                Console.ReadLine();
                wiimote.Rumble = true;
                Console.WriteLine("Press return to stop rumbling & disconnect the Wiimote...");
                Console.ReadLine();
                Console.WriteLine("Stopping the rumble!");
                wiimote.Rumble = false;
            }
            else
            {
                Console.WriteLine($"Unable to connect to Wiimote. Reason: {e.Message}");
            }
            waitHandle.Set();
        }
    }
}