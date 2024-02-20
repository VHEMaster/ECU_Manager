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

        public void Initialize(ComponentStructure componentStructure, Editor2DMode mode, int sizex, int sizey, double min, double max, double step, double mindiffx, double mindiffy, double chartminy, double chartmaxy, double intervalx, double intervaly, int decplaces, bool reverseColorSchema = false, bool log10 = false)
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
            iArraySizeX = sizex;
            iArraySizeY = sizey;
            iDecPlaces = decplaces;
            eMode = mode;

            bLog10 = log10;

            cs = componentStructure;
            forcePositions = new List<ForcePosition>();

            iSizeX = iArraySizeX;
            iSizeY = iArraySizeY;

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

        public void SetConfig(string arrayname, string depx, string depy)
        {
            sArrayName = arrayname;
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
            Array arraydef = null;
            EcuParamTransform transform = default(EcuParamTransform);
            byte[] arraycalib = null;
            float[] array2d = null;

            try
            {
                if (!string.IsNullOrWhiteSpace(sProgressTable))
                {
                    FieldInfo calibrationTable = cs.ConfigStruct.corrections.GetType().GetField(sProgressTable);
                    if (calibrationTable != null)
                        arraycalib = (byte[])calibrationTable.GetValue(cs.ConfigStruct.corrections);
                }

                if (!string.IsNullOrWhiteSpace(sArrayName))
                {
                    string transformName = sArrayName;
                    for (int i = 1; i <= Consts.ECU_CYLINDERS_COUNT; i++)
                    {
                        transformName = transformName.Replace($"cy{i}", "cy");
                    }
                    if (eMode == Editor2DMode.EcuTable)
                    {
                        FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                        FieldInfo fieldTransform = cs.ConfigStruct.tables[cs.CurrentTable].transform.GetType().GetField(transformName);
                        if (fieldArray != null && fieldTransform != null)
                        {
                            transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.tables[cs.CurrentTable].transform);
                            arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                            array2d = EcuConfigTransform.FromInteger(arraydef, transform);
                        }
                    }
                    else if (eMode == Editor2DMode.CorrectionsTable)
                    {
                        FieldInfo fieldArray = cs.ConfigStruct.corrections.GetType().GetField(sArrayName);
                        FieldInfo fieldTransform = cs.ConfigStruct.corrections.transform.GetType().GetField(transformName);
                        if (fieldArray != null && fieldTransform != null)
                        {
                            transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.corrections.transform);
                            arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.corrections);
                            array2d = EcuConfigTransform.FromInteger(arraydef, transform);
                        }
                    }

                }

                if (iArraySizeX > 0 && iArraySizeY > 0)
                {
                    if (arraycalib != null)
                    {
                        for (int i = 0; i < iArraySizeX * iArraySizeY; i++)
                        {
                            arraycalib[i] = 0;
                        }
                    }
                    if (array2d != null)
                    {
                        for (int i = 0; i < iArraySizeX * iArraySizeY; i++)
                        {
                            array2d[i] = 0;
                        }
                        EcuConfigTransform.ToInteger(array2d, arraydef, transform);
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
                
                float[] depx = null;
                float[] depy = null;
                float[] array2d = null;

                float paramx = 0;
                float paramy = 0;
                float paramd = 0;
                

                if (!string.IsNullOrWhiteSpace(sArrayName))
                {
                    string transformName = sArrayName;
                    for (int i = 1; i <= Consts.ECU_CYLINDERS_COUNT; i++)
                    {
                        transformName = transformName.Replace($"cy{i}", "cy");
                    }
                    if (eMode == Editor2DMode.EcuTable)
                    {
                        FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                        FieldInfo fieldTransform = cs.ConfigStruct.tables[cs.CurrentTable].transform.GetType().GetField(transformName);
                        if (fieldArray != null && fieldTransform != null)
                        {
                            EcuParamTransform transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.tables[cs.CurrentTable].transform);
                            Array arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                            array2d = EcuConfigTransform.FromInteger(arraydef, transform);
                        }
                    }
                    else if (eMode == Editor2DMode.CorrectionsTable)
                    {
                        FieldInfo fieldArray = cs.ConfigStruct.corrections.GetType().GetField(sArrayName);
                        FieldInfo fieldTransform = cs.ConfigStruct.corrections.transform.GetType().GetField(transformName);
                        if (fieldArray != null && fieldTransform != null)
                        {
                            EcuParamTransform transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.corrections.transform);
                            Array arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.corrections);
                            array2d = EcuConfigTransform.FromInteger(arraydef, transform);
                        }
                    }

                }

                if (!string.IsNullOrWhiteSpace(sConfigDepX))
                {
                    FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sConfigDepX);
                    FieldInfo fieldTransform = cs.ConfigStruct.tables[cs.CurrentTable].transform.GetType().GetField(sConfigDepX);
                    if (fieldArray != null && fieldTransform != null)
                    {
                        EcuParamTransform transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.tables[cs.CurrentTable].transform);
                        Array arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                        depx = EcuConfigTransform.FromInteger(arraydef, transform);
                    }
                }

                if (!string.IsNullOrWhiteSpace(sConfigDepY))
                {
                    FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sConfigDepY);
                    FieldInfo fieldTransform = cs.ConfigStruct.tables[cs.CurrentTable].transform.GetType().GetField(sConfigDepY);
                    if (fieldArray != null && fieldTransform != null)
                    {
                        EcuParamTransform transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.tables[cs.CurrentTable].transform);
                        Array arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                        depy = EcuConfigTransform.FromInteger(arraydef, transform);
                    }
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
                    chart2DChart.ChartAreas[0].AxisX.Maximum = depx[iArraySizeX - 1] + (dMinDiffX - (depx[iArraySizeX - 1] % dMinDiffX));

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

                    if (iArraySizeY > 0 && iArraySizeX > 0)
                    {
                        for (int i = 0; i < iArraySizeY; i++)
                        {
                            series = chart2DChart.Series.Add(depy[i].ToString(sFormatStatusD));
                            series.Tag = i;
                            series.ChartType = SeriesChartType.Line;
                            series.XAxisType = AxisType.Primary;
                            series.XValueType = ChartValueType.Single;
                            series.YAxisType = AxisType.Primary;
                            series.YValueType = ChartValueType.Single;
                            series.BorderWidth = 2;
                            float trans = (float)i / (float)(iArraySizeY - 1);
                            series.Color = Color.FromArgb((int)(min.R * (1.0f - trans) + max.R * trans), (int)(min.G * (1.0f - trans) + max.G * trans), (int)(min.B * (1.0f - trans) + max.B * trans));
                            for (int j = 0; j < iArraySizeX; j++)
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

                    cPoint3D[,] i_Points3D = new cPoint3D[iArraySizeX, iArraySizeY];

                    for (int y = 0; y < iArraySizeY; y++)
                    {
                        for (int x = 0; x < iArraySizeX; x++)
                        {
                            i_Points3D[x, y] = new cPoint3D(depy[y], depx[x], array2d[y * iArraySizeX + x]);
                        }
                    }
                    cMinMax3D cMinMax3D = new cMinMax3D(depy[0], depy[iArraySizeY - 1], depx[0], depx[iArraySizeX - 1],
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
            float[] depx = null;
            float[] depy = null;
            float[] array2d = null;
            byte[] arraycalib = null;

            float paramx = 0;
            float paramy = 0;
            float paramd = 0;

            string paramstext = string.Empty;
           
            if (!string.IsNullOrWhiteSpace(sArrayName))
            {
                string transformName = sArrayName;
                for(int i = 1; i <= Consts.ECU_CYLINDERS_COUNT; i++)
                {
                    transformName = transformName.Replace($"cy{i}", "cy");
                }
                if (eMode == Editor2DMode.EcuTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                    FieldInfo fieldTransform = cs.ConfigStruct.tables[cs.CurrentTable].transform.GetType().GetField(transformName);
                    if (fieldArray != null && fieldTransform != null)
                    {
                        EcuParamTransform transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.tables[cs.CurrentTable].transform);
                        Array arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                        array2d = EcuConfigTransform.FromInteger(arraydef, transform);
                    }
                }
                else if (eMode == Editor2DMode.CorrectionsTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.corrections.GetType().GetField(sArrayName);
                    FieldInfo fieldTransform = cs.ConfigStruct.corrections.transform.GetType().GetField(transformName);
                    if (fieldArray != null && fieldTransform != null)
                    {
                        EcuParamTransform transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.corrections.transform);
                        Array arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.corrections);
                        array2d = EcuConfigTransform.FromInteger(arraydef, transform);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(sConfigDepX))
            {
                FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sConfigDepX);
                FieldInfo fieldTransform = cs.ConfigStruct.tables[cs.CurrentTable].transform.GetType().GetField(sConfigDepX);
                if (fieldArray != null && fieldTransform != null)
                {
                    EcuParamTransform transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.tables[cs.CurrentTable].transform);
                    Array arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                    depx = EcuConfigTransform.FromInteger(arraydef, transform);
                }
            }

            if (!string.IsNullOrWhiteSpace(sConfigDepY))
            {
                FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sConfigDepY);
                FieldInfo fieldTransform = cs.ConfigStruct.tables[cs.CurrentTable].transform.GetType().GetField(sConfigDepY);
                if (fieldArray != null && fieldTransform != null)
                {
                    EcuParamTransform transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.tables[cs.CurrentTable].transform);
                    Array arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                    depy = EcuConfigTransform.FromInteger(arraydef, transform);
                }
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
                Interpolation interpolationX = new Interpolation(paramx, depx, iArraySizeX);
                Interpolation interpolationY = new Interpolation(paramd, depy, iArraySizeY);
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

                imageTable1.ValueInterpolationX = interpolationX;
                imageTable1.ValueInterpolationY = interpolationY;
                
                if (iArraySizeX > 0 && iArraySizeY > 0)
                {
                    if (!string.IsNullOrWhiteSpace(sProgressTable))
                    {
                        FieldInfo calibrationTable = cs.ConfigStruct.corrections.GetType().GetField(sProgressTable);
                        if (calibrationTable != null)
                            arraycalib = (byte[])calibrationTable.GetValue(cs.ConfigStruct.corrections);
                    }

                    for (int i = 0; i < array2d.Length; i++)
                    {
                        int index = i;
                        int xpos = index % iArraySizeX;
                        int ypos = index / iArraySizeX;

                        if (xpos < iArraySizeX && ypos < iArraySizeY)
                        {
                            if ((float)chart2DChart.Series[ypos].Points[xpos].YValues[0] != array2d[index])
                                chart2DChart.Series[ypos].Points[xpos].YValues = new double[1] { array2d[index] };
                        }
                    }


                    if (!string.IsNullOrWhiteSpace(sProgressTable) && bCalibrationEnabled)
                    {
                        imageTable1.ArrayCalib = arraycalib;
                        imageTable1.CalibrationColorTransience = CalibrationColorTransience;
                        imageTable1.UseCalibrationColorTransience = true;
                    } else
                    {
                        imageTable1.UseCalibrationColorTransience = false;
                    }
                    imageTable1.Array = array2d;
                    imageTable1.RedrawTable();

                    cPoint3D[,] i_Points3D = new cPoint3D[iArraySizeX, iArraySizeY];

                    for (int y = 0; y < iArraySizeY; y++)
                    {
                        for (int x = 0; x < iArraySizeX; x++)
                        {
                            i_Points3D[x, y] = new cPoint3D(depy[y], depx[x], array2d[y * iArraySizeX + x]);
                        }
                    }
                    cMinMax3D cMinMax3D = new cMinMax3D(depy[0], depy[iArraySizeY - 1], depx[0], depx[iArraySizeX - 1],
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
                EcuParamTransform transform = default(EcuParamTransform);
                Array arraydef = null;
                int index = e.Index;
                float value = e.Value;
                int x = index % iArraySizeX;
                int y = index / iArraySizeX;
                //Color text = Color.Black;
                //Color back = nud.BackColor;
                float[] depx = null;
                float[] depy = null;
                float[] array2d = null;

                if (!string.IsNullOrWhiteSpace(sArrayName))
                {
                    string transformName = sArrayName;
                    for (int i = 1; i <= Consts.ECU_CYLINDERS_COUNT; i++)
                    {
                        transformName = transformName.Replace($"cy{i}", "cy");
                    }
                    if (eMode == Editor2DMode.EcuTable)
                    {
                        FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                        FieldInfo fieldTransform = cs.ConfigStruct.tables[cs.CurrentTable].transform.GetType().GetField(transformName);
                        if (fieldArray != null && fieldTransform != null)
                        {
                            transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.tables[cs.CurrentTable].transform);
                            arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                            array2d = EcuConfigTransform.FromInteger(arraydef, transform);
                        }
                    }
                    else if (eMode == Editor2DMode.CorrectionsTable)
                    {
                        FieldInfo fieldArray = cs.ConfigStruct.corrections.GetType().GetField(sArrayName);
                        FieldInfo fieldTransform = cs.ConfigStruct.corrections.transform.GetType().GetField(transformName);
                        if (fieldArray != null && fieldTransform != null)
                        {
                            transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.corrections.transform);
                            arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.corrections);
                            array2d = EcuConfigTransform.FromInteger(arraydef, transform);
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(sConfigDepX))
                {
                    FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sConfigDepX);
                    FieldInfo fieldTransform = cs.ConfigStruct.tables[cs.CurrentTable].transform.GetType().GetField(sConfigDepX);
                    if (fieldArray != null && fieldTransform != null)
                    {
                        EcuParamTransform transform1 = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.tables[cs.CurrentTable].transform);
                        Array arraydef1 = (Array)fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                        depx = EcuConfigTransform.FromInteger(arraydef1, transform1);
                    }
                }

                if (!string.IsNullOrWhiteSpace(sConfigDepY))
                {
                    FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sConfigDepY);
                    FieldInfo fieldTransform = cs.ConfigStruct.tables[cs.CurrentTable].transform.GetType().GetField(sConfigDepY);
                    if (fieldArray != null && fieldTransform != null)
                    {
                        EcuParamTransform transform1 = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.tables[cs.CurrentTable].transform);
                        Array arraydef1 = (Array)fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                        depy = EcuConfigTransform.FromInteger(arraydef1, transform1);
                    }
                }

                EcuConfigTransform.SingleToInteger(arraydef, transform, index, value);

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

                cPoint3D[,] i_Points3D = new cPoint3D[iArraySizeX, iArraySizeY];

                for (int iy = 0; iy < iArraySizeY; iy++)
                    for (int ix = 0; ix < iArraySizeX; ix++)
                        i_Points3D[ix, iy] = new cPoint3D(depy[iy], depx[ix], array2d[iy * iArraySizeX + ix]);

                cMinMax3D cMinMax3D = new cMinMax3D(depy[0], depy[iArraySizeY - 1], depx[0], depx[iArraySizeX - 1],
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
            float[] array2d = null;
            Array arraydef = null;
            EcuParamTransform transform = default(EcuParamTransform);
            float[] new2d = null;
            byte[] arraycalib = null;
            double koff = (double)nudInterpolationKoff.Value;
            int amount = (int)nudInterpolationAmount.Value;
            int radius = (int)nudInterpolationRadius.Value;
            
            if (!string.IsNullOrWhiteSpace(sArrayName))
            {
                string transformName = sArrayName;
                for (int i = 1; i <= Consts.ECU_CYLINDERS_COUNT; i++)
                {
                    transformName = transformName.Replace($"cy{i}", "cy");
                }
                if (eMode == Editor2DMode.EcuTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                    FieldInfo fieldTransform = cs.ConfigStruct.tables[cs.CurrentTable].transform.GetType().GetField(transformName);
                    if (fieldArray != null && fieldTransform != null)
                    {
                        transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.tables[cs.CurrentTable].transform);
                        arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                        array2d = EcuConfigTransform.FromInteger(arraydef, transform);
                    }
                }
                else if (eMode == Editor2DMode.CorrectionsTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.corrections.GetType().GetField(sArrayName);
                    FieldInfo fieldTransform = cs.ConfigStruct.corrections.transform.GetType().GetField(transformName);
                    if (fieldArray != null && fieldTransform != null)
                    {
                        transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.corrections.transform);
                        arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.corrections);
                        array2d = EcuConfigTransform.FromInteger(arraydef, transform);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(sProgressTable))
            {
                FieldInfo calibrationTable = cs.ConfigStruct.corrections.GetType().GetField(sProgressTable);
                if (calibrationTable != null)
                    arraycalib = (byte[])calibrationTable.GetValue(cs.ConfigStruct.corrections);
            }

            if (iArraySizeX > 0 && iArraySizeY > 0 && array2d != null)
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
                    for(int y = 0; y < iArraySizeY; y++)
                    {
                        for (int x = 0; x < iArraySizeX; x++)
                        {
                            index = y * iArraySizeX + x;
                            valuex1 = valuey1 = new2d[index];
                            valuex2 = valuey2 = new2d[index];
                            for (int dx = -radius; dx < 0; dx++)
                            {
                                if ((dx + x) >= 0 && (dx + x) < iArraySizeX)
                                {
                                    dindex = y * iArraySizeX + (dx + x);
                                    temp_koff = ((radius + 1) - Math.Abs(dx)) / (double)(radius + 1);

                                    valuex1 = array2d[dindex] * temp_koff + valuex1 * (1.0D - temp_koff);
                                }
                            }
                            for (int dx = radius; dx > 0; dx--)
                            {
                                if ((dx + x) >= 0 && (dx + x) < iArraySizeX)
                                {
                                    dindex = y * iArraySizeX + (dx + x);
                                    temp_koff = ((radius + 1) - Math.Abs(dx)) / (double)(radius + 1);

                                    valuex2 = array2d[dindex] * temp_koff + valuex2 * (1.0D - temp_koff);
                                }
                            }
                            for (int dy = -radius; dy < 0; dy++)
                            {
                                if ((dy + y) >= 0 && (dy + y) < iArraySizeY)
                                {
                                    dindex = (dy + y) * iArraySizeX + x;
                                    temp_koff = ((radius + 1) - Math.Abs(dy)) / (double)(radius + 1);

                                    valuey1 = array2d[dindex] * temp_koff + valuey1 * (1.0D - temp_koff);
                                }
                            }
                            for (int dy = radius; dy > 0; dy--)
                            {
                                if ((dy + y) >= 0 && (dy + y) < iArraySizeY)
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
                    EcuConfigTransform.ToInteger(new2d, arraydef, transform);
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
            float[] array2d = null;
            int index;
            string text = string.Empty;
            string line = string.Empty;
            string decplaces = string.Empty;

            if (!string.IsNullOrWhiteSpace(sArrayName))
            {
                string transformName = sArrayName;
                for (int i = 1; i <= Consts.ECU_CYLINDERS_COUNT; i++)
                {
                    transformName = transformName.Replace($"cy{i}", "cy");
                }
                if (eMode == Editor2DMode.EcuTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                    FieldInfo fieldTransform = cs.ConfigStruct.tables[cs.CurrentTable].transform.GetType().GetField(transformName);
                    if (fieldArray != null && fieldTransform != null)
                    {
                        EcuParamTransform transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.tables[cs.CurrentTable].transform);
                        Array arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                        array2d = EcuConfigTransform.FromInteger(arraydef, transform);
                    }
                }
                else if (eMode == Editor2DMode.CorrectionsTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.corrections.GetType().GetField(sArrayName);
                    FieldInfo fieldTransform = cs.ConfigStruct.corrections.transform.GetType().GetField(transformName);
                    if (fieldArray != null && fieldTransform != null)
                    {
                        EcuParamTransform transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.corrections.transform);
                        Array arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.corrections);
                        array2d = EcuConfigTransform.FromInteger(arraydef, transform);
                    }
                }
            }

            if (iArraySizeX > 0 && iArraySizeY > 0 && array2d != null)
            {
                if (iDecPlaces > 0)
                    decplaces = "." + Enumerable.Repeat("0", iDecPlaces).Aggregate((sum, next) => sum + next);

                for (int y = 0; y < iArraySizeY; y++)
                {
                    line = string.Empty;
                    text += "\t{ ";
                    for (int x = 0; x < iArraySizeX; x++)
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
            EcuParamTransform transform = default(EcuParamTransform);
            Array arraydef = null;
            float[] array2d = null;
            float[][] array;
            int index;
            
            if (!string.IsNullOrWhiteSpace(sArrayName))
            {
                string transformName = sArrayName;
                for (int i = 1; i <= Consts.ECU_CYLINDERS_COUNT; i++)
                {
                    transformName = transformName.Replace($"cy{i}", "cy");
                }
                if (eMode == Editor2DMode.EcuTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                    FieldInfo fieldTransform = cs.ConfigStruct.tables[cs.CurrentTable].transform.GetType().GetField(transformName);
                    if (fieldArray != null && fieldTransform != null)
                    {
                        transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.tables[cs.CurrentTable].transform);
                        arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                        array2d = EcuConfigTransform.FromInteger(arraydef, transform);
                    }
                }
                else if (eMode == Editor2DMode.CorrectionsTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.corrections.GetType().GetField(sArrayName);
                    FieldInfo fieldTransform = cs.ConfigStruct.corrections.transform.GetType().GetField(transformName);
                    if (fieldArray != null && fieldTransform != null)
                    {
                        transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.corrections.transform);
                        arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.corrections);
                        array2d = EcuConfigTransform.FromInteger(arraydef, transform);
                    }
                }
            }

            if (iArraySizeX > 0 && iArraySizeY > 0 && array2d != null)
            {
                if (dlgImport2DChart.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        array = Serializator<float[][]>.Deserialize(dlgImport2DChart.FileName);

                        if (array.Length != iArraySizeY)
                            throw new Exception("Y size not equals to original array.");

                        for (int y = 0; y < iArraySizeY; y++)
                        {
                            if (array[y].Length != iArraySizeX)
                                throw new Exception("X size not equals to original array.");

                            for (int x = 0; x < iArraySizeX; x++)
                            {
                                index = y * iArraySizeX + x;
                                array2d[index] = array[y][x];
                            }
                        }
                        
                        EcuConfigTransform.ToInteger(array2d, arraydef, transform);
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
            float[] array2d = null;
            float[][] array;
            int index;

            if (!string.IsNullOrWhiteSpace(sArrayName))
            {
                string transformName = sArrayName;
                for (int i = 1; i <= Consts.ECU_CYLINDERS_COUNT; i++)
                {
                    transformName = transformName.Replace($"cy{i}", "cy");
                }
                if (eMode == Editor2DMode.EcuTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                    FieldInfo fieldTransform = cs.ConfigStruct.tables[cs.CurrentTable].transform.GetType().GetField(transformName);
                    if (fieldArray != null && fieldTransform != null)
                    {
                        EcuParamTransform transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.tables[cs.CurrentTable].transform);
                        Array arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                        array2d = EcuConfigTransform.FromInteger(arraydef, transform);
                    }
                }
                else if (eMode == Editor2DMode.CorrectionsTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.corrections.GetType().GetField(sArrayName);
                    FieldInfo fieldTransform = cs.ConfigStruct.corrections.transform.GetType().GetField(transformName);
                    if (fieldArray != null && fieldTransform != null)
                    {
                        EcuParamTransform transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.corrections.transform);
                        Array arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.corrections);
                        array2d = EcuConfigTransform.FromInteger(arraydef, transform);
                    }
                }
            }

            if (iArraySizeX > 0 && iArraySizeY > 0 && array2d != null)
            {
                if (dlgExport2DChart.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        array = new float[iArraySizeY][];
                        for(int y = 0; y < iArraySizeY; y++)
                        {
                            array[y] = new float[iArraySizeX];
                            for (int x = 0; x < iArraySizeX; x++)
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
            EcuParamTransform transform = default(EcuParamTransform);
            Array arraydef = null;
            float[] array2d = null;
            float[] array_initial = null;
            int index_array;
            int index_output;

            if (!string.IsNullOrWhiteSpace(sArrayName))
            {
                string transformName = sArrayName;
                for (int i = 1; i <= Consts.ECU_CYLINDERS_COUNT; i++)
                {
                    transformName = transformName.Replace($"cy{i}", "cy");
                }
                if (eMode == Editor2DMode.EcuTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                    FieldInfo fieldTransform = cs.ConfigStruct.tables[cs.CurrentTable].transform.GetType().GetField(transformName);
                    if (fieldArray != null && fieldTransform != null)
                    {
                        transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.tables[cs.CurrentTable].transform);
                        arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                        array2d = EcuConfigTransform.FromInteger(arraydef, transform);
                    }
                }
                else if (eMode == Editor2DMode.CorrectionsTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.corrections.GetType().GetField(sArrayName);
                    FieldInfo fieldTransform = cs.ConfigStruct.corrections.transform.GetType().GetField(transformName);
                    if (fieldArray != null && fieldTransform != null)
                    {
                        transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.corrections.transform);
                        arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.corrections);
                        array2d = EcuConfigTransform.FromInteger(arraydef, transform);
                    }
                }
            }


            if (iArraySizeX > 0 && iArraySizeY > 0 && array2d != null)
            {
                array_initial = new float[iArraySizeY * iArraySizeX];
                for (int y = 0; y < iArraySizeY; y++)
                {
                    for (int x = 0; x < iArraySizeX; x++)
                    {
                        index_array = y * iArraySizeX + x;
                        index_output = y * iArraySizeX + x;
                        if (array2d[index_array] > (float)dMaxY)
                            array2d[index_array] = (float)dMaxY;
                        if (array2d[index_array] < (float)dMinY)
                            array2d[index_array] = (float)dMinY;
                        array_initial[index_output] = array2d[index_array];
                    }
                }

                ImportCCodeForm importCCodeForm = new ImportCCodeForm(ArrayType.Array2D, array_initial, iArraySizeX, iArraySizeY);

                DialogResult result = importCCodeForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    float[] output = importCCodeForm.GetResult();

                    for (int y = 0; y < iArraySizeY; y++)
                    {
                        for (int x = 0; x < iArraySizeX; x++)
                        {
                            index_array = y * iArraySizeX + x;
                            index_output = y * iArraySizeX + x;
                            array2d[index_array] = output[index_output];
                        }
                    }

                    EcuConfigTransform.ToInteger(array2d, arraydef, transform);
                    this.UpdateChart();
                    UpdateTableEvent?.Invoke(sender, new EventArgs());
                }
            }
        }

        private void btnCopyToText_Click(object sender, EventArgs e)
        {
            float[] array2d = null;
            int index;
            string text = string.Empty;
            string line = string.Empty;
            string decplaces = string.Empty;

            if (!string.IsNullOrWhiteSpace(sArrayName))
            {
                string transformName = sArrayName;
                for (int i = 1; i <= Consts.ECU_CYLINDERS_COUNT; i++)
                {
                    transformName = transformName.Replace($"cy{i}", "cy");
                }
                if (eMode == Editor2DMode.EcuTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                    FieldInfo fieldTransform = cs.ConfigStruct.tables[cs.CurrentTable].transform.GetType().GetField(transformName);
                    if (fieldArray != null && fieldTransform != null)
                    {
                        EcuParamTransform transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.tables[cs.CurrentTable].transform);
                        Array arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                        array2d = EcuConfigTransform.FromInteger(arraydef, transform);
                    }
                }
                else if (eMode == Editor2DMode.CorrectionsTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.corrections.GetType().GetField(sArrayName);
                    FieldInfo fieldTransform = cs.ConfigStruct.corrections.transform.GetType().GetField(transformName);
                    if (fieldArray != null && fieldTransform != null)
                    {
                        EcuParamTransform transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.corrections.transform);
                        Array arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.corrections);
                        array2d = EcuConfigTransform.FromInteger(arraydef, transform);
                    }
                }
            }

            if (iArraySizeX > 0 && iArraySizeY > 0 && array2d != null)
            {
                if (iDecPlaces > 0)
                    decplaces = "." + Enumerable.Repeat("0", iDecPlaces).Aggregate((sum, next) => sum + next);

                for (int y = 0; y < iArraySizeY; y++)
                {
                    line = string.Empty;
                    for (int x = 0; x < iArraySizeX; x++)
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
            EcuParamTransform transform = default(EcuParamTransform);
            Array arraydef = null;
            float[] array2d = null;
            float[] array_initial = null;
            int index_array;
            int index_output;
            
            if (!string.IsNullOrWhiteSpace(sArrayName))
            {
                string transformName = sArrayName;
                for (int i = 1; i <= Consts.ECU_CYLINDERS_COUNT; i++)
                {
                    transformName = transformName.Replace($"cy{i}", "cy");
                }
                if (eMode == Editor2DMode.EcuTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.tables[cs.CurrentTable].GetType().GetField(sArrayName);
                    FieldInfo fieldTransform = cs.ConfigStruct.tables[cs.CurrentTable].transform.GetType().GetField(transformName);
                    if (fieldArray != null && fieldTransform != null)
                    {
                        transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.tables[cs.CurrentTable].transform);
                        arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.tables[cs.CurrentTable]);
                        array2d = EcuConfigTransform.FromInteger(arraydef, transform);
                    }
                }
                else if (eMode == Editor2DMode.CorrectionsTable)
                {
                    FieldInfo fieldArray = cs.ConfigStruct.corrections.GetType().GetField(sArrayName);
                    FieldInfo fieldTransform = cs.ConfigStruct.corrections.transform.GetType().GetField(transformName);
                    if (fieldArray != null && fieldTransform != null)
                    {
                        transform = (EcuParamTransform)fieldTransform.GetValue(cs.ConfigStruct.corrections.transform);
                        arraydef = (Array)fieldArray.GetValue(cs.ConfigStruct.corrections);
                        array2d = EcuConfigTransform.FromInteger(arraydef, transform);
                    }
                }
            }


            if (iArraySizeX > 0 && iArraySizeY > 0 && array2d != null)
            {
                array_initial = new float[iArraySizeY * iArraySizeX];
                for (int y = 0; y < iArraySizeY; y++)
                {
                    for (int x = 0; x < iArraySizeX; x++)
                    {
                        index_array = y * iArraySizeX + x;
                        index_output = y * iArraySizeX + x;
                        if (array2d[index_array] > (float)dMaxY)
                            array2d[index_array] = (float)dMaxY;
                        if (array2d[index_array] < (float)dMinY)
                            array2d[index_array] = (float)dMinY;
                        array_initial[index_output] = array2d[index_array];
                    }
                }

                ImportTextForm importTextForm = new ImportTextForm(ArrayType.Array2D, array_initial, iArraySizeX, iArraySizeY);

                DialogResult result = importTextForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    float[] output = importTextForm.GetResult();

                    for (int y = 0; y < iArraySizeY; y++)
                    {
                        for (int x = 0; x < iArraySizeX; x++)
                        {
                            index_array = y * iArraySizeX + x;
                            index_output = y * iArraySizeX + x;
                            array2d[index_array] = output[index_output];
                        }
                    }
                    EcuConfigTransform.ToInteger(array2d, arraydef, transform);
                    this.UpdateChart();
                    UpdateTableEvent?.Invoke(sender, new EventArgs());
                }
            }
        }
    }
}
