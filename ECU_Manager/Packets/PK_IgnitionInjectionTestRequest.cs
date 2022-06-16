using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Manager.Protocol;

namespace ECU_Manager.Packets
{
    public struct PK_IgnitionInjectionTestRequest
    {
        public ushort PacketID;
        public ushort PacketLength;

        public byte IgnitionEnabled;
        public byte InjectionEnabled;
        public ushort Count;
        public uint Period;
        public uint IgnitionPulse;
        public uint InjectionPulse;
        public PK_IgnitionInjectionTestRequest(int _, bool ignitionEnabled, bool injectionEnabled, int count, int period, int ignitionPulse, int injectionPulse)
        {
            PacketID = (byte)Packets.IgnitionInjectionTestRequestID;

            IgnitionEnabled = (byte)(ignitionEnabled ? 1 : 0);
            InjectionEnabled = (byte)(injectionEnabled ? 1 : 0);
            Count = (ushort)count;
            Period = (uint)period;
            IgnitionPulse = (uint)ignitionPulse;
            InjectionPulse = (uint)injectionPulse;
            
            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
