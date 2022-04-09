using ECU_Manager.Structs;
using ECU_Manager.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ECU_Manager.Controls
{
    public partial class Editor1D : Form
    {
        private string sName;
        private int sArraySizeX;
        private string sConfigSizeX;
        private string sConfigDepX;
        private string sParamsStatusX;
        private string sParamsStatusY;
        private string sTitleStatusX;
        private string sTitleStatusY;
        private string sArrayName;
        private float fMinY;
        private float fMaxY;
        private float fStepSize;
        private float fIntervalX;
        private float fIntervalY;
        private float fChartMinY;
        private float fChartMaxY;
        private bool bLog10;

        public Editor1D(string name, float min, float max, float step, float chartminy, float chartmaxy, float intervalx, float intervaly, bool log10 = false)
        {
            InitializeComponent();

            sName = name;
            fMinY = min;
            fMaxY = max;
            fStepSize = step;
            fIntervalX = intervalx;
            fIntervalY = intervaly;
            fChartMinY = chartminy;
            fChartMaxY = chartmaxy;

            bLog10 = log10;
        }

        public void SetConfig(string arrayname, string sizex, string depx, int arraysizex)
        {
            sArrayName = arrayname;
            sConfigSizeX = sizex;
            sConfigDepX = depx;
            sArraySizeX = arraysizex;
    }

        public void SetX(string param, string title)
        {
            sParamsStatusX = param;
            sTitleStatusX = title;
        }

        public void SetY(string param, string title)
        {
            sParamsStatusY = param;
            sTitleStatusY = title;
        }

        public void Update(EcuParameters parameters, EcuTable table, ParamsTable config)
        {
            float[] array1d = null;
            float[] dep1d = null;
            int size = 0;
            float min, max;

            if (!string.IsNullOrWhiteSpace(sConfigSizeX))
            {
                FieldInfo fieldSize = config.GetType().GetField(sConfigSizeX);
                if (fieldSize != null)
                    size = (int)fieldSize.GetValue(config);
            }

            if (!string.IsNullOrWhiteSpace(sConfigDepX))
            {
                FieldInfo fieldDepX = table.GetType().GetField(sConfigDepX);
                if (fieldDepX != null)
                    dep1d = (float[])fieldDepX.GetValue(table);
            }

            if (!string.IsNullOrWhiteSpace(sArrayName))
            {
                FieldInfo fieldArrayX = table.GetType().GetField(sArrayName);
                if (fieldArrayX != null)
                    array1d = (float[])fieldArrayX.GetValue(table);
            }


            chart1DChart.Series.Clear();
            if (array1d != null)
            {
                chart1DChart.Series[0].XAxisType = AxisType.Primary;
                chart1DChart.Series[0].YAxisType = AxisType.Primary;
                chart1DChart.Series[0].YValueType = ChartValueType.Single;
                chart1DChart.Series[0].MarkerSize = 8;
                chart1DChart.Series[0].MarkerColor = Color.Black;
                chart1DChart.Series[0].MarkerStyle = MarkerStyle.Circle;
                chart1DChart.Series[0].ChartType = SeriesChartType.Line;

                min = array1d.Take(size).Min();
                max = array1d.Take(size).Max();

                if (min > fChartMinY)
                    min = fChartMinY;

                if (max < fChartMaxY)
                    max = fChartMaxY;

                chart1DChart.ChartAreas[0].AxisY.Minimum = min;
                chart1DChart.ChartAreas[0].AxisY.Maximum = max;
                chart1DChart.ChartAreas[0].AxisY.Interval = fIntervalY;

                nudItem.Minimum = 1;
                nudItem.Maximum = sArraySizeX;

                nudValue.Minimum = (decimal)fMinY;
                nudValue.Maximum = (decimal)fMaxY;
                nudValue.Increment = (decimal)fStepSize;
                nudValue.Value = (decimal)array1d[(int)nudItem.Value - 1];

                if (dep1d == null)
                {
                    chart1DChart.Series[0].XValueType = ChartValueType.Int32;
                    chart1DChart.ChartAreas[0].AxisX.Minimum = 1;
                    chart1DChart.ChartAreas[0].AxisX.Maximum = size;
                    chart1DChart.ChartAreas[0].AxisX.Interval = 1;

                    for (int i = 0; i < size; i++)
                        chart1DChart.Series[0].Points[chart1DChart.Series[0].Points.AddXY(i + 1, array1d[i])].Tag = i;
                }
                else
                {
                    chart1DChart.Series[0].XValueType = ChartValueType.Single;
                    chart1DChart.ChartAreas[0].AxisX.Interval = fIntervalX;
                    chart1DChart.ChartAreas[0].AxisX.Minimum = dep1d[0];
                    chart1DChart.ChartAreas[0].AxisX.Maximum = dep1d[size - 1];
                    for (int i = 0; i < size; i++)
                        chart1DChart.Series[0].Points[chart1DChart.Series[0].Points.AddXY(dep1d[i], array1d[i])].Tag = i;
                }


                if (!string.IsNullOrWhiteSpace(sParamsStatusX) || !string.IsNullOrWhiteSpace(sParamsStatusY))
                {

                    FieldInfo fieldParamX = table.GetType().GetField(sParamsStatusX);
                    FieldInfo fieldParamY = table.GetType().GetField(sParamsStatusY);

                    Series series = chart1DChart.Series.Add("Current");
                    series.ChartType = SeriesChartType.Point;
                    series.XAxisType = AxisType.Primary;
                    series.XValueType = ChartValueType.Single;
                    series.YAxisType = AxisType.Primary;
                    series.YValueType = ChartValueType.Single;
                    series.BorderWidth = 8;
                    series.Color = Color.Red;
                    series.MarkerStyle = MarkerStyle.Circle;

                    if (fieldParamX != null && fieldParamY != null)
                    {
                        series.Points.AddXY((float)fieldParamX.GetValue(parameters), (float)fieldParamY.GetValue(parameters));
                    }
                    else if (fieldParamX != null && dep1d != null)
                    {
                        Interpolate interpolate = new Interpolate((float)fieldParamX.GetValue(parameters), dep1d, size);
                        series.Points.AddXY((float)fieldParamX.GetValue(parameters), interpolate.Interpolate1D(array1d));
                    }
                    else if (fieldParamY != null)
                    {
                        Interpolate interpolate = new Interpolate((float)fieldParamY.GetValue(parameters), array1d, size);
                        series.Points.AddXY((float)(interpolate.indexes[0] + interpolate.mult), (float)fieldParamY.GetValue(parameters));
                    }
                }

                nudValue.Enabled = true;
                nudItem.Enabled = true;
            }
            else
            {
                nudValue.Enabled = false;
                nudItem.Enabled = false;
            }

        }
    }
}
