using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
{
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
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
