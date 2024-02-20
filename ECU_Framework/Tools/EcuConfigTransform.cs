using ECU_Framework.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECU_Framework.Tools
{
    public static class EcuConfigTransform
    {
        public static float[] FromInteger(Array input, EcuParamTransform transform)
        {
            float[] array_float = new float[input.Length];

            for (int i = 0; i < array_float.Length; i++)
            {
                array_float[i] = Convert.ToSingle(input.GetValue(i)) * transform.gain + transform.offset;
            }

            return array_float;
        }
        public static void ToInteger(Array input, Array output, EcuParamTransform transform)
        {
            float value;

            float min = GetMinValue(output.GetType().GetElementType());
            float max = GetMaxValue(output.GetType().GetElementType());
           
            for (int i = 0; i < output.Length; i++)
            {
                value = (Convert.ToSingle(input.GetValue(i)) - transform.offset) / transform.gain;

                if (value > max)
                    value = max;
                else if (value < min)
                    value = min;

                output.SetValue(Convert.ChangeType(value, output.GetType().GetElementType()), i);
            }
        }
        private static float GetMinValue(Type type)
        {
            try
            {
                return Convert.ToSingle(type.GetField("MinValue").GetValue(null));
            }
            catch
            {
                throw new InvalidOperationException($"Unsupported type {type.Name}");
            }
        }
        private static float GetMaxValue(Type type)
        {
            try
            {
                return Convert.ToSingle(type.GetField("MaxValue").GetValue(null));
            }
            catch
            {
                throw new InvalidOperationException($"Unsupported type {type.Name}");
            }
        }
    }
}
