using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Framework.Protocol;

namespace ECU_Framework.Packets
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PK_RestoreConfig
    {
        public ushort PacketID;
        public ushort PacketLength;
        public PK_RestoreConfig(int dummy)
        {
            PacketID = (byte)Packets.RestoreConfigID;
            
            
            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
