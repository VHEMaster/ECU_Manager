using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Framework.Protocol;

namespace ECU_Framework.Packets
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PK_TableMemoryAcknowledge
    {
        public ushort PacketID;
        public ushort PacketLength;

        public uint ErrorCode;

        public uint Table;
        public uint TableSize;
        public uint Offset;
        public uint Size;

        public PK_TableMemoryAcknowledge(int dummy, int tablesize, int table, int offset, int size, int errorcode)
        {
            PacketID = (byte)Packets.TableMemoryAcknowledgeID;
            
            Table = (uint)table;
            TableSize = (uint)tablesize;
            Offset = (uint)offset;
            Size = (uint)size;
            ErrorCode = (uint)errorcode;

            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
