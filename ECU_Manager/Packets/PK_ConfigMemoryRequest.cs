using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
{
    public struct PK_ConfigMemoryRequest
    {
        public byte PacketID;
        public byte PacketLength;
        public byte Destination;
        public byte Dummy;
        
        public uint ConfigSize;
        public uint Offset;
        public uint Size;

        public PK_ConfigMemoryRequest(Channel destination, int configsize, int offset, int size)
        {
            PacketID = (byte)Packets.ConfigMemoryRequestID;
            Destination = (byte)destination;
            Dummy = 0;
            ConfigSize = (uint)configsize;
            Offset = (uint)offset;
            Size = (uint)size;

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
