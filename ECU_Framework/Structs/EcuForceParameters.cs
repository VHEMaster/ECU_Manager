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
    public struct EcuForceParameters
    {
        public float IgnitionAdvance;
        public float InjectionPhase;
        public float IgnitionOctane;
        public float IgnitionPulse;
        public float InjectionPulse;
        public float WishFuelRatio;
        public float WishIdleRPM;
        public float WishIdleValvePosition;
        public float WishIdleIgnitionAdvance;
        public float WishIdleMassAirFlow;
        public float WishIdleThrottlePosition;
        public float WishThrottleTargetPosition;
        public float MaximumThrottlePosition;
        public int FanRelay;
        public int FanSwitch;
        public int FuelPumpRelay;
        public int CheckEngine;

        public byte EnableIgnitionAdvance;
        public byte EnableInjectionPhase;
        public byte EnableIgnitionOctane;
        public byte EnableIgnitionPulse;
        public byte EnableInjectionPulse;
        public byte EnableWishFuelRatio;
        public byte EnableWishIdleRPM;
        public byte EnableWishIdleValvePosition;
        public byte EnableWishIdleIgnitionAdvance;
        public byte EnableWishIdleMassAirFlow;
        public byte EnableWishIdleThrottlePosition;
        public byte EnableWishThrottleTargetPosition;
        public byte EnableMaximumThrottlePosition;
        public byte EnableFanRelay;
        public byte EnableFanSwitch;
        public byte EnableFuelPumpRelay;
        public byte EnableCheckEngine;

        public uint pad1;
        public byte LambdaForceEnabled;
        public uint pad2;
    }
}
