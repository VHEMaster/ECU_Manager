using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Framework.Protocol;
using ECU_Framework.Structs;

namespace ECU_Framework.Packets
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PK_DragPointResponse
    {
        public ushort PacketID;
        public ushort PacketLength;

        public uint ErrorCode;
        public float FromSpeed;
        public float ToSpeed;
        public uint PointNumber;

        public DragPoint Point;

        public PK_DragPointResponse(int dummy)
        {
            PacketID = (byte)Packets.DragPointResponseID;
            
            
            ErrorCode = 0;
            FromSpeed = 0;
            ToSpeed = 0;
            PointNumber = 0;

            Point = new DragPoint();

            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
