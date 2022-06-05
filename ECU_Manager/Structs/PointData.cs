using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECU_Manager.Structs
{
    [Serializable]
    public class PointData
    {
        public EcuParameters Parameters;
        public double Seconds;

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType())
                return false;
            return Seconds == (obj as PointData).Seconds;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    
}
