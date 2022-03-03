using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ACIS_Manager.Protocol;

namespace ACIS_Manager.Packets
{
    public struct PK_Ping
    {
        public byte PacketID;
        public byte PacketLength;
        public byte Destination;
        public byte Dummy;
        public int RandomPing;
        public PK_Ping(Channel destination, int value = 0)
        {
            PacketID = (byte)Packets.PingID;
            Destination = (byte)destination;
            Dummy = 0;
            RandomPing = value;

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
