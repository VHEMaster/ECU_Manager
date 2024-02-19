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
    public struct EcuCorrectionsTransform
    {
        public EcuParamTransform ignitions;
        public EcuParamTransform filling_gbc_map;
        public EcuParamTransform filling_gbc_tps;
        public EcuParamTransform idle_valve_position;
        public EcuParamTransform knock_cy_level_multiplier;
        public EcuParamTransform ignition_corr_cy;
        public EcuParamTransform injection_corr_cy;
        public EcuParamTransform knock_detonation_counter;
        public EcuParamTransform progress;
    }
}
