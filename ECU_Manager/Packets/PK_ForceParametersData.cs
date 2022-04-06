﻿using ECU_Manager.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ECU_Manager.Structs;

namespace ECU_Manager.Packets
{
    public struct PK_ForceParametersData
    {
        public ushort PacketID;
        public ushort PacketLength;
        public EcuForceParameters ForceParameters;
        public PK_ForceParametersData(int dummy)
        {
            PacketID = (byte)Packets.ForceParametersDataID;

            ForceParameters = new EcuForceParameters();

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }
    }
}
