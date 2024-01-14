using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Framework.Protocol;

namespace ECU_Framework.Packets
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PK_DragStopAcknowledge
    {
        public ushort PacketID;
        public ushort PacketLength;
        public byte ErrorCode;

        public float FromSpeed;
        public float ToSpeed;

        public PK_DragStopAcknowledge(int dummy)
        {
            PacketID = (byte)Packets.DragStopAcknowledgeID;
            
            FromSpeed = 0;
            ToSpeed = 0;
            ErrorCode = 0;

            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
