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
    public struct EcuParameters
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Consts.TABLE_STRING_MAX)]
        public string CurrentTableName;
        public int SwitchPosition;
        public int CurrentTable;
        public int InjectorChannel;

        public float AdcKnockVoltage;
        public float AdcAirTemp;
        public float AdcEngineTemp;
        public float AdcManifoldAirPressure;
        public float AdcThrottlePosition;
        public float AdcPowerVoltage;
        public float AdcReferenceVoltage;
        public float AdcLambdaUR;
        public float AdcLambdaUA;

        public float KnockSensor;
        public float KnockSensorFiltered;
        public float AirTemp;
        public float EngineTemp;
        public float ManifoldAirPressure;
        public float ThrottlePosition;
        public float ReferenceVoltage;
        public float PowerVoltage;
        public float FuelRatio;
        public float LongTermCorrection;
        public float IdleCorrection;

        public int IdleFlag;
        public float RPM;
        public float Speed;
        public float Acceleration;
        public float MassAirFlow;
        public float CyclicAirFlow;
        public float EffectiveVolume;
        public float AirDestiny;
        public float RelativeFilling;
        public float WishFuelRatio;
        public float IdleValvePosition;
        public float WishIdleRPM;
        public float WishIdleMassAirFlow;
        public float WishIdleValvePosition;
        public float WishIdleIgnitionAngle;
        public float IgnitionAngle;
        public float InjectionPhase;
        public float InjectionPhaseDuration;
        public float InjectionPulse;
        public float InjectionDutyCycle;
        public float InjectionEnrichment;
        public float InjectionLag;
        public float IgnitionPulse;
        public float IdleSpeedShift;

        public float DrivenKilometers;
        public float FuelConsumed;
        public float FuelConsumption;

        public int OilSensor;
        public int StarterSensor;
        public int HandbrakeSensor;
        public int ChargeSensor;
        public int ClutchSensor;
        public int IgnSensor;

        public int FuelPumpRelay;
        public int FanRelay;
        public int CheckEngine;
        public int StarterRelay;
        public int Rsvd1Output;
        public int IgnOutput;

        public int StartAllowed;
        public int IsRunning;
        public int IsCheckEngine;
    }
}
