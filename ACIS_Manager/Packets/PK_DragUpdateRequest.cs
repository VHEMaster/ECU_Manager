using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ACIS_Manager.Protocol;

namespace ACIS_Manager.Packets
{
    public struct PK_DragUpdateRequest
    {
        public byte PacketID;
        public byte PacketLength;
        public byte Destination;
        public byte Dummy;

        public PK_DragUpdateRequest(Channel destination)
        {
            PacketID = (byte)Packets.DragUpdateRequestID;
            Destination = (byte)destination;
            Dummy = 0;

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
