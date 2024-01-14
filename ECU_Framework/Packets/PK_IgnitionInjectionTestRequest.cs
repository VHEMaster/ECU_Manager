using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Framework.Protocol;

namespace ECU_Framework.Packets
{
    [StructLayout(LayoutKind.Sequential)]
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
        public PK_IgnitionInjectionTestRequest(int _, byte ignitionEnabled, byte injectionEnabled, int count, int period, int ignitionPulse, int injectionPulse)
        {
            PacketID = (byte)Packets.IgnitionInjectionTestRequestID;

            IgnitionEnabled = ignitionEnabled;
            InjectionEnabled = injectionEnabled;
            Count = (ushort)count;
            Period = (uint)period;
            IgnitionPulse = (uint)ignitionPulse;
            InjectionPulse = (uint)injectionPulse;
            
            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
