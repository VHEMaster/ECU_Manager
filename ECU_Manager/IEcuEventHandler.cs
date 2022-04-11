using ECU_Manager.Packets;
using ECU_Manager.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECU_Manager
{
    public interface IEcuEventHandler
    {
        void SynchronizedEvent(int errorCode);
        void UpdateParametersEvent(EcuParameters parameters);
        void UpdateDragStatusEvent(PK_DragUpdateResponse dur);
        void UpdateDragPointEvent(PK_DragPointResponse dpr);
        void DragStartAckEvent(PK_DragStartAcknowledge dsaa);
        void DragStopAckEvent(PK_DragStopAcknowledge dsta);
    }
}
