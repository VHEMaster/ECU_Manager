using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
{
    public struct PK_TableMemoryRequest
    {
        public ushort PacketID;
        public ushort PacketLength;

        public uint Table;
        public uint TableSize;
        public uint Offset;
        public uint Size;

        public PK_TableMemoryRequest(int dummy, int tablesize, int table, int offset, int size)
        {
            PacketID = (byte)Packets.TableMemoryRequestID;
            
            Table = (uint)table;
            TableSize = (uint)tablesize;
            Offset = (uint)offset;
            Size = (uint)size;

            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
