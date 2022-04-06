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
    public struct PK_StatusResponse
    {
        public ushort PacketID;
        public ushort PacketLength;
        public EcuStatus CheckBitmap;
        public EcuStatus CheckBitmapRecorded;

        public PK_StatusResponse(int dummy)
        {
            PacketID = (byte)Packets.StatusResponseID;

            CheckBitmap = new EcuStatus();
            CheckBitmapRecorded = new EcuStatus();

            PacketLength = 0;
            PacketLength = (byte)Marshal.SizeOf(GetType());
        }
    }
}
