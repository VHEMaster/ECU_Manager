using ECU_Framework.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECU_Framework.Tools
{
    public class InterpolationExtended
    {
        public double[] values { private set; get; } = new double[2];
        public int[] indexes { private set; get; } = new int[2];
        public int size { private set; get; }
        public double input { private set; get; }
        public double mult { private set; get; }

        public InterpolationExtended(float value, dynamic[] table, int size, EcuParamTransform transform)
        {
            float[] table_float = new float[size];

            for(int i = 0; i < size; i++)
            {
                table_float[i] = Convert.ToSingle(table[i]) * transform.gain + transform.offset;
            }

            int find_index = -1;

            if (float.IsNaN(value))
            {
                this.mult = double.NaN;
                return;
            }

            this.input = value;
            this.size = size;

            if (size == 1)
            {
                this.indexes[0] = 0;
                this.indexes[1] = 0;
                this.values[0] = table_float[0];
                this.values[1] = table_float[0];
            }
            else if (value <= table_float[1])
            {
                this.indexes[0] = 0;
                this.indexes[1] = 1;
                this.values[0] = table_float[this.indexes[0]];
                this.values[1] = table_float[this.indexes[1]];
            }
            else if (value >= table_float[size - 2])
            {
                this.indexes[0] = size - 2;
                this.indexes[1] = size - 1;
                this.values[0] = table_float[this.indexes[0]];
                this.values[1] = table_float[this.indexes[1]];
            }
            else
            {
                find_index = BinarySearch<float>.Find(table_float, 0, size - 1, value);
                if (find_index >= 0)
                {
                    this.indexes[0] = find_index;
                    this.indexes[1] = find_index + 1;
                    this.values[0] = table_float[find_index];
                    this.values[1] = table_float[find_index + 1];
                }
            }

            if (this.values[1] != this.values[0])
                this.mult = (value - this.values[0]) / (this.values[1] - this.values[0]);
            else this.mult = 1.0f;
        }

        public float Interpolate1D(float[] table)
        {
            float result;
            double[] output = new double[2];

            if (double.IsNaN(this.mult))
                return 0;

            if (table == null)
                return 0;

            output[0] = table[this.indexes[0]];
            output[1] = table[this.indexes[1]];

            result = (float)((output[1] - output[0]) * this.mult + output[0]);

            return result;
        }

        public static float Interpolate2D(InterpolationExtended input_x, InterpolationExtended input_y, dynamic[] table, int x_size, EcuParamTransform transform)
        {
            float result = 0.0f;
            double[] output_1d = new double[2];
            double[][] input_2d = new double[2][] { new double[2], new double[2] };

            if (double.IsNaN(input_x.mult) || double.IsNaN(input_y.mult))
                return 0;

            input_2d[0][0] = Convert.ToSingle(table[input_y.indexes[0] * x_size + input_x.indexes[0]]) * transform.gain + transform.offset;
            input_2d[0][1] = Convert.ToSingle(table[input_y.indexes[0] * x_size + input_x.indexes[1]]) * transform.gain + transform.offset;
            input_2d[1][0] = Convert.ToSingle(table[input_y.indexes[1] * x_size + input_x.indexes[0]]) * transform.gain + transform.offset;
            input_2d[1][1] = Convert.ToSingle(table[input_y.indexes[1] * x_size + input_x.indexes[1]]) * transform.gain + transform.offset;

            output_1d[0] = (input_2d[0][1] - input_2d[0][0]) * input_x.mult + input_2d[0][0];
            output_1d[1] = (input_2d[1][1] - input_2d[1][0]) * input_x.mult + input_2d[1][0];
            result = (float)((output_1d[1] - output_1d[0]) * input_y.mult + output_1d[0]);

            return result;
        }

        public static float Interpolate2D(InterpolationExtended input_x, InterpolationExtended input_y, dynamic[][] table, EcuParamTransform transform)
        {
            float result = 0.0f;
            double[] output_1d = new double[2];
            double[][] input_2d = new double[2][] { new double[2], new double[2] };

            if (double.IsNaN(input_x.mult) || double.IsNaN(input_y.mult))
                return 0;

            input_2d[0][0] = Convert.ToSingle(table[input_y.indexes[0]][input_x.indexes[0]]) * transform.gain + transform.offset;
            input_2d[0][1] = Convert.ToSingle(table[input_y.indexes[0]][input_x.indexes[1]]) * transform.gain + transform.offset;
            input_2d[1][0] = Convert.ToSingle(table[input_y.indexes[1]][input_x.indexes[0]]) * transform.gain + transform.offset;
            input_2d[1][1] = Convert.ToSingle(table[input_y.indexes[1]][input_x.indexes[1]]) * transform.gain + transform.offset;

            output_1d[0] = (input_2d[0][1] - input_2d[0][0]) * input_x.mult + input_2d[0][0];
            output_1d[1] = (input_2d[1][1] - input_2d[1][0]) * input_x.mult + input_2d[1][0];
            result = (float)((output_1d[1] - output_1d[0]) * input_y.mult + output_1d[0]);

            return result;
        }

        public float Interpolate2DAsX(InterpolationExtended input_y, dynamic[] table, int x_size, EcuParamTransform transform)
        {
            return Interpolate2D(this, input_y, table, x_size, transform);
        }

        public float Interpolate2DAsY(InterpolationExtended input_x, dynamic[] table, int x_size, EcuParamTransform transform)
        {
            return Interpolate2D(input_x, this, table, x_size, transform);
        }

        public float Interpolate2DAsX(InterpolationExtended input_y, dynamic[][] table, EcuParamTransform transform)
        {
            return Interpolate2D(this, input_y, table, transform);
        }

        public float Interpolate2DAsY(InterpolationExtended input_x, dynamic[][] table, EcuParamTransform transform)
        {
            return Interpolate2D(input_x, this, table, transform);
        }
    }
}
