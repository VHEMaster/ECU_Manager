using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
{
    public struct PK_GeneralStatusRequest
    {
        public byte PacketID;
        public byte PacketLength;
        public byte Destination;
        public byte Dummy;
        public PK_GeneralStatusRequest(Channel destination)
        {
            PacketID = (byte)Packets.GeneralStatusRequestID;
            Destination = (byte)destination;
            Dummy = 0;

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
