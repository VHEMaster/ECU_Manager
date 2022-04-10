using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;
using ECU_Manager.Structs;

namespace ECU_Manager.Packets
{
    public struct PK_ConfigMemoryData
    {
        public ushort PacketID;
        public ushort PacketLength;

        public uint ErrorCode;
        
        public uint ConfigSize;
        public uint Offset;
        public uint Size;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.PACKET_CONFIG_MAX_SIZE)]
        public byte[] Data;

        public PK_ConfigMemoryData(int dummy, int configsize, int offset, int size, byte[] data)
        {
            PacketID = (byte)Packets.ConfigMemoryDataID;
            
            ConfigSize = (uint)configsize;
            Offset = (uint)offset;
            Size = (uint)size;
            ErrorCode = 0;
            Data = new byte[Consts.PACKET_CONFIG_MAX_SIZE];

            for (int i = 0; i < size; i++)
                Data[i] = data[i];

            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
