using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECU_Manager.Protocol;
using ECU_Manager.Tools;
using ECU_Manager.Packets;

namespace ECU_Manager.Packets
{
    public class PacketHandler
    {
        ProtocolHandler protocolHandler;
        public PacketHandler(ProtocolHandler protocolHandler)
        {
            this.protocolHandler = protocolHandler;
        }

        public object GetPacket(byte[] bytes)
        {
            object result = null;

            Packets packetId = (Packets)bytes[0];
            switch(packetId)
            {
                case Packets.PingID:
                    StructCopy<PK_Ping> pc1 = new StructCopy<PK_Ping>();
                    result = pc1.FromBytes(bytes);
                    break;
                case Packets.PongID:
                    StructCopy<PK_Pong> pc2 = new StructCopy<PK_Pong>();
                    result = pc2.FromBytes(bytes);
                    break;
                case Packets.GeneralStatusResponseID:
                    StructCopy<PK_GeneralStatusResponse> pc4 = new StructCopy<PK_GeneralStatusResponse>();
                    result = pc4.FromBytes(bytes);
                    break;
                case Packets.TableMemoryAcknowledgeID:
                    StructCopy<PK_TableMemoryAcknowledge> pc7 = new StructCopy<PK_TableMemoryAcknowledge>();
                    result = pc7.FromBytes(bytes);
                    break;
                case Packets.TableMemoryDataID:
                    StructCopy<PK_TableMemoryData> pc6 = new StructCopy<PK_TableMemoryData>();
                    result = pc6.FromBytes(bytes);
                    break;
                case Packets.ConfigMemoryAcknowledgeID:
                    StructCopy<PK_ConfigMemoryAcknowledge> pc10 = new StructCopy<PK_ConfigMemoryAcknowledge>();
                    result = pc10.FromBytes(bytes);
                    break;
                case Packets.ConfigMemoryDataID:
                    StructCopy<PK_ConfigMemoryData> pc9 = new StructCopy<PK_ConfigMemoryData>();
                    result = pc9.FromBytes(bytes);
                    break;
                case Packets.SaveConfigAcknowledgeID:
                    StructCopy<PK_SaveConfigAcknowledge> pc13 = new StructCopy<PK_SaveConfigAcknowledge>();
                    result = pc13.FromBytes(bytes);
                    break;
                case Packets.RestoreConfigAcknowledgeID:
                    StructCopy<PK_RestoreConfigAcknowledge> pc14 = new StructCopy<PK_RestoreConfigAcknowledge>();
                    result = pc14.FromBytes(bytes);
                    break;
                case Packets.DragUpdateResponseID:
                    StructCopy<PK_DragUpdateResponse> pc17 = new StructCopy<PK_DragUpdateResponse>();
                    result = pc17.FromBytes(bytes);
                    break;
                case Packets.DragPointResponseID:
                    StructCopy<PK_DragPointResponse> pc20 = new StructCopy<PK_DragPointResponse>();
                    result = pc20.FromBytes(bytes);
                    break;
                case Packets.DragStartAcknowledgeID:
                    StructCopy<PK_DragStartAcknowledge> pc21 = new StructCopy<PK_DragStartAcknowledge>();
                    result = pc21.FromBytes(bytes);
                    break;
                case Packets.DragStopAcknowledgeID:
                    StructCopy<PK_DragStopAcknowledge> pc22 = new StructCopy<PK_DragStopAcknowledge>();
                    result = pc22.FromBytes(bytes);
                    break;
                default:
                    break;
            }

            return result;
        }

        public void SendPing(Channel dest, int value = 0)
        {
            PK_Ping ping = new PK_Ping(dest, value);
            StructCopy<PK_Ping> StructCopy = new StructCopy<PK_Ping>();
            byte[] bytes = StructCopy.GetBytes(ping);
            protocolHandler.Send(dest, bytes);
        }

        public void SendPong(Channel dest, int value)
        {
            PK_Pong packet = new PK_Pong(dest, value);
            StructCopy<PK_Pong> StructCopy = new StructCopy<PK_Pong>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(dest, bytes);
        }

        public void SendGeneralStatusRequest()
        {
            PK_GeneralStatusRequest packet = new PK_GeneralStatusRequest(Channel.etrECU);
            StructCopy<PK_GeneralStatusRequest> StructCopy = new StructCopy<PK_GeneralStatusRequest>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendPcConnected()
        {
            PK_PcConnected packet = new PK_PcConnected(Channel.etrCTRL);
            StructCopy<PK_PcConnected> StructCopy = new StructCopy<PK_PcConnected>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrCTRL, bytes);
        }

        public void SendConfigRequest(int size, int offset, int stepsize)
        {
            PK_ConfigMemoryRequest packet = new PK_ConfigMemoryRequest(Channel.etrECU, size, offset, stepsize);
            StructCopy<PK_ConfigMemoryRequest> StructCopy = new StructCopy<PK_ConfigMemoryRequest>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendConfigData(int size, int offset, int stepsize, byte[] data)
        {
            PK_ConfigMemoryData packet = new PK_ConfigMemoryData(Channel.etrECU, size, offset, stepsize, data);
            StructCopy<PK_ConfigMemoryData> StructCopy = new StructCopy<PK_ConfigMemoryData>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendTableData(int table, int size, int offset, int stepsize, byte[] data)
        {
            PK_TableMemoryData packet = new PK_TableMemoryData(Channel.etrECU, size, table, offset, stepsize, data);
            StructCopy<PK_TableMemoryData> StructCopy = new StructCopy<PK_TableMemoryData>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendConfigAcknowledge(int size, int offset, int stepsize, int errorcode)
        {
            PK_ConfigMemoryAcknowledge packet = new PK_ConfigMemoryAcknowledge(Channel.etrECU, size, offset, stepsize, errorcode);
            StructCopy<PK_ConfigMemoryAcknowledge> StructCopy = new StructCopy<PK_ConfigMemoryAcknowledge>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendTableRequest(int table, int size, int offset, int stepsize)
        {
            PK_TableMemoryRequest packet = new PK_TableMemoryRequest(Channel.etrECU, size, table, offset, stepsize);
            StructCopy<PK_TableMemoryRequest> StructCopy = new StructCopy<PK_TableMemoryRequest>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendTableAcknowledge(int table, int size, int offset, int stepsize, int errorcode)
        {
            PK_TableMemoryAcknowledge packet = new PK_TableMemoryAcknowledge(Channel.etrECU, size, table, offset, stepsize, errorcode);
            StructCopy<PK_TableMemoryAcknowledge> StructCopy = new StructCopy<PK_TableMemoryAcknowledge>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendSaveRequest()
        {
            PK_SaveConfig packet = new PK_SaveConfig(Channel.etrECU);
            StructCopy<PK_SaveConfig> StructCopy = new StructCopy<PK_SaveConfig>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendRestoreRequest()
        {
            PK_RestoreConfig packet = new PK_RestoreConfig(Channel.etrECU);
            StructCopy<PK_RestoreConfig> StructCopy = new StructCopy<PK_RestoreConfig>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendDragStartRequest(float fromrpm, float torpm)
        {
            PK_DragStart packet = new PK_DragStart(Channel.etrECU, fromrpm, torpm);
            StructCopy<PK_DragStart> StructCopy = new StructCopy<PK_DragStart>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendDragStopRequest(float fromrpm, float torpm)
        {
            PK_DragStop packet = new PK_DragStop(Channel.etrECU, fromrpm, torpm);
            StructCopy<PK_DragStop> StructCopy = new StructCopy<PK_DragStop>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendDragUpdateRequest()
        {
            PK_DragUpdateRequest packet = new PK_DragUpdateRequest(Channel.etrECU);
            StructCopy<PK_DragUpdateRequest> StructCopy = new StructCopy<PK_DragUpdateRequest>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendDragPointRequest(float fromrpm, float torpm, int point)
        {
            PK_DragPointRequest packet = new PK_DragPointRequest(Channel.etrECU, fromrpm, torpm, point);
            StructCopy<PK_DragPointRequest> StructCopy = new StructCopy<PK_DragPointRequest>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

    }
}
