using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ECU_Framework.Structs
{
    [Serializable]
    public struct EcuCriticalBackup
    {
        public float km_driven;
        public float fuel_consumed;
        public EcuStatus status_recorded;
        public byte idle_valve_position;
    }
}
