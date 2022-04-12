using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECU_Manager.Structs
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

        public override string ToString()
        {
            return (Active ? "Active" : "Inactive") + $":{ErrorCode.ToString()}:{Message}";
        }
    }

    public static class CheckData
    {
        public static List<CheckDataItem> CheckDataList { get; private set; } = new List<CheckDataItem>();

        public static List<CheckDataItem> GenerateFromBitmap(byte[] check_bitmap_status, byte[] check_bitmap_recorded)
        {
            List<CheckDataItem> checkDataList = new List<CheckDataItem>();
            CheckDataItem Item;

            for (int i = 0; i < Consts.CHECK_ITEMS_MAX; i++)
            {
                if ((check_bitmap_recorded[i >> 3] & (1 << (i & 7))) > 0 || (check_bitmap_status[i >> 3] & (1 << (i & 7))) > 0)
                {
                    try
                    {
                        Item = new CheckDataItem((ErrorCode)i, ErrorStrings[i], (check_bitmap_status[i >> 3] & (1 << (i & 7))) > 0);
                        CheckDataList.Add(Item);
                    }
                    catch
                    {

                    }
                }
            }

            CheckDataList = checkDataList;
            return checkDataList;
        }

        private static readonly string[] ErrorStrings = new string[74]
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
            "OutRsvd1: OpenCircuit",
            "OutRsvd1: ShortToBatOrOverheat",
            "OutRsvd1: ShortToGND",
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

            "Knock: Detonation Found",
            "Knock: Low Noise Level",
        };

        public static string GetStringForCode(ErrorCode errorCode)
        {
            if(errorCode < ErrorCode.Count)
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
        OutRsvd1OpenCirtuit,
        OutRsvd1ShortToBatOrOverheat,
        OutRsvd1ShortToGND,
        StarterRelayOpenCirtuit,
        StarterRelayShortToBatOrOverheat,
        StarterRelayShortToGND,
        FamRelayOpenCirtuit,
        FanRelayShortToBatOrOverheat,
        FanRelayShortToGND,
        Outputs2CommunicationFailure,
        IdleValveFailure,
        IdleValveDriverFailure,
        InjectionUnderflow,
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
        KnockDetonationFound,
        KnockLowNoiseLevel,
        Count,
    };
}
