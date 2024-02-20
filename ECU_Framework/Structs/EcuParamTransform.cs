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
    public struct EcuParamTransform
    {
        public float gain;
        public float offset;
        public EcuParamTransform(float gain, float offset)
        {
            this.gain = gain;
            this.offset = offset;
        }
    }
}