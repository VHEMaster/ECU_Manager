using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
{
    public struct PK_TableMemoryAcknowledge
    {
        public byte PacketID;
        public byte PacketLength;
        public byte Destination;
        public byte Dummy;

        public uint ErrorCode;

        public uint Table;
        public uint TableSize;
        public uint Offset;
        public uint Size;

        public PK_TableMemoryAcknowledge(Channel destination, int tablesize, int table, int offset, int size, int errorcode)
        {
            PacketID = (byte)Packets.TableMemoryAcknowledgeID;
            Destination = (byte)destination;
            Dummy = 0;
            Table = (uint)table;
            TableSize = (uint)tablesize;
            Offset = (uint)offset;
            Size = (uint)size;
            ErrorCode = (uint)errorcode;

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
