using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CWiid.Net
{
    //And here comes the code for the wmdemo Example.It is not the way i would normally write a C# Application, but i wanted to keep as close as possible to the original C wmdemo Example.

    // project created on 11.11.2007 at 18:36 Simple Demo based on wmdemo.c in the cwiid package

    using monowii;
    using System;

    namespace wmdemocil
    {
        internal class MainClass
        {
            private static string MenuString = "1: toggle LED 1\n" +
                "2: toggle LED 2\n" +
                    "3: toggle LED 3\n" +
                    "4: toggle LED 4\n" +
                    "5: toggle rumble\n" +
                    "a: toggle accelerometer reporting\n" +
                    "b: toggle button reporting\n" +
                    "e: toggle extension reporting\n" +
                    "i: toggle ir reporting\n" +
                    "m: toggle messages\n" +
                    "p: print this menu\n" +
                    "r: request status message ((t) enables callback output)\n" +
                    "s: print current state\n" +
                    "t: toggle status reporting\n" +
                    "x: exit\n";

            public static cwiid.cwiid_err_t err_delegate = new monowii.cwiid.cwiid_err_t(err);
            public static cwiid.cwiid_mesg_callback_t cwiid_callback_delegate = new monowii.cwiid.cwiid_mesg_callback_t(cwiid_callback);

            public static void err(IntPtr wiimote, string s, IntPtr ap)
            {
                if (wiimote.ToInt32() != 0)
                    Console.Write("{0}:", cwiid.cwiid_get_id(wiimote));
                else
                    Console.Write("-1:");
                Console.WriteLine(s);
            }

            public static int Main(string[] args)
            {
                IntPtr wiimote; /* wiimote handle */
                cwiid.cwiid_state state = new monowii.cwiid.cwiid_state();  /* wiimote state */
                Bluetooth.bdaddr_t bdaddr;  /* bluetooth device address */
                bool mesg = false;
                cwiid.CWIID_LED_FLAGS led_state = 0;
                cwiid.CWIID_RPT_FLAGS rpt_mode = 0;
                byte rumble = 0;
                bool exit = false;

                cwiid.cwiid_set_err(err_delegate);

                /* Connect to address given on command-line, if present */
                if (args.Length > 1)
                {
                    bdaddr = new monowii.Bluetooth.bdaddr_t(args[1]);
                }
                else
                {
                    bdaddr = Bluetooth.BDADDR_ANY;
                }

                /* Connect to the wiimote */
                Console.WriteLine("Put Wiimote in discoverable mode now (press 1+2)...");
                wiimote = cwiid.cwiid_open(ref bdaddr, 0);
                if (wiimote.ToInt32() == 0)
                {
                    Console.WriteLine("Unable to connect to wiimote");
                    return -1;
                }
                if (cwiid.cwiid_set_mesg_callback(wiimote, cwiid_callback_delegate) > 0)
                {
                    Console.WriteLine("Unable to set message callback");
                }

                Console.Write("Note: To demonstrate the new API interfaces, wmdemo no longer " +
               "enables messages by default.\n" +
               "Output can be gathered through the new state-based interface (s), " +
               "or by enabling the messages interface (c).\n");

                /* Menu */
                Console.Write(MenuString);

                while (!exit)
                {
                    string commandChar = Console.ReadLine();
                    if (commandChar.Length > 0)
                        switch (commandChar.ToCharArray()[0])
                        {
                            case '1':
                                led_state = led_state ^ cwiid.CWIID_LED_FLAGS.LED1_ON;
                                set_led_state(wiimote, led_state);
                                break;

                            case '2':
                                led_state = led_state ^ cwiid.CWIID_LED_FLAGS.LED2_ON;
                                set_led_state(wiimote, led_state);
                                break;

                            case '3':
                                led_state = led_state ^ cwiid.CWIID_LED_FLAGS.LED3_ON;
                                set_led_state(wiimote, led_state);
                                break;

                            case '4':
                                led_state = led_state ^ cwiid.CWIID_LED_FLAGS.LED4_ON;
                                set_led_state(wiimote, led_state);
                                break;

                            case '5':
                                rumble = (byte)(rumble ^ 1);
                                if (cwiid.cwiid_set_rumble(wiimote, rumble) > 0)
                                {
                                    Console.Write("Error setting rumble\n");
                                }
                                break;

                            case 'a':
                                rpt_mode = rpt_mode ^ cwiid.CWIID_RPT_FLAGS.ACC;
                                set_rpt_mode(wiimote, rpt_mode);
                                break;

                            case 'b':
                                rpt_mode = rpt_mode ^ cwiid.CWIID_RPT_FLAGS.BTN;
                                set_rpt_mode(wiimote, rpt_mode);
                                break;

                            case 'e':
                                /* CWIID_RPT_EXT is actually
                                 * CWIID_RPT_NUNCHUK | CWIID_RPT_CLASSIC */
                                rpt_mode = rpt_mode ^ cwiid.CWIID_RPT_FLAGS.EXT;
                                set_rpt_mode(wiimote, rpt_mode);
                                break;

                            case 'i':
                                /* libwiimote picks the highest quality IR mode available with the
                                 * other options selected (not including as-yet-undeciphered
                                 * interleaved mode */
                                rpt_mode = rpt_mode ^ cwiid.CWIID_RPT_FLAGS.IR;
                                set_rpt_mode(wiimote, rpt_mode);
                                break;

                            case 'm':
                                if (!mesg)
                                {
                                    if (cwiid.cwiid_enable(wiimote, cwiid.CWIID_FLAGS.MESG_IFC) > 0)
                                    {
                                        Console.Write("Error enabling messages\n");
                                    }
                                    else
                                    {
                                        mesg = true;
                                    }
                                }
                                else
                                {
                                    if (cwiid.cwiid_disable(wiimote, cwiid.CWIID_FLAGS.MESG_IFC) > 0)
                                    {
                                        Console.Write("Error disabling message\n");
                                    }
                                    else
                                    {
                                        mesg = false;
                                    }
                                }
                                break;

                            case 'p':
                                Console.Write(MenuString);
                                break;

                            case 'r':
                                if (cwiid.cwiid_request_status(wiimote) > 0)
                                {
                                    Console.Write("Error requesting status message\n");
                                }
                                break;

                            case 's':
                                if (cwiid.cwiid_get_state(wiimote, ref state) > 0)
                                {
                                    Console.Write("Error getting state\n");
                                }
                                print_state(state);
                                break;

                            case 't':
                                rpt_mode = rpt_mode ^ cwiid.CWIID_RPT_FLAGS.STATUS;
                                set_rpt_mode(wiimote, rpt_mode);
                                break;

                            case 'x':
                                exit = true;
                                break;

                            case '\n':
                                break;

                            default:
                                Console.Write("invalid option\n");
                                break;
                        }
                }

                if (cwiid.cwiid_close(wiimote) > 0)
                {
                    Console.Write("Error on wiimote disconnect\n");
                    return -1;
                }
                return 0;
            }

            private static void set_led_state(IntPtr wiimote, cwiid.CWIID_LED_FLAGS led_state)
            {
                if (cwiid.cwiid_set_led(wiimote, led_state) > 0)
                {
                    Console.Write("Error setting LEDs \n");
                }
            }

            private static void set_rpt_mode(IntPtr wiimote, cwiid.CWIID_RPT_FLAGS rpt_mode)
            {
                if (cwiid.cwiid_set_rpt_mode(wiimote, rpt_mode) > 0)
                {
                    Console.Write("Error setting report mode \n");
                }
            }

            private static void print_state(cwiid.cwiid_state state)
            {
                Console.WriteLine(state.ToString());
            }

            public static void cwiid_callback(IntPtr wiimote, int mesg_count, cwiid.cwiid_mesg[] mesg_array, ref cwiid.timespec timestamp)
            {
                for (int i = 0; i < mesg_count; i++)
                {
                    cwiid.cwiid_mesg message = mesg_array[i];
                    // Console.WriteLine(message.type.ToString() +"\n");
                    switch (message.type)
                    {
                        case cwiid.cwiid_mesg_type.CWIID_MESG_STATUS:
                            Console.Write("Status Report: battery={0} extension=",
                                          message.status_mesg.battery.ToString());
                            switch (message.status_mesg.ext_type)
                            {
                                case cwiid.cwiid_ext_type.CWIID_EXT_NONE:
                                    Console.Write("none");
                                    break;

                                case cwiid.cwiid_ext_type.CWIID_EXT_NUNCHUK:
                                    Console.Write("Nunchuk");
                                    break;

                                case cwiid.cwiid_ext_type.CWIID_EXT_CLASSIC:
                                    Console.Write("Classic Controller");
                                    break;

                                default:
                                    Console.Write("Unknown Extension");
                                    break;
                            }
                            Console.Write("\n");
                            break;

                        case cwiid.cwiid_mesg_type.CWIID_MESG_BTN:
                            Console.WriteLine("Buttons Report: " + message.btn_mesg.buttons.ToString());
                            break;

                        case cwiid.cwiid_mesg_type.CWIID_MESG_ACC:
                            Console.Write("Acc Report: x={0}, y={1}, z={2}\n",
                                          message.acc_mesg.accX.ToString(),
                                          message.acc_mesg.accY,
                                          message.acc_mesg.accZ);
                            break;

                        case cwiid.cwiid_mesg_type.CWIID_MESG_IR:
                            Console.Write("IR Report: ");
                            bool validSource = false;
                            for (int j = 0; j < cwiid.CWIID_IR_SRC_COUNT; j++)
                            {
                                cwiid.cwiid_ir_src irSrc = message.ir_mesg.src.GetValue(j);
                                if (irSrc.valid > 0)
                                {
                                    validSource = true;
                                    Console.Write("(" + irSrc.posX.ToString() + "," + irSrc.posY.ToString() + ")");
                                }
                            }
                            if (!validSource)
                            {
                                Console.Write("no sources detected");
                            }
                            Console.Write("\n");
                            break;

                        case cwiid.cwiid_mesg_type.CWIID_MESG_NUNCHUK:
                            Console.Write("Nunchuk Report: btns={0} stick=({1},{2}) acc.x={3} acc.y={4} " +
                                          "acc.z={5}\n", message.nunchuk_mesg.buttons.ToString(), message.nunchuk_mesg.stick[0].ToString(),
                                          message.nunchuk_mesg.stick[1].ToString(), message.nunchuk_mesg.accX.ToString(), message.nunchuk_mesg.accY.ToString(),
                                          message.nunchuk_mesg.accZ.ToString());
                            break;

                        case cwiid.cwiid_mesg_type.CWIID_MESG_CLASSIC:
                            Console.Write("Classic Report: btns={0} l_stick=({1},{2}) r_stick=({3},{4}) " +
                                          "l={5} r={5}\n", message.classic_mesg.buttons.ToString(), message.classic_mesg.l_stick[0].ToString(),
                                                     message.classic_mesg.l_stick[1].ToString(), message.classic_mesg.r_stick[0].ToString(), message.classic_mesg.r_stick[1].ToString(),
                                                      message.classic_mesg.l.ToString(), message.classic_mesg.r.ToString());
                            break;

                        case cwiid.cwiid_mesg_type.CWIID_MESG_ERROR:
                            if (cwiid.cwiid_close(wiimote) > 0)
                            {
                                Console.Write("Error on wiimote disconnect\n");
                                System.Environment.Exit(-1);
                            }
                            System.Environment.Exit(0);
                            break;

                        case cwiid.cwiid_mesg_type.CWIID_MESG_UNKNOWN:
                            Console.WriteLine("Unknown Report");
                            break;

                        default:
                            break;
                    }
                }
            }
        }
    }
}