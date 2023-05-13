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
        
        public float injector_performance;
        public int is_fuel_pressure_const;
        public int is_fuel_phase_by_end;
        public int enrichment_ph_async_enabled;
        public int enrichment_ph_sync_enabled;
        public int enrichment_pp_async_enabled;
        public int enrichment_pp_sync_enabled;
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

        public int idle_rotates_count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("idle_rotates")]
        [XmlArrayItem("idle_rotate")]
        public float[] idle_rotates;

        public int throttles_count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_THROTTLES_MAX)]
        [XmlArray("throttles")]
        [XmlArrayItem("throttle")]
        public float[] throttles;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_PRESSURES_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("fill_by_map")]
        [XmlArrayItem("filling")]
        public float[] fill_by_map;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_THROTTLES_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("map_by_thr")]
        [XmlArrayItem("pressure")]
        public float[] map_by_thr;

        public int enrichment_load_type;
        public float enrichment_load_dead_band;
        public float enrichment_accel_dead_band;
        public float enrichment_ign_corr_decay_time;
        public float enrichment_detect_duration;

        public int enrichment_async_pulses_divider;
        public float enrichment_injection_phase_decay_time;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("enrichment_injection_phase")]
        [XmlArrayItem("phase")]
        public float[] enrichment_injection_phase;

        public float enrichment_end_injection_final_phase;
        public int enrichment_ph_post_injection_enabled;
        public int enrichment_pp_post_injection_enabled;
        public float enrichment_end_injection_final_amount;

        public int enrichment_rate_start_load_count;
        public int enrichment_rate_load_derivative_count;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ENRICHMENT_PERCENTS_MAX)]
        [XmlArray("enrichment_rate_start_load")]
        [XmlArrayItem("percentage")]
        public float[] enrichment_rate_start_load;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ENRICHMENT_PERCENTS_MAX)]
        [XmlArray("enrichment_rate_load_derivative")]
        [XmlArrayItem("percentage")]
        public float[] enrichment_rate_load_derivative;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ENRICHMENT_PERCENTS_MAX * Consts.TABLE_ENRICHMENT_PERCENTS_MAX)]
        [XmlArray("enrichment_rate")]
        [XmlArrayItem("rate")]
        public float[] enrichment_rate;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("enrichment_sync_amount")]
        [XmlArrayItem("amount")]
        public float[] enrichment_sync_amount;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("enrichment_async_amount")]
        [XmlArrayItem("amount")]
        public float[] enrichment_async_amount;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX * Consts.TABLE_ENRICHMENT_PERCENTS_MAX)]
        [XmlArray("enrichment_ign_corr")]
        [XmlArrayItem("corr")]
        public float[] enrichment_ign_corr;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("enrichment_temp_mult")]
        [XmlArrayItem("mult")]
        public float[] enrichment_temp_mult;

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
        [XmlArray("injection_phase_lpf")]
        [XmlArrayItem("lpf")]
        public float[] injection_phase_lpf;

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
        [XmlArrayItem("mixcorr")]
        public float[] air_temp_mix_corr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX * Consts.TABLE_FILLING_MAX)]
        [XmlArray("air_temp_ign_corr")]
        [XmlArrayItem("igncorr")]
        public float[] air_temp_ign_corr;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("idle_wish_rotates")]
        [XmlArrayItem("rotate")]
        public float[] idle_wish_rotates;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("idle_wish_massair")]
        [XmlArrayItem("massair")]
        public float[] idle_wish_massair;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("idle_wish_ignition")]
        [XmlArrayItem("angle")]
        public float[] idle_wish_ignition;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("idle_wish_ignition_static")]
        [XmlArrayItem("angle")]
        public float[] idle_wish_ignition_static;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("idle_valve_to_rpm")]
        [XmlArrayItem("valve")]
        public float[] idle_valve_to_rpm;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("idle_rpm_pid_act_1")]
        [XmlArrayItem("koff")]
        public float[] idle_rpm_pid_act_1;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("idle_rpm_pid_act_2")]
        [XmlArrayItem("koff")]
        public float[] idle_rpm_pid_act_2;

        public int idle_pids_rpm_koffs_count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("idle_pids_rpm_koffs")]
        [XmlArrayItem("koff")]
        public float[] idle_pids_rpm_koffs;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("idle_valve_to_massair_pid_p")]
        [XmlArrayItem("koff")]
        public float[] idle_valve_to_massair_pid_p;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("idle_valve_to_massair_pid_i")]
        [XmlArrayItem("koff")]
        public float[] idle_valve_to_massair_pid_i;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("idle_valve_to_massair_pid_d")]
        [XmlArrayItem("koff")]
        public float[] idle_valve_to_massair_pid_d;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("idle_ign_to_rpm_pid_p")]
        [XmlArrayItem("koff")]
        public float[] idle_ign_to_rpm_pid_p;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("idle_ign_to_rpm_pid_i")]
        [XmlArrayItem("koff")]
        public float[] idle_ign_to_rpm_pid_i;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("idle_ign_to_rpm_pid_d")]
        [XmlArrayItem("koff")]
        public float[] idle_ign_to_rpm_pid_d;

        public float short_term_corr_pid_p;
        public float short_term_corr_pid_i;
        public float short_term_corr_pid_d;

        public float idle_ign_deviation_max;
        public float idle_ign_deviation_min;

        public float idle_ign_fan_low_corr;
        public float idle_ign_fan_high_corr;

        public float idle_air_fan_low_corr;
        public float idle_air_fan_high_corr;

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
        [XmlArray("cold_start_idle_corrs")]
        [XmlArrayItem("corr")]
        public float[] cold_start_idle_corrs;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("cold_start_idle_times")]
        [XmlArrayItem("time")]
        public float[] cold_start_idle_times;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("start_injection_phase")]
        [XmlArrayItem("phase")]
        public float[] start_injection_phase;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("start_idle_valve_pos")]
        [XmlArrayItem("valve")]
        public float[] start_idle_valve_pos;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("start_ignition")]
        [XmlArrayItem("ignition")]
        public float[] start_ignition;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_THROTTLES_MAX)]
        [XmlArray("start_tps_corrs")]
        [XmlArrayItem("throttle")]
        public float[] start_tps_corrs;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("start_async_filling")]
        [XmlArrayItem("filling")]
        public float[] start_async_filling;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("start_large_filling")]
        [XmlArrayItem("filling")]
        public float[] start_large_filling;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("start_small_filling")]
        [XmlArrayItem("filling")]
        public float[] start_small_filling;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("start_filling_time")]
        [XmlArrayItem("time")]
        public float[] start_filling_time;
        public int start_large_count;

        public int idle_speeds_shift_count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_SPEEDS_MAX)]
        [XmlArray("idle_rpm_shift_speeds")]
        [XmlArrayItem("speed")]
        public float[] idle_rpm_shift_speeds;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_SPEEDS_MAX)]
        [XmlArray("idle_rpm_shift")]
        [XmlArrayItem("shift")]
        public float[] idle_rpm_shift;

        public float knock_ign_corr_max;
        public float knock_inj_corr_max;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("knock_noise_level")]
        [XmlArrayItem("noise")]
        public float[] knock_noise_level;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("knock_threshold")]
        [XmlArrayItem("threshold")]
        public float[] knock_threshold;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_FILLING_MAX * Consts.TABLE_ROTATES_MAX)]
        [XmlArray("knock_zone")]
        [XmlArrayItem("multiplier")]
        public float[] knock_zone;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("knock_gain")]
        [XmlArrayItem("frequency")]
        public float[] knock_gain;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("knock_filter_frequency")]
        [XmlArrayItem("frequency")]
        public float[] knock_filter_frequency;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.ECU_CYLINDERS_COUNT)]
        [XmlArray("cy_corr_injection")]
        [XmlArrayItem("corr")]
        public float[] cy_corr_injection;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.ECU_CYLINDERS_COUNT)]
        [XmlArray("cy_corr_ignition")]
        [XmlArrayItem("corr")]
        public float[] cy_corr_ignition;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("tsps_relative_pos")]
        [XmlArrayItem("angle")]
        public float[] tsps_relative_pos;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_ROTATES_MAX)]
        [XmlArray("tsps_desync_thr")]
        [XmlArrayItem("angle")]
        public float[] tsps_desync_thr;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_THROTTLES_MAX)]
        [XmlArray("idle_ignition_time_by_tps")]
        [XmlArrayItem("time")]
        public float[] idle_ignition_time_by_tps;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("idle_econ_delay")]
        [XmlArrayItem("time")]
        public float[] idle_econ_delay;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_TEMPERATURES_MAX)]
        [XmlArray("start_econ_delay")]
        [XmlArrayItem("time")]
        public float[] start_econ_delay;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1700)]
        [XmlArray("Reserved")]
        [XmlArrayItem("value")]
        public int[] Reserved;
    }
}
