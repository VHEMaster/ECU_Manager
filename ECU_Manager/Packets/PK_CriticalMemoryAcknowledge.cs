
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
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
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
