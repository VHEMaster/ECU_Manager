using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ECU_Framework.Protocol;

namespace ECU_Framework.Packets
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PK_EtcTestRequest
    {
        public ushort PacketID;
        public ushort PacketLength;

        public float StartPosition;
        public uint StartDelay;
        public float EndPosition;
        public uint MovePeriod;
        public uint FinalDelay;
        public PK_EtcTestRequest(int _, float start_position, int start_delay, float end_position, int move_period, int final_delay)
        {
            PacketID = (byte)Packets.EtcTestRequestID;

            StartPosition = start_position;
            StartDelay = (uint)start_delay;
            EndPosition = end_position;
            MovePeriod = (uint)move_period;
            FinalDelay = (uint)final_delay;

            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }

    }
}
