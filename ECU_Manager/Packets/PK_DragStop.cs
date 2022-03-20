using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
{
    public struct PK_DragStop
    {
        public byte PacketID;
        public byte PacketLength;
        public byte Destination;
        public byte Dummy;

        public float FromRPM;
        public float ToRPM;

        public PK_DragStop(Channel destination, float fromrpm, float torpm)
        {
            PacketID = (byte)Packets.DragStopID;
            Destination = (byte)destination;
            Dummy = 0;
            FromRPM = fromrpm;
            ToRPM = torpm;

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
