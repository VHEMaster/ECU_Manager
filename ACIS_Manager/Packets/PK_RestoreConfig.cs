﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ACIS_Manager.Protocol;

namespace ACIS_Manager.Packets
{
    public struct PK_RestoreConfig
    {
        public byte PacketID;
        public byte PacketLength;
        public byte Destination;
        public byte Dummy;
        public PK_RestoreConfig(Channel destination)
        {
            PacketID = (byte)Packets.RestoreConfigID;
            Destination = (byte)destination;
            Dummy = 0;

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }

    }
}
