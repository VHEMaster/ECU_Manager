using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
{
    public struct PK_SaveConfig
    {
        public ushort PacketID;
        public ushort PacketLength;
        public PK_SaveConfig(int dummy)
        {
            PacketID = (byte)Packets.SaveConfigID;
            
            
            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
