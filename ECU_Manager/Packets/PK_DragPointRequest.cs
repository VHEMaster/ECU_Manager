using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
{
    public struct PK_DragPointRequest
    {
        public byte PacketID;
        public byte PacketLength;
        public byte Destination;
        public byte Dummy;

        public uint PointNumber;
        public float FromRPM;
        public float ToRPM;

        public PK_DragPointRequest(Channel destination, float fromrpm, float torpm, int pointnumber)
        {
            PacketID = (byte)Packets.DragPointRequestID;
            Destination = (byte)destination;
            Dummy = 0;
            FromRPM = fromrpm;
            ToRPM = torpm;
            PointNumber = (uint)pointnumber;

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
