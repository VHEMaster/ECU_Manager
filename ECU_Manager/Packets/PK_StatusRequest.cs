using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
{
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
