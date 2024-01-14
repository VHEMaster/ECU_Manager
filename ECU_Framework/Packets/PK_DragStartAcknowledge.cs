using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Framework.Protocol;

namespace ECU_Framework.Packets
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PK_DragStartAcknowledge
    {
        public ushort PacketID;
        public ushort PacketLength;
        public byte ErrorCode;
        
        public float FromSpeed;
        public float ToSpeed;

        public PK_DragStartAcknowledge(int dummy)
        {
            PacketID = (byte)Packets.DragStartAcknowledgeID;
            
            FromSpeed = 0;
            ToSpeed = 0;
            ErrorCode = 0;

            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
