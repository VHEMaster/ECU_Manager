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
    public struct EcuTable
    {
        public EcuTableTransform transform;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Consts.TABLE_STRING_MAX)]
        public string name;

        public int inj_channel;
        
        public float injector_performance;
        public int is_fuel_phase_by_end;
        public int enrichment_ph_async_enabled;
        public int enrichment_ph_sync_enabled;
        public int enrichment_pp_async_enabled;
        public int enrichment_pp_sync_enabled;
        public float fuel_mass_per_cc;
        public float fuel_afr;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.ECU_CYLINDERS_COUNT)]
        [XmlArray("cylinders")]
        [XmlArrayItem("number")]
        public byte[] cylinders;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_VOLTAGES)]
        [XmlArray("voltages")]
        [XmlArrayItem("voltage")]
        public byte[] voltages;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_PRESSURES_16)]
        [XmlArray("pressures_16")]
        [XmlArrayItem("pressure")]
        public ushort[] pressures_16;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_PRESSURES_32)]
        [XmlArray("pressures_32")]
        [XmlArrayItem("pressure")]
        public ushort[] pressures_32;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("rotates_16")]
        [XmlArrayItem("rotate")]
        public ushort[] rotates_16;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_32)]
        [XmlArray("rotates_32")]
        [XmlArrayItem("rotate")]
        public ushort[] rotates_32;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("idle_rotates")]
        [XmlArrayItem("idle_rotate")]
        public ushort[] idle_rotates;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_THROTTLES_16)]
        [XmlArray("throttles_16")]
        [XmlArrayItem("throttle")]
        public ushort[] throttles_16;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_THROTTLES_32)]
        [XmlArray("throttles_32")]
        [XmlArrayItem("throttle")]
        public ushort[] throttles_32;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_PEDALS)]
        [XmlArray("pedals")]
        [XmlArrayItem("pedal")]
        public ushort[] pedals;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_PRESSURES_32 * Consts.TABLE_ROTATES_32)]
        [XmlArray("filling_gbc_map")]
        [XmlArrayItem("filling")]
        public ushort[] filling_gbc_map;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_THROTTLES_32 * Consts.TABLE_ROTATES_32)]
        [XmlArray("filling_gbc_tps")]
        [XmlArrayItem("filling")]
        public ushort[] filling_gbc_tps;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16 * Consts.TABLE_PEDALS)]
        [XmlArray("throttle_position")]
        [XmlArrayItem("throttle")]
        public ushort[] throttle_position;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_PEDALS)]
        [XmlArray("stop_throttle_position")]
        [XmlArrayItem("throttle")]
        public ushort[] stop_throttle_position;

        public int idle_valve_pos_min;
        public int idle_valve_pos_max;
        public float idle_throttle_pos_min;
        public float idle_throttle_pos_max;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("filling_select_koff_tps")]
        [XmlArrayItem("rotate")]
        public byte[] filling_select_koff_tps;

        public int enrichment_load_type;
        public float enrichment_load_dead_band;
        public float enrichment_ign_corr_decay_time;
        public float enrichment_detect_duration;

        public int enrichment_async_pulses_divider;
        public float enrichment_injection_phase_decay_time;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("enrichment_injection_phase")]
        [XmlArrayItem("phase")]
        public byte[] enrichment_injection_phase;

        public float enrichment_end_injection_final_phase;
        public int enrichment_ph_post_injection_enabled;
        public int enrichment_pp_post_injection_enabled;
        public float enrichment_end_injection_final_amount;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ENRICHMENT_PERCENTS)]
        [XmlArray("enrichment_rate_start_load")]
        [XmlArrayItem("percentage")]
        public byte[] enrichment_rate_start_load;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ENRICHMENT_PERCENTS)]
        [XmlArray("enrichment_rate_load_derivative")]
        [XmlArrayItem("percentage")]
        public byte[] enrichment_rate_load_derivative;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ENRICHMENT_PERCENTS * Consts.TABLE_ENRICHMENT_PERCENTS)]
        [XmlArray("enrichment_rate")]
        [XmlArrayItem("rate")]
        public byte[] enrichment_rate;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("enrichment_sync_amount")]
        [XmlArrayItem("amount")]
        public byte[] enrichment_sync_amount;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("enrichment_async_amount")]
        [XmlArrayItem("amount")]
        public byte[] enrichment_async_amount;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16 * Consts.TABLE_ENRICHMENT_PERCENTS)]
        [XmlArray("enrichment_ign_corr")]
        [XmlArrayItem("corr")]
        public byte[] enrichment_ign_corr;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("enrichment_temp_mult")]
        [XmlArrayItem("mult")]
        public byte[] enrichment_temp_mult;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_16)]
        [XmlArray("fillings_16")]
        [XmlArrayItem("filling")]
        public byte[] fillings_16;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_32)]
        [XmlArray("fillings_32")]
        [XmlArrayItem("filling")]
        public byte[] fillings_32;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_32 * Consts.TABLE_ROTATES_32)]
        [XmlArray("ignitions")]
        [XmlArrayItem("angle")]
        public byte[] ignitions;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_32 * Consts.TABLE_ROTATES_32)]
        [XmlArray("ignition_corr_cy1")]
        [XmlArrayItem("angle")]
        public sbyte[] ignition_corr_cy1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_32 * Consts.TABLE_ROTATES_32)]
        [XmlArray("ignition_corr_cy2")]
        [XmlArrayItem("angle")]
        public sbyte[] ignition_corr_cy2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_32 * Consts.TABLE_ROTATES_32)]
        [XmlArray("ignition_corr_cy3")]
        [XmlArrayItem("angle")]
        public sbyte[] ignition_corr_cy3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_32 * Consts.TABLE_ROTATES_32)]
        [XmlArray("ignition_corr_cy4")]
        [XmlArrayItem("angle")]
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

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_16 * Consts.TABLE_ROTATES_16)]
        [XmlArray("fuel_mixtures")]
        [XmlArrayItem("mixture")]
        public byte[] fuel_mixtures;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_16 * Consts.TABLE_ROTATES_16)]
        [XmlArray("injection_phase")]
        [XmlArrayItem("phase")]
        public byte[] injection_phase;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("ignition_time_rpm_mult")]
        [XmlArrayItem("multiplier")]
        public byte[] ignition_time_rpm_mult;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_VOLTAGES)]
        [XmlArray("ignition_time")]
        [XmlArrayItem("time")]
        public byte[] ignition_time;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_VOLTAGES)]
        [XmlArray("injector_lag")]
        [XmlArrayItem("time")]
        public byte[] injector_lag;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("engine_temps")]
        [XmlArrayItem("temperature")]
        public sbyte[] engine_temps;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("air_temps")]
        [XmlArrayItem("temperature")]
        public sbyte[] air_temps;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES * Consts.TABLE_FILLING_16)]
        [XmlArray("air_temp_mix_corr")]
        [XmlArrayItem("mixcorr")]
        public sbyte[] air_temp_mix_corr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES * Consts.TABLE_FILLING_16)]
        [XmlArray("air_temp_ign_corr")]
        [XmlArrayItem("igncorr")]
        public sbyte[] air_temp_ign_corr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES * Consts.TABLE_FILLING_16)]
        [XmlArray("engine_temp_mix_corr")]
        [XmlArrayItem("mixcorr")]
        public sbyte[] engine_temp_mix_corr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES * Consts.TABLE_FILLING_16)]
        [XmlArray("engine_temp_ign_corr")]
        [XmlArrayItem("igncorr")]
        public sbyte[] engine_temp_ign_corr;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("idle_wish_rotates")]
        [XmlArrayItem("rotate")]
        public byte[] idle_wish_rotates;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("idle_wish_massair")]
        [XmlArrayItem("massair")]
        public byte[] idle_wish_massair;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("idle_wish_ignition")]
        [XmlArrayItem("angle")]
        public byte[] idle_wish_ignition;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("idle_wish_ignition_static")]
        [XmlArrayItem("angle")]
        public byte[] idle_wish_ignition_static;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("idle_valve_position")]
        [XmlArrayItem("valve")]
        public byte[] idle_valve_position;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("idle_throttle_position")]
        [XmlArrayItem("throttle")]
        public byte[] idle_throttle_position;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("idle_rpm_pid_act_1")]
        [XmlArrayItem("koff")]
        public byte[] idle_rpm_pid_act_1;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("idle_rpm_pid_act_2")]
        [XmlArrayItem("koff")]
        public byte[] idle_rpm_pid_act_2;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("idle_pids_rpm_koffs")]
        [XmlArrayItem("koff")]
        public byte[] idle_pids_rpm_koffs;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("idle_valve_to_massair_pid_p")]
        [XmlArrayItem("koff")]
        public short[] idle_valve_to_massair_pid_p;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("idle_valve_to_massair_pid_i")]
        [XmlArrayItem("koff")]
        public short[] idle_valve_to_massair_pid_i;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("idle_valve_to_massair_pid_d")]
        [XmlArrayItem("koff")]
        public short[] idle_valve_to_massair_pid_d;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("idle_valve_to_rpm_pid_p")]
        [XmlArrayItem("koff")]
        public short[] idle_valve_to_rpm_pid_p;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("idle_valve_to_rpm_pid_i")]
        [XmlArrayItem("koff")]
        public short[] idle_valve_to_rpm_pid_i;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("idle_valve_to_rpm_pid_d")]
        [XmlArrayItem("koff")]
        public short[] idle_valve_to_rpm_pid_d;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("idle_throttle_to_massair_pid_p")]
        [XmlArrayItem("koff")]
        public short[] idle_throttle_to_massair_pid_p;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("idle_throttle_to_massair_pid_i")]
        [XmlArrayItem("koff")]
        public short[] idle_throttle_to_massair_pid_i;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("idle_throttle_to_massair_pid_d")]
        [XmlArrayItem("koff")]
        public short[] idle_throttle_to_massair_pid_d;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("idle_throttle_to_rpm_pid_p")]
        [XmlArrayItem("koff")]
        public short[] idle_throttle_to_rpm_pid_p;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("idle_throttle_to_rpm_pid_i")]
        [XmlArrayItem("koff")]
        public short[] idle_throttle_to_rpm_pid_i;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("idle_throttle_to_rpm_pid_d")]
        [XmlArrayItem("koff")]
        public short[] idle_throttle_to_rpm_pid_d;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("idle_ign_to_rpm_pid_p")]
        [XmlArrayItem("koff")]
        public short[] idle_ign_to_rpm_pid_p;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("idle_ign_to_rpm_pid_i")]
        [XmlArrayItem("koff")]
        public short[] idle_ign_to_rpm_pid_i;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("idle_ign_to_rpm_pid_d")]
        [XmlArrayItem("koff")]
        public short[] idle_ign_to_rpm_pid_d;

        public float short_term_corr_pid_p;
        public float short_term_corr_pid_i;
        public float short_term_corr_pid_d;

        public float idle_ign_deviation_max;
        public float idle_ign_deviation_min;

        public float idle_ign_fan_low_corr;
        public float idle_ign_fan_high_corr;

        public float idle_air_fan_low_corr;
        public float idle_air_fan_high_corr;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("warmup_mixtures")]
        [XmlArrayItem("mixture")]
        public byte[] warmup_mixtures;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("warmup_mix_koffs")]
        [XmlArrayItem("koff")]
        public byte[] warmup_mix_koffs;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("warmup_mix_corrs")]
        [XmlArrayItem("corr")]
        public byte[] warmup_mix_corrs;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("cold_start_idle_corrs")]
        [XmlArrayItem("corr")]
        public byte[] cold_start_idle_corrs;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("cold_start_idle_times")]
        [XmlArrayItem("time")]
        public byte[] cold_start_idle_times;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("start_injection_phase")]
        [XmlArrayItem("phase")]
        public byte[] start_injection_phase;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("start_idle_valve_pos")]
        [XmlArrayItem("valve")]
        public byte[] start_idle_valve_pos;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("start_throttle_position")]
        [XmlArrayItem("throttle")]
        public byte[] start_throttle_position;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("start_ignition")]
        [XmlArrayItem("ignition")]
        public byte[] start_ignition;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_THROTTLES_16)]
        [XmlArray("start_tps_corrs")]
        [XmlArrayItem("throttle")]
        public byte[] start_tps_corrs;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("start_async_filling")]
        [XmlArrayItem("filling")]
        public ushort[] start_async_filling;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("start_large_filling")]
        [XmlArrayItem("filling")]
        public ushort[] start_large_filling;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("start_small_filling")]
        [XmlArrayItem("filling")]
        public ushort[] start_small_filling;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("start_filling_time")]
        [XmlArrayItem("time")]
        public byte[] start_filling_time;
        public int start_large_count;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_SPEEDS)]
        [XmlArray("idle_rpm_shift_speeds")]
        [XmlArrayItem("speed")]
        public byte[] idle_rpm_shift_speeds;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_SPEEDS)]
        [XmlArray("idle_rpm_shift")]
        [XmlArrayItem("shift")]
        public byte[] idle_rpm_shift;

        public float knock_ign_corr_max;
        public float knock_inj_corr_max;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_32)]
        [XmlArray("knock_noise_level")]
        [XmlArrayItem("noise")]
        public byte[] knock_noise_level;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_32)]
        [XmlArray("knock_threshold")]
        [XmlArrayItem("threshold")]
        public byte[] knock_threshold;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_16 * Consts.TABLE_ROTATES_16)]
        [XmlArray("knock_zone")]
        [XmlArrayItem("multiplier")]
        public byte[] knock_zone;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_32)]
        [XmlArray("knock_gain")]
        [XmlArrayItem("frequency")]
        public byte[] knock_gain;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_32)]
        [XmlArray("knock_filter_frequency")]
        [XmlArrayItem("frequency")]
        public byte[] knock_filter_frequency;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.ECU_CYLINDERS_COUNT * Consts.TABLE_ROTATES_32)]
        [XmlArray("knock_cy_level_multiplier")]
        [XmlArrayItem("multiplier")]
        public byte[] knock_cy_level_multiplier;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.ECU_CYLINDERS_COUNT)]
        [XmlArray("cy_corr_injection")]
        [XmlArrayItem("corr")]
        public float[] cy_corr_injection;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.ECU_CYLINDERS_COUNT)]
        [XmlArray("cy_corr_ignition")]
        [XmlArrayItem("corr")]
        public float[] cy_corr_ignition;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_32)]
        [XmlArray("tsps_relative_pos")]
        [XmlArrayItem("angle")]
        public sbyte[] tsps_relative_pos;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("tsps_desync_thr")]
        [XmlArrayItem("angle")]
        public byte[] tsps_desync_thr;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_THROTTLES_32)]
        [XmlArray("idle_ignition_time_by_tps")]
        [XmlArrayItem("time")]
        public byte[] idle_ignition_time_by_tps;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("idle_econ_delay")]
        [XmlArrayItem("time")]
        public byte[] idle_econ_delay;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("start_econ_delay")]
        [XmlArrayItem("time")]
        public byte[] start_econ_delay;

        public float fan_advance_control_low;
        public float fan_advance_control_mid;
        public float fan_advance_control_high;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES * Consts.TABLE_SPEEDS)]
        [XmlArray("fan_advance_control")]
        [XmlArrayItem("value")]
        public sbyte[] fan_advance_control;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_32)]
        [XmlArray("idle_valve_econ_position")]
        [XmlArrayItem("valve")]
        public byte[] idle_valve_econ_position;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_32)]
        [XmlArray("idle_throttle_econ_position")]
        [XmlArrayItem("throttle")]
        public byte[] idle_throttle_econ_position;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("pedal_ignition_control")]
        [XmlArrayItem("pedal")]
        public byte[] pedal_ignition_control;

        public int throttle_position_use_1d;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_PEDALS)]
        [XmlArray("throttle_position_1d")]
        [XmlArrayItem("throttle")]
        public ushort[] throttle_position_1d;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("throttle_startup_move_time")]
        [XmlArrayItem("time")]
        public byte[] throttle_startup_move_time;


        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_32)]
        [XmlArray("knock_detect_phase_start")]
        [XmlArrayItem("phase")]
        public sbyte[] knock_detect_phase_start;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_32)]
        [XmlArray("knock_detect_phase_end")]
        [XmlArrayItem("phase")]
        public sbyte[] knock_detect_phase_end;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_32)]
        [XmlArray("knock_integrator_time")]
        [XmlArrayItem("time")]
        public byte[] knock_integrator_time;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ENRICHMENT_PERCENTS * Consts.TABLE_ENRICHMENT_PERCENTS)]
        [XmlArray("enrichment_tps_selection")]
        [XmlArrayItem("koff")]
        public byte[] enrichment_tps_selection;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("enrichment_accel_dead_band")]
        [XmlArrayItem("value")]
        public byte[] enrichment_accel_dead_band;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_16)]
        [XmlArray("dynamic_fuel_corr_gbc")]
        [XmlArrayItem("value")]
        public byte[] dynamic_fuel_corr_gbc;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES)]
        [XmlArray("dynamic_fuel_corr_temp")]
        [XmlArrayItem("value")]
        public byte[] dynamic_fuel_corr_temp;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_16)]
        [XmlArray("dynamic_fuel_corr_lpf")]
        [XmlArrayItem("value")]
        public byte[] dynamic_fuel_corr_lpf;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1304)]
        [XmlArray("Reserved")]
        [XmlArrayItem("value")]
        public int[] Reserved;
    }
}
