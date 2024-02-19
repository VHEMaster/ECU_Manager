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
    public struct ConfigStruct
    {
        public ParamsTable parameters;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_SETUPS)]
        [XmlArray("tables")]
        [XmlArrayItem("table")]
        public EcuTable[] tables;
        public EcuCriticalBackup critical;
        public EcuCorrections corrections;

        public ConfigStruct(int _)
        {
            parameters = new ParamsTable();
            tables = new EcuTable[Consts.TABLE_SETUPS];
            for (int i = 0; i < Consts.TABLE_SETUPS; i++)
                tables[i] = new EcuTable();
            corrections = new EcuCorrections();
            critical = new EcuCriticalBackup();
        }
    }
}
