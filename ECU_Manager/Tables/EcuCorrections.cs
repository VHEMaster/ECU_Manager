using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ECU_Manager.Tables
{
    [Serializable]
    public struct EcuCorrections
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("ignitions")]
        [XmlArrayItem("angle")]
        public float[] ignitions;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_PRESSURES_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("fill_by_map")]
        [XmlArrayItem("filling")]
        public float[] fill_by_map;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_THROTTLES_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("map_by_thr")]
        [XmlArrayItem("pressure")]
        public float[] map_by_thr;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("idle_valve_to_rpm")]
        [XmlArrayItem("value")]
        public float[] idle_valve_to_rpm;

        public float long_term_correction;
    }
}
