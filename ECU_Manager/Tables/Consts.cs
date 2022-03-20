using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECU_Manager.Tables
{
    public class Consts
    {
        public const int TABLE_SETUPS_MAX = 4;
        public const int TABLE_PRESSURES_MAX = 24;
        public const int TABLE_ROTATES_MAX = 24;
        public const int TABLE_TEMPERATURES_MAX = 12;
        public const int TABLE_STRING_MAX = 12;

        public const int PACKET_TABLE_MAX_SIZE = 224;
        public const int PACKET_CONFIG_MAX_SIZE = PACKET_TABLE_MAX_SIZE;

        public const int DRAG_MAX_POINTS = 3072;
        public const int DRAG_POINTS_DISTANCE = 20000;
    }
}
