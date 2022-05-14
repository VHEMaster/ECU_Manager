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
    public struct EcuTable
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Consts.TABLE_STRING_MAX)]
        public string name;

        public int inj_channel;

        public float ignition_initial;
        public float injector_performance;
        public int is_fuel_pressure_const;
        public float fuel_pressure;
        public float fuel_mass_per_cc;
        public float fuel_afr;

        public int voltages_count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_VOLTAGES_MAX)]
        [XmlArray("voltages")]
        [XmlArrayItem("voltage")]
        public float[] voltages;

        public int pressures_count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_PRESSURES_MAX)]
        [XmlArray("pressures")]
        [XmlArrayItem("pressure")]
        public float[] pressures;

        public int rotates_count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("rotates")]
        [XmlArrayItem("rotate")]
        public float[] rotates;

        public int throttles_count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_THROTTLES_MAX)]
        [XmlArray("throttles")]
        [XmlArrayItem("throttle")]
        public float[] throttles;

        public float enrichment_proportion_map_vs_thr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_PRESSURES_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("fill_by_map")]
        [XmlArrayItem("filling")]
        public float[] fill_by_map;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_THROTTLES_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("map_by_thr")]
        [XmlArrayItem("pressure")]
        public float[] map_by_thr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_PRESSURES_MAX)]
        [XmlArray("enrichment_by_map_sens")]
        [XmlArrayItem("enrichemt")]
        public float[] enrichment_by_map_sens;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("enrichment_by_map_hpf")]
        [XmlArrayItem("filter")]
        public float[] enrichment_by_map_hpf;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_THROTTLES_MAX)]
        [XmlArray("enrichment_by_thr_sens")]
        [XmlArrayItem("enrichemt")]
        public float[] enrichment_by_thr_sens;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("enrichment_by_thr_hpf")]
        [XmlArrayItem("filter")]
        public float[] enrichment_by_thr_hpf;

        public int fillings_count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_MAX)]
        [XmlArray("fillings")]
        [XmlArrayItem("filling")]
        public float[] fillings;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("ignitions")]
        [XmlArrayItem("angle")]
        public float[] ignitions;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("fuel_mixtures")]
        [XmlArrayItem("mixture")]
        public float[] fuel_mixtures;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("injection_phase")]
        [XmlArrayItem("phase")]
        public float[] injection_phase;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("ignition_time_rpm_mult")]
        [XmlArrayItem("multiplier")]
        public float[] ignition_time_rpm_mult;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_VOLTAGES_MAX)]
        [XmlArray("ignition_time")]
        [XmlArrayItem("time")]
        public float[] ignition_time;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_VOLTAGES_MAX)]
        [XmlArray("injector_lag")]
        [XmlArrayItem("time")]
        public float[] injector_lag;

        public int engine_temp_count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("engine_temps")]
        [XmlArrayItem("temperature")]
        public float[] engine_temps;
        
        public int air_temp_count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("air_temps")]
        [XmlArrayItem("temperature")]
        public float[] air_temps;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX * Consts.TABLE_FILLING_MAX)]
        [XmlArray("air_temp_mix_corr")]
        [XmlArrayItem("corr")]
        public float[] air_temp_mix_corr;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("idle_wish_rotates")]
        [XmlArrayItem("rotate")]
        public float[] idle_wish_rotates;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("idle_wish_massair")]
        [XmlArrayItem("massair")]
        public float[] idle_wish_massair;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("idle_wish_ignition")]
        [XmlArrayItem("angle")]
        public float[] idle_wish_ignition;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("idle_valve_to_rpm")]
        [XmlArrayItem("valve")]
        public float[] idle_valve_to_rpm;

        public float idle_rpm_pid_act;

        public float idle_valve_to_massair_pid_p;
        public float idle_valve_to_massair_pid_i;
        public float idle_valve_to_massair_pid_d;

        public float idle_ign_to_rpm_pid_p;
        public float idle_ign_to_rpm_pid_i;
        public float idle_ign_to_rpm_pid_d;

        public float short_term_corr_pid_p;
        public float short_term_corr_pid_i;
        public float short_term_corr_pid_d;

        public float idle_ign_deviation_max;
        public float idle_ign_deviation_min;

        public float idle_ign_fan_corr;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("warmup_mixtures")]
        [XmlArrayItem("mixture")]
        public float[] warmup_mixtures;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("warmup_mix_koffs")]
        [XmlArrayItem("koff")]
        public float[] warmup_mix_koffs;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("warmup_mix_corrs")]
        [XmlArrayItem("corr")]
        public float[] warmup_mix_corrs;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("start_mixtures")]
        [XmlArrayItem("mixture")]
        public float[] start_mixtures;

        public int idle_speeds_shift_count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_SPEEDS_MAX)]
        [XmlArray("idle_rpm_shift_speeds")]
        [XmlArrayItem("speed")]
        public float[] idle_rpm_shift_speeds;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_SPEEDS_MAX)]
        [XmlArray("idle_rpm_shift")]
        [XmlArrayItem("shift")]
        public float[] idle_rpm_shift;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("knock_noise_level")]
        [XmlArrayItem("noise")]
        public float[] knock_noise_level;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("knock_threshold")]
        [XmlArrayItem("threshold")]
        public float[] knock_threshold;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.ECU_CYLINDERS_COUNT)]
        [XmlArray("cy_corr_injection")]
        [XmlArrayItem("corr")]
        public float[] cy_corr_injection;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.ECU_CYLINDERS_COUNT)]
        [XmlArray("cy_corr_ignition")]
        [XmlArrayItem("corr")]
        public float[] cy_corr_ignition;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 829)]
        [XmlArray("Reserved")]
        [XmlArrayItem("value")]
        public int[] Reserved;
    }
}
