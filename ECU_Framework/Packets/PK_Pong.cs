using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Framework.Protocol;

namespace ECU_Framework.Packets
{
    [StructLayout(LayoutKind.Sequential)]
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
