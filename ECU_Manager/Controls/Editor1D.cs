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
        private string sFormatStatusX;
        private string sFormatStatusY;
        private string sArrayName;
        private float fMinY;
        private float fMaxY;
        private float fStepSize;
        private float fIntervalX;
        private float fIntervalY;
        private float fChartMinY;
        private float fChartMaxY;
        private float fMinDiff;
        private bool bLog10;
        private EventHandler UpdateTableEvent = null;

        private ComponentStructure cs;

        public Editor1D(ComponentStructure componentStructure, string name, float min, float max, float step, float mindiff, float chartminy, float chartmaxy, float intervalx, float intervaly, bool log10 = false)
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
            fMinDiff = mindiff;

            bLog10 = log10;
            lblTitle.Text = sName;

            cs = componentStructure;


        }

        public void SetTableEventHandler(EventHandler eventHandler)
        {
            UpdateTableEvent = eventHandler;
        }

        public void SetConfig(string arrayname, string sizex, string depx, int arraysizex)
        {
            sArrayName = arrayname;
            sConfigSizeX = sizex;
            sConfigDepX = depx;
            sArraySizeX = arraysizex;
    }

        public void SetX(string param, string title, string format)
        {
            sParamsStatusX = param;
            sTitleStatusX = title;
            sFormatStatusX = format;
        }

        public void SetY(string param, string title, string format)
        {
            sParamsStatusY = param;
            sTitleStatusY = title;
            sFormatStatusY = format;
        }

        public void UpdateChart()
        {
            float[] array1d = null;
            float[] dep1d = null;
            int size = 0;
            float min, max;

            if (!string.IsNullOrWhiteSpace(sConfigSizeX))
            {
                FieldInfo fieldSize = cs.ConfigStruct.parameters.GetType().GetField(sConfigSizeX);
                if (fieldSize != null)
                    size = (int)fieldSize.GetValue(cs.ConfigStruct.parameters);
            }

            if (!string.IsNullOrWhiteSpace(sConfigDepX))
            {
                FieldInfo fieldDepX = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sConfigDepX);
                if (fieldDepX != null)
                    dep1d = (float[])fieldDepX.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
            }

            if (!string.IsNullOrWhiteSpace(sArrayName))
            {
                FieldInfo fieldArrayX = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                if (fieldArrayX != null)
                    array1d = (float[])fieldArrayX.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
            }


            lblParams.Text = string.Empty;
            lblItemValue.Text = string.Empty;
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
                    if (!string.IsNullOrWhiteSpace(sTitleStatusX)) {
                        lblItemValue.Text = $"{sTitleStatusX}: ";
                        lblItemValue.Text += $"{dep1d[(int)nudItem.Value - 1].ToString(sFormatStatusX)}";
                    }

                    chart1DChart.Series[0].XValueType = ChartValueType.Single;
                    chart1DChart.ChartAreas[0].AxisX.Interval = fIntervalX;
                    chart1DChart.ChartAreas[0].AxisX.Minimum = dep1d[0];
                    chart1DChart.ChartAreas[0].AxisX.Maximum = dep1d[size - 1];
                    for (int i = 0; i < size; i++)
                        chart1DChart.Series[0].Points[chart1DChart.Series[0].Points.AddXY(dep1d[i], array1d[i])].Tag = i;
                }


                if (!string.IsNullOrWhiteSpace(sParamsStatusX) || !string.IsNullOrWhiteSpace(sParamsStatusY))
                {

                    FieldInfo fieldParamX = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sParamsStatusX);
                    FieldInfo fieldParamY = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sParamsStatusY);

                    if (fieldParamX != null && !string.IsNullOrWhiteSpace(sFormatStatusX))
                    {
                        if (!string.IsNullOrWhiteSpace(lblParams.Text))
                            lblParams.Text += "   ";
                        if (!string.IsNullOrWhiteSpace(sTitleStatusX))
                            lblParams.Text += $"{sTitleStatusX}: ";
                        lblParams.Text += $"{((float)fieldParamX.GetValue(cs.EcuParameters)).ToString(sFormatStatusX)}";
                    }
                    if (fieldParamY != null && !string.IsNullOrWhiteSpace(sFormatStatusY))
                    {
                        if (!string.IsNullOrWhiteSpace(lblParams.Text))
                            lblParams.Text += "   ";
                        if (!string.IsNullOrWhiteSpace(sTitleStatusY))
                            lblParams.Text += $"{sTitleStatusY}: ";
                        lblParams.Text += $"{((float)fieldParamY.GetValue(cs.EcuParameters)).ToString(sFormatStatusY)}";
                    }

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
                        series.Points.AddXY((float)fieldParamX.GetValue(cs.EcuParameters), (float)fieldParamY.GetValue(cs.EcuParameters));
                    }
                    else if (fieldParamX != null && dep1d != null)
                    {
                        Interpolation interpolate = new Interpolation((float)fieldParamX.GetValue(cs.EcuParameters), dep1d, size);
                        series.Points.AddXY((float)fieldParamX.GetValue(cs.EcuParameters), interpolate.Interpolate1D(array1d));
                    }
                    else if (fieldParamY != null)
                    {
                        Interpolation interpolate = new Interpolation((float)fieldParamY.GetValue(cs.EcuParameters), array1d, size);
                        series.Points.AddXY((float)(interpolate.indexes[0] + interpolate.mult), (float)fieldParamY.GetValue(cs.EcuParameters));
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

        private void nudItem_ValueChanged(object sender, EventArgs e)
        {
            float[] array1d = null;
            float[] dep1d = null;

            if (!string.IsNullOrWhiteSpace(sConfigDepX))
            {
                FieldInfo fieldDepX = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sConfigDepX);
                if (fieldDepX != null)
                    dep1d = (float[])fieldDepX.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
            }
            if (!string.IsNullOrWhiteSpace(sArrayName))
            {
                FieldInfo fieldArrayX = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                if (fieldArrayX != null)
                    array1d = (float[])fieldArrayX.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
            }

            nudItem.Value = (decimal)array1d[(int)((NumericUpDown)sender).Value - 1];

            lblItemValue.Text = string.Empty;
            if (dep1d != null && !string.IsNullOrWhiteSpace(sTitleStatusX))
            {
                lblItemValue.Text = $"{sTitleStatusX}: ";
                lblItemValue.Text += $"{dep1d[(int)((NumericUpDown)sender).Value - 1].ToString(sFormatStatusX)}";
            }
        }

        private void nudValue_ValueChanged(object sender, EventArgs e)
        {
            float[] array1d = null;
            int size = 0;

            if (!string.IsNullOrWhiteSpace(sConfigSizeX))
            {
                FieldInfo fieldSize = cs.ConfigStruct.parameters.GetType().GetField(sConfigSizeX);
                if (fieldSize != null)
                    size = (int)fieldSize.GetValue(cs.ConfigStruct.parameters);
            }

            if (!string.IsNullOrWhiteSpace(sArrayName))
            {
                FieldInfo fieldArrayX = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                if (fieldArrayX != null)
                    array1d = (float[])fieldArrayX.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
            }

            if (size > 0 && array1d != null)
            {
                float value = (float)((NumericUpDown)sender).Value;
                int index = (int)nudItem.Value - 1;
                if (index > 0 && value - fMinDiff < array1d[index - 1])
                {
                    value = array1d[index - 1] + fMinDiff;
                    ((NumericUpDown)sender).Value = (decimal)value;
                }
                else if (index < size - 1 && value + fMinDiff > array1d[index + 1])
                {
                    value = array1d[index + 1] - fMinDiff;
                    ((NumericUpDown)sender).Value = (decimal)value;
                }
                array1d[index] = value;
                if (chart1DChart.Series.Count > 0)
                {
                    var result = chart1DChart.Series[0].Points.Where(n => (int)n.Tag == index);
                    if (result.FirstOrDefault() != null)
                    {
                        result.FirstOrDefault().YValues = new double[1] { value };
                    }
                }

                UpdateTableEvent?.Invoke(sender, new EventArgs());
            }

        }

        private void btnPressApply_Click(object sender, EventArgs e)
        {
            UpdateTableEvent?.Invoke(sender, new EventArgs());
        }
    }
}
