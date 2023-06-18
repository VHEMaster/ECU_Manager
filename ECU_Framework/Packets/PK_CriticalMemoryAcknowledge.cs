
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Framework.Protocol;

namespace ECU_Framework.Packets
{
    public struct PK_CriticalMemoryAcknowledge
    {
        public ushort PacketID;
        public ushort PacketLength;

        public uint ErrorCode;

        public uint CriticalSize;
        public uint Offset;
        public uint Size;

        public PK_CriticalMemoryAcknowledge(int dummy, int Criticalsize, int offset, int size, int errorcode)
        {
            PacketID = (byte)Packets.CriticalMemoryAcknowledgeID;
            
            CriticalSize = (uint)Criticalsize;
            Offset = (uint)offset;
            Size = (uint)size;
            ErrorCode = (uint)errorcode;

            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
