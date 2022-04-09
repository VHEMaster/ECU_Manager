using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
{
    public struct PK_DragStart
    {
        public ushort PacketID;
        public ushort PacketLength;

        public float FromSpeed;
        public float ToSpeed;

        public PK_DragStart(int dummy, float fromspeed, float tospeed)
        {
            PacketID = (byte)Packets.DragStartID;
            
            FromSpeed = fromspeed;
            ToSpeed = tospeed;

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
