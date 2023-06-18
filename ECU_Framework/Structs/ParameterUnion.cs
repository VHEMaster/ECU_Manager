using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ECU_Framework.Structs
{
    public struct ParameterUnion
    {
        private uint dummy;
    }
    public struct ParameterUnionFloat
    {
        public float Value;
    }
    public struct ParameterUnionUint
    {
        public uint Value;
    }
    public struct ParameterUnionBytes
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Values;
    }
}
