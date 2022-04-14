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
    public enum Editor2DMode
    {
        EcuTable = 0,
        CorrectionsTable
    }
    public partial class Editor2D : UserControl
    {
        private int iArraySizeX;
        private int iArraySizeY;
        private string sConfigSizeX;
        private string sConfigSizeY;
        private string sConfigDepX;
        private string sConfigDepY;
        private string sParamsStatusX;
        private string sParamsStatusY;
        private string sParamsStatusD;
        private string sTitleStatusX;
        private string sTitleStatusY;
        private string sTitleStatusD;
        private string sFormatStatusX;
        private string sFormatStatusY;
        private string sFormatStatusD;
        private string sArrayName;
        private string sCalibrationTable;
        private double dStepSize;
        private double dIntervalX;
        private double dIntervalY;
        private double dChartMinY;
        private double dChartMaxY;
        private double dMinDiffX;
        private double dMinDiffY;
        private double dMinY;
        private double dMaxY;
        private int iSizeX;
        private int iSizeY;
        private bool bLog10;
        private Editor2DMode eMode;

        private ColorTransience ColorTransience = null;
        private ColorTransience CalibrationColorTransience = null;

        private EventHandler UpdateTableEvent = null;

        private class ForcePosition
        {
            public int x;
            public int y;
            public Color color;
        }

        private List<ForcePosition> forcePositions = null;

        private ComponentStructure cs;
        
        List<int> tlpRows = new List<int>();
        List<int> tlpColumns = new List<int>();
        private int tlpInfoRow = 0;
        private int tlpInfoColumn = 0;
    
        public Editor2D()
        {
            InitializeComponent();
            chart2DChart.Series.Clear();
            chart2DChart.ChartAreas.Clear();
            chart2DChart.Legends.Clear();
            chart2DChart.Titles.Clear();
            chart2DChart.Annotations.Clear();
        }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Chart"),
        Description("The 2D chart for this control")]
        public Chart Chart
        {
            get
            {
                return chart2DChart;
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

        public void Initialize(ComponentStructure componentStructure, Editor2DMode mode, int sizex, int sizey, double min, double max, double step, double mindiffx, double mindiffy, double chartminy, double chartmaxy, double intervalx, double intervaly, int arraysizex, int arraysizey, int decplaces, bool log10 = false)
        {
            dMinY = min;
            dMaxY = max;
            dStepSize = step;
            dIntervalX = intervalx;
            dIntervalY = intervaly;
            dChartMinY = chartminy;
            dChartMaxY = chartmaxy;
            dMinDiffX = mindiffx;
            dMinDiffY = mindiffy;
            iArraySizeX = arraysizex;
            iArraySizeY = arraysizey;
            eMode = mode;

            bLog10 = log10;

            cs = componentStructure;
            forcePositions = new List<ForcePosition>();

            iSizeX = sizex;
            iSizeY = sizey;

            tlp2DTable.RowStyles.Clear();
            tlp2DTable.ColumnStyles.Clear();

            tlpInfoColumn = tlp2DTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 0.0f));
            tlpInfoRow = tlp2DTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 0.0f));
            for (int y = 0; y < iArraySizeY; y++)
                tlpRows.Add(tlp2DTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 0.0f)));
            for (int x = 0; x < iArraySizeX; x++)
                tlpColumns.Add(tlp2DTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 0.0f)));
            tlp2DTable.ColumnCount = iArraySizeX + 1;
            tlp2DTable.RowCount = iArraySizeY + 1;

            CalibrationColorTransience = new ColorTransience(0.0f, 1.0f, Color.Black);
            CalibrationColorTransience.Add(Color.FromArgb(255, 0, 0), 0.0f);
            CalibrationColorTransience.Add(Color.FromArgb(0, 192, 0), 1.0f);


            for (int y = -1; y < iArraySizeY; y++)
            {
                for (int x = -1; x < iArraySizeX; x++)
                {
                    if (x >= 0 && y >= 0)
                    {
                        NumericUpDown nud = new NumericUpDown();
                        nud.Margin = new Padding(0);
                        nud.Minimum = (decimal)dMinY;
                        nud.Maximum = (decimal)dMaxY;
                        nud.DecimalPlaces = decplaces;
                        nud.Increment = (decimal)dStepSize;
                        nud.Tag = y * iArraySizeX + x;
                        nud.Value = (decimal)((dMaxY - dMinY) / 2);
                        nud.ValueChanged += nudTableItem_ValueChanged;
                        nud.Location = new Point(0, 0);
                        nud.Dock = DockStyle.Fill;
                        nud.Visible = false;
                        tlp2DTable.Controls.Add(nud, tlpColumns[x], tlpRows[y]);
                    }
                    else
                    {
                        Label lbl = new Label();
                        lbl.Dock = DockStyle.Fill;
                        lbl.TextAlign = x != -1 ? ContentAlignment.MiddleCenter : ContentAlignment.MiddleLeft;
                        lbl.AutoSize = true;
                        if ((x == -1 && y != -1) || (y == -1 && x != -1))
                        {
                            lbl.Visible = false;
                            lbl.Text = "0";
                            if (x != -1)
                            {
                                lbl.Tag = 0 * (iArraySizeX + 1) + (x + 1);
                                tlp2DTable.Controls.Add(lbl, tlpColumns[x], tlpInfoRow);
                            }
                            else if (y != -1)
                            {
                                lbl.Tag = (y + 1) * (iArraySizeX + 1) + 0;
                                tlp2DTable.Controls.Add(lbl, tlpInfoColumn, tlpRows[y]);
                            }
                        }
                        else if (x == -1 && y == -1)
                        {
                            lbl.Text = @"Pa\RPM";
                            lbl.Visible = true;
                            lbl.Tag = null;
                            tlp2DTable.Controls.Add(lbl, tlpInfoColumn, tlpInfoRow);
                        }
                    }
                }
            }
        }

        public void SetTableEventHandler(EventHandler eventHandler)
        {
            UpdateTableEvent = eventHandler;
        }

        public void SetConfig(string arrayname, string sizex, string sizey, string depx, string depy)
        {
            sArrayName = arrayname;
            sConfigSizeX = sizex;
            sConfigSizeY = sizey;
            sConfigDepX = depx;
            sConfigDepY = depy;
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

        public void SetD(string param, string title, string format)
        {
            sParamsStatusD = param;
            sTitleStatusD = title;
            sFormatStatusD = format;
        }

        public void SetCalibrationTable(string table)
        {
            sCalibrationTable = table;
        }

        public void SetTableColorTrans(ColorTransience colorTransience)
        {
            ColorTransience = colorTransience;
        }

        public void SetForceColor(int x, int y, Color color)
        {
            IEnumerable<ForcePosition> en = forcePositions.Where(c => c.x == x && c.y == y);
            if (en == null || en.Count() == 0)
            {
                forcePositions.Add(new ForcePosition { x = x, y = y, color = color });
            }
            else
            {
                foreach (ForcePosition pos in en)
                    pos.color = color;
            }
        }

        public void DeleteForceColor(int x, int y)
        {
            IEnumerable<ForcePosition> en = forcePositions.Where(c => c.x == x && c.y == y);
            if (en != null)
                foreach (ForcePosition pos in en)
                    forcePositions.Remove(pos);
        }

        public void SynchronizeChart()
        {
            Series series;

            int sizex = 0;
            int sizey = 0;

            float[] depx = null;
            float[] depy = null;
            float[] array2d = null;

            float paramx = 0;
            float paramy = 0;
            float paramd = 0;


            if (!string.IsNullOrWhiteSpace(sConfigSizeX))
            {
                FieldInfo fieldSizeX = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sConfigSizeX);
                if (fieldSizeX != null)
                    sizex = (int)fieldSizeX.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
            }

            if (!string.IsNullOrWhiteSpace(sConfigSizeY))
            {
                FieldInfo fieldSizeY = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sConfigSizeY);
                if (fieldSizeY != null)
                    sizey = (int)fieldSizeY.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
            }

            if (!string.IsNullOrWhiteSpace(sArrayName))
            {
                FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                if (fieldArray != null)
                {
                    if (eMode == Editor2DMode.EcuTable)
                        array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                    else if (eMode == Editor2DMode.CorrectionsTable)
                        array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.corrections);
                }
            }

            if (!string.IsNullOrWhiteSpace(sConfigDepX))
            {
                FieldInfo fieldDepX = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sConfigDepX);
                if (fieldDepX != null)
                    depx = (float[])fieldDepX.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
            }

            if (!string.IsNullOrWhiteSpace(sConfigDepY))
            {
                FieldInfo fieldDepY = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sConfigDepY);
                if (fieldDepY != null)
                    depy = (float[])fieldDepY.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
            }

            lblParams.Text = string.Empty;
            if (!string.IsNullOrWhiteSpace(sParamsStatusX))
            {
                FieldInfo fieldParamX = cs.EcuParameters.GetType().GetField(sParamsStatusX);
                if (fieldParamX != null)
                {
                    paramx = (float)fieldParamX.GetValue(cs.EcuParameters);

                    if (!string.IsNullOrWhiteSpace(lblParams.Text))
                        lblParams.Text += "  ";
                    if (!string.IsNullOrWhiteSpace(sTitleStatusX))
                        lblParams.Text += $"{sTitleStatusX}: ";
                    lblParams.Text += $"{paramx.ToString(sFormatStatusX)}";
                }
            }

            if (!string.IsNullOrWhiteSpace(sParamsStatusY))
            {
                FieldInfo fieldParamY = cs.EcuParameters.GetType().GetField(sParamsStatusY);
                if (fieldParamY != null)
                {
                    paramy = (float)fieldParamY.GetValue(cs.EcuParameters);

                    if (!string.IsNullOrWhiteSpace(lblParams.Text))
                        lblParams.Text += "  ";
                    if (!string.IsNullOrWhiteSpace(sTitleStatusY))
                        lblParams.Text += $"{sTitleStatusY}: ";
                    lblParams.Text += $"{paramy.ToString(sFormatStatusY)}";
                }
            }

            if (!string.IsNullOrWhiteSpace(sParamsStatusD))
            {
                FieldInfo fieldParamD = cs.EcuParameters.GetType().GetField(sParamsStatusD);
                if (fieldParamD != null)
                {
                    paramd = (float)fieldParamD.GetValue(cs.EcuParameters);

                    if (!string.IsNullOrWhiteSpace(lblParams.Text))
                        lblParams.Text += "  ";
                    if (!string.IsNullOrWhiteSpace(sTitleStatusD))
                        lblParams.Text += $"{sTitleStatusD}: ";
                    lblParams.Text += $"{paramd.ToString(sFormatStatusD)}";
                }
            }

            if (depx != null && depy != null)
            {

                tlp2DTable.SuspendLayout();
                tlp2DTable.RowStyles[tlpInfoRow] = new RowStyle(SizeType.AutoSize);
                tlp2DTable.ColumnStyles[tlpInfoColumn] = new ColumnStyle(SizeType.AutoSize);

                for (int y = 0; y < sizey; y++)
                    tlp2DTable.RowStyles[tlpRows[y]] = new RowStyle(SizeType.Percent, 10F);
                for (int y = sizey; y < iArraySizeY; y++)
                    tlp2DTable.RowStyles[tlpRows[y]] = new RowStyle(SizeType.Absolute, 0.0f);
                for (int x = 0; x < sizex; x++)
                    tlp2DTable.ColumnStyles[tlpColumns[x]] = new ColumnStyle(SizeType.Percent, 10.0f);
                for (int x = sizex; x < iArraySizeX; x++)
                    tlp2DTable.ColumnStyles[tlpColumns[x]] = new ColumnStyle(SizeType.Absolute, 0.0f);

                foreach (Control control in tlp2DTable.Controls)
                {
                    if (control.Tag != null)
                    {

                        if (control.GetType() == typeof(NumericUpDown))
                        {
                            int x = ((int)control.Tag) % (iArraySizeX);
                            int y = ((int)control.Tag) / (iArraySizeX);
                            control.Visible = x < sizex && y < sizey;

                            NumericUpDown nud = (NumericUpDown)control;
                            nud.Value = (decimal)array2d[(int)nud.Tag];
                        }
                        else if (control.GetType() == typeof(Label))
                        {
                            int x = ((int)control.Tag) % (iArraySizeX + 1);
                            int y = ((int)control.Tag) / (iArraySizeX + 1);
                            control.Visible = (x - 1) < sizex && (y - 1) < sizey;

                            Label lbl = (Label)control;
                            if (x == 0 && y != 0) lbl.Text = depy[y - 1].ToString(sFormatStatusD);
                            else if (y == 0 && x != 0) lbl.Text = depx[x - 1].ToString(sFormatStatusX);
                        }
                    }
                }

                tlp2DTable.ResumeLayout();

                Color min = Color.SpringGreen;
                Color max = Color.IndianRed;

                double chartMin = dChartMinY;
                double chartMax = dChartMaxY;
                double chartMaxX = double.MinValue;

                chart2DChart.Series.Clear();
                chart2DChart.ChartAreas[0].AxisX.Minimum = depx[0] - (depx[0] % dMinDiffX);
                chart2DChart.ChartAreas[0].AxisX.Maximum = depx[sizex - 1] + (dMinDiffX - (depx[sizex - 1] % dMinDiffX));

                chart2DChart.ChartAreas[0].AxisY.Minimum = chartMin;
                chart2DChart.ChartAreas[0].AxisY.Maximum = chartMax;

                chart2DChart.ChartAreas[0].AxisX.Interval = dIntervalX;
                chart2DChart.ChartAreas[0].AxisY.Interval = dIntervalY;

                chart2DChart.ChartAreas[0].AxisX.MajorGrid.Interval = dIntervalX;
                chart2DChart.ChartAreas[0].AxisY.MajorGrid.Interval = dIntervalY;

                chart2DChart.ChartAreas[0].AxisX.LabelStyle.Interval = dIntervalX;
                chart2DChart.ChartAreas[0].AxisY.LabelStyle.Interval = dIntervalY;

                chart2DChart.ChartAreas[0].AxisX.MajorTickMark.Interval = dIntervalX;
                chart2DChart.ChartAreas[0].AxisY.MajorTickMark.Interval = dIntervalY;

                if (sizey > 0 && sizex > 0)
                {
                    for (int i = 0; i < sizey; i++)
                    {
                        series = chart2DChart.Series.Add(depy[i].ToString(sFormatStatusD));
                        series.Tag = i;
                        series.ChartType = SeriesChartType.Line;
                        series.XAxisType = AxisType.Primary;
                        series.XValueType = ChartValueType.Single;
                        series.YAxisType = AxisType.Primary;
                        series.YValueType = ChartValueType.Single;
                        series.BorderWidth = 2;
                        float trans = (float)i / (float)(sizey - 1);
                        series.Color = Color.FromArgb((int)(min.R * (1.0f - trans) + max.R * trans), (int)(min.G * (1.0f - trans) + max.G * trans), (int)(min.B * (1.0f - trans) + max.B * trans));
                        for (int j = 0; j < sizex; j++)
                        {
                            float value = array2d[i * iArraySizeX + j];
                            if (chartMaxX < depx[j])
                                chartMaxX = depx[j];
                            int point = series.Points.AddXY(depx[j], value);
                            series.Points[point].Tag = j;
                            if (value > chartMax)
                                chartMax = value;
                            if (value < chartMin)
                                chartMin = value;
                        }
                    }
                    chart2DChart.ChartAreas[0].AxisY.Minimum = (chartMin - (chartMin % dMinDiffY));
                    chart2DChart.ChartAreas[0].AxisY.Maximum = (chartMax + (dMinDiffY - (chartMax % dMinDiffY)));
                    

                }

                series = chart2DChart.Series.Add("Current");
                series.ChartType = SeriesChartType.Point;
                series.XAxisType = AxisType.Primary;
                series.XValueType = ChartValueType.Single;
                series.YAxisType = AxisType.Primary;
                series.YValueType = ChartValueType.Single;
                series.MarkerSize = 8;
                series.Color = Color.Red;
                series.MarkerStyle = MarkerStyle.Circle;
                series.Points.AddXY(paramx, paramy);
                
                if(chartMaxX != double.MinValue)
                {
                    chart2DChart.ChartAreas[0].AxisX.IsLogarithmic = bLog10;
                    chart2DChart.ChartAreas[0].AxisX.LogarithmBase = 10;
                }
                else
                {
                    chart2DChart.ChartAreas[0].AxisX.IsLogarithmic = bLog10;
                }
            }
        }

        public void UpdateChart()
        {
            int sizex = 0;
            int sizey = 0;

            float[] depx = null;
            float[] depy = null;
            float[] array2d = null;
            float[] arraycalib = null;

            float paramx = 0;
            float paramy = 0;
            float paramd = 0;


            if (!string.IsNullOrWhiteSpace(sConfigSizeX))
            {
                FieldInfo fieldSizeX = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sConfigSizeX);
                if (fieldSizeX != null)
                    sizex = (int)fieldSizeX.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
            }

            if (!string.IsNullOrWhiteSpace(sConfigSizeY))
            {
                FieldInfo fieldSizeY = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sConfigSizeY);
                if (fieldSizeY != null)
                    sizey = (int)fieldSizeY.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
            }

            if (!string.IsNullOrWhiteSpace(sArrayName))
            {
                FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                if (fieldArray != null)
                {
                    if (eMode == Editor2DMode.EcuTable)
                        array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                    else if (eMode == Editor2DMode.CorrectionsTable)
                        array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.corrections);
                }
            }

            if (!string.IsNullOrWhiteSpace(sConfigDepX))
            {
                FieldInfo fieldDepX = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sConfigDepX);
                if (fieldDepX != null)
                    depx = (float[])fieldDepX.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
            }

            if (!string.IsNullOrWhiteSpace(sConfigDepY))
            {
                FieldInfo fieldDepY = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sConfigDepY);
                if (fieldDepY != null)
                    depy = (float[])fieldDepY.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
            }

            lblParams.Text = string.Empty;
            if (!string.IsNullOrWhiteSpace(sParamsStatusX))
            {
                FieldInfo fieldParamX = cs.EcuParameters.GetType().GetField(sParamsStatusX);
                if (fieldParamX != null)
                {
                    paramx = (float)fieldParamX.GetValue(cs.EcuParameters);

                    if (!string.IsNullOrWhiteSpace(lblParams.Text))
                        lblParams.Text += "  ";
                    if (!string.IsNullOrWhiteSpace(sTitleStatusX))
                        lblParams.Text += $"{sTitleStatusX}: ";
                    lblParams.Text += $"{paramx.ToString(sFormatStatusX)}";
                }
            }

            if (!string.IsNullOrWhiteSpace(sParamsStatusY))
            {
                FieldInfo fieldParamY = cs.EcuParameters.GetType().GetField(sParamsStatusY);
                if (fieldParamY != null)
                {
                    paramy = (float)fieldParamY.GetValue(cs.EcuParameters);

                    if (!string.IsNullOrWhiteSpace(lblParams.Text))
                        lblParams.Text += "  ";
                    if (!string.IsNullOrWhiteSpace(sTitleStatusY))
                        lblParams.Text += $"{sTitleStatusY}: ";
                    lblParams.Text += $"{paramy.ToString(sFormatStatusY)}";
                }
            }

            if (!string.IsNullOrWhiteSpace(sParamsStatusD))
            {
                FieldInfo fieldParamD = cs.EcuParameters.GetType().GetField(sParamsStatusD);
                if (fieldParamD != null)
                {
                    paramd = (float)fieldParamD.GetValue(cs.EcuParameters);

                    if (!string.IsNullOrWhiteSpace(lblParams.Text))
                        lblParams.Text += "  ";
                    if (!string.IsNullOrWhiteSpace(sTitleStatusD))
                        lblParams.Text += $"{sTitleStatusD}: ";
                    lblParams.Text += $"{paramd.ToString(sFormatStatusD)}";
                }
            }

            int seriescount = chart2DChart.Series.Count;
            if (seriescount > 0 && chart2DChart.Series[0].Points.Count > 0)
            {
                chart2DChart.Series[seriescount - 1].Points[0].XValue = paramx;
                chart2DChart.Series[seriescount - 1].Points[0].YValues[0] = paramy;
            }

            if (depx != null && depy != null)
            {
                Interpolation interpolationX = new Interpolation(paramx, depx, iArraySizeX);
                Interpolation interpolationY = new Interpolation(paramd, depy, iArraySizeY);


                if (sizex > 0 && sizey > 0)
                {
                    if (!string.IsNullOrWhiteSpace(sCalibrationTable))
                    {
                        FieldInfo calibrationTable = cs.ConfigStruct.corrections.GetType().GetField(sCalibrationTable);
                        if (calibrationTable != null)
                            arraycalib = (float[])calibrationTable.GetValue(cs.ConfigStruct.corrections);
                    }

                    for (int i = 0; i < tlp2DTable.Controls.Count; i++)
                    {
                        if (tlp2DTable.Controls[i] is NumericUpDown)
                        {
                            NumericUpDown nud = (NumericUpDown)tlp2DTable.Controls[i];

                            int r, g, b;
                            int index = (int)nud.Tag;
                            Color color;
                            Color original;
                            double[] mult = new double[4];
                            bool handled = false;

                            if (!string.IsNullOrWhiteSpace(sCalibrationTable))
                            {
                                if (arraycalib == null)
                                {
                                    original = Color.DarkGray;
                                }
                                else
                                {
                                    original = CalibrationColorTransience.Get(arraycalib[index]);
                                }
                            }
                            else
                            {
                                original = ColorTransience.Get((float)nud.Value);
                            }

                            for (int y = 0; y < 2; y++)
                            {
                                for (int x = 0; x < 2; x++)
                                {
                                    double value;
                                    if (x == 0 && y == 0)
                                        value = (1.0 - interpolationX.mult) * (1.0f - interpolationY.mult);
                                    else if (x == 0 && y != 0)
                                        value = (1.0 - interpolationX.mult) * interpolationY.mult;
                                    else if (x != 0 && y == 0)
                                        value = interpolationX.mult * (1.0f - interpolationY.mult);
                                    else
                                        value = interpolationX.mult * interpolationY.mult;
                                    mult[y * 2 + x] = value;
                                }
                            }

                            for (int y = 0; y < 2; y++)
                            {
                                for (int x = 0; x < 2; x++)
                                {
                                    if (index == interpolationY.indexes[y] * iArraySizeX + interpolationX.indexes[x])
                                    {
                                        color = Color.DarkGray;

                                        r = (int)((color.R - original.R) * mult[y * 2 + x] + original.R);
                                        g = (int)((color.G - original.G) * mult[y * 2 + x] + original.G);
                                        b = (int)((color.B - original.B) * mult[y * 2 + x] + original.B);

                                        nud.BackColor = Color.FromArgb(r, g, b);
                                        nud.ForeColor = Color.White;
                                        if(mult[y * 2 + x] == mult.Max() || mult[y * 2 + x] > 0.35)
                                            nud.Font = new Font(nud.Font, FontStyle.Bold);
                                        else
                                            nud.Font = new Font(nud.Font, FontStyle.Regular);
                                        handled = true;
                                    }
                                }
                            }
                            if (!handled && nud.BackColor != original)
                            {
                                nudTableItem_ValueChanged(nud, new EventArgs());
                            }

                        }
                    }
                }
            }
        }

        private void nudTableItem_ValueChanged(object sender, EventArgs e)
        { 
            NumericUpDown nud = (NumericUpDown)sender;
            int index = (int)nud.Tag;
            int x = index % iArraySizeX;
            int y = index / iArraySizeX;
            float value = (float)nud.Value;
            Color text = Color.Black;
            Color back = Color.White;

            float[] array2d = null;
            
            if (!string.IsNullOrWhiteSpace(sArrayName))
            {
                FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                if (fieldArray != null)
                {
                    if (eMode == Editor2DMode.EcuTable)
                        array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                    else if (eMode == Editor2DMode.CorrectionsTable)
                        array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.corrections);
                }
            }

            if (chart2DChart.Series.Count > y && chart2DChart.Series[y].Points.Count > x)
            {
                chart2DChart.Series[y].Points[x].YValues = new double[1] { value };
            }

            //TODO: check if need to check if IsSynchronizing
            if (array2d[index] != value)
            {
                array2d[index] = value;
                UpdateTableEvent?.Invoke(sender, new EventArgs());
            }

            if (ColorTransience != null || !string.IsNullOrWhiteSpace(sCalibrationTable))
            {
                text = Color.White;
                if(string.IsNullOrWhiteSpace(sCalibrationTable))
                    back = ColorTransience.Get(value);
            }

            nud.ForeColor = text;
            nud.BackColor = back;
            nud.Font = new Font(nud.Font, FontStyle.Regular);

            double chartMin = dChartMinY;
            double chartMax = dChartMaxY;

            for (int i = 0; i < chart2DChart.Series.Count; i++)
            {
                for (int j = 0; j < chart2DChart.Series[i].Points.Count; j++)
                {
                    value = (float)chart2DChart.Series[i].Points[j].YValues[0];
                    if (value > chartMax)
                        chartMax = value;
                    if (value < chartMin)
                        chartMin = value;
                }
            }

            if (chartMin != dMinY && chartMax != dMaxY)
            {
                chart2DChart.ChartAreas[0].AxisY.Minimum = (chartMin - (chartMin % dMinDiffY));
                chart2DChart.ChartAreas[0].AxisY.Maximum = (chartMax + (dMinDiffY - (chartMax % dMinDiffY)));
            }
        }

    }
}
