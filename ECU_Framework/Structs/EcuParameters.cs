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

        public float AdcEtcTps1;
        public float AdcEtcTps2;
        public float AdcEtcPedal1;
        public float AdcEtcPedal2;
        public float AdcEtcRsvd5;
        public float AdcEtcRsvd6;
        public float AdcEtcReferenceVoltage;
        public float AdcEtcPowerVoltage;

        public float ThrottlePosition;
        public float PedalPosition;
        public float ThrottleDefaultPosition;
        public float ThrottleTargetPosition;
        public float WishThrottleTargetPosition;

        public float KnockSensor;
        public float KnockSensorFiltered;
        public float KnockSensorDetonate;
        public float KnockZone;
        public float KnockAdvance;
        public int KnockCount;
        public int KnockCountCy1;
        public int KnockCountCy2;
        public int KnockCountCy3;
        public int KnockCountCy4;
        public float KnockSaturation;
        public float AirTemp;
        public float EngineTemp;
        public float CalculatedAirTemp;
        public float ManifoldAirPressure;
        public float ReferenceVoltage;
        public float PowerVoltage;
        public float FuelRatio;
        public float FuelRatioDiff;
        public float LambdaValue;
        public float LambdaTemperature;
        public float LambdaHeaterVoltage;
        public float ShortTermCorrection;
        public float LongTermCorrection;
        public float IdleCorrection;

        public int IdleFlag;
        public int IdleCorrFlag;
        public int IdleEconFlag;
        public float RPM;
        public float Speed;
        public float MassAirFlow;
        public float CyclicAirFlow;
        public float EffectiveVolume;
        public float EngineLoad;
        public float EstimatedPower;
        public float EstimatedTorque;
        public float WishFuelRatio;
        public float IdleValvePosition;
        public float IdleRegThrRPM;
        public float WishIdleRPM;
        public float WishIdleMassAirFlow;
        public float WishIdleValvePosition;
        public float WishIdleIgnitionAdvance;
        public float IgnitionAdvance;
        public float InjectionPhase;
        public float InjectionPhaseDuration;
        public float InjectionPulse;
        public float InjectionDutyCycle;
        public float InjectionEnrichment;
        public float InjectionLag;
        public float IgnitionPulse;
        public float IdleSpeedShift;

        public float EnrichmentSyncAmount;
        public float EnrichmentAsyncAmount;
        public float EnrichmentStartLoad;
        public float EnrichmentLoadDerivative;

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

        public int EtcMotorActiveFlag;
        public int EtcStandaloneFlag;
        public int EtcMotorFullCloseFlag;

        public int EtcOutCruizeG;
        public int EtcOutCruizeR;
        public int EtcOutRsvd3;
        public int EtcOutRsvd4;
        public int EtcInCruizeStart;
        public int EtcInCruizeStop;
        public int EtcInBrake;
        public int EtcInRsvd4;
        public int EtcInRsvd5;
        public int EtcInRsvd6;

        public int CylinderIgnitionBitmask;
        public int CylinderInjectionBitmask;
    }
}
