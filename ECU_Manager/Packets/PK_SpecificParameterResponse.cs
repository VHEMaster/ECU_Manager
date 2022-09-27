using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;
using ECU_Manager.Structs;

namespace ECU_Manager.Packets
{
    public struct PK_SpecificParameterResponse
    {
        public ushort PacketID;
        public ushort PacketLength;
        public uint Addr;
        public ParameterUnion Parameter;
        public PK_SpecificParameterResponse(int dummy, int addr)
        {
            PacketID = (byte)Packets.SpecificParameterResponseID;

            PacketLength = 0;
            Addr = (uint)addr;
            Parameter = new ParameterUnion();

            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
