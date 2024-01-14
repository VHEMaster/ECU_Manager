using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Framework.Protocol;

namespace ECU_Framework.Packets
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PK_ResetStatusRequest
    {
        public ushort PacketID;
        public ushort PacketLength;
        public PK_ResetStatusRequest(int dummy)
        {
            PacketID = (byte)Packets.ResetStatusRequestID;
            
            
            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
