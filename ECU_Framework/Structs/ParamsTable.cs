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
    public struct ParamsTable
    {
        public float engineVolume;

        public int isForceTable;
        public int isSwitchByExternal;
        public int startupTableNumber;
        public int switchPos1Table;
        public int switchPos0Table;
        public int switchPos2Table;
        public int forceTable;

        public int cutoffMode;
        public float cutoffRPM;
        public float cutoffAdvance;
        public float cutoffMixture;
        public float oilPressureCutoffRPM;

        public float speedInputCorrection;
        public float speedOutputCorrection;

        public int useLambdaSensor;
        public int res1;
        public int phasedMode;
        public int useKnockSensor;
        public int performAdaptation;
        public int isSingleCoil;
        public int isIndividualCoils;
        public int isEconEnabled;

        public float fanHighTemperature;
        public float fanMidTemperature;
        public float fanLowTemperature;

        public int isBluetoothEnabled;
        public int bluetoothPin;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Consts.TABLE_STRING_MAX)]
        public string bluetoothName;

        public int shiftMode;
        public float shiftThrThr;
        public float shiftRpmThr;
        public float shiftRpmTill;
        public float shiftAdvance;
        public float shiftMixture;

        public int useIdleValve;
        public int useETC;
        public float res2;

        public int useShortTermCorr;
        public int useLongTermCorr;

        public int res3;
        public int performIdleAdaptation;
        public float learn_cycles_delay_mult;
        public float air_temp_corr_koff_min;
        public float air_temp_corr_koff_max;

        public float tps_voltage_low;
        public float tps_voltage_high;

        public float map_pressure_gain;
        public float map_pressure_offset;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 970)]
        [XmlArray("reserveds")]
        [XmlArrayItem("reserved")]
        public int[] Reserved;
    }
}
