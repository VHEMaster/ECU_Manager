using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ACIS_Manager.Protocol;

namespace ACIS_Manager.Packets
{
    public struct PK_Pong
    {
        public byte PacketID;
        public byte PacketLength;
        public byte Destination;
        public byte Dummy;
        public int RandomPong;
        public PK_Pong(Channel destination, int value)
        {
            PacketID = (byte)Packets.PongID;
            Destination = (byte)destination;
            Dummy = 0;
            RandomPong = value;

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
