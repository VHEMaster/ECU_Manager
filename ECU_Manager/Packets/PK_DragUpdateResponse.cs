using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;
using ECU_Manager.Structs;

namespace ECU_Manager.Packets
{
    public struct PK_DragUpdateResponse
    {
        public ushort PacketID;
        public ushort PacketLength;

        public byte ErrorCode;

        public float FromSpeed;
        public float ToSpeed;

        public DragPoint Point;

        public uint TotalPoints;

        public byte Started;
        public byte Completed;

        public PK_DragUpdateResponse(int dummy)
        {
            PacketID = (byte)Packets.DragUpdateResponseID;
            
            FromSpeed = 0;
            ToSpeed = 0;
            ErrorCode = 0;

            Point = new DragPoint();
            TotalPoints = 0;
            Started = 0;
            Completed = 0;

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
