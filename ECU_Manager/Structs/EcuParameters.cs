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
        public float KnockSensorDetonate;
        public float KnockZone;
        public float KnockAdvance;
        public int KnockCount;
        public float AirTemp;
        public float EngineTemp;
        public float ManifoldAirPressure;
        public float ThrottlePosition;
        public float ReferenceVoltage;
        public float PowerVoltage;
        public float FuelRatio;
        public float FuelRatioDiff;
        public float LambdaValue;
        public float LambdaTemperature;
        public float LambdaHeaterVoltage;
        public float LambdaTemperatureVoltage;
        public float ShortTermCorrection;
        public float LongTermCorrection;
        public float IdleCorrection;

        public int IdleFlag;
        public int IdleCorrFlag;
        public int IdleEconFlag;
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
        public float IdleRegThrRPM;
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
        public float FuelHourly;
        public float TspsRelativePosition;
        public float IdleWishToRpmRelation;

        public int LambdaValid;

        public int OilSensor;
        public int FanForceSwitch;
        public int HandbrakeSensor;
        public int ChargeSensor;
        public int ClutchSensor;
        public int IgnSensor;

        public int FuelPumpRelay;
        public int FanRelay;
        public int CheckEngine;
        public int StarterRelay;
        public int FanSwitch;
        public int IgnOutput;

        public int StartAllowed;
        public int IsRunning;
        public int IsCheckEngine;
    }
}
