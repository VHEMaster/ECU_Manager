using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECU_Framework.Structs
{
    public class Consts
    {
        public const int ECU_CYLINDERS_COUNT = 4;

        public const int TABLE_SETUPS = 1;
        public const int TABLE_PRESSURES_16 = 16;
        public const int TABLE_THROTTLES_16 = 16;
        public const int TABLE_FILLING_16 = 16;
        public const int TABLE_ROTATES_16 = 16;
        public const int TABLE_PRESSURES_32 = 32;
        public const int TABLE_THROTTLES_32 = 32;
        public const int TABLE_FILLING_32 = 32;
        public const int TABLE_ROTATES_32 = 32;
        public const int TABLE_PEDALS = 16;
        public const int TABLE_SPEEDS = 16;
        public const int TABLE_TEMPERATURES = 16;
        public const int TABLE_VOLTAGES = 16;
        public const int TABLE_STRING_MAX = 16;
        public const int TABLE_ENRICHMENT_PERCENTS = 8;

        public const int IDLE_VALVE_POS_MAX = 160;

        public const int CHECK_ITEMS_MAX = 128;
        public const int CHECK_BITMAP_SIZE = 16;

        public const int PACKET_TABLE_MAX_SIZE = 512;
        public const int PACKET_CONFIG_MAX_SIZE = PACKET_TABLE_MAX_SIZE;
        public const int PACKET_CRITICAL_MAX_SIZE = PACKET_TABLE_MAX_SIZE;
        public const int PACKET_CORRECTION_MAX_SIZE = PACKET_TABLE_MAX_SIZE;
        public const int PACKET_SPECIFIC_PARAMETERS_ARRAY_MAX_SIZE = 224;

        public const int SPECIFIC_PARAMETERS_ARRAY_MAX_ITEMS = 4;
        public const int SPECIFIC_PARAMETERS_ARRAY_POINTS = 1024;

        public const int DRAG_POINTS = 3072;
        public const int DRAG_POINTS_DISTANCE = 20000;
    }
}
