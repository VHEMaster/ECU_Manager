using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Framework.Protocol;

namespace ECU_Framework.Packets
{
    public struct PK_ParametersRequest
    {
        public ushort PacketID;
        public ushort PacketLength;
        public PK_ParametersRequest(int dummy)
        {
            PacketID = (byte)Packets.ParametersRequestID;
            
            
            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
