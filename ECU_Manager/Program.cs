using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using ECU_Manager.Packets;
using ECU_Manager.Protocol;

namespace ECU_Manager
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {

            /*
            ProtocolHandler protocolHandler = new ProtocolHandler("COM5", packetReceivedEvent);
            while(true)
            {
                Thread.Sleep(100);
                PK_PcConnected pcConnected = new PK_PcConnected(Channel.etrCTRL);
                StructCopy<PK_PcConnected> StructCopy = new StructCopy<PK_PcConnected>();
                byte[] bytes = StructCopy.GetBytes(pcConnected);
                protocolHandler.Send(Channel.etrCTRL, bytes);

            }
            */

            CultureInfo customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = customCulture;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ComPortSelector());
        }
    }
}
