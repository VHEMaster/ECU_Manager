﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ECU_Manager.Tables
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

        public float cutoffRPM;
        public int cutoffMode;
        public float cutoffAngle;
        public float cutoffMixture;

        public float speedCorrection;

        public int useLambdaSensor;
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

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 995)]
        [XmlArray("reserveds")]
        [XmlArrayItem("reserved")]
        public int[] Reserved;
    }
}
