using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Framework.Protocol;

namespace ECU_Framework.Packets
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
