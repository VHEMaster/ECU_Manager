using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Framework.Protocol;

namespace ECU_Framework.Packets
{
    public struct PK_DragStop
    {
        public ushort PacketID;
        public ushort PacketLength;

        public float FromSpeed;
        public float ToSpeed;

        public PK_DragStop(int dummy, float fromspeed, float tospeed)
        {
            PacketID = (byte)Packets.DragStopID;
            
            FromSpeed = fromspeed;
            ToSpeed = tospeed;

            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
