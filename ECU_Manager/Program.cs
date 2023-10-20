using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using ECU_Framework.Packets;
using ECU_Framework.Protocol;

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
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ComPortSelector());
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            string exString = string.Empty;

            exString += $"Unhandled exception in main thread: {e.Exception.Message}\r\n";
            exString += $"Date and time: {DateTime.Now.ToString()}\r\n\r\n";
            exString += $"Stack trace:\r\n{ e.Exception.StackTrace}\r\n";
            MessageBox.Show(exString, "AutoECU Exception", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

            Application.Exit();
        }
    }
}
