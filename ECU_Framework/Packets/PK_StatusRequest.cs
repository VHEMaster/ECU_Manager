using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Framework.Protocol;

namespace ECU_Framework.Packets
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PK_StatusRequest
    {
        public ushort PacketID;
        public ushort PacketLength;
        public PK_StatusRequest(int dummy)
        {
            PacketID = (byte)Packets.StatusRequestID;
            
            
            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
