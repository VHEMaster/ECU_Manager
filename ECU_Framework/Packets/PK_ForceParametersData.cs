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
    public struct PK_ForceParametersData
    {
        public ushort PacketID;
        public ushort PacketLength;
        public EcuForceParameters ForceParameters;
        public PK_ForceParametersData(int dummy, EcuForceParameters parameters)
        {
            PacketID = (byte)Packets.ForceParametersDataID;

            ForceParameters = parameters;

            PacketLength = 0;
            PacketLength = (ushort)Marshal.SizeOf(GetType());
        }
    }
}
