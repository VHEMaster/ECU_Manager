using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECU_Framework.Structs
{
    public class CheckDataItem
    {
        public readonly ErrorCode ErrorCode;
        public readonly string Message;
        public readonly bool Active;

        public CheckDataItem(ErrorCode errorCode, string message, bool active)
        {
            this.ErrorCode = errorCode;
            this.Message = message;
            this.Active = active;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj.GetType() == this.GetType() && this.ErrorCode == ((CheckDataItem)obj).ErrorCode;
        }

        public override string ToString()
        {
            return $"{this.ErrorCode.ToString()}({((int)this.ErrorCode).ToString()}) {{{(this.Active ? "Active" : "Inactive")}}}: {this.Message}";

        }
    }


    public static class CheckData
    {

        public static List<CheckDataItem> GenerateFromBitmap(byte[] check_bitmap_status, byte[] check_bitmap_recorded)
        {
            List<CheckDataItem> checkDataList = new List<CheckDataItem>();

            for (int i = 0; i < Consts.CHECK_ITEMS_MAX; i++)
            {
                if ((check_bitmap_recorded[i >> 3] & (1 << (i & 7))) > 0 || (check_bitmap_status[i >> 3] & (1 << (i & 7))) > 0)
                {
                    try
                    {
                        CheckDataItem Item = new CheckDataItem((ErrorCode)i, ErrorStrings[i], (check_bitmap_status[i >> 3] & (1 << (i & 7))) > 0);
                        checkDataList.Add(Item);
                    }
                    catch
                    {

                    }
                }
            }

            return checkDataList;
        }

        private static readonly string[] ErrorStrings = new string[95]
        {
            "Invalid",
            "Flash: Load failure",
            "Flash: Save failure",
            "Flash: Init failure",
            "Bkpsram: Save failure",
            "Bkpsram: Load failure",
            "Sensor: MAP failure",
            "Sensor: Knock failure",
            "Sensor: CSPS failure",
            "Sensor: TSPS failure",
            "Sensor: AirTemp failure",
            "Sensor: EngineTemp failure",
            "Sensor: TPS failure",
            "Sensor: RefVoltage failure",
            "Sensor: PwrVoltage failure",
            "Sensor: Lambda failure",

            "Output: Driver failure",
            "CAN: Init failure",
            "CAN: Test failure",
            "KLine: Protocol failure",
            "KLine: Loopback failure",

            "Injector 4: OpenCircuit",
            "Injector 4: ShortToBatOrOverheat",
            "Injector 4: ShortToGND",
            "Injector 3: OpenCircuit",
            "Injector 3: ShortToBatOrOverheat",
            "Injector 3: ShortToGND",
            "Injector 2: OpenCircuit",
            "Injector 2: ShortToBatOrOverheat",
            "Injector 2: ShortToGND",
            "Injector 1: OpenCircuit",
            "Injector 1: ShortToBatOrOverheat",
            "Injector 1: ShortToGND",
            "Injector: Communication failure",

            "CheckEngine: OpenCircuit",
            "CheckEngine: ShortToBatOrOverheat",
            "CheckEngine: ShortToGND",
            "SpeedMeter: OpenCircuit",
            "SpeedMeter: ShortToBatOrOverheat",
            "SpeedMeter: ShortToGND",
            "Tachometer: OpenCircuit",
            "Tachometer: ShortToBatOrOverheat",
            "Tachometer: ShortToGND",
            "FuelPump: OpenCircuit",
            "FuelPump: ShortToBatOrOverheat",
            "FuelPump: ShortToGND",
            "Outputs1: Communication failure",

            "OutIgn: OpenCircuit",
            "OutIgn: ShortToBatOrOverheat",
            "OutIgn: ShortToGND",
            "FanSwitch: OpenCircuit",
            "FanSwitch: ShortToBatOrOverheat",
            "FanSwitch: ShortToGND",
            "StarterRelay: OpenCircuit",
            "StarterRelay: ShortToBatOrOverheat",
            "StarterRelay: ShortToGND",
            "FanRelay: OpenCircuit",
            "FanRelay: ShortToBatOrOverheat",
            "FanRelay: ShortToGND",
            "Outputs2: Communication failure",

            "IdleValve: Failure",
            "IdleValve: Driver failure",
            "Injection: Fuel underflow",
            "ADC: Communication Failure",

            "Lambda: Communication failure",
            "Lambda: VM ShortToBat",
            "Lambda: VM LowBattery",
            "Lambda: VM ShortToGnd",
            "Lambda: UN ShortToBat",
            "Lambda: UN LowBattery",
            "Lambda: UN ShortToGnd",
            "Lambda: IAIP ShortToBat",
            "Lambda: IAIP LowBattery",
            "Lambda: IAIP ShortToGnd",
            "Lambda: DIAHGD ShortToBat",
            "Lambda: DIAHGD OpenCircuit",
            "Lambda: DIAHGD ShortToGnd",
            "Lambda: Temperature failure",
            "Lambda: Heater failure",

            "Knock: Detonation Found",
            "Knock: Low Noise Level",

            "TSPS: Phase Desynchronized",
            "CPU: HardFault Exception",

            "Sensor: MAP/TPS Mismatch",
            "Sensor: Lean Mixture",
            "Sensor: Rich Mixture",
            "Sensor: Lean Idle Mixture",
            "Sensor: Rich Idle Mixture",

            "Engine: No Oil Pressure",
            "Engine: No Battery Charge",

            "ECU: BootLoader Mode",

            "Knock: Detonation Cy1",
            "Knock: Detonation Cy2",
            "Knock: Detonation Cy3",
            "Knock: Detonation Cy4",
        };

        public static string GetStringForCode(ErrorCode errorCode)
        {
            if (errorCode < ErrorCode.Count)
            {
                return ErrorStrings[(int)errorCode];
            }
            return $"Invalid error code: {(int)errorCode}";
        }
    }
    public enum ErrorCode
    {
        NoFailure = 0,
        FlashLoadFailure,
        FlashSaveFailure,
        FlashInitFailure,
        BkpsramSaveFailure,
        BkpsramLoadFailure,
        SensorMapFailure,
        SensorKnockFailure,
        SensorCspsFailure,
        SensorTspsFailure,
        SensorAirTempFailure,
        SensorEngineTempFailure,
        SensorTPSFailure,
        SensorRefVoltageFailure,
        SensorPwrVoltageFailure,
        SensorLambdaFailure,
        OutputDriverFailure,
        CanInitFailure,
        CanTestFailure,
        KlineProtocolFailure,
        KlineLoopbackFailure,
        Injector4OpenCircuit,
        Injector4ShortToBatOrOverheat,
        Injector4ShortToGND,
        Injector3OpenCircuit,
        Injector3ShortToBatOrOverheat,
        Injector3ShortToGND,
        Injector2OpenCircuit,
        Injector2ShortToBatOrOverheat,
        Injector2ShortToGND,
        Injector1OpenCircuit,
        Injector1ShortToBatOrOverheat,
        Injector1ShortToGND,
        InjectorCommunicationFailure,
        CheckEngineOpenCirtuit,
        CheckEngineShortToBatOrOverheat,
        CheckEngineShortToGND,
        SpeedMeterOpenCirtuit,
        SpeedMeterShortToBatOrOverheat,
        SpeedMeterShortToGND,
        TachometerOpenCirtuit,
        TachometerShortToBatOrOverheat,
        TachometerShortToGND,
        FuelPumpOpenCirtuit,
        FuelPumpShortToBatOrOverheat,
        FuelPumpShortToGND,
        Outputs1CommunicationFailure,
        OutIgnOpenCirtuit,
        OutIgnShortToBatOrOverheat,
        OutIgnShortToGND,
        FanSwitchOpenCirtuit,
        FanSwitchShortToBatOrOverheat,
        FanSwitchShortToGND,
        StarterRelayOpenCirtuit,
        StarterRelayShortToBatOrOverheat,
        StarterRelayShortToGND,
        FanRelayOpenCirtuit,
        FanRelayShortToBatOrOverheat,
        FanRelayShortToGND,
        Outputs2CommunicationFailure,
        IdleValveFailure,
        IdleValveDriverFailure,
        InjectionUnderflow,
        AdcFailure,
        LambdaCommunicationFailure,
        LambdaVMShortToBat,
        LambdaVMLowBattery,
        LambdaVMShortToGND,
        LambdaUNShortToBat,
        LambdaUNLowBattery,
        LambdaUNShortToGND,
        LambdaIAIPShortToBat,
        LambdaIAIPLowBattery,
        LambdaIAIPShortToGND,
        LambdaDIAHGDShortToBat,
        LambdaDIAHGDOpenCirtuit,
        LambdaDIAHGDShortToGND,
        CheckLambdaTemperatureFailure,
        CheckLambdaHeaterFailure,
        KnockDetonationFound,
        KnockLowNoiseLevel,
        TspsDesynchronized,
        HardFaultException,
        SensorMapTpsMismatch,
        EngineLeanMixture,
        EngineRichMixture,
        EngineLeanIdleMixture,
        EngineRichIdleMixture,
        NoOilPressure,
        NoBatteryCharge,
        BootLoaderMode,
        KnockDetonationCy1,
        KnockDetonationCy2,
        KnockDetonationCy3,
        KnockDetonationCy4,
        Count,
    };
}