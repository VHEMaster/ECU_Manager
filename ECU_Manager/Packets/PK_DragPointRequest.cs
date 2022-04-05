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

        public uint PointNumber;
        public float FromRPM;
        public float ToRPM;

        public PK_DragPointRequest(int dummy, float fromrpm, float torpm, int pointnumber)
        {
            PacketID = (byte)Packets.DragPointRequestID;
            
            FromRPM = fromrpm;
            ToRPM = torpm;
            PointNumber = (uint)pointnumber;

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
