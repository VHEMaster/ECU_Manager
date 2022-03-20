using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECU_Manager.Protocol
{
    public static class Crc
    {
        public static byte crc_8(byte[] src, int size)
        {
            ushort crc = crc_16(src, size);
            return (byte)((crc & 0xFF) ^ (crc >> 8));
        }
        public static ushort crc_16(byte[] buf, int len)
        {
            ushort crc = 0xFFFF;
            for (int pos = 0; pos < len; pos++)
            {
                crc ^= buf[pos];
                for (int i = 8; i != 0; i--)
                {    
                    if ((crc & 0x0001) != 0)
                    {          
                        crc >>= 1; 
                        crc ^= 0xA001;
                    }
                    else crc >>= 1;   
                }
            }
            return crc;
        }
    }
}
