using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ACIS_Manager.Protocol;

namespace ACIS_Manager.Packets
{
    public struct PK_DragUpdateResponse
    {
        public byte PacketID;
        public byte PacketLength;
        public byte Destination;
        public byte Dummy;

        public byte ErrorCode;
        public float FromRPM;
        public float ToRPM;

        public float CurrentRPM;
        public float CurrentPressure;
        public float CurrentLoad;
        public float CurrentIgnition;

        public uint Time;
        public uint TotalPoints;

        public byte Started;
        public byte Completed;

        public PK_DragUpdateResponse(Channel destination)
        {
            PacketID = (byte)Packets.DragUpdateResponseID;
            Destination = (byte)destination;
            Dummy = 0;
            FromRPM = 0;
            ToRPM = 0;
            ErrorCode = 0;

            CurrentRPM = 0;
            CurrentPressure = 0;
            CurrentLoad = 0;
            CurrentIgnition = 0;
            Time = 0;
            TotalPoints = 0;
            Started = 0;
            Completed = 0;

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
