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
    public struct EcuForceParameters
    {
        public float IgnitionAngle;
        public float InjectionPhase;
        public float IgnitionOctane;
        public float IgnitionTime;
        public float InjectionTime;
        public float WishFuelRatio;
        public float WishIdleRPM;
        public float WishIdleValvePosition;
        public float WishIdleIgnitionAngle;
        public float WishIdleMassAirFlow;
        public int FanRelay;
        public int FuelPumpRelay;
        public int CheckEngine;

        public byte EnableIgnitionAngle;
        public byte EnableInjectionPhase;
        public byte EnableIgnitionOctane;
        public byte EnableIgnitionTime;
        public byte EnableInjectionTime;
        public byte EnableWishFuelRatio;
        public byte EnableWishIdleRPM;
        public byte EnableWishIdleValvePosition;
        public byte EnableWishIdleIgnitionAngle;
        public byte EnableWishIdleMassAirFlow;
        public byte EnableFanRelay;
        public byte EnableFuelPumpRelay;
        public byte EnableCheckEngine;

        public uint pad;
    }
}
