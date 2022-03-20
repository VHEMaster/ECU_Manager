using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ECU_Manager.Tables
{
    [Serializable]
    public struct ConfigStruct
    {
        public ParamsTable parameters;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Consts.TABLE_SETUPS_MAX)]
        [XmlArray("tables")]
        [XmlArrayItem("table")]
        public IgnitionTable[] tables;

        public ConfigStruct(int _)
        {
            parameters = new ParamsTable();
            tables = new IgnitionTable[4];
            for (int i = 0; i < 4; i++)
                tables[i] = new IgnitionTable(); 
        }
    }
}
