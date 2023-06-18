using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Framework.Protocol;

namespace ECU_Framework.Packets
{
    public struct PK_SpecificParameterRequest
    {
        public ushort PacketID;
        public ushort PacketLength;
        public uint Addr;
        public PK_SpecificParameterRequest(int dummy, int addr)
        {
            PacketID = (byte)Packets.SpecificParameterRequestID;
            
            PacketLength = 0;
            Addr = (uint)addr;

            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
