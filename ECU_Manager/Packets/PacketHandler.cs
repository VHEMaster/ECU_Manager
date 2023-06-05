using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECU_Manager.Protocol;
using ECU_Manager.Tools;
using ECU_Manager.Packets;
using ECU_Manager.Structs;

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
            try
            {
                Packets packetId = (Packets)bytes[0];
                switch (packetId)
                {
                    case Packets.PingID:
                        StructCopy<PK_Ping> pc1 = new StructCopy<PK_Ping>();
                        result = pc1.FromBytes(bytes);
                        break;
                    case Packets.PongID:
                        StructCopy<PK_Pong> pc2 = new StructCopy<PK_Pong>();
                        result = pc2.FromBytes(bytes);
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
                    case Packets.CorrectionsMemoryDataID:
                        StructCopy<PK_CorrectionsMemoryData> pc26 = new StructCopy<PK_CorrectionsMemoryData>();
                        result = pc26.FromBytes(bytes);
                        break;
                    case Packets.CorrectionsMemoryAcknowledgeID:
                        StructCopy<PK_CorrectionsMemoryAcknowledge> pc27 = new StructCopy<PK_CorrectionsMemoryAcknowledge>();
                        result = pc27.FromBytes(bytes);
                        break;
                    case Packets.CriticalMemoryDataID:
                        StructCopy<PK_CriticalMemoryData> pc29 = new StructCopy<PK_CriticalMemoryData>();
                        result = pc29.FromBytes(bytes);
                        break;
                    case Packets.CriticalMemoryAcknowledgeID:
                        StructCopy<PK_CriticalMemoryAcknowledge> pc30 = new StructCopy<PK_CriticalMemoryAcknowledge>();
                        result = pc30.FromBytes(bytes);
                        break;
                    case Packets.ParametersResponseID:
                        StructCopy<PK_ParametersResponse> pc32 = new StructCopy<PK_ParametersResponse>();
                        result = pc32.FromBytes(bytes);
                        break;
                    case Packets.ForceParametersDataAcknowledgeID:
                        StructCopy<PK_ForceParametersDataAcknowledge> pc34 = new StructCopy<PK_ForceParametersDataAcknowledge>();
                        result = pc34.FromBytes(bytes);
                        break;
                    case Packets.StatusResponseID:
                        StructCopy<PK_StatusResponse> pc36 = new StructCopy<PK_StatusResponse>();
                        result = pc36.FromBytes(bytes);
                        break;
                    case Packets.ResetStatusResponseID:
                        StructCopy<PK_ResetStatusResponse> pc38 = new StructCopy<PK_ResetStatusResponse>();
                        result = pc38.FromBytes(bytes);
                        break;
                    case Packets.IgnitionInjectionTestResponseID:
                        StructCopy<PK_IgnitionInjectionTestResponse> pc40 = new StructCopy<PK_IgnitionInjectionTestResponse>();
                        result = pc40.FromBytes(bytes);
                        break; 
                    case Packets.SpecificParameterArrayConfigureResponseID:
                        StructCopy<PK_SpecificParameterArrayConfigureResponse> pc44 = new StructCopy<PK_SpecificParameterArrayConfigureResponse>();
                        result = pc44.FromBytes(bytes);
                        break;
                    case Packets.SpecificParameterArrayResponseID:
                        StructCopy<PK_SpecificParameterArrayResponse> pc46 = new StructCopy<PK_SpecificParameterArrayResponse>();
                        result = pc46.FromBytes(bytes);
                        break;
                    default:
                        break;
                }
            }
            catch
            {

            }

            return result;
        }

        public void SendPing(Channel dest, int value = 0)
        {
            PK_Ping ping = new PK_Ping(0, value);
            StructCopy<PK_Ping> StructCopy = new StructCopy<PK_Ping>();
            byte[] bytes = StructCopy.GetBytes(ping);
            protocolHandler.Send(dest, bytes);
        }

        public void SendPong(Channel dest, int value)
        {
            PK_Pong packet = new PK_Pong(0, value);
            StructCopy<PK_Pong> StructCopy = new StructCopy<PK_Pong>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(dest, bytes);
        }

        public void SendParametersRequest()
        {
            PK_ParametersRequest packet = new PK_ParametersRequest(0);
            StructCopy<PK_ParametersRequest> StructCopy = new StructCopy<PK_ParametersRequest>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendPcConnected()
        {
            PK_PcConnected packet = new PK_PcConnected(0);
            StructCopy<PK_PcConnected> StructCopy = new StructCopy<PK_PcConnected>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrCTRL, bytes);
        }

        public void SendConfigRequest(int size, int offset, int stepsize)
        {
            PK_ConfigMemoryRequest packet = new PK_ConfigMemoryRequest(0, size, offset, stepsize);
            StructCopy<PK_ConfigMemoryRequest> StructCopy = new StructCopy<PK_ConfigMemoryRequest>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendConfigData(int size, int offset, int stepsize, byte[] data)
        {
            PK_ConfigMemoryData packet = new PK_ConfigMemoryData(0, size, offset, stepsize, data);
            StructCopy<PK_ConfigMemoryData> StructCopy = new StructCopy<PK_ConfigMemoryData>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendCriticalData(int size, int offset, int stepsize, byte[] data)
        {
            PK_CriticalMemoryData packet = new PK_CriticalMemoryData(0, size, offset, stepsize, data);
            StructCopy<PK_CriticalMemoryData> StructCopy = new StructCopy<PK_CriticalMemoryData>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendCriticalRequest(int size, int offset, int stepsize)
        {
            PK_CriticalMemoryRequest packet = new PK_CriticalMemoryRequest(0, size, offset, stepsize);
            StructCopy<PK_CriticalMemoryRequest> StructCopy = new StructCopy<PK_CriticalMemoryRequest>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendCorrectionsRequest(int size, int offset, int stepsize)
        {
            PK_CorrectionsMemoryRequest packet = new PK_CorrectionsMemoryRequest(0, size, offset, stepsize);
            StructCopy<PK_CorrectionsMemoryRequest> StructCopy = new StructCopy<PK_CorrectionsMemoryRequest>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendCorrectionsData(int size, int offset, int stepsize, byte[] data)
        {
            PK_CorrectionsMemoryData packet = new PK_CorrectionsMemoryData(0, size, offset, stepsize, data);
            StructCopy<PK_CorrectionsMemoryData> StructCopy = new StructCopy<PK_CorrectionsMemoryData>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendTableData(int table, int size, int offset, int stepsize, byte[] data)
        {
            PK_TableMemoryData packet = new PK_TableMemoryData(0, size, table, offset, stepsize, data);
            StructCopy<PK_TableMemoryData> StructCopy = new StructCopy<PK_TableMemoryData>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendForceParametersData(EcuForceParameters data)
        {
            PK_ForceParametersData packet = new PK_ForceParametersData(0, data);
            StructCopy<PK_ForceParametersData> StructCopy = new StructCopy<PK_ForceParametersData>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendConfigAcknowledge(int size, int offset, int stepsize, int errorcode)
        {
            PK_ConfigMemoryAcknowledge packet = new PK_ConfigMemoryAcknowledge(0, size, offset, stepsize, errorcode);
            StructCopy<PK_ConfigMemoryAcknowledge> StructCopy = new StructCopy<PK_ConfigMemoryAcknowledge>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendCorrectionsAcknowledge(int size, int offset, int stepsize, int errorcode)
        {
            PK_CorrectionsMemoryAcknowledge packet = new PK_CorrectionsMemoryAcknowledge(0, size, offset, stepsize, errorcode);
            StructCopy<PK_CorrectionsMemoryAcknowledge> StructCopy = new StructCopy<PK_CorrectionsMemoryAcknowledge>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendCriticalAcknowledge(int size, int offset, int stepsize, int errorcode)
        {
            PK_CriticalMemoryAcknowledge packet = new PK_CriticalMemoryAcknowledge(0, size, offset, stepsize, errorcode);
            StructCopy<PK_CriticalMemoryAcknowledge> StructCopy = new StructCopy<PK_CriticalMemoryAcknowledge>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendTableRequest(int table, int size, int offset, int stepsize)
        {
            PK_TableMemoryRequest packet = new PK_TableMemoryRequest(0, size, table, offset, stepsize);
            StructCopy<PK_TableMemoryRequest> StructCopy = new StructCopy<PK_TableMemoryRequest>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendTableAcknowledge(int table, int size, int offset, int stepsize, int errorcode)
        {
            PK_TableMemoryAcknowledge packet = new PK_TableMemoryAcknowledge(0, size, table, offset, stepsize, errorcode);
            StructCopy<PK_TableMemoryAcknowledge> StructCopy = new StructCopy<PK_TableMemoryAcknowledge>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendSaveRequest()
        {
            PK_SaveConfig packet = new PK_SaveConfig(0);
            StructCopy<PK_SaveConfig> StructCopy = new StructCopy<PK_SaveConfig>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendRestoreRequest()
        {
            PK_RestoreConfig packet = new PK_RestoreConfig(0);
            StructCopy<PK_RestoreConfig> StructCopy = new StructCopy<PK_RestoreConfig>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendStatusRequest()
        {
            PK_StatusRequest packet = new PK_StatusRequest(0);
            StructCopy<PK_StatusRequest> StructCopy = new StructCopy<PK_StatusRequest>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendResetStatusRequest()
        {
            PK_ResetStatusRequest packet = new PK_ResetStatusRequest(0);
            StructCopy<PK_ResetStatusRequest> StructCopy = new StructCopy<PK_ResetStatusRequest>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendDragStartRequest(float fromrpm, float torpm)
        {
            PK_DragStart packet = new PK_DragStart(0, fromrpm, torpm);
            StructCopy<PK_DragStart> StructCopy = new StructCopy<PK_DragStart>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendDragStopRequest(float fromrpm, float torpm)
        {
            PK_DragStop packet = new PK_DragStop(0, fromrpm, torpm);
            StructCopy<PK_DragStop> StructCopy = new StructCopy<PK_DragStop>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendDragUpdateRequest()
        {
            PK_DragUpdateRequest packet = new PK_DragUpdateRequest(0);
            StructCopy<PK_DragUpdateRequest> StructCopy = new StructCopy<PK_DragUpdateRequest>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendDragPointRequest(float fromrpm, float torpm, int point)
        {
            PK_DragPointRequest packet = new PK_DragPointRequest(0, fromrpm, torpm, point);
            StructCopy<PK_DragPointRequest> StructCopy = new StructCopy<PK_DragPointRequest>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }

        public void SendIgnitionInjectionTestRequest(byte ignitionEnabled, byte injectionEnabled, int count, int period, int ignitionPulse, int injectionPulse)
        {
            PK_IgnitionInjectionTestRequest packet = new PK_IgnitionInjectionTestRequest(0, ignitionEnabled, injectionEnabled, count, period, ignitionPulse, injectionPulse);
            StructCopy<PK_IgnitionInjectionTestRequest> StructCopy = new StructCopy<PK_IgnitionInjectionTestRequest>();
            byte[] bytes = StructCopy.GetBytes(packet);
            protocolHandler.Send(Channel.etrECU, bytes);
        }


    }
}
