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
    public struct EcuStatus
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        [XmlArray("status")]
        [XmlArrayItem("byte")]
        public byte[] bytes;
    }
}




 