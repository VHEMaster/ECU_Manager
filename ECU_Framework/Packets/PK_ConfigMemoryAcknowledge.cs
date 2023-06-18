
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Framework.Protocol;

namespace ECU_Framework.Packets
{
    public struct PK_ConfigMemoryAcknowledge
    {
        public ushort PacketID;
        public ushort PacketLength;

        public uint ErrorCode;

        public uint ConfigSize;
        public uint Offset;
        public uint Size;

        public PK_ConfigMemoryAcknowledge(int dummy, int configsize, int offset, int size, int errorcode)
        {
            PacketID = (byte)Packets.ConfigMemoryAcknowledgeID;
            
            ConfigSize = (uint)configsize;
            Offset = (uint)offset;
            Size = (uint)size;
            ErrorCode = (uint)errorcode;

            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
