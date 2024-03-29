﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECU_Framework.Tools
{
    public class Interpolation
    {
        public double[] values { private set; get; } = new double[2];
        public int[] indexes { private set; get; } = new int[2];
        public int size { private set; get; }
        public double input { private set; get; }
        public double mult { private set; get; }

        public Interpolation(float value, float[] table, int size)
        {
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
                this.values[0] = table[0];
                this.values[1] = table[0];
            }
            else if (value <= table[1])
            {
                this.indexes[0] = 0;
                this.indexes[1] = 1;
                this.values[0] = table[this.indexes[0]];
                this.values[1] = table[this.indexes[1]];
            }
            else if (value >= table[size - 2])
            {
                this.indexes[0] = size - 2;
                this.indexes[1] = size - 1;
                this.values[0] = table[this.indexes[0]];
                this.values[1] = table[this.indexes[1]];
            }
            else
            {
                find_index = BinarySearch<float>.Find(table, 0, size - 1, value);
                if (find_index >= 0)
                {
                    this.indexes[0] = find_index;
                    this.indexes[1] = find_index + 1;
                    this.values[0] = table[find_index];
                    this.values[1] = table[find_index + 1];
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

        public static float Interpolate2D(Interpolation input_x, Interpolation input_y, float[] table, int x_size)
        {
            float result = 0.0f;
            double[] output_1d = new double[2];
            double[][] input_2d = new double[2][] { new double[2], new double[2] };

            if (double.IsNaN(input_x.mult) || double.IsNaN(input_y.mult))
                return 0;

            input_2d[0][0] = table[input_y.indexes[0] * x_size + input_x.indexes[0]];
            input_2d[0][1] = table[input_y.indexes[0] * x_size + input_x.indexes[1]];
            input_2d[1][0] = table[input_y.indexes[1] * x_size + input_x.indexes[0]];
            input_2d[1][1] = table[input_y.indexes[1] * x_size + input_x.indexes[1]];

            output_1d[0] = (input_2d[0][1] - input_2d[0][0]) * input_x.mult + input_2d[0][0];
            output_1d[1] = (input_2d[1][1] - input_2d[1][0]) * input_x.mult + input_2d[1][0];
            result = (float)((output_1d[1] - output_1d[0]) * input_y.mult + output_1d[0]);

            return result;
        }

        public static float Interpolate2D(Interpolation input_x, Interpolation input_y, float[][] table)
        {
            float result = 0.0f;
            double[] output_1d = new double[2];
            double[][] input_2d = new double[2][] { new double[2], new double[2] };

            if (double.IsNaN(input_x.mult) || double.IsNaN(input_y.mult))
                return 0;

            input_2d[0][0] = table[input_y.indexes[0]][input_x.indexes[0]];
            input_2d[0][1] = table[input_y.indexes[0]][input_x.indexes[1]];
            input_2d[1][0] = table[input_y.indexes[1]][input_x.indexes[0]];
            input_2d[1][1] = table[input_y.indexes[1]][input_x.indexes[1]];

            output_1d[0] = (input_2d[0][1] - input_2d[0][0]) * input_x.mult + input_2d[0][0];
            output_1d[1] = (input_2d[1][1] - input_2d[1][0]) * input_x.mult + input_2d[1][0];
            result = (float)((output_1d[1] - output_1d[0]) * input_y.mult + output_1d[0]);

            return result;
        }

        public float Interpolate2DAsX(Interpolation input_y, float[] table, int x_size)
        {
            return Interpolate2D(this, input_y, table, x_size);
        }

        public float Interpolate2DAsY(Interpolation input_x, float[] table, int x_size)
        {
            return Interpolate2D(input_x, this, table, x_size);
        }

        public float Interpolate2DAsX(Interpolation input_y, float[][] table)
        {
            return Interpolate2D(this, input_y, table);
        }

        public float Interpolate2DAsY(Interpolation input_x, float[][] table)
        {
            return Interpolate2D(input_x, this, table);
        }
    }
}
