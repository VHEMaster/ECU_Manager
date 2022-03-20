using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
{
    public struct PK_TableMemoryRequest
    {
        public byte PacketID;
        public byte PacketLength;
        public byte Destination;
        public byte Dummy;

        public uint Table;
        public uint TableSize;
        public uint Offset;
        public uint Size;

        public PK_TableMemoryRequest(Channel destination, int tablesize, int table, int offset, int size)
        {
            PacketID = (byte)Packets.TableMemoryRequestID;
            Destination = (byte)destination;
            Dummy = 0;
            Table = (uint)table;
            TableSize = (uint)tablesize;
            Offset = (uint)offset;
            Size = (uint)size;

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
