using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECU_Framework.Structs
{
    public class Parameter
    {
        private MiddleLayer middleLayer;
        public FieldInfo FieldInfo;
        public string Name;
        public string FloatFormat;
        public Type Type;
        public float Min;
        public float Max;
        public float Step;
        public FieldInfo DepFieldInfo;
        public FieldInfo EnableFieldInfo;
        public NumericUpDown NumericUpDown;
        public TrackBar TrackBar;
        public object ValueOld = null;
        public object Value
        {
            get
            {
                if (middleLayer == null)
                {
                    if (this.Type == typeof(float))
                        return 0.0F;
                    else return 0;
                }

                if (Enabled)
                {
                    return FieldInfo.GetValue(middleLayer.ComponentStructure.ForceParameters);
                }
                else
                {
                    if (DepFieldInfo != null)
                        return DepFieldInfo.GetValue(middleLayer.ComponentStructure.EcuParameters);
                    else if (this.Type == typeof(float))
                        return 0.0F;
                    else return 0;
                }
            }
            set
            {
                if (middleLayer != null)
                {
                    if (Enabled)
                    {
                        FieldInfo.SetValueDirect(__makeref(middleLayer.ComponentStructure.ForceParameters), value);
                    }
                }
            }
        }
        public bool Enabled
        {
            get
            {
                if (middleLayer == null)
                    return false;
                return (byte)EnableFieldInfo.GetValue(middleLayer.ComponentStructure.ForceParameters) > 0;
            }
            set
            {
                if (middleLayer != null)
                {
                    if (!Enabled)
                    {
                        if (DepFieldInfo != null)
                        {
                            object val = DepFieldInfo.GetValue(middleLayer.ComponentStructure.EcuParameters);
                            FieldInfo.SetValueDirect(__makeref(middleLayer.ComponentStructure.ForceParameters), val);
                        }
                    }
                    EnableFieldInfo.SetValueDirect(__makeref(middleLayer.ComponentStructure.ForceParameters), (byte)(value ? 1 : 0));
                }
            }
        }

        public Parameter(MiddleLayer middleLayer)
        {
            this.middleLayer = middleLayer;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
