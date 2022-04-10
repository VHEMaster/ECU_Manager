using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
{
    public struct PK_Pong
    {
        public ushort PacketID;
        public ushort PacketLength;
        public int RandomPong;
        public PK_Pong(int dummy, int value)
        {
            PacketID = (byte)Packets.PongID;
            
            RandomPong = value;

            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
