using ECU_Manager.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ECU_Manager.Structs;

namespace ECU_Manager.Packets
{
    public struct PK_ResetStatusResponse
    {
        public ushort PacketID;
        public ushort PacketLength;
        public int ErrorCode;

        public PK_ResetStatusResponse(int dummy)
        {
            PacketID = (byte)Packets.ResetStatusResponseID;

            ErrorCode = 0;

            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }
    }
}
