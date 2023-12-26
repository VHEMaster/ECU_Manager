using ECU_Framework.Structs;
using ECU_Framework.Tools;
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


using delRendererFunction = ECU_Manager.Controls.Graph3D.delRendererFunction;
using cPoint3D = ECU_Manager.Controls.Graph3D.cPoint3D;
using eRaster = ECU_Manager.Controls.Graph3D.eRaster;
using cScatter = ECU_Manager.Controls.Graph3D.cScatter;
using eNormalize = ECU_Manager.Controls.Graph3D.eNormalize;
using cMinMax3D = ECU_Manager.Controls.Graph3D.cMinMax3D;
using ECU_Manager.Dialogs;
using ECU_Manager.Classes;

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
        private int iDecPlaces;
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
        private string sProgressTable;
        private bool bCalibrationEnabled;
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

        private bool valueChangedEnabled = true;

        private class ForcePosition
        {
            public int x;
            public int y;
            public Color color;
        }

        private List<ForcePosition> forcePositions = null;

        private ComponentStructure cs;

        Color[] ColorScheme;

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
        Description("The 3D chart for this control")]
        public Graph3D Graph3D
        {
            get
            {
                return graph3D;
            }
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

        public void Initialize(ComponentStructure componentStructure, Editor2DMode mode, int sizex, int sizey, double min, double max, double step, double mindiffx, double mindiffy, double chartminy, double chartmaxy, double intervalx, double intervaly, int arraysizex, int arraysizey, int decplaces, bool reverseColorSchema = false, bool log10 = false)
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
            iDecPlaces = decplaces;
            eMode = mode;

            bLog10 = log10;

            cs = componentStructure;
            forcePositions = new List<ForcePosition>();

            iSizeX = sizex;
            iSizeY = sizey;

            CalibrationColorTransience = new ColorTransience(0.0f, 1.0f, Color.Black);
            CalibrationColorTransience.Add(Color.FromArgb(128, 0, 0), 0.0f);
            CalibrationColorTransience.Add(Color.FromArgb(255, 128, 0), 0.5f);
            CalibrationColorTransience.Add(Color.FromArgb(192, 192, 0), 0.8f);
            CalibrationColorTransience.Add(Color.FromArgb(192, 192, 0), 0.9f);
            CalibrationColorTransience.Add(Color.FromArgb(0, 128, 0), 1.0f);
            
            byte[,] u8_RGB = new byte[,] { { 0, 0, 143 }, { 0, 0, 159 }, { 0, 0, 175 }, { 0, 0, 191 }, { 0, 0, 207 }, { 0, 0, 223 }, { 0, 0, 239 }, { 0, 0, 255 }, { 0, 16, 255 }, { 0, 32, 255 }, { 0, 48, 255 }, { 0, 64, 255 }, { 0, 80, 255 }, { 0, 96, 255 }, { 0, 112, 255 }, { 0, 128, 255 }, { 0, 143, 255 }, { 0, 159, 255 }, { 0, 175, 255 }, { 0, 191, 255 }, { 0, 207, 255 }, { 0, 223, 255 }, { 0, 239, 255 }, { 0, 255, 255 }, { 16, 255, 239 }, { 32, 255, 223 }, { 48, 255, 207 }, { 64, 255, 191 }, { 80, 255, 175 }, { 96, 255, 159 }, { 112, 255, 143 }, { 128, 255, 128 }, { 143, 255, 112 }, { 159, 255, 96 }, { 175, 255, 80 }, { 191, 255, 64 }, { 207, 255, 48 }, { 223, 255, 32 }, { 239, 255, 16 }, { 255, 255, 0 }, { 255, 239, 0 }, { 255, 223, 0 }, { 255, 207, 0 }, { 255, 191, 0 }, { 255, 175, 0 }, { 255, 159, 0 }, { 255, 143, 0 }, { 255, 128, 0 }, { 255, 112, 0 }, { 255, 96, 0 }, { 255, 80, 0 }, { 255, 64, 0 }, { 255, 48, 0 }, { 255, 32, 0 }, { 255, 16, 0 }, { 255, 0, 0 }, { 239, 0, 0 }, { 223, 0, 0 }, { 207, 0, 0 }, { 191, 0, 0 }, { 175, 0, 0 }, { 159, 0, 0 }, { 143, 0, 0 }, { 128, 0, 0 } };

            ColorScheme = new Color[u8_RGB.GetLength(0)];
            for (int i = 0; i < ColorScheme.Length; i++)
            {
                int ptr = i;
                if (reverseColorSchema)
                    ptr = ColorScheme.Length - i - 1;
                ColorScheme[i] = Color.FromArgb(u8_RGB[ptr, 0], u8_RGB[ptr, 1], u8_RGB[ptr, 2]);
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

        public void SetProgressTable(string table)
        {
            Action action = new Action(() =>
            {
                cbInterpolateUseProgress.Enabled = false;
                cbInterpolateUseProgress.Visible = true;
            });
            if (this.InvokeRequired)
                this.BeginInvoke(action);
            else action.Invoke();

            sProgressTable = table;
            bCalibrationEnabled = true;
        }

        public void ClearPregressTable()
        {
            Action action = new Action(() =>
            {
                cbInterpolateUseProgress.Enabled = true;
                cbInterpolateUseProgress.Visible = true;
            });
            if (this.InvokeRequired)
                this.BeginInvoke(action);
            else action.Invoke();

            bCalibrationEnabled = false;
        }

        public void ClearTable()
        {
            int sizex = 0;
            int sizey = 0;
            byte[] arraycalib = null;
            float[] array2d = null;

            try
            {
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

                if (!string.IsNullOrWhiteSpace(sProgressTable))
                {
                    FieldInfo calibrationTable = cs.ConfigStruct.corrections.GetType().GetField(sProgressTable);
                    if (calibrationTable != null)
                        arraycalib = (byte[])calibrationTable.GetValue(cs.ConfigStruct.corrections);
                }

                if (!string.IsNullOrWhiteSpace(sProgressTable))
                {
                    FieldInfo calibrationTable = cs.ConfigStruct.corrections.GetType().GetField(sProgressTable);
                    if (calibrationTable != null)
                        arraycalib = (byte[])calibrationTable.GetValue(cs.ConfigStruct.corrections);
                }

                if (!string.IsNullOrWhiteSpace(sArrayName))
                {
                    if (eMode == Editor2DMode.EcuTable)
                    {
                        FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                        if (fieldArray != null)
                            array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                    }
                    else if (eMode == Editor2DMode.CorrectionsTable)
                    {
                        FieldInfo fieldArray = cs.ConfigStruct.corrections.GetType().GetField(sArrayName);
                        if (fieldArray != null)
                            array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.corrections);
                    }

                }

                if (sizex > 0 && sizey > 0)
                {
                    if (arraycalib != null)
                    {
                        for (int i = 0; i < sizex * sizey; i++)
                        {
                            arraycalib[i] = 0;
                        }
                    }
                    if (array2d != null)
                    {
                        for (int i = 0; i < sizex * sizey; i++)
                        {
                            array2d[i] = 0;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("\r\n");
            }

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
            try
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
                    if (eMode == Editor2DMode.EcuTable)
                    {
                        FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                        if (fieldArray != null)
                            array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                    }
                    else if (eMode == Editor2DMode.CorrectionsTable)
                    {
                        FieldInfo fieldArray = cs.ConfigStruct.corrections.GetType().GetField(sArrayName);
                        if (fieldArray != null)
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
                    imageTable1.ColorTransience = ColorTransience;
                    imageTable1.ArraySizeX = iArraySizeX;
                    imageTable1.ArraySizeY = iArraySizeY;
                    imageTable1.ValueMin = (float)dMinY;
                    imageTable1.ValueMax = (float)dMaxY;
                    imageTable1.SizeX = iSizeX;
                    imageTable1.SizeY = iSizeY;
                    imageTable1.RowTitles = Array.ConvertAll(depy, a => a.ToString(sFormatStatusD));
                    imageTable1.ColumnTitles = Array.ConvertAll(depx, a => a.ToString(sFormatStatusX));
                    imageTable1.Array = array2d;
                    imageTable1.DecPlaces = iDecPlaces;
                    imageTable1.Increment = (float)dStepSize;
                    imageTable1.Initialized = true;

                    imageTable1.UpdateTableEvent += internalUpdateTableEvent;

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

                    if (chartMaxX != double.MinValue)
                    {
                        chart2DChart.ChartAreas[0].AxisX.IsLogarithmic = bLog10;
                        chart2DChart.ChartAreas[0].AxisX.LogarithmBase = 10;
                    }
                    else
                    {
                        chart2DChart.ChartAreas[0].AxisX.IsLogarithmic = bLog10;
                    }

                    graph3D.AxisX_Color = Color.Red;
                    graph3D.AxisY_Color = Color.Green;
                    graph3D.AxisZ_Color = Color.Blue;

                    graph3D.AxisX_Legend = sTitleStatusX;
                    graph3D.AxisY_Legend = sTitleStatusD;
                    graph3D.AxisZ_Legend = sTitleStatusY;
                    graph3D.Raster = eRaster.Labels;

                    cPoint3D[,] i_Points3D = new cPoint3D[sizex, sizey];

                    for (int y = 0; y < sizey; y++)
                    {
                        for (int x = 0; x < sizex; x++)
                        {
                            i_Points3D[x, y] = new cPoint3D(depy[y], depx[x], array2d[y * sizex + x]);
                        }
                    }
                    cMinMax3D cMinMax3D = new cMinMax3D(depy[0], depy[sizey - 1], depx[0], depx[sizex - 1],
                        chart2DChart.ChartAreas[0].AxisY.Minimum, chart2DChart.ChartAreas[0].AxisY.Maximum);
                    graph3D.SetSurfacePoints(i_Points3D, cMinMax3D, eNormalize.Separate);
                    graph3D.SetColorScheme(ColorScheme, 3.0F);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("\r\n");
            }
        }

        public void UpdateChart()
        {
            int sizex = 0;
            int sizey = 0;

            float[] depx = null;
            float[] depy = null;
            float[] array2d = null;
            byte[] arraycalib = null;

            float paramx = 0;
            float paramy = 0;
            float paramd = 0;

            string paramstext = string.Empty;

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
                if (eMode == Editor2DMode.EcuTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                    if (fieldArray != null)
                        array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                }
                else if (eMode == Editor2DMode.CorrectionsTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.corrections.GetType().GetField(sArrayName);
                    if (fieldArray != null)
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

            paramstext = string.Empty;
            if (!string.IsNullOrWhiteSpace(sParamsStatusX))
            {
                FieldInfo fieldParamX = cs.EcuParameters.GetType().GetField(sParamsStatusX);
                if (fieldParamX != null)
                {
                    paramx = (float)fieldParamX.GetValue(cs.EcuParameters);

                    if (!string.IsNullOrWhiteSpace(paramstext))
                        paramstext += "  ";
                    if (!string.IsNullOrWhiteSpace(sTitleStatusX))
                        paramstext += $"{sTitleStatusX}: ";
                    paramstext += $"{paramx.ToString(sFormatStatusX)}";
                }
            }

            if (!string.IsNullOrWhiteSpace(sParamsStatusD))
            {
                FieldInfo fieldParamD = cs.EcuParameters.GetType().GetField(sParamsStatusD);
                if (fieldParamD != null)
                {
                    paramd = (float)fieldParamD.GetValue(cs.EcuParameters);

                    if (!string.IsNullOrWhiteSpace(paramstext))
                        paramstext += "  ";
                    if (!string.IsNullOrWhiteSpace(sTitleStatusD))
                        paramstext += $"{sTitleStatusD}: ";
                    paramstext += $"{paramd.ToString(sFormatStatusD)}";
                }
            }

            if (!string.IsNullOrWhiteSpace(sParamsStatusY))
            {
                FieldInfo fieldParamY = cs.EcuParameters.GetType().GetField(sParamsStatusY);
                if (fieldParamY != null)
                {
                    paramy = (float)fieldParamY.GetValue(cs.EcuParameters);

                    if (!string.IsNullOrWhiteSpace(paramstext))
                        paramstext += "  ";
                    if (!string.IsNullOrWhiteSpace(sTitleStatusY))
                        paramstext += $"{sTitleStatusY}: ";
                    paramstext += $"{paramy.ToString(sFormatStatusY)}";
                }
            }
            else if (!string.IsNullOrWhiteSpace(sParamsStatusX) && !string.IsNullOrWhiteSpace(sParamsStatusD))
            {
                Interpolation interpolationX = new Interpolation(paramx, depx, sizex);
                Interpolation interpolationY = new Interpolation(paramd, depy, sizey);
                paramy = Interpolation.Interpolate2D(interpolationX, interpolationY, array2d, iArraySizeX);

                if (!string.IsNullOrWhiteSpace(paramstext))
                    paramstext += "  ";
                if (!string.IsNullOrWhiteSpace(sTitleStatusY))
                    paramstext += $"{sTitleStatusY}: ";
                paramstext += $"{paramy.ToString(sFormatStatusY)}";
            }

            int seriescount = chart2DChart.Series.Count;
            if (seriescount > 0 && chart2DChart.Series[0].Points.Count > 0)
            {
                chart2DChart.Series[seriescount - 1].Points[0].XValue = paramx;
                chart2DChart.Series[seriescount - 1].Points[0].YValues[0] = paramy;
            }

            if (depx != null && depy != null)
            {
                Interpolation interpolationX = null;
                Interpolation interpolationY = null;
                if (!string.IsNullOrWhiteSpace(sParamsStatusX) && !string.IsNullOrWhiteSpace(sParamsStatusD))
                {
                    interpolationX = new Interpolation(paramx, depx, iArraySizeX);
                    interpolationY = new Interpolation(paramd, depy, iArraySizeY);
                }


                if (sizex > 0 && sizey > 0)
                {
                    if (!string.IsNullOrWhiteSpace(sProgressTable))
                    {
                        FieldInfo calibrationTable = cs.ConfigStruct.corrections.GetType().GetField(sProgressTable);
                        if (calibrationTable != null)
                            arraycalib = (byte[])calibrationTable.GetValue(cs.ConfigStruct.corrections);
                    }

                    for (int i = 0; i < array2d.Length; i++)
                    {
                        int r, g, b;
                        int index = i;
                        int xpos = index % iArraySizeX;
                        int ypos = index / iArraySizeX;
                        Color color;
                        Color original = Color.DarkGray;
                        double[] mult = new double[4];
                        bool handled = false;

                        if (xpos < sizex && ypos < sizey)
                        {
                            if ((float)chart2DChart.Series[ypos].Points[xpos].YValues[0] != array2d[index])
                                chart2DChart.Series[ypos].Points[xpos].YValues = new double[1] { array2d[index] };

                            if (!string.IsNullOrWhiteSpace(sProgressTable) && bCalibrationEnabled)
                            {
                                if (arraycalib == null)
                                {
                                    original = Color.DarkGray;
                                }
                                else if (CalibrationColorTransience != null)
                                {
                                    original = CalibrationColorTransience.Get((float)arraycalib[index] / 255.0F);
                                }
                            }
                            else if (ColorTransience != null)
                            {
                                original = ColorTransience.Get(array2d[index]);
                            }

                            //if (interpolationX != null && interpolationY != null)
                            //{
                            //    for (int y = 0; y < 2; y++)
                            //    {
                            //        for (int x = 0; x < 2; x++)
                            //        {
                            //            double value;
                            //            if (x == 0 && y == 0)
                            //                value = (1.0 - interpolationX.mult) * (1.0f - interpolationY.mult);
                            //            else if (x == 0 && y != 0)
                            //                value = (1.0 - interpolationX.mult) * interpolationY.mult;
                            //            else if (x != 0 && y == 0)
                            //                value = interpolationX.mult * (1.0f - interpolationY.mult);
                            //            else
                            //                value = interpolationX.mult * interpolationY.mult;
                            //            mult[y * 2 + x] = value;
                            //        }
                            //    }


                            //    for (int y = 0; y < 2; y++)
                            //    {
                            //        for (int x = 0; x < 2; x++)
                            //        {
                            //            if (index == interpolationY.indexes[y] * iArraySizeX + interpolationX.indexes[x])
                            //            {
                            //                color = Color.DarkGray;

                            //                if (mult[y * 2 + x] > 1.0)
                            //                    mult[y * 2 + x] = 1.0;
                            //                else if (mult[y * 2 + x] < 0.0)
                            //                    mult[y * 2 + x] = 0.0;

                            //                r = (int)((color.R - original.R) * mult[y * 2 + x] + original.R);
                            //                g = (int)((color.G - original.G) * mult[y * 2 + x] + original.G);
                            //                b = (int)((color.B - original.B) * mult[y * 2 + x] + original.B);

                            //                nud.BackColor = Color.FromArgb(r, g, b);
                            //                nud.ForeColor = Color.White;
                            //                if (mult[y * 2 + x] == mult.Max() || mult[y * 2 + x] > 0.35)
                            //                {
                            //                    if (nud.Font.Style != FontStyle.Bold)
                            //                        nud.Font = new Font(nud.Font, FontStyle.Bold);
                            //                }
                            //                else
                            //                {
                            //                    if (nud.Font.Style != FontStyle.Regular)
                            //                        nud.Font = new Font(nud.Font, FontStyle.Regular);
                            //                }
                            //                handled = true;
                            //            }
                            //        }
                            //    }
                            //}
                            //if (!handled && nud.BackColor != original)
                            //{
                            //    nud.BackColor = original;
                            //    if (!nud.Focused)
                            //        nudTableItem_ValueChanged(nud, new EventArgs());
                            //}
                        }
                    }
                    

                    imageTable1.RedrawTable();

                    cPoint3D[,] i_Points3D = new cPoint3D[sizex, sizey];

                    for (int y = 0; y < sizey; y++)
                    {
                        for (int x = 0; x < sizex; x++)
                        {
                            i_Points3D[x, y] = new cPoint3D(depy[y], depx[x], array2d[y * sizex + x]);
                        }
                    }
                    cMinMax3D cMinMax3D = new cMinMax3D(depy[0], depy[sizey - 1], depx[0], depx[sizex - 1],
                        chart2DChart.ChartAreas[0].AxisY.Minimum, chart2DChart.ChartAreas[0].AxisY.Maximum);
                    graph3D.SetSurfacePoints(i_Points3D, cMinMax3D, eNormalize.Separate);
                    graph3D.SetColorScheme(ColorScheme, 3.0F);
                }
            }
            if (!lblParams.Text.Equals(paramstext))
                lblParams.Text = paramstext;
        }
        
        private void cbTableItem_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            int y = (int)cb.Tag;

            if (y < chart2DChart.Series.Count)
            {
                chart2DChart.Series[y].Enabled = cb.Checked;
            }
        }

        private void internalUpdateTableEvent(object sender, ImageTableEventArg e)
        {
            if (valueChangedEnabled)
            {
                int index = e.Index;
                float value;
                int x = index % iArraySizeX;
                int y = index / iArraySizeX;
                //Color text = Color.Black;
                //Color back = nud.BackColor;
                int sizex = 0;
                int sizey = 0;
                float[] depx = null;
                float[] depy = null;
                float[] array2d = null;

                if (!string.IsNullOrWhiteSpace(sArrayName))
                {
                    if (eMode == Editor2DMode.EcuTable)
                    {
                        FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                        if (fieldArray != null)
                            array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                    }
                    else if (eMode == Editor2DMode.CorrectionsTable)
                    {
                        FieldInfo fieldArray = cs.ConfigStruct.corrections.GetType().GetField(sArrayName);
                        if (fieldArray != null)
                            array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.corrections);
                    }
                }

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

                value = array2d[index];

                if (chart2DChart.Series.Count > y && chart2DChart.Series[y].Points.Count > x)
                {
                    chart2DChart.Series[y].Points[x].YValues = new double[1] { value };
                }

                //TODO: check if need to check if IsSynchronizing
                UpdateTableEvent?.Invoke(sender, new EventArgs());

                //if (ColorTransience != null || !string.IsNullOrWhiteSpace(sProgressTable))
                //{
                //    text = Color.White;
                //    if (string.IsNullOrWhiteSpace(sProgressTable))
                //        back = ColorTransience.Get(value);
                //}
                
                //nud.ForeColor = text;
                //nud.BackColor = back;
                //if(nud.Font.Style != FontStyle.Regular)
                //    nud.Font = new Font(nud.Font, FontStyle.Regular);

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

                cPoint3D[,] i_Points3D = new cPoint3D[sizex, sizey];

                for (int iy = 0; iy < sizey; iy++)
                    for (int ix = 0; ix < sizex; ix++)
                        i_Points3D[ix, iy] = new cPoint3D(depy[iy], depx[ix], array2d[iy * sizex + ix]);

                cMinMax3D cMinMax3D = new cMinMax3D(depy[0], depy[sizey - 1], depx[0], depx[sizex - 1],
                    chart2DChart.ChartAreas[0].AxisY.Minimum, chart2DChart.ChartAreas[0].AxisY.Maximum);
                graph3D.SetSurfacePoints(i_Points3D, cMinMax3D, eNormalize.Separate);
            }
        }

        private void Editor2D_Load(object sender, EventArgs e)
        {
            tpGraph.BackColor = this.BackColor;
            tpInterpolation.BackColor = this.BackColor;
            tpTools.BackColor = this.BackColor;

        }

        private void btnInterpolate_Click(object sender, EventArgs e)
        {
            int sizex = 0;
            int sizey = 0;
            float[] array2d = null;
            float[] new2d = null;
            byte[] arraycalib = null;
            double koff = (double)nudInterpolationKoff.Value;
            int amount = (int)nudInterpolationAmount.Value;
            int radius = (int)nudInterpolationRadius.Value;
            
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
                if (eMode == Editor2DMode.EcuTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                    if (fieldArray != null)
                        array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                }
                else if (eMode == Editor2DMode.CorrectionsTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.corrections.GetType().GetField(sArrayName);
                    if (fieldArray != null)
                        array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.corrections);
                }
            }

            if (!string.IsNullOrWhiteSpace(sProgressTable))
            {
                FieldInfo calibrationTable = cs.ConfigStruct.corrections.GetType().GetField(sProgressTable);
                if (calibrationTable != null)
                    arraycalib = (byte[])calibrationTable.GetValue(cs.ConfigStruct.corrections);
            }

            if (sizex > 0 && sizey > 0 && array2d != null)
            {
                new2d = new float[array2d.Length];
                array2d.CopyTo(new2d, 0);
                double temp_koff;
                double calib_koff = 1.0;
                double val;
                int index;
                int dindex;
                double valuex1, valuey1, valuex2, valuey2;

                for (int i = 0; i < amount; i++)
                {
                    for(int y = 0; y < sizey; y++)
                    {
                        for (int x = 0; x < sizex; x++)
                        {
                            index = y * iArraySizeX + x;
                            valuex1 = valuey1 = new2d[index];
                            valuex2 = valuey2 = new2d[index];
                            for (int dx = -radius; dx < 0; dx++)
                            {
                                if ((dx + x) >= 0 && (dx + x) < sizex)
                                {
                                    dindex = y * iArraySizeX + (dx + x);
                                    temp_koff = ((radius + 1) - Math.Abs(dx)) / (double)(radius + 1);

                                    valuex1 = array2d[dindex] * temp_koff + valuex1 * (1.0D - temp_koff);
                                }
                            }
                            for (int dx = radius; dx > 0; dx--)
                            {
                                if ((dx + x) >= 0 && (dx + x) < sizex)
                                {
                                    dindex = y * iArraySizeX + (dx + x);
                                    temp_koff = ((radius + 1) - Math.Abs(dx)) / (double)(radius + 1);

                                    valuex2 = array2d[dindex] * temp_koff + valuex2 * (1.0D - temp_koff);
                                }
                            }
                            for (int dy = -radius; dy < 0; dy++)
                            {
                                if ((dy + y) >= 0 && (dy + y) < sizey)
                                {
                                    dindex = (dy + y) * iArraySizeX + x;
                                    temp_koff = ((radius + 1) - Math.Abs(dy)) / (double)(radius + 1);

                                    valuey1 = array2d[dindex] * temp_koff + valuey1 * (1.0D - temp_koff);
                                }
                            }
                            for (int dy = radius; dy > 0; dy--)
                            {
                                if ((dy + y) >= 0 && (dy + y) < sizey)
                                {
                                    dindex = (dy + y) * iArraySizeX + x;
                                    temp_koff = ((radius + 1) - Math.Abs(dy)) / (double)(radius + 1);

                                    valuey2 = array2d[dindex] * temp_koff + valuey2 * (1.0D - temp_koff);
                                }
                            }

                            val = ((valuex1 + valuey1 + valuex2 + valuey2) * koff * 0.25D) + new2d[index] * (1.0D - koff);

                            if (arraycalib != null && cbInterpolateUseProgress.Checked)
                                calib_koff = 1.0D - (arraycalib[index] / 255.0D);
                            else calib_koff = 1.0D;

                            new2d[index] = (float)(val * calib_koff + new2d[index] * (1.0D - calib_koff));

                        }
                    }
                    new2d.CopyTo(array2d, 0);
                }

            }

            valueChangedEnabled = false;
            imageTable1.RedrawTable();
            valueChangedEnabled = true;
            UpdateChart();
            UpdateTableEvent?.Invoke(sender, new EventArgs());

        }

        private void btnCopyToC_Click(object sender, EventArgs e)
        {
            int sizex = 0;
            int sizey = 0;
            float[] array2d = null;
            int index;
            string text = string.Empty;
            string line = string.Empty;
            string decplaces = string.Empty;

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
                if (eMode == Editor2DMode.EcuTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                    if (fieldArray != null)
                        array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                }
                else if (eMode == Editor2DMode.CorrectionsTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.corrections.GetType().GetField(sArrayName);
                    if (fieldArray != null)
                        array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.corrections);
                }
            }

            if (sizex > 0 && sizey > 0 && array2d != null)
            {
                if (iDecPlaces > 0)
                    decplaces = "." + Enumerable.Repeat("0", iDecPlaces).Aggregate((sum, next) => sum + next);

                for (int y = 0; y < sizey; y++)
                {
                    line = string.Empty;
                    text += "\t{ ";
                    for (int x = 0; x < sizex; x++)
                    {
                        index = y * iArraySizeX + x;
                        line += string.Format("{0:0" + decplaces + "}" + (iDecPlaces > 0 ? "f" : "") + ", ", array2d[index]);
                    }
                    text += line;
                    text += "},\r\n";
                }
                Clipboard.SetText(text);
            }
        }

        private void btnImport2DChart_Click(object sender, EventArgs e)
        {
            int sizex = 0;
            int sizey = 0;
            float[] array2d = null;
            float[][] array;
            int index;

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
                if (eMode == Editor2DMode.EcuTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                    if (fieldArray != null)
                        array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                }
                else if (eMode == Editor2DMode.CorrectionsTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.corrections.GetType().GetField(sArrayName);
                    if (fieldArray != null)
                        array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.corrections);
                }
            }

            if (sizex > 0 && sizey > 0 && array2d != null)
            {
                if (dlgImport2DChart.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        array = Serializator<float[][]>.Deserialize(dlgImport2DChart.FileName);

                        if (array.Length != sizey)
                            throw new Exception("Y size not equals to original array.");

                        for (int y = 0; y < sizey; y++)
                        {
                            if (array[y].Length != sizex)
                                throw new Exception("X size not equals to original array.");

                            for (int x = 0; x < sizex; x++)
                            {
                                index = y * iArraySizeX + x;
                                array2d[index] = array[y][x];
                            }
                        }

                        valueChangedEnabled = false;
                        imageTable1.RedrawTable();
                        valueChangedEnabled = true;
                        UpdateChart();
                        UpdateTableEvent?.Invoke(sender, new EventArgs());

                        MessageBox.Show($"2D Chart import success.", "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"2d Chart import failed.\r\n{ex.Message}", "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnExport2DChart_Click(object sender, EventArgs e)
        {
            int sizex = 0;
            int sizey = 0;
            float[] array2d = null;
            float[][] array;
            int index;

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
                if (eMode == Editor2DMode.EcuTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                    if (fieldArray != null)
                        array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                }
                else if (eMode == Editor2DMode.CorrectionsTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.corrections.GetType().GetField(sArrayName);
                    if (fieldArray != null)
                        array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.corrections);
                }
            }

            if (sizex > 0 && sizey > 0 && array2d != null)
            {
                if (dlgExport2DChart.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        array = new float[sizey][];
                        for(int y = 0; y < sizey; y++)
                        {
                            array[y] = new float[sizex];
                            for (int x = 0; x < sizex; x++)
                            {
                                index = y * iArraySizeX + x;
                                array[y][x] = array2d[index];
                            }
                        }
                        Serializator<float[][]>.Serialize(dlgExport2DChart.FileName, array);
                        MessageBox.Show($"2D Chart export success.", "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"2d Chart export failed.\r\n{ex.Message}", "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnImportFromCCode_Click(object sender, EventArgs e)
        {
            int sizex = 0;
            int sizey = 0;
            float[] array2d = null;
            float[] array_initial = null;
            int index_array;
            int index_output;

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
                if (eMode == Editor2DMode.EcuTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                    if (fieldArray != null)
                        array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                }
                else if (eMode == Editor2DMode.CorrectionsTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.corrections.GetType().GetField(sArrayName);
                    if (fieldArray != null)
                        array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.corrections);
                }
            }


            if (sizex > 0 && sizey > 0 && array2d != null)
            {
                array_initial = new float[sizey * sizex];
                for (int y = 0; y < sizey; y++)
                {
                    for (int x = 0; x < sizex; x++)
                    {
                        index_array = y * iArraySizeX + x;
                        index_output = y * sizex + x;
                        if (array2d[index_array] > (float)dMaxY)
                            array2d[index_array] = (float)dMaxY;
                        if (array2d[index_array] < (float)dMinY)
                            array2d[index_array] = (float)dMinY;
                        array_initial[index_output] = array2d[index_array];
                    }
                }

                ImportCCodeForm importCCodeForm = new ImportCCodeForm(ArrayType.Array2D, array_initial, sizex, sizey);

                DialogResult result = importCCodeForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    float[] output = importCCodeForm.GetResult();

                    for (int y = 0; y < sizey; y++)
                    {
                        for (int x = 0; x < sizex; x++)
                        {
                            index_array = y * iArraySizeX + x;
                            index_output = y * sizex + x;
                            array2d[index_array] = output[index_output];
                        }
                    }
                    this.UpdateChart();
                }
            }
        }

        private void btnCopyToText_Click(object sender, EventArgs e)
        {
            int sizex = 0;
            int sizey = 0;
            float[] array2d = null;
            int index;
            string text = string.Empty;
            string line = string.Empty;
            string decplaces = string.Empty;

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
                if (eMode == Editor2DMode.EcuTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                    if (fieldArray != null)
                        array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                }
                else if (eMode == Editor2DMode.CorrectionsTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.corrections.GetType().GetField(sArrayName);
                    if (fieldArray != null)
                        array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.corrections);
                }
            }

            if (sizex > 0 && sizey > 0 && array2d != null)
            {
                if (iDecPlaces > 0)
                    decplaces = "." + Enumerable.Repeat("0", iDecPlaces).Aggregate((sum, next) => sum + next);

                for (int y = 0; y < sizey; y++)
                {
                    line = string.Empty;
                    for (int x = 0; x < sizex; x++)
                    {
                        index = y * iArraySizeX + x;
                        line += string.Format("{0:0" + decplaces + "}\t", array2d[index]);
                    }
                    line = line.TrimEnd('\t');
                    text += line;
                    text += "\r\n";
                }
                Clipboard.SetText(text);
            }
        }

        private void btnImportFromText_Click(object sender, EventArgs e)
        {
            int sizex = 0;
            int sizey = 0;
            float[] array2d = null;
            float[] array_initial = null;
            int index_array;
            int index_output;

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
                if (eMode == Editor2DMode.EcuTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                    if (fieldArray != null)
                        array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                }
                else if (eMode == Editor2DMode.CorrectionsTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.corrections.GetType().GetField(sArrayName);
                    if (fieldArray != null)
                        array2d = (float[])fieldArray.GetValue(cs.ConfigStruct.corrections);
                }
            }


            if (sizex > 0 && sizey > 0 && array2d != null)
            {
                array_initial = new float[sizey * sizex];
                for (int y = 0; y < sizey; y++)
                {
                    for (int x = 0; x < sizex; x++)
                    {
                        index_array = y * iArraySizeX + x;
                        index_output = y * sizex + x;
                        if (array2d[index_array] > (float)dMaxY)
                            array2d[index_array] = (float)dMaxY;
                        if (array2d[index_array] < (float)dMinY)
                            array2d[index_array] = (float)dMinY;
                        array_initial[index_output] = array2d[index_array];
                    }
                }

                ImportTextForm importTextForm = new ImportTextForm(ArrayType.Array2D, array_initial, sizex, sizey);

                DialogResult result = importTextForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    float[] output = importTextForm.GetResult();

                    for (int y = 0; y < sizey; y++)
                    {
                        for (int x = 0; x < sizex; x++)
                        {
                            index_array = y * iArraySizeX + x;
                            index_output = y * sizex + x;
                            array2d[index_array] = output[index_output];
                        }
                    }
                    this.UpdateChart();
                    UpdateTableEvent?.Invoke(sender, new EventArgs());
                }
            }
        }
    }
}
