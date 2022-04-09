using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
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
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
