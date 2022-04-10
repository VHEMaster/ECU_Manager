using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
{
    public struct PK_ConfigMemoryRequest
    {
        public ushort PacketID;
        public ushort PacketLength;

        public uint ConfigSize;
        public uint Offset;
        public uint Size;

        public PK_ConfigMemoryRequest(int dummy, int configsize, int offset, int size)
        {
            PacketID = (byte)Packets.ConfigMemoryRequestID;
            
            ConfigSize = (uint)configsize;
            Offset = (uint)offset;
            Size = (uint)size;

            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
