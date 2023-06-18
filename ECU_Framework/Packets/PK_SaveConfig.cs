using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Framework.Protocol;

namespace ECU_Framework.Packets
{
    public struct PK_SaveConfig
    {
        public ushort PacketID;
        public ushort PacketLength;
        public PK_SaveConfig(int dummy)
        {
            PacketID = (byte)Packets.SaveConfigID;
            
            
            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
