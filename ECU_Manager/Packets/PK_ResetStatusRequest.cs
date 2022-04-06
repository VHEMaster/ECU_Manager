using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
{
    public struct PK_ResetStatusRequest
    {
        public ushort PacketID;
        public ushort PacketLength;
        public PK_ResetStatusRequest(int dummy)
        {
            PacketID = (byte)Packets.ResetStatusRequestID;
            
            
            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
