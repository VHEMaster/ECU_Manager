using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
{
    public struct PK_ParametersRequest
    {
        public ushort PacketID;
        public ushort PacketLength;
        public PK_ParametersRequest(int dummy)
        {
            PacketID = (byte)Packets.ParametersRequestID;
            
            
            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
