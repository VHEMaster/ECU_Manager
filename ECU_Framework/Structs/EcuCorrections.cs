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
        [XmlArray("filling_gbc_map")]
        [XmlArrayItem("filling")]
        public float[] filling_gbc_map;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_THROTTLES_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("filling_gbc_tps")]
        [XmlArrayItem("filling")]
        public float[] filling_gbc_tps;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("idle_valve_position")]
        [XmlArrayItem("value")]
        public float[] idle_valve_position;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.ECU_CYLINDERS_COUNT * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("knock_cy_level_multiplier")]
        [XmlArrayItem("value")]
        public float[] knock_cy_level_multiplier;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("ignition_corr_cy1")]
        [XmlArrayItem("value")]
        public float[] ignition_corr_cy1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("ignition_corr_cy2")]
        [XmlArrayItem("value")]
        public float[] ignition_corr_cy2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("ignition_corr_cy3")]
        [XmlArrayItem("value")]
        public float[] ignition_corr_cy3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("ignition_corr_cy4")]
        [XmlArrayItem("value")]
        public float[] ignition_corr_cy4;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("knock_detonation_counter")]
        [XmlArrayItem("count")]
        public float[] knock_detonation_counter;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("progress_ignitions")]
        [XmlArrayItem("angle")]
        public byte[] progress_ignitions;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_PRESSURES_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("progress_filling_gbc_map")]
        [XmlArrayItem("value")]
        public byte[] progress_filling_gbc_map;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_THROTTLES_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("progress_filling_gbc_tps")]
        [XmlArrayItem("value")]
        public byte[] progress_filling_gbc_tps;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("progress_idle_valve_position")]
        [XmlArrayItem("value")]
        public byte[] progress_idle_valve_position;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.ECU_CYLINDERS_COUNT * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("progress_knock_cy_level_multiplier")]
        [XmlArrayItem("value")]
        public byte[] progress_knock_cy_level_multiplier;

        public float long_term_correction;
        public float idle_correction;
    }
}
