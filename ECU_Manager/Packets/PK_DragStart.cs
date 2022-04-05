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

        public float FromRPM;
        public float ToRPM;

        public PK_DragStart(int dummy, float fromrpm, float torpm)
        {
            PacketID = (byte)Packets.DragStartID;
            
            FromRPM = fromrpm;
            ToRPM = torpm;

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
