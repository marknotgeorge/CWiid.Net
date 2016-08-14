using CWiid.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static CWiid.Net.Wiimote;

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

            Console.WriteLine("CWiid.Net example app");
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
                DisplayMenu();
            }
            else
            {
                Console.WriteLine($"Unable to connect to Wiimote. Reason: {e.Message}");
            }
            waitHandle.Set();
        }

        private static void DisplayMenu()
        {
            bool quitRequested = false;
            Console.Clear();
            Console.WriteLine("Wiimote connected! Press a key to pick an option:");
            Console.WriteLine("1. Toggle LED 1");
            Console.WriteLine("2. Toggle LED 2");
            Console.WriteLine("3. Toggle LED 3");
            Console.WriteLine("4. Toggle LED 4");
            Console.WriteLine("5. Toggle Rumble");
            Console.WriteLine("b. Toggle button reporting");
            Console.WriteLine("s. Get State");
            Console.WriteLine("x. Exit");
            do
            {
                var keyPressed = Console.ReadKey(true).KeyChar;
                switch (keyPressed)
                {
                    case '1':
                        wiimote.LEDs = wiimote.LEDs ^ LEDFlags.Led1;
                        break;

                    case '2':
                        wiimote.LEDs = wiimote.LEDs ^ LEDFlags.Led2;
                        break;

                    case '3':
                        wiimote.LEDs = wiimote.LEDs ^ LEDFlags.Led3;
                        break;

                    case '4':
                        wiimote.LEDs = wiimote.LEDs ^ LEDFlags.Led4;
                        break;

                    case '5':
                        wiimote.Rumble = !wiimote.Rumble;
                        break;

                    case 'b':
                        var verb = (wiimote.ReportingMode.HasFlag(ReportModeFlags.Button)) ? "unset" : "set";
                        wiimote.ReportingMode = wiimote.ReportingMode ^ ReportModeFlags.Button;
                        Console.WriteLine($"Button reporting {verb}");

                        break;

                    case 's':
                        if (wiimote.State == null)
                            Console.WriteLine("Error getting state.");
                        else
                            Console.WriteLine(wiimote.State.ToString());
                        break;

                    case 'x':
                        quitRequested = true;
                        break;
                }
            } while (!quitRequested);
            waitHandle.Set();
        }
    }
}