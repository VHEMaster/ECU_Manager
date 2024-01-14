using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Framework.Protocol;

namespace ECU_Framework.Packets
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PK_SaveConfigAcknowledge
    {
        public ushort PacketID;
        public ushort PacketLength;
        public uint ErrorCode;
        public PK_SaveConfigAcknowledge(int dummy)
        {
            PacketID = (byte)Packets.SaveConfigAcknowledgeID;
            
            ErrorCode = 0;

            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
