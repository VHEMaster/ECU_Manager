using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECU_Framework.Structs
{
    [Serializable]
    public struct DragPoint
    {
        public float RPM;
        public float Speed;
        public float Acceleration;
        public float Pressure;
        public float MassAirFlow;
        public float CycleAirFlow;
        public float Throttle;
        public float Ignition;
        public float Mixture;
        public uint Time;
    }
}
