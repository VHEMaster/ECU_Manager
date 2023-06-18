using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;

namespace ECU_Framework.Protocol
{
    public class Sender
    {
        private SerialPort sp;
        public Sender(SerialPort sp)
        {
            this.sp = sp;
        }

        public void Send(Channel destination, Channel source, byte[] data, ushort packetId)
        {
            lock (this)
            {
                byte[] packet = new byte[data.Length + 10];

                packet[0] = 0x55;
                packet[1] = (byte)source;
                packet[2] = (byte)destination;
                packet[3] = (byte)((data.Length + 10) & 0xFF);
                packet[4] = (byte)(((data.Length + 10) >> 8) & 0xFF);
                packet[5] = (byte)(packetId & 0xFF);
                packet[6] = (byte)((packetId >> 8) & 0xFF);
                packet[7] = Crc.crc_8(packet, 7);
                for (int i = 0; i < data.Length; i++)
                    packet[i + 8] = data[i];
                ushort crc16 = Crc.crc_16(packet, packet.Length - 2);
                packet[packet.Length - 2] = (byte)(crc16 & 0xFF);
                packet[packet.Length - 1] = (byte)((crc16 >> 8) & 0xFF);
                sp.Write(packet, 0, packet.Length);
            }
        }

        public void SendAck(Channel destination, Channel source, ushort packetId)
        {
            lock (this)
            {
                byte[] packet = new byte[8];
                ushort length = 8;

                packet[0] = 0x55;
                packet[1] = (byte)source;
                packet[2] = (byte)destination;
                packet[3] = (byte)(length & 0xFF);
                packet[4] = (byte)((length >> 8) & 0xFF);
                packet[5] = (byte)(packetId & 0xFF);
                packet[6] = (byte)((packetId >> 8) & 0xFF);
                packet[7] = Crc.crc_8(packet, 7);
                sp.Write(packet, 0, packet.Length);
            }
        }
    }
}
