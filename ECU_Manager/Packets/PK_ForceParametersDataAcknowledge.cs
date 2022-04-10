using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
{
    public struct PK_ForceParametersDataAcknowledge
    {
        public ushort PacketID;
        public ushort PacketLength;

        public int ErrorCode;
        public PK_ForceParametersDataAcknowledge(int dummy)
        {
            PacketID = (byte)Packets.ForceParametersDataAcknowledgeID;

            ErrorCode = 0;

            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
