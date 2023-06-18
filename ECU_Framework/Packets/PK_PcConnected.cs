using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Framework.Protocol;

namespace ECU_Framework.Packets
{
    public struct PK_PcConnected
    {
        public ushort PacketID;
        public ushort PacketLength;
        public PK_PcConnected(int dummy)
        {
            PacketID = (byte)Packets.PcConnectedID;
            
            
            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
