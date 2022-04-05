using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
{
    public struct PK_CorrectionsMemoryRequest
    {
        public ushort PacketID;
        public ushort PacketLength;

        public uint CorrectionsSize;
        public uint Offset;
        public uint Size;

        public PK_CorrectionsMemoryRequest(int dummy, int Correctionssize, int offset, int size)
        {
            PacketID = (byte)Packets.CorrectionsMemoryRequestID;
            
            CorrectionsSize = (uint)Correctionssize;
            Offset = (uint)offset;
            Size = (uint)size;

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
