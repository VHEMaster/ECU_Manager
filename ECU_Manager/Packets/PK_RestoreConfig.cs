using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
{
    public struct PK_RestoreConfig
    {
        public ushort PacketID;
        public ushort PacketLength;
        public PK_RestoreConfig(int dummy)
        {
            PacketID = (byte)Packets.RestoreConfigID;
            
            
            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
