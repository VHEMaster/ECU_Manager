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
    [StructLayout(LayoutKind.Sequential)]
    public struct EcuCorrections
    {
        public EcuCorrectionsTransform transform;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_32 * Consts.TABLE_ROTATES_32)]
        [XmlArray("ignitions")]
        [XmlArrayItem("angle")]
        public sbyte[] ignitions;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_PRESSURES_32 * Consts.TABLE_ROTATES_32)]
        [XmlArray("filling_gbc_map")]
        [XmlArrayItem("filling")]
        public sbyte[] filling_gbc_map;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_THROTTLES_32 * Consts.TABLE_ROTATES_32)]
        [XmlArray("filling_gbc_tps")]
        [XmlArrayItem("filling")]
        public sbyte[] filling_gbc_tps;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("idle_valve_position")]
        [XmlArrayItem("value")]
        public sbyte[] idle_valve_position;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.ECU_CYLINDERS_COUNT * Consts.TABLE_ROTATES_32)]
        [XmlArray("knock_cy_level_multiplier")]
        [XmlArrayItem("value")]
        public sbyte[] knock_cy_level_multiplier;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_32 * Consts.TABLE_ROTATES_32)]
        [XmlArray("ignition_corr_cy1")]
        [XmlArrayItem("value")]
        public sbyte[] ignition_corr_cy1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_32 * Consts.TABLE_ROTATES_32)]
        [XmlArray("ignition_corr_cy2")]
        [XmlArrayItem("value")]
        public sbyte[] ignition_corr_cy2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_32 * Consts.TABLE_ROTATES_32)]
        [XmlArray("ignition_corr_cy3")]
        [XmlArrayItem("value")]
        public sbyte[] ignition_corr_cy3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_32 * Consts.TABLE_ROTATES_32)]
        [XmlArray("ignition_corr_cy4")]
        [XmlArrayItem("value")]
        public sbyte[] ignition_corr_cy4;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_32 * Consts.TABLE_ROTATES_32)]
        [XmlArray("injection_corr_cy1")]
        [XmlArrayItem("value")]
        public sbyte[] injection_corr_cy1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_32 * Consts.TABLE_ROTATES_32)]
        [XmlArray("injection_corr_cy2")]
        [XmlArrayItem("value")]
        public sbyte[] injection_corr_cy2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_32 * Consts.TABLE_ROTATES_32)]
        [XmlArray("injection_corr_cy3")]
        [XmlArrayItem("value")]
        public sbyte[] injection_corr_cy3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_32 * Consts.TABLE_ROTATES_32)]
        [XmlArray("injection_corr_cy4")]
        [XmlArrayItem("value")]
        public sbyte[] injection_corr_cy4;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_32 * Consts.TABLE_ROTATES_32)]
        [XmlArray("knock_detonation_counter")]
        [XmlArrayItem("count")]
        public sbyte[] knock_detonation_counter;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_32 * Consts.TABLE_ROTATES_32)]
        [XmlArray("progress_ignitions")]
        [XmlArrayItem("angle")]
        public byte[] progress_ignitions;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_PRESSURES_32 * Consts.TABLE_ROTATES_32)]
        [XmlArray("progress_filling_gbc_map")]
        [XmlArrayItem("value")]
        public byte[] progress_filling_gbc_map;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_THROTTLES_32 * Consts.TABLE_ROTATES_32)]
        [XmlArray("progress_filling_gbc_tps")]
        [XmlArrayItem("value")]
        public byte[] progress_filling_gbc_tps;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("progress_idle_valve_position")]
        [XmlArrayItem("value")]
        public byte[] progress_idle_valve_position;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.ECU_CYLINDERS_COUNT * Consts.TABLE_ROTATES_32)]
        [XmlArray("progress_knock_cy_level_multiplier")]
        [XmlArrayItem("value")]
        public byte[] progress_knock_cy_level_multiplier;

        public float long_term_correction;
        public float idle_correction;
    }
}
