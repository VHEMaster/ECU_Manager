using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;
using ECU_Manager.Structs;

namespace ECU_Manager.Packets
{
    public struct PK_SpecificParameterArrayConfigureResponse
    {
        public ushort PacketID;
        public ushort PacketLength;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.SPECIFIC_PARAMETERS_ARRAY_MAX_ITEMS)]
        public uint[] Addrs;

        public uint Period;
        public uint ErrorCode;
        public uint BufferSize;
        public PK_SpecificParameterArrayConfigureResponse(int dummy, int[] addrs, int period, int errorcode, int buffersize)
        {
            PacketID = (byte)Packets.SpecificParameterArrayConfigureResponseID;
            
            PacketLength = 0;
            Addrs = new uint[Consts.SPECIFIC_PARAMETERS_ARRAY_MAX_ITEMS];
            for (int i = 0; i < Consts.SPECIFIC_PARAMETERS_ARRAY_MAX_ITEMS; i++)
                Addrs[i] = (uint)addrs[i];
            Period = (uint)period;
            ErrorCode = (uint)errorcode;
            BufferSize = (uint)buffersize;

            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
