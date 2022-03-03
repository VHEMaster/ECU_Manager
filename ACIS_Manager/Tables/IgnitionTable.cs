using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ACIS_Manager.Tables
{
    [Serializable]
    public struct IgnitionTable
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Consts.TABLE_STRING_MAX)]
        public string name;

        public int valve_channel;
        public int valve_timeout;

        public float initial_ignition;
        public float octane_corrector;

        public int idles_count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("idle_ignitions")]
        [XmlArrayItem("angle")]
        public float[] idle_ignitions;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("idle_rotates")]
        [XmlArrayItem("rpm")]
        public float[] idle_rotates;

        public int pressures_count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_PRESSURES_MAX)]
        [XmlArray("pressures")]
        [XmlArrayItem("pressure")]
        public float[] pressures;

        public int rotates_count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("rotates")]
        [XmlArrayItem("rpm")]
        public float[] rotates;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_PRESSURES_MAX*Consts.TABLE_ROTATES_MAX)]
        [XmlArray("ignitions")]
        [XmlArrayItem("angle")]
        public float[] ignitions;

        public int temperatures_count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("temperatures")]
        [XmlArrayItem("temperature")]
        public float[] temperatures;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        public float[] temperature_ignitions;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("servo_accelerations")]
        [XmlArrayItem("value")]
        public float[] servo_acc;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("servo_chokes")]
        [XmlArrayItem("value")]
        public float[] servo_choke;

        public float fuel_rate;
        public float fuel_volume;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 62)]
        [XmlArray("reserveds")]
        [XmlArrayItem("reserved")]
        public int[] Reserved;

    }
}
