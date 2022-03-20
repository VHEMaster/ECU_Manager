using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
{
    public struct PK_RestoreConfigAcknowledge
    {
        public byte PacketID;
        public byte PacketLength;
        public byte Destination;
        public byte Dummy;
        public uint ErrorCode;
        public PK_RestoreConfigAcknowledge(Channel destination)
        {
            PacketID = (byte)Packets.RestoreConfigAcknowledgeID;
            Destination = (byte)destination;
            Dummy = 0;
            ErrorCode = 0;

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
