using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
{
    public struct PK_Ping
    {
        public ushort PacketID;
        public ushort PacketLength;
        public int RandomPing;
        public PK_Ping(int dummy, int value = 0)
        {
            PacketID = (byte)Packets.PingID;
            
            RandomPing = value;

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
