using ECU_Framework.Packets;
using ECU_Framework.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECU_Framework
{
    public interface IEcuEventHandler
    {
        void SynchronizedEvent(int errorCode, bool fast);
        void UpdateParametersEvent(EcuParameters parameters);
        void UpdateDragStatusEvent(PK_DragUpdateResponse dur);
        void UpdateDragPointEvent(PK_DragPointResponse dpr);
        void DragStartAckEvent(PK_DragStartAcknowledge dsaa);
        void DragStopAckEvent(PK_DragStopAcknowledge dsta);
        void UpdateStatusEvent(IEnumerable<CheckDataItem> checkDataList);
    }
}
