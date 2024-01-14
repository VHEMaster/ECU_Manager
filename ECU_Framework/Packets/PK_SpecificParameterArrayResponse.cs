using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Framework.Protocol;
using ECU_Framework.Structs;

namespace ECU_Framework.Packets
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PK_SpecificParameterArrayResponse
    {
        public ushort PacketID;
        public ushort PacketLength;

        public uint ErrorCode;
        public uint Items;
        public uint Underflow;
        public uint Length;
        public uint IsLeft;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.PACKET_SPECIFIC_PARAMETERS_ARRAY_MAX_SIZE)]
        public ParameterUnion[] Parameters;

        public PK_SpecificParameterArrayResponse(int dummy, int errorcode, int items, int underflow, int length, bool isleft, ParameterUnion[] parameters)
        {
            PacketID = (byte)Packets.SpecificParameterArrayResponseID;
            
            PacketLength = 0;
            Parameters = new ParameterUnion[Consts.PACKET_SPECIFIC_PARAMETERS_ARRAY_MAX_SIZE];
            for (int i = 0; i < length; i++)
                Parameters[i] = parameters[i];

            ErrorCode = (uint)errorcode;
            Items = (uint)items;
            Underflow = (uint)underflow;
            Length = (uint)length;
            IsLeft = (uint)(isleft ? 1 : 0);

            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
