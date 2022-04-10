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
    public struct PK_ParametersResponse
    {
        public ushort PacketID;
        public ushort PacketLength;
        public EcuParameters Parameters;
        public PK_ParametersResponse(int dummy)
        {
            PacketID = (byte)Packets.ParametersResponseID;

            Parameters = new EcuParameters();

            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }
    }
}
