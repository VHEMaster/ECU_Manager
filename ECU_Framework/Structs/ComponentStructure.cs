using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECU_Framework.Structs
{
    public class ComponentStructure
    {
        public ConfigStruct ConfigStruct;
        public EcuParameters EcuParameters;
        public EcuForceParameters ForceParameters;
        public int CurrentTable;
        public ComponentStructure()
        {
            ConfigStruct = new ConfigStruct(0);
            EcuParameters = new EcuParameters();
            ForceParameters = new EcuForceParameters();
        }
    }
}
