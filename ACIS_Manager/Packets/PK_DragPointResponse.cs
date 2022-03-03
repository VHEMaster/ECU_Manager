using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ACIS_Manager.Protocol;

namespace ACIS_Manager.Packets
{
    public struct PK_DragPointResponse
    {
        public byte PacketID;
        public byte PacketLength;
        public byte Destination;
        public byte Dummy;
        
        public uint ErrorCode;
        public float FromRPM;
        public float ToRPM;
        public uint PointNumber;

        public float RPM;
        public float Pressure;
        public float Load;
        public float Ignition;
        public uint Time;

        public PK_DragPointResponse(Channel destination)
        {
            PacketID = (byte)Packets.DragPointResponseID;
            Destination = (byte)destination;
            Dummy = 0;

            ErrorCode = 0;
            FromRPM = 0;
            ToRPM = 0;
            PointNumber = 0;

            RPM = 0;
            Pressure = 0;
            Load = 0;
            Ignition = 0;
            Time = 0;

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
