using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECU_Framework.Packets
{
	public enum Packets
    {
		Invalid = 0,
        PingID = 1,
        PongID = 2,
        GeneralStatusRequestID = 3,
        GeneralStatusResponseID = 4,
        TableMemoryRequestID = 5,
        TableMemoryDataID = 6,
        TableMemoryAcknowledgeID = 7,
        ConfigMemoryRequestID = 8,
        ConfigMemoryDataID = 9,
        ConfigMemoryAcknowledgeID = 10,
        SaveConfigID = 11,
        RestoreConfigID = 12,
        SaveConfigAcknowledgeID = 13,
        RestoreConfigAcknowledgeID = 14,
        DragStartID = 15,
        DragUpdateRequestID = 16,
        DragUpdateResponseID = 17,
        DragStopID = 18,
        DragPointRequestID = 19,
        DragPointResponseID = 20,
        DragStartAcknowledgeID = 21,
        DragStopAcknowledgeID = 22,
        PcConnectedID = 23,
        FuelSwitchID = 24,
        CorrectionsMemoryRequestID = 25,
        CorrectionsMemoryDataID = 26,
        CorrectionsMemoryAcknowledgeID = 27,
        CriticalMemoryRequestID = 28,
        CriticalMemoryDataID = 29,
        CriticalMemoryAcknowledgeID = 30,
        ParametersRequestID = 31,
        ParametersResponseID = 32,
        ForceParametersDataID = 33,
        ForceParametersDataAcknowledgeID = 34,
        StatusRequestID = 35,
        StatusResponseID = 36,
        ResetStatusRequestID = 37,
        ResetStatusResponseID = 38,
        IgnitionInjectionTestRequestID = 39,
        IgnitionInjectionTestResponseID = 40,
        SpecificParameterRequestID = 41,
        SpecificParameterResponseID = 42,
        SpecificParameterArrayConfigureRequestID = 43,
        SpecificParameterArrayConfigureResponseID = 44,
        SpecificParameterArrayRequestID = 45,
        SpecificParameterArrayResponseID = 46,
        EtcTestRequestID = 47,
        EtcTestResponseID = 48,
    }
}
