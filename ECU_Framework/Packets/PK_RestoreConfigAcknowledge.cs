using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Framework.Protocol;

namespace ECU_Framework.Packets
{
    public struct PK_RestoreConfigAcknowledge
    {
        public ushort PacketID;
        public ushort PacketLength;
        public uint ErrorCode;
        public PK_RestoreConfigAcknowledge(int dummy)
        {
            PacketID = (byte)Packets.RestoreConfigAcknowledgeID;
            
            ErrorCode = 0;

            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
