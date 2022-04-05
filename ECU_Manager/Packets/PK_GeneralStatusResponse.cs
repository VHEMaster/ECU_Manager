﻿using ECU_Manager.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ECU_Manager.Packets
{
    public struct PK_GeneralStatusResponse
    {
        public ushort PacketID;
        public ushort PacketLength;
        public byte tablenum;
        public byte valvenum;
        public byte check;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string tablename;
        public float RealRPM;
        public float RPM;
        public float Pressure;
        public float Load;
        public float IgnitionAngle;
        public float IgnitionTime;
        public float Voltage;
        public float Temperature;
        public float FuelUsage;
        public PK_GeneralStatusResponse(int dummy)
        {
            PacketID = (byte)Packets.GeneralStatusResponseID;
            
            
            tablenum = 0;
            valvenum = 0;
            check = 0;
            tablename = string.Empty;
            RealRPM = 0;
            RPM = 0;
            Pressure = 0;
            Load = 0;
            IgnitionAngle = 0;
            IgnitionTime = 0;
            Voltage = 0;
            Temperature = 0;
            FuelUsage = 0;

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }
    }
}
