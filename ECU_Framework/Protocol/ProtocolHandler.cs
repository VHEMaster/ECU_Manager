using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ECU_Framework.Protocol
{
    public delegate void PacketReceivedEvent(Packet packet);
    public delegate void PacketSentEvent(Packet packet);
    public delegate void PacketTimeoutEvent(Packet packet);

    public class ProtocolHandler
    {
        const int RETRIES_MAX = 20;
        const int RETRY_TIMEOUT = 50;

        SerialPort sp;
        string comPort;
        PacketReceivedEvent receivedEvent;
        PacketSentEvent sentEvent;
        PacketTimeoutEvent timeoutEvent;
        Thread getterThread;
        Thread senderThread;
        Sender sender;
        Getter getter;
        Random random;
        BlockingCollection<Packet> senderQueue;
        BlockingCollection<Packet> feedbackQueue;

        bool bIsAckRequired = false;
        ushort uAckPacketNumber = 0;
        int iAckRetries = 0;
        public ProtocolHandler(string comPort, PacketReceivedEvent receivedEvent, PacketSentEvent sentEvent = null, PacketTimeoutEvent timeoutEvent = null)
        {
            this.receivedEvent = receivedEvent;
            this.timeoutEvent = timeoutEvent;
            this.sentEvent = sentEvent;
            this.comPort = comPort;

            sp = new SerialPort(comPort, 960000, Parity.None, 8, StopBits.One);
            sp.ReadBufferSize = 4096;
            sp.WriteBufferSize = 4096;
            sp.Open();
            sp.DiscardInBuffer();
            sp.DiscardOutBuffer();

            sender = new Sender(sp);
            getter = new Getter(sp, sender);

            senderQueue = new BlockingCollection<Packet>(4);
            feedbackQueue = new BlockingCollection<Packet>(16);
            random = new Random();
              

            getterThread = new Thread(Getter);
            getterThread.Name = "Getter Thread";
            getterThread.IsBackground = true;
            getterThread.Priority = ThreadPriority.Highest;
            senderThread = new Thread(Sender);
            senderThread.Name = "Sender Thread";
            senderThread.IsBackground = true;
            senderThread.Priority = ThreadPriority.AboveNormal;

            senderThread.Start();
            getterThread.Start();
        }

        private void reopen()
        {
            Thread.Sleep(100);
            try
            {
                try
                {
                    sp.Close();
                }
                catch { }
                
                sp.Open();
                sp.DiscardInBuffer();
                sp.DiscardOutBuffer();
            }
            catch (Exception) { }
        }

        public void Send(Channel dest, byte[] data)
        {
            Packet packet = new Packet();
            packet.Destination = dest;
            packet.Source = Channel.etrPC;
            packet.Message = data;
            packet.Length = (ushort)data.Length;
            lock (random)
            {
                packet.PacketId = (ushort)(Math.Abs(random.Next()) % 0xFFFF);
            }
            senderQueue.Add(packet);
        }

        private void Getter()
        {
            ushort[][] packetids = new ushort[4][] { new ushort[10], new ushort[10], new ushort[10], new ushort[10] };
            int[] packetindex = new int[4] { 0,0,0,0 };
            while (true)
            {
                try
                {
                    Packet packet = getter.Get();
                    if(packet.Message == null)
                    {
                        if(bIsAckRequired)
                        {
                            if(uAckPacketNumber == packet.PacketId)
                            {
                                feedbackQueue.Add(packet);
                            }
                            else Console.WriteLine("Received ACK of unexpected packet id");
                        }
                        else Console.WriteLine("Received ACK while not waiting for it");
                    }
                    else
                    {
                        sender.SendAck(packet.Source, Channel.etrPC, packet.PacketId);
                        if (!packetids[(int)packet.Source].Contains(packet.PacketId))
                        {
                            receivedEvent?.BeginInvoke(packet, null, null);
                            packetids[(int)packet.Source][packetindex[(int)packet.Source]] = packet.PacketId;
                            if (++packetindex[(int)packet.Source] >= packetids.Length)
                                packetindex[(int)packet.Source] = 0;
                        }
                    }
                }
                catch (TimeoutException) { }
                catch (InvalidOperationException) { reopen(); }
                catch (IOException) { reopen(); }
            }
        }

        private void Sender()
        {
            while (true)
            {
                try
                {
                    Packet packet = senderQueue.Take();
                    Packet feedback;

                    bIsAckRequired = true;
                    uAckPacketNumber = packet.PacketId;
                    iAckRetries = 0;

                    DateTime now = DateTime.Now;
                    while (true)
                    {
                        sender.Send(packet.Destination, packet.Source, packet.Message, packet.PacketId);
                        if (feedbackQueue.TryTake(out feedback, RETRY_TIMEOUT))
                        {
                            if (feedback.PacketId == packet.PacketId && feedback.Destination == packet.Source && feedback.Source == packet.Destination)
                            {
                                DateTime now2 = DateTime.Now;
                                Console.WriteLine($"Ack time{{{iAckRetries}}}: {(now2 - now).TotalMilliseconds.ToString("F2")}");
                                sentEvent?.BeginInvoke(packet, null, null);
                                break;
                            }
                        }
                        else if (iAckRetries++ >= RETRIES_MAX)
                        {
                            timeoutEvent?.BeginInvoke(packet, null, null);
                            Console.WriteLine("Packet Loss!");
                            break;
                        }
                        Console.WriteLine($"Timeout occured{{{iAckRetries}}}.");
                    }
                }
                catch (TimeoutException) { }
                catch (InvalidOperationException) { reopen(); }
                catch (IOException) { reopen(); }
                bIsAckRequired = false;
            }
        }
    }
}
