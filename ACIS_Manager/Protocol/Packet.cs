using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACIS_Manager.Protocol
{
    public enum Channel
    {
        etrNone = 0,
        etrPC,
        etrACIS,
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
