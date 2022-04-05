using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
{
    public struct PK_CriticalMemoryRequest
    {
        public ushort PacketID;
        public ushort PacketLength;

        public uint CriticalSize;
        public uint Offset;
        public uint Size;

        public PK_CriticalMemoryRequest(int dummy, int Criticalsize, int offset, int size)
        {
            PacketID = (byte)Packets.CriticalMemoryRequestID;
            
            CriticalSize = (uint)Criticalsize;
            Offset = (uint)offset;
            Size = (uint)size;

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
