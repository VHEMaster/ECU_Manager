using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ECU_Manager.Structs
{
    [Serializable]
    public struct EcuCriticalBackup
    {
        public float km_driven;
        public float fuel_consumed;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        [XmlArray("status_recorded")]
        [XmlArrayItem("byte")]

        public byte[] status_recorded;
        public byte idle_valve_position;
    }
}
