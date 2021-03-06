﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net;
using static RMUD.Core;

namespace RMUD
{
    class Program
    {
        static void Main(string[] args)
        {
            Telnet.TelnetClientSource telnetListener = null;

            if (Core.Start(StartupFlags.NoFlags, "database/", new RuntimeDatabase()))
            {
                telnetListener = new Telnet.TelnetClientSource();
                telnetListener.Port = Core.SettingsObject.TelnetPort;
                telnetListener.Listen();

                while (!Core.ShuttingDown)
                {
                    //Todo: Shutdown server command breaks this loop.
                }

                telnetListener.Shutdown();
            }
            else
            {
                while (true) { }
            }
        }
    }
}
