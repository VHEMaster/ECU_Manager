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
        public float cutoffAngle;
        public float cutoffMixture;
        public float oilPressureCutoffRPM;

        public float speedCorrection;

        public int useLambdaSensor;
        public int isLambdaForceEnabled;
        public int useTSPS;
        public int useKnockSensor;
        public int performAdaptation;
        public int isSingleCoil;
        public int isIndividualCoils;

        public float fanHighTemperature;
        public float fanLowTemperature;

        public int isBluetoothEnabled;
        public int bluetoothPin;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Consts.TABLE_STRING_MAX)]
        public string bluetoothName;

        public int shiftMode;
        public float shiftThrThr;
        public float shiftRpmThr;
        public float shiftRpmTill;
        public float shiftAngle;
        public float shiftMixture;

        public float tspsRelPos;
        public float tspsDesyncThr;

        public int useShortTermCorr;
        public int useLongTermCorr;
       
        public int knockIntegratorTime;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 982)]
        [XmlArray("reserveds")]
        [XmlArrayItem("reserved")]
        public int[] Reserved;
    }
}
