using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECU_Framework.Packets;
using ECU_Framework.Structs;
using ECU_Framework.Protocol;
using ECU_Framework.Tools;
using System.Threading;
using System.Diagnostics;

namespace ECU_Debugger
{
    class Program
    {
        static ProtocolHandler protocolHandler;
        static PacketHandler packetHandler;
        static bool packetHandled = false;
        static Stopwatch packetTimer = new Stopwatch();

        static void Main(string[] args)
        {
            string comport = args[0];
            byte[] dataBytes;

            protocolHandler = new ProtocolHandler(comport, PacketReceivedEventHandler, null, PacketTimeoutEventHandler);
            packetHandler = new PacketHandler(protocolHandler);

            packetHandled = true;
            packetTimer.Restart();

            while (true)
            {
                while (!packetHandled && packetTimer.ElapsedMilliseconds < 5000) Thread.Sleep(1);
                packetHandled = false;

                PK_ParametersRequest parametersRequest = new PK_ParametersRequest(0);
                dataBytes = new StructCopy<PK_ParametersRequest>().GetBytes(parametersRequest);

                Console.WriteLine("APP: Sending PK_ParametersRequest");
                packetTimer.Restart();
                protocolHandler.Send(Channel.etrECU, dataBytes);
                
            }
        }

        private static void PacketTimeoutEventHandler(Packet packet)
        {
            packetHandled = true;
        }

        private static void PacketReceivedEventHandler(Packet packet)
        {
            object pack = packetHandler.GetPacket(packet.Message);

            if (pack != null)
            {
                Console.WriteLine($"APP: Received in {packetTimer.ElapsedMilliseconds}ms: {pack.GetType().ToString()}");
                packetHandled = true;
            }
        }
    }
}
