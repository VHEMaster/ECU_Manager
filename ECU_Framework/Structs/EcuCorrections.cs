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

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.ECU_CYLINDERS_COUNT * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("knock_cy_level_multiplier")]
        [XmlArrayItem("value")]
        public float[] knock_cy_level_multiplier;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("knock_detonation_counter")]
        [XmlArrayItem("count")]
        public float[] knock_detonation_counter;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_PRESSURES_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("idle_filling_by_map")]
        [XmlArrayItem("filling")]
        public float[] idle_filling_by_map;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("progress_ignitions")]
        [XmlArrayItem("angle")]
        public byte[] progress_ignitions;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_PRESSURES_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("progress_fill_by_map")]
        [XmlArrayItem("value")]
        public byte[] progress_fill_by_map;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_THROTTLES_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("progress_map_by_thr")]
        [XmlArrayItem("value")]
        public byte[] progress_map_by_thr;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("progress_idle_valve_to_rpm")]
        [XmlArrayItem("value")]
        public byte[] progress_idle_valve_to_rpm;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.ECU_CYLINDERS_COUNT * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("progress_knock_cy_level_multiplier")]
        [XmlArrayItem("value")]
        public byte[] progress_knock_cy_level_multiplier;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_PRESSURES_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("progress_idle_filling_by_map")]
        [XmlArrayItem("value")]
        public byte[] progress_idle_filling_by_map;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.ECU_CYLINDERS_COUNT)]
        [XmlArray("knock_ignition_correctives")]
        [XmlArrayItem("value")]
        public float[] knock_ignition_correctives;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.ECU_CYLINDERS_COUNT)]
        [XmlArray("knock_injection_correctives")]
        [XmlArrayItem("value")]
        public float[] knock_injection_correctives;

        public float long_term_correction;
        public float idle_correction;
    }
}
