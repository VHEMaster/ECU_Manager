using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
{
    public struct PK_DragStartAcknowledge
    {
        public byte PacketID;
        public byte PacketLength;
        public byte Destination;
        public byte Dummy;
        public byte ErrorCode;

        public float FromRPM;
        public float ToRPM;

        public PK_DragStartAcknowledge(Channel destination)
        {
            PacketID = (byte)Packets.DragStartAcknowledgeID;
            Destination = (byte)destination;
            Dummy = 0;
            FromRPM = 0;
            ToRPM = 0;
            ErrorCode = 0;

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
