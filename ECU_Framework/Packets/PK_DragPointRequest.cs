using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Framework.Protocol;

namespace ECU_Framework.Packets
{
    public struct PK_DragPointRequest
    {
        public ushort PacketID;
        public ushort PacketLength;

        public float FromSpeed;
        public float ToSpeed;
        public uint PointNumber;

        public PK_DragPointRequest(int dummy, float fromspeed, float tospeed, int pointnumber)
        {
            PacketID = (byte)Packets.DragPointRequestID;
            
            FromSpeed = fromspeed;
            ToSpeed = tospeed;
            PointNumber = (uint)pointnumber;

            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
