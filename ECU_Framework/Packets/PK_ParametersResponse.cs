using ECU_Framework.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ECU_Framework.Structs;

namespace ECU_Framework.Packets
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
