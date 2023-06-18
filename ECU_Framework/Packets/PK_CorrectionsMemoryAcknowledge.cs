
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Framework.Protocol;

namespace ECU_Framework.Packets
{
    public struct PK_CorrectionsMemoryAcknowledge
    {
        public ushort PacketID;
        public ushort PacketLength;

        public uint ErrorCode;

        public uint CorrectionsSize;
        public uint Offset;
        public uint Size;

        public PK_CorrectionsMemoryAcknowledge(int dummy, int Correctionssize, int offset, int size, int errorcode)
        {
            PacketID = (byte)Packets.CorrectionsMemoryAcknowledgeID;
            
            CorrectionsSize = (uint)Correctionssize;
            Offset = (uint)offset;
            Size = (uint)size;
            ErrorCode = (uint)errorcode;

            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
