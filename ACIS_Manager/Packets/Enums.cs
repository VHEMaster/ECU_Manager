using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACIS_Manager.Packets
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
    }
}
