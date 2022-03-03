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
    public struct ParamsTable
    {
        public int tables_count;
        public int isCutoffEnabled;
        public int isTemperatureEnabled;
        public int isEconomEnabled;
        public int isAutostartEnabled;
        public int isIgnitionByHall;
        public int isHallLearningMode;
        public int isSwitchByExternal;
        public int isEconOutAsStrobe;
        public int isForceTable;
        public int forceTableNumber;
        public int switchPos1Table;
        public int switchPos0Table;
        public int switchPos2Table;

        public float EconRpmThreshold;
        public float CutoffRPM;
        public int CutoffMode;
        public float CutoffAngle;
        public int isEconIgnitionOff;
        public int isForceIdle;
        public int engineVolume;
        public int isForceIgnition;
        public int forceIgnitionAngle;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
        [XmlArray("reserveds")]
        [XmlArrayItem("reserved")]
        public int[] Reserved;
    }
}
