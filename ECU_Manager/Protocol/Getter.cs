﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;

namespace ECU_Manager.Protocol
{
    public class Getter
    {
        private SerialPort sp;
        private Sender sender;
        private Queue<byte> rxfifo;
        private Thread rxthread;
        private Mutex fifomutex;
        public Getter(SerialPort sp, Sender sender)
        {
            this.sender = sender;
            this.sp = sp;
            rxfifo = new Queue<byte>(20480);
            fifomutex = new Mutex();
            rxthread = new Thread(RxThread);
            rxthread.Name = "RX Thread";
            rxthread.IsBackground = true;
            rxthread.Priority = ThreadPriority.Highest;
            rxthread.Start();
        }

        private void RxThread()
        {
            while (true)
            {
                try
                {
                    while (true)
                    {
                        if (sp.IsOpen)
                        {
                            if (sp.BytesToRead > 0)
                            {
                                int bytestoread = sp.BytesToRead;
                                byte[] data = new byte[bytestoread];
                                sp.Read(data, 0, bytestoread);
                                fifomutex.WaitOne();
                                foreach (byte b in data)
                                    rxfifo.Enqueue(b);
                                fifomutex.ReleaseMutex();
                            }
                            else
                            {
                                byte data = (byte)sp.ReadByte();
                                fifomutex.WaitOne();
                                rxfifo.Enqueue(data);
                                fifomutex.ReleaseMutex();
                            }

                        }
                    }
                }
                catch { }
            }
        }

        private byte PeekElement(Queue<byte> queue, int index)
        {
            byte result;
            while (queue.Count < index + 1) Thread.Sleep(1);
            fifomutex.WaitOne();
            result = rxfifo.ElementAt(index);
            fifomutex.ReleaseMutex();
            return result;


        }

        private int wrongheadercounter = 0;
        public Packet Get(Channel source = Channel.etrNone)
        {
            Packet packet = new Packet();
            byte[] header = new byte[8];
            byte[] crc16 = new byte[2];
            byte[] data;

            while(true)
            {
                for (int i = 0; i < 8; i++)
                    header[i] = PeekElement(rxfifo, i);

                if (header[0] != 0x55)
                {
                    if(wrongheadercounter++ == 0)
                        Console.WriteLine("Received packet with wrong header preamble.");
                    rxfifo.Dequeue();
                    continue;
                }
                wrongheadercounter = 0;

                packet.Source = (Channel)header[1];
                packet.Destination = (Channel)header[2];
                packet.Length = (ushort)((header[4] << 8) | header[3]);
                packet.PacketId = (ushort)((header[6] << 8) | header[5]);
                packet.Message = null;

                ushort crc_header = Crc.crc_8(header, 7);
                if (crc_header != header[7])
                {
                    Console.WriteLine("Received packet with wrong header CRC.");
                    rxfifo.Dequeue();
                    continue;
                }

                if (packet.Destination >= Channel.etrCount)
                {
                    Console.WriteLine("Received packet invalid Destination.");
                    rxfifo.Dequeue();
                    continue;
                }

                if (packet.Source >= Channel.etrCount)
                {
                    Console.WriteLine("Received packet invalid Source.");
                    rxfifo.Dequeue();
                    continue;
                }

                if (packet.Length > 8)
                {
                    if (packet.Length < 768)
                    {
                        data = new byte[packet.Length - 10];

                        for (int i = 0; i < packet.Length - 10; i++)
                            data[i] = PeekElement(rxfifo, i + 8);
                        for (int i = 0; i < 2; i++)
                            crc16[i] = PeekElement(rxfifo, packet.Length - 2 + i);

                        ushort crc_calc = Crc.crc_16(header.Concat(data).ToArray(), packet.Length - 2);
                        ushort crc_got = (ushort)((crc16[1] << 8) | crc16[0]);
                        if (crc_calc != crc_got)
                        {
                            Console.WriteLine("Received packet with wrong data CRC.");
                            rxfifo.Dequeue();
                            continue;
                        }

                        packet.Message = data;
                    }
                    else
                    {
                        Console.WriteLine("Received packet too long.");
                        rxfifo.Dequeue();
                        continue;
                    }
                }
                else if(packet.Length < 8)
                {
                    Console.WriteLine("Received packet too short.");
                    rxfifo.Dequeue();
                    continue;
                }

                for (int i = 0; i < packet.Length; i++)
                    rxfifo.Dequeue();

                if (packet.Destination != Channel.etrPC)
                {
                    Console.WriteLine("Received packet with different destination.");
                    continue;
                }
                break;
            }

            return packet;
        }
    }
}
