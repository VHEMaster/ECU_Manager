using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECU_Manager.Protocol
{
    public enum Channel
    {
        etrNone = 0,
        etrPC,
        etrECU,
        etrCTRL,
        etrCount,
    };

    public class Packet
    {
        public byte[] Message;
        public Channel Source;
        public Channel Destination;
        public ushort PacketId;
        public ushort Length;
    }
}
