﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
{
    public struct PK_DragStopAcknowledge
    {
        public ushort PacketID;
        public ushort PacketLength;
        public byte ErrorCode;

        public float FromRPM;
        public float ToRPM;

        public PK_DragStopAcknowledge(int dummy)
        {
            PacketID = (byte)Packets.DragStopAcknowledgeID;
            
            FromRPM = 0;
            ToRPM = 0;
            ErrorCode = 0;

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
