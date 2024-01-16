using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Framework.Protocol;

namespace ECU_Framework.Packets
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PK_EtcTestResponse
    {
        public ushort PacketID;
        public ushort PacketLength;
        public uint ErrorCode;
        public PK_EtcTestResponse(int _)
        {
            PacketID = (byte)Packets.EtcTestResponseID;

            ErrorCode = 0;

            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
