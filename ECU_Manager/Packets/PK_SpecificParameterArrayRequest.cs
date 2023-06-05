using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
{
    public struct PK_SpecificParameterArrayRequest
    {
        public ushort PacketID;
        public ushort PacketLength;
        public PK_SpecificParameterArrayRequest(int dummy)
        {
            PacketID = (byte)Packets.SpecificParameterArrayRequestID;
            
            
            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
