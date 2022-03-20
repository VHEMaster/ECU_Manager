using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace ECU_Manager.Protocol
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
                packet[1] = 0x55;
                packet[2] = (byte)(((byte)source | (((byte)destination & 0x7) << 3)) & 0xFF);
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
                sp.BaseStream.Write(packet, 0, packet.Length);
                sp.BaseStream.Flush();
            }
        }

        public void SendAck(Channel destination, Channel source, ushort packetId)
        {
            lock (this)
            {
                byte[] packet = new byte[8];

                packet[0] = 0x55;
                packet[1] = 0x55;
                packet[2] = (byte)(((byte)source | (((byte)destination & 0x7) << 3)) & 0xFF);
                packet[3] = (byte)(8 & 0xFF);
                packet[4] = (byte)(((8) >> 8) & 0xFF);
                packet[5] = (byte)(packetId & 0xFF);
                packet[6] = (byte)((packetId >> 8) & 0xFF);
                packet[7] = Crc.crc_8(packet, 7);
                sp.BaseStream.Write(packet, 0, packet.Length);
                sp.BaseStream.Flush();
            }
        }
    }
}
