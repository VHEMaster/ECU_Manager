using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Framework.Protocol;

namespace ECU_Framework.Packets
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PK_DragUpdateRequest
    {
        public ushort PacketID;
        public ushort PacketLength;

        public PK_DragUpdateRequest(int dummy)
        {
            PacketID = (byte)Packets.DragUpdateRequestID;
            
            
            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
