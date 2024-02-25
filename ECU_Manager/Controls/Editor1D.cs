using ECU_Framework.Tools;
using ECU_Manager.Dialogs;
using ECU_Framework.Structs;
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
using System.Windows.Input;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace ECU_Manager.Controls
{
    public partial class Editor1D : UserControl
    {
        private int iDecPlaces;
        private int iArraySizeX;
        private string sConfigDepX;
        private string sParamsStatusX;
        private string sParamsStatusY;
        private string sTitleStatusX;
        private string sTitleStatusY;
        private string sFormatStatusX;
        private string sFormatStatusY;
        private string sArrayName;
        private double dMinY;
        private double dMaxY;
        private double dStepSize;
        private double dIntervalX;
        private double dIntervalY;
        private double dChartMinY;
        private double dChartMaxY;
        private double dMinDiff;
        private bool bLog10;
        private EventHandler UpdateTableEvent = null;

        private ComponentStructure cs;

        private bool bSelected = false;
        private bool bSelecting = false;
        private Point pSelectionStart = new Point();
        private Point pSelectionEnd = new Point();

        private double dValueStartX;
        private double dValueStartY;
        private double dValueEndX;
        private double dValueEndY;

        private int[] iSelectedIndexes = new int[0];
        private DateTime dtSelectedLast = DateTime.Now;
        private int iPrevDataPoint = -1;

        public Editor1D()
        {
            InitializeComponent();
            chart1DChart.Series.Clear();
            chart1DChart.ChartAreas.Clear();
            chart1DChart.Legends.Clear();
            chart1DChart.Titles.Clear();
            chart1DChart.Annotations.Clear();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Chart"),
        Description("The 1D chart for this control")]
        public Chart Chart
        {
            get
            {
                return chart1DChart;
            }
        }

        [Category("Chart"),
        Description("The title for this control")]
        public string LabelTitle
        {
            get
            {
                return lblTitle.Text;
            }
            set
            {
                lblTitle.Text = value;
            }
        }

        public void Initialize(ComponentStructure componentStructure, int size, double min, double max, double step, double mindiff, double chartminy, double chartmaxy, double intervalx, double intervaly, int decplaces, bool log10 = false)
        {
            dMinY = min;
            dMaxY = max;
            dStepSize = step;
            dIntervalX = intervalx;
            dIntervalY = intervaly;
            dChartMinY = chartminy;
            dChartMaxY = chartmaxy;
            dMinDiff = mindiff;
            iDecPlaces = decplaces;
            iArraySizeX = size;

            bLog10 = log10;

            nudValue.DecimalPlaces = decplaces;

            chart1DChart.MouseWheel += chart1DChart_MouseWheel;

            cs = componentStructure;
            
        }

        public void SetTableEventHandler(EventHandler eventHandler)
        {
            UpdateTableEvent = eventHandler;
        }

        public void SetConfig(string arrayname, string depx)
        {
            sArrayName = arrayname;
            sConfigDepX = depx;
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
            try
            {
                EcuParamTransform transform = EcuParamTransform.Default;
                EcuParamTransform transformdep = EcuParamTransform.Default;
                Array array = null;
                Array arraydep = null;
                float[] array1d = null;
                float[] dep1d = null;
                double min, max;
                bool update = false;

                double chartMin = dChartMinY;
                double chartMax = dChartMaxY;
                
                if (!string.IsNullOrWhiteSpace(sConfigDepX))
                {
                    FieldInfo fieldDepX = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sConfigDepX);
                    FieldInfo fieldDepTransformX = cs.ConfigStruct.tables[cs.CurrentTable].transform.GetType().GetField(sConfigDepX);
                    if (fieldDepX != null && fieldDepTransformX != null)
                    {
                        transformdep = (EcuParamTransform)fieldDepTransformX.GetValue(cs.ConfigStruct.tables[cs.CurrentTable].transform);
                        arraydep = (Array)fieldDepX.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                        dep1d = EcuConfigTransform.FromInteger(arraydep, transformdep);
                    }
                }

                if (!string.IsNullOrWhiteSpace(sArrayName))
                {
                    FieldInfo fieldArrayX = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                    FieldInfo fieldTransformX = cs.ConfigStruct.tables[cs.CurrentTable].transform.GetType().GetField(sArrayName);
                    if (fieldArrayX != null && fieldTransformX != null)
                    {
                        transform = (EcuParamTransform)fieldTransformX.GetValue(cs.ConfigStruct.tables[cs.CurrentTable].transform);
                        array = (Array)fieldArrayX.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                        array1d = EcuConfigTransform.FromInteger(array, transform);
                    }

                    if (dStepSize < transform.gain)
                    {
                        dStepSize = transform.gain;
                    }
                }

                if (chart1DChart.Series.Count == 0 || chart1DChart.Series[0].Points.Count != iArraySizeX)
                {
                    update = true;
                }
                else
                {
                    for(int i = 0; i < iArraySizeX; i++)
                    {
                        if(array1d[i] != (float)chart1DChart.Series[0].Points[i].YValues[0])
                        {
                            update = true;
                            break;
                        }
                    }
                }

                string str = string.Empty;
                if (update)
                {
                    chart1DChart.Series.Clear();
                    if (array1d != null)
                    {
                        chart1DChart.Series.Add("Config");
                        chart1DChart.Series[0].XAxisType = AxisType.Primary;
                        chart1DChart.Series[0].YAxisType = AxisType.Primary;
                        chart1DChart.Series[0].YValueType = ChartValueType.Single;
                        chart1DChart.Series[0].MarkerSize = 8;
                        chart1DChart.Series[0].BorderWidth = 3;
                        chart1DChart.Series[0].MarkerColor = Color.White;
                        chart1DChart.Series[0].MarkerStyle = MarkerStyle.Circle;
                        chart1DChart.Series[0].ChartType = SeriesChartType.Line;
                        chart1DChart.Series[0].IsValueShownAsLabel = true;
                        chart1DChart.Series[0].LabelFormat = $"F{nudValue.DecimalPlaces}";
                        chart1DChart.Series[0].LabelForeColor = Color.White;

                        min = array1d.Take(iArraySizeX).Min();
                        max = array1d.Take(iArraySizeX).Max();

                        if (min > dChartMinY)
                            min = dChartMinY;

                        if (max < dChartMaxY)
                            max = dChartMaxY;

                        chart1DChart.ChartAreas[0].AxisX.Interval = dIntervalX;
                        chart1DChart.ChartAreas[0].AxisY.Interval = dIntervalY;

                        chart1DChart.ChartAreas[0].AxisX.MajorGrid.Interval = dIntervalX;
                        chart1DChart.ChartAreas[0].AxisY.MajorGrid.Interval = dIntervalY;

                        chart1DChart.ChartAreas[0].AxisX.LabelStyle.Interval = dIntervalX;
                        chart1DChart.ChartAreas[0].AxisY.LabelStyle.Interval = dIntervalY;

                        chart1DChart.ChartAreas[0].AxisX.MajorTickMark.Interval = dIntervalX;
                        chart1DChart.ChartAreas[0].AxisY.MajorTickMark.Interval = dIntervalY;

                        nudItem.Minimum = 1;
                        nudItem.Maximum = iArraySizeX;

                        nudValue.Value = (decimal)array1d[(int)nudItem.Value - 1];
                        nudValue.Minimum = (decimal)dMinY;
                        nudValue.Maximum = (decimal)dMaxY;
                        nudValue.Increment = (decimal)dStepSize;



                        if (dep1d == null)
                        {
                            chart1DChart.Series[0].XValueType = ChartValueType.Int32;
                            chart1DChart.ChartAreas[0].AxisX.Minimum = 1;
                            chart1DChart.ChartAreas[0].AxisX.Maximum = iArraySizeX;

                            for (int i = 0; i < iArraySizeX; i++)
                            {
                                int point = chart1DChart.Series[0].Points.AddXY(i + 1, array1d[i]);
                                if ((DateTime.Now - dtSelectedLast).TotalMilliseconds % 1000 < 500 && iSelectedIndexes.Contains(point))
                                    chart1DChart.Series[0].Points[point].MarkerColor = Color.FromArgb(0, 96, 0);
                                chart1DChart.Series[0].Points[point].Tag = i;

                                if (array1d[i] > chartMax)
                                    chartMax = array1d[i];
                                if (array1d[i] < chartMin)
                                    chartMin = array1d[i];

                                if (i == nudItem.Value - 1)
                                {
                                    chart1DChart.Series[0].Points[point].MarkerStyle = MarkerStyle.Cross;
                                    chart1DChart.Series[0].Points[point].MarkerSize = 15;
                                }

                                if (iPrevDataPoint >= 0 && iPrevDataPoint == point)
                                {
                                    chart1DChart.Series[0].Points[point].MarkerSize = 16;
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(sTitleStatusX))
                            {
                                str = $"{sTitleStatusX}: ";
                                str += $"{dep1d[(int)nudItem.Value - 1].ToString(sFormatStatusX)}";
                            }

                            chart1DChart.Series[0].XValueType = ChartValueType.Single;
                            chart1DChart.ChartAreas[0].AxisX.Minimum = dep1d[0];
                            chart1DChart.ChartAreas[0].AxisX.Maximum = dep1d[iArraySizeX - 1];
                            for (int i = 0; i < iArraySizeX; i++)
                            {
                                int point = chart1DChart.Series[0].Points.AddXY(dep1d[i], array1d[i]);
                                if ((DateTime.Now - dtSelectedLast).TotalMilliseconds % 1000 < 500 && iSelectedIndexes.Contains(point))
                                    chart1DChart.Series[0].Points[point].MarkerColor = Color.FromArgb(0, 96, 0);
                                chart1DChart.Series[0].Points[point].Tag = i;

                                if (array1d[i] > chartMax)
                                    chartMax = array1d[i];
                                if (array1d[i] < chartMin)
                                    chartMin = array1d[i];

                                if (i == nudItem.Value - 1)
                                {
                                    chart1DChart.Series[0].Points[point].MarkerStyle = MarkerStyle.Cross;
                                    chart1DChart.Series[0].Points[point].MarkerSize = 15;
                                }
                            }
                        }

                        chart1DChart.ChartAreas[0].AxisY.Minimum = (chartMin - (chartMin % dMinDiff));
                        chart1DChart.ChartAreas[0].AxisY.Maximum = (chartMax + (dMinDiff - (chartMax % dMinDiff)));
                        

                        nudValue.Enabled = true;
                        nudItem.Enabled = true;

                        if (!lblItemValue.Text.Equals(str))
                            lblItemValue.Text = str;
                    }
                    else
                    {
                        nudValue.Enabled = false;
                        nudItem.Enabled = false;
                    }
                }

                str = string.Empty;
                if (!string.IsNullOrWhiteSpace(sParamsStatusX) || !string.IsNullOrWhiteSpace(sParamsStatusY))
                {

                    FieldInfo fieldParamX = null;
                    FieldInfo fieldParamY = null;

                    if (!string.IsNullOrWhiteSpace(sParamsStatusX))
                        fieldParamX = cs.EcuParameters.GetType().GetField(sParamsStatusX);
                    if (!string.IsNullOrWhiteSpace(sParamsStatusY))
                        fieldParamY = cs.EcuParameters.GetType().GetField(sParamsStatusY);

                    if (fieldParamX != null)
                    {
                        if (!string.IsNullOrWhiteSpace(str))
                            str += "   ";
                        if (!string.IsNullOrWhiteSpace(sTitleStatusX))
                            str += $"{sTitleStatusX}: ";
                        str += $"{((float)fieldParamX.GetValue(cs.EcuParameters)).ToString(sFormatStatusX)}";
                    }
                    if (fieldParamY != null)
                    {
                        if (!string.IsNullOrWhiteSpace(str))
                            str += "   ";
                        if (!string.IsNullOrWhiteSpace(sTitleStatusY))
                            str += $"{sTitleStatusY}: ";
                        str += $"{((float)fieldParamY.GetValue(cs.EcuParameters)).ToString(sFormatStatusY)}";
                    }

                    Series series;

                    float x = 0;
                    float y = 0;

                    if (fieldParamX != null && fieldParamY != null)
                    {
                        x = (float)fieldParamX.GetValue(cs.EcuParameters);
                        y = (float)fieldParamY.GetValue(cs.EcuParameters);
                    }
                    else if (fieldParamX != null && dep1d != null)
                    {
                        Interpolation interpolate = new Interpolation((float)fieldParamX.GetValue(cs.EcuParameters), dep1d, iArraySizeX);
                        x = (float)fieldParamX.GetValue(cs.EcuParameters);
                        y = interpolate.Interpolate1D(array1d);
                    }
                    else if (fieldParamY != null)
                    {
                        Interpolation interpolate = new Interpolation((float)fieldParamY.GetValue(cs.EcuParameters), array1d, iArraySizeX);
                        x = (float)(interpolate.indexes[0] + interpolate.mult) + 1;
                        y = (float)fieldParamY.GetValue(cs.EcuParameters);
                    }

                    if (chart1DChart.Series.Count > 1)
                    {
                        series = chart1DChart.Series[1];

                    }
                    else
                    {
                        series = chart1DChart.Series.Add("Current");
                        series.ChartType = SeriesChartType.Point;
                        series.XAxisType = AxisType.Primary;
                        series.XValueType = ChartValueType.Single;
                        series.YAxisType = AxisType.Primary;
                        series.YValueType = ChartValueType.Single;
                        series.MarkerSize = 11;
                        series.Color = Color.Red;
                        series.MarkerStyle = MarkerStyle.Circle;
                    }

                    if(series.Points.Count > 0)
                    {
                        if((float)series.Points[0].XValue != x)
                            series.Points[0].XValue = x;
                        if ((float)series.Points[0].YValues[0] != y)
                            series.Points[0].YValues = new double[1] { y };
                    }
                    else
                    {
                        series.Points.AddXY(x, y);
                    }
                }
                if (!lblParams.Text.Equals(str))
                    lblParams.Text = str;
            }
            catch (Exception ex)
            {

            }

        }

        private void nudItem_ValueChanged(object sender, EventArgs e)
        {
            EcuParamTransform transform = EcuParamTransform.Default;
            Array arraydef = null;
            float[] array1d = null;
            float[] dep1d = null;

            if (!string.IsNullOrWhiteSpace(sConfigDepX))
            {
                FieldInfo fieldDepX = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sConfigDepX);
                FieldInfo fieldDepTransformX = cs.ConfigStruct.tables[cs.CurrentTable].transform.GetType().GetField(sConfigDepX);
                if (fieldDepX != null && fieldDepTransformX != null)
                {
                    EcuParamTransform transform1 = (EcuParamTransform)fieldDepTransformX.GetValue(cs.ConfigStruct.tables[cs.CurrentTable].transform);
                    Array array1 = (Array)fieldDepX.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                    dep1d = EcuConfigTransform.FromInteger(array1, transform1);
                }
            }
            if (!string.IsNullOrWhiteSpace(sArrayName))
            {
                FieldInfo fieldArrayX = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                FieldInfo fieldTransformX = cs.ConfigStruct.tables[cs.CurrentTable].transform.GetType().GetField(sArrayName);
                if (fieldArrayX != null && fieldTransformX != null)
                {
                    transform = (EcuParamTransform)fieldTransformX.GetValue(cs.ConfigStruct.tables[cs.CurrentTable].transform);
                    arraydef = (Array)fieldArrayX.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                    array1d = EcuConfigTransform.FromInteger(arraydef, transform);
                }
            }

            foreach(DataPoint point in chart1DChart.Series[0].Points)
            {
                point.MarkerStyle = MarkerStyle.Circle;
                point.MarkerSize = 8;
            }

            chart1DChart.Series[0].Points[(int)((NumericUpDown)sender).Value - 1].MarkerStyle = MarkerStyle.Cross;
            chart1DChart.Series[0].Points[(int)((NumericUpDown)sender).Value - 1].MarkerSize = 15;

            nudValue.Value = (decimal)array1d[(int)((NumericUpDown)sender).Value - 1];

            string str = string.Empty;
            if (dep1d != null && !string.IsNullOrWhiteSpace(sTitleStatusX))
            {
                str = $"{sTitleStatusX}: ";
                str += $"{dep1d[(int)((NumericUpDown)sender).Value - 1].ToString(sFormatStatusX)}";
            }
            lblItemValue.Text = str;
        }

        private void nudValue_ValueChanged(object sender, EventArgs e)
        {
            EcuParamTransform transform = EcuParamTransform.Default;
            Array arraydef = null;
            float[] array1d = null;
            double chartMin = dChartMinY;
            double chartMax = dChartMaxY;
            
            if (!string.IsNullOrWhiteSpace(sArrayName))
            {
                FieldInfo fieldArrayX = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                FieldInfo fieldTransformX = cs.ConfigStruct.tables[cs.CurrentTable].transform.GetType().GetField(sArrayName);
                if (fieldArrayX != null && fieldTransformX != null)
                {
                    transform = (EcuParamTransform)fieldTransformX.GetValue(cs.ConfigStruct.tables[cs.CurrentTable].transform);
                    arraydef = (Array)fieldArrayX.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                    array1d = EcuConfigTransform.FromInteger(arraydef, transform);
                }
            }

            if (iArraySizeX > 0 && array1d != null)
            {
                float value = (float)((NumericUpDown)sender).Value;
                int index = (int)nudItem.Value - 1;

                if (string.IsNullOrWhiteSpace(sParamsStatusX))
                {
                    if (index > 0 && value - dMinDiff < array1d[index - 1])
                    {
                        value = (float)(array1d[index - 1] + dMinDiff);
                    }
                    else if (index < iArraySizeX - 1 && value + dMinDiff > array1d[index + 1])
                    {
                        value = (float)(array1d[index + 1] - dMinDiff);
                    }
                }

                if (array1d[index] != value)
                {
                    array1d[index] = value;
                    EcuConfigTransform.SingleToInteger(arraydef, transform, index, value);

                    ((NumericUpDown)sender).Value = (decimal)value;

                    //TODO: check if need to check if IsSynchronizing
                    UpdateTableEvent?.Invoke(sender, new EventArgs());
                }

                if (chart1DChart.Series.Count > 0)
                {
                    var result = chart1DChart.Series[0].Points.Where(n => (int)n.Tag == index);
                    if (result.FirstOrDefault() != null)
                    {
                        result.FirstOrDefault().YValues = new double[1] { value };
                    }
                }

                for (int i = 0; i < iArraySizeX; i++)
                {
                    if (array1d[i] > chartMax)
                        chartMax = array1d[i];
                    if (array1d[i] < chartMin)
                        chartMin = array1d[i];
                }

                chart1DChart.ChartAreas[0].AxisY.Minimum = (chartMin - (chartMin % dMinDiff));
                chart1DChart.ChartAreas[0].AxisY.Maximum = (chartMax + (dMinDiff - (chartMax % dMinDiff)));
            }

        }

        private void btnPressApply_Click(object sender, EventArgs e)
        {
            UpdateTableEvent?.Invoke(sender, new EventArgs());
        }

        private void chart1DChart_Paint(object sender, PaintEventArgs e)
        {
            DataPoint[] points;
            int x1 = pSelectionStart.X;
            int y1 = pSelectionStart.Y;
            int x2 = pSelectionEnd.X;
            int y2 = pSelectionEnd.Y;

            double dx1, dy1, dx2, dy2;

            if (x1 > x2)
            {
                x1 ^= x2;
                x2 ^= x1;
                x1 ^= x2;
            }

            if (y1 > y2)
            {
                y1 ^= y2;
                y2 ^= y1;
                y1 ^= y2;
            }

            if (bSelecting)
            {
                try
                {
                    Pen pen = new Pen(Color.White, 1);
                    pen.DashPattern = new float[] { 4.0F, 2.0F, 1.0F, 3.0F };
                    Rectangle rectangle = new Rectangle(x1, y1, x2 - x1, y2 - y1);

                    e.Graphics.DrawRectangle(pen, rectangle);

                    dx1 = Chart.ChartAreas[0].AxisX.PixelPositionToValue(x1);
                    dy1 = Chart.ChartAreas[0].AxisY.PixelPositionToValue(y1);
                    dx2 = Chart.ChartAreas[0].AxisX.PixelPositionToValue(x2);
                    dy2 = Chart.ChartAreas[0].AxisY.PixelPositionToValue(y2);

                    if (dx1 > dx2)
                    {
                        dx1 = dx1 + dx2;
                        dx2 = dx1 - dx2;
                        dx1 = dx1 - dx2;
                    }

                    if (dy1 > dy2)
                    {
                        dy1 = dy1 + dy2;
                        dy2 = dy1 - dy2;
                        dy1 = dy1 - dy2;
                    }

                    dValueStartX = dx1;
                    dValueStartY = dy1;
                    dValueEndX = dx2;
                    dValueEndY = dy2;

                    points = Chart.Series[0].Points.Where(p => p.XValue >= dValueStartX && p.XValue <= dValueEndX && p.YValues[0] >= dValueStartY && p.YValues[0] <= dValueEndY).ToArray();
                    iSelectedIndexes = new int[points.Length];
                    for (int i = 0; i < points.Length; i++)
                    {
                        iSelectedIndexes[i] = Chart.Series[0].Points.IndexOf(points[i]);
                    }
                }
                catch
                {

                }

            }
            if (bSelected)
            {
                try
                {
                    for (int i = 0; i < Chart.Series[0].Points.Count; i++)
                    {
                        if (!iSelectedIndexes.Contains(i) || (DateTime.Now - dtSelectedLast).TotalMilliseconds % 1000 > 500)
                        {
                            if (Chart.Series[0].Points[i].MarkerColor != Color.White)
                                Chart.Series[0].Points[i].MarkerColor = Color.White;
                        }
                        else
                        {
                            if (Chart.Series[0].Points[i].MarkerColor != Color.FromArgb(0, 96, 0))
                                Chart.Series[0].Points[i].MarkerColor = Color.FromArgb(0, 96, 0);
                        }
                    }
                }
                catch
                {

                }
            }
        }

        private void chart1DChart_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                bSelected = true;
                bSelecting = true;
                pSelectionStart.X = e.X;
                pSelectionStart.Y = e.Y;
                pSelectionEnd.X = e.X;
                pSelectionEnd.Y = e.Y;
                dtSelectedLast = DateTime.Now;
                this.Refresh();
            }
        }

        private void chart1DChart_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                bSelecting = false;
                pSelectionEnd.X = e.X;
                pSelectionEnd.Y = e.Y;
                dtSelectedLast = DateTime.Now;
                this.Refresh();
            }
        }

        private void chart1DChart_MouseMove(object sender, MouseEventArgs e)
        {
            bool refresh = false;
            HitTestResult result = chart1DChart.HitTest(e.X, e.Y);

            if (result.ChartElementType == ChartElementType.DataPoint || result.ChartElementType == ChartElementType.DataPointLabel)
            {
                DataPoint point = (DataPoint)result.Object;
                if (chart1DChart.Series[0].Points.Contains(point))
                {
                    if (iPrevDataPoint >= 0)
                    {
                        if (nudItem.Value - 1 == iPrevDataPoint)
                            chart1DChart.Series[0].Points[iPrevDataPoint].MarkerSize = 15;
                        else chart1DChart.Series[0].Points[iPrevDataPoint].MarkerSize = 8;
                        iPrevDataPoint = -1;
                    }

                    point.MarkerSize = 16;
                    iPrevDataPoint = chart1DChart.Series[0].Points.IndexOf(point);
                    refresh = true;
                }
            }
            else
            {
                if (iPrevDataPoint >= 0)
                {
                    if (nudItem.Value - 1 == iPrevDataPoint)
                        chart1DChart.Series[0].Points[iPrevDataPoint].MarkerSize = 15;
                    else chart1DChart.Series[0].Points[iPrevDataPoint].MarkerSize = 8;
                    iPrevDataPoint = -1;
                    refresh = true;
                }
            }

            if (bSelecting)
            {
                Point point = new Point(e.X, e.Y);

                if (point.X < 0)
                    point.X = 0;
                if (point.Y < 0)
                    point.Y = 0;
                if (point.X >= chart1DChart.Size.Width)
                    point.X = chart1DChart.Size.Width - 1;
                if (point.Y >= chart1DChart.Size.Height)
                    point.Y = chart1DChart.Size.Height - 1;


                pSelectionEnd.X = point.X;
                pSelectionEnd.Y = point.Y;
                dtSelectedLast = DateTime.Now;
            }

            if(refresh)
                this.Refresh();
        }

        private void chart1DChart_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.None)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    if (e.Delta > 0)
                    {
                        decimal value = nudItem.Value;
                        value += nudItem.Increment;
                        if (value > nudItem.Maximum)
                            value = nudItem.Maximum;
                        else if (value < nudItem.Minimum)
                            value = nudItem.Minimum;
                        nudItem.Value = value;
                    }
                    else
                    {
                        decimal value = nudItem.Value;
                        value -= nudItem.Increment;
                        if (value > nudItem.Maximum)
                            value = nudItem.Maximum;
                        else if (value < nudItem.Minimum)
                            value = nudItem.Minimum;
                        nudItem.Value = value;
                    }
                }
                else
                {
                    if (e.Delta > 0)
                    {
                        decimal value = nudValue.Value;
                        value += nudValue.Increment;
                        if (value > nudValue.Maximum)
                            value = nudValue.Maximum;
                        else if (value < nudValue.Minimum)
                            value = nudValue.Minimum;
                        nudValue.Value = value;
                    }
                    else
                    {
                        decimal value = nudValue.Value;
                        value -= nudValue.Increment;
                        if (value > nudValue.Maximum)
                            value = nudValue.Maximum;
                        else if (value < nudValue.Minimum)
                            value = nudValue.Minimum;
                        nudValue.Value = value;
                    }
                }
            }
        }

        private void chart1DChart_Click(object sender, EventArgs e)
        {
            if(iPrevDataPoint >= 0)
            {
                nudItem.Value = iPrevDataPoint + 1;
            }
        }

        private void btnCopyToC_Click(object sender, EventArgs e)
        {
            float[] array1d = null;
            string text = string.Empty;
            string decplaces = string.Empty;
            
            if (!string.IsNullOrWhiteSpace(sArrayName))
            {
                FieldInfo fieldArrayX = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                FieldInfo fieldTransformX = cs.ConfigStruct.tables[cs.CurrentTable].transform.GetType().GetField(sArrayName);
                if (fieldArrayX != null && fieldTransformX != null)
                {
                    EcuParamTransform transform = (EcuParamTransform)fieldTransformX.GetValue(cs.ConfigStruct.tables[cs.CurrentTable].transform);
                    Array array = (Array)fieldArrayX.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                    array1d = EcuConfigTransform.FromInteger(array, transform);
                }
            }

            if (iArraySizeX > 0 && array1d != null)
            {
                if (iDecPlaces > 0)
                    decplaces = "." + Enumerable.Repeat("0", iDecPlaces).Aggregate((sum, next) => sum + next);

                text = "\t";
                for (int x = 0; x < iArraySizeX; x++)
                {
                    text += string.Format("{0:0" + decplaces + "}"+ (iDecPlaces > 0 ? "f" : "") +", ", array1d[x]);
                    if ((x + 1) % 8 == 0 && (x + 1) < iArraySizeX && x > 0)
                        text += "\r\n\t";
                }
                text += "\r\n";
                Clipboard.SetText(text);
            }
        }

        private void btnImportFromCCode_Click(object sender, EventArgs e)
        {
            EcuParamTransform transform = EcuParamTransform.Default;
            Array arraydef = null;
            float[] array1d = null;

            if (!string.IsNullOrWhiteSpace(sArrayName))
            {
                FieldInfo fieldArrayX = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                FieldInfo fieldTransformX = cs.ConfigStruct.tables[cs.CurrentTable].transform.GetType().GetField(sArrayName);
                if (fieldArrayX != null && fieldTransformX != null)
                {
                    transform = (EcuParamTransform)fieldTransformX.GetValue(cs.ConfigStruct.tables[cs.CurrentTable].transform);
                    arraydef = (Array)fieldArrayX.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                    array1d = EcuConfigTransform.FromInteger(arraydef, transform);
                }
            }


            if (iArraySizeX > 0 && array1d != null)
            {
                ImportCCodeForm importCCodeForm = new ImportCCodeForm(ArrayType.Array1D, array1d, iArraySizeX);

                DialogResult result = importCCodeForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    float[] output = importCCodeForm.GetResult();
                    for (int x = 0; x < iArraySizeX; x++)
                    {
                        if (output[x] > (float)dMaxY)
                            output[x] = (float)dMaxY;
                        if (output[x] < (float)dMinY)
                            output[x] = (float)dMinY;
                        array1d[x] = output[x];
                    }
                    EcuConfigTransform.ToInteger(array1d, arraydef, transform);
                    this.UpdateChart();
                    UpdateTableEvent?.Invoke(sender, new EventArgs());
                }
            }
        }
    }
}
