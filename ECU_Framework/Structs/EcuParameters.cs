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

        public ushort KnockCount;
        public ushort KnockCountCy1;
        public ushort KnockCountCy2;
        public ushort KnockCountCy3;
        public ushort KnockCountCy4;

        public byte SwitchPosition;
        public byte CurrentTable;
        public byte InjectorChannel;

        public byte IdleFlag;
        public byte IdleCorrFlag;
        public byte IdleEconFlag;

        public byte LambdaValid;

        public byte OilSensor;
        public byte FanForceSwitch;
        public byte HandbrakeSensor;
        public byte ChargeSensor;
        public byte ClutchSensor;
        public byte IgnSensor;

        public byte FuelPumpRelay;
        public byte FanRelay;
        public byte CheckEngine;
        public byte StarterRelay;
        public byte FanSwitch;
        public byte IgnOutput;

        public byte StartAllowed;
        public byte IsRunning;
        public byte IsCheckEngine;

        public byte EtcMotorActiveFlag;
        public byte EtcStandaloneFlag;
        public byte EtcMotorFullCloseFlag;

        public byte EtcOutCruizeG;
        public byte EtcOutCruizeR;
        public byte EtcOutRsvd3;
        public byte EtcOutRsvd4;
        public byte EtcInCruizeStart;
        public byte EtcInCruizeStop;
        public byte EtcInBrake;
        public byte EtcInRsvd4;
        public byte EtcInRsvd5;
        public byte EtcInRsvd6;

        public byte CylinderIgnitionBitmask;
        public byte CylinderInjectionBitmask;
    }
}
