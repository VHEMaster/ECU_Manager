using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ACIS_Manager.Protocol;

namespace ACIS_Manager.Packets
{
    public struct PK_DragStart
    {
        public byte PacketID;
        public byte PacketLength;
        public byte Destination;
        public byte Dummy;

        public float FromRPM;
        public float ToRPM;

        public PK_DragStart(Channel destination, float fromrpm, float torpm)
        {
            PacketID = (byte)Packets.DragStartID;
            Destination = (byte)destination;
            Dummy = 0;
            FromRPM = fromrpm;
            ToRPM = torpm;

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
