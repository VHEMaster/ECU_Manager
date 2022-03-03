
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ACIS_Manager.Protocol;

namespace ACIS_Manager.Packets
{
    public struct PK_ConfigMemoryAcknowledge
    {
        public byte PacketID;
        public byte PacketLength;
        public byte Destination;
        public byte Dummy;

        public uint ErrorCode;

        public uint ConfigSize;
        public uint Offset;
        public uint Size;

        public PK_ConfigMemoryAcknowledge(Channel destination, int configsize, int offset, int size, int errorcode)
        {
            PacketID = (byte)Packets.ConfigMemoryAcknowledgeID;
            Destination = (byte)destination;
            Dummy = 0;
            ConfigSize = (uint)configsize;
            Offset = (uint)offset;
            Size = (uint)size;
            ErrorCode = (uint)errorcode;

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
