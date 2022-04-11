using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ECU_Manager.Packets;
using ECU_Manager.Structs;
using ECU_Manager.Tools;

namespace ECU_Manager
{
    public partial class DiagForm : Form, IEcuEventHandler
    { 
        [Serializable]
        class PointData
        {
            public EcuParameters Parameters;
            public double Seconds;
        }
        
        int[] TimeScaleTime = new int[(int)TimeScaleEnum.TimeScaleCount] { 1,2,5,10,20,30,60,120,180,300,600 };

        enum TimeScaleEnum
        {
            OneSecond = 0,
            TwoSeconds,
            FiveSeconds,
            TenSeconds,
            TwentySeconds,
            ThirtySeconds,
            OneMinute,
            TwoMinutes,
            ThreeMinutes,
            FiveMinutes,
            TenMinutes,
            TimeScaleCount,
        }

        double dTimeFrom;
        double dTimeTo;
        TimeScaleEnum TimeScale = TimeScaleEnum.TenSeconds;
        List<PointData> points = new List<PointData>(1048576);
        Stopwatch stopwatch = new Stopwatch();
        FieldInfo[] fieldInfos = typeof(EcuParameters).GetFields();
        bool widthBigPrev = false;

        List<Label> lblValues = new List<Label>();
        List<Chart> chartValues = new List<Chart>();

        public DiagForm(MiddleLayer middleLayer)
        {
            InitializeComponent();
            middleLayer.RegisterEventHandler(this);

            lbAvailable.Items.Clear();
            lbUsed.Items.Clear();

            foreach(FieldInfo fieldInfo in fieldInfos)
            {
                lbAvailable.Items.Add(fieldInfo.Name);
            }

            lbUsed.Items.Add("RPM");
            lbUsed.Items.Add("ManifoldAirPressure");
            lbUsed.Items.Add("ThrottlePosition");
            lbUsed.Items.Add("IgnitionAngle");
            lbUsed.Items.Add("FuelRatio");
            lbUsed.Items.Add("CyclicAirFlow");
            lbUsed.Items.Add("InjectionPulse");

            foreach(string item in lbUsed.Items)
            {
                lbAvailable.Items.Remove(item);
            }

            UpdateChartsSetup();

            stopwatch.Restart();
            this.DoubleBuffered = true;

            dTimeFrom = 0;
            dTimeTo = TimeScaleTime[(int)TimeScale];

        }

        private string GetFormat(FieldInfo fieldInfo)
        {
            if (fieldInfo.Name == "AdcKnockVoltage") return "F2"; //&ecu_parameters.AdcKnockVoltage, .title = "%s%0.2f"},
            if (fieldInfo.Name == "AdcAirTemp") return "F2"; //&ecu_parameters.AdcAirTemp, .title = "%s%0.2f"},
            if (fieldInfo.Name == "AdcEngineTemp") return "F2"; //&ecu_parameters.AdcEngineTemp, .title = "%s%0.2f"},
            if (fieldInfo.Name == "AdcPressure") return "F2"; //&ecu_parameters.AdcManifoldAirPressure, .title = "%s%0.2f"},
            if (fieldInfo.Name == "AdcThrottlePosition") return "F2"; //&ecu_parameters.AdcThrottlePosition, .title = "%s%0.2f"},
            if (fieldInfo.Name == "AdcPowerVoltage") return "F2"; //&ecu_parameters.AdcPowerVoltage, .title = "%s%0.2f"},
            if (fieldInfo.Name == "AdcReferenceVoltage") return "F2"; //&ecu_parameters.AdcReferenceVoltage, .title = "%s%0.2f"},
            if (fieldInfo.Name == "AdcLambdaUR") return "F2"; //&ecu_parameters.AdcLambdaUR, .title = "%s%0.2f"},
            if (fieldInfo.Name == "AdcLambdaUA") return "F2"; //&ecu_parameters.AdcLambdaUA, .title = "%s%0.2f"},
            if (fieldInfo.Name == "KnockSensor") return "F2"; //&ecu_parameters.KnockSensor, .title = "%s%0.2f"},
            if (fieldInfo.Name == "KnockSensorFiltered") return "F2"; //&ecu_parameters.KnockSensorFiltered, .title = "%s%0.2f"},
            if (fieldInfo.Name == "AirTemp") return "F1"; //&ecu_parameters.AirTemp, .title = "%s%0.1f"},
            if (fieldInfo.Name == "EngineTemp") return "F1"; //&ecu_parameters.EngineTemp, .title = "%s%0.1f"},
            if (fieldInfo.Name == "Pressure") return "F0"; //&ecu_parameters.ManifoldAirPressure, .title = "%s%0.0f"},
            if (fieldInfo.Name == "ThrottlePosition") return "F1"; //&ecu_parameters.ThrottlePosition, .title = "%s%0.1f"},
            if (fieldInfo.Name == "ReferenceVoltage") return "F2"; //&ecu_parameters.ReferenceVoltage, .title = "%s%0.2f"},
            if (fieldInfo.Name == "PowerVoltage") return "F2"; //&ecu_parameters.PowerVoltage, .title = "%s%0.2f"},
            if (fieldInfo.Name == "FuelRatio") return "F2"; //&ecu_parameters.FuelRatio, .title = "%s%0.2f"},
            if (fieldInfo.Name == "LongTermCorrection") return "F2"; //&ecu_parameters.LongTermCorrection, .title = "%s%0.2f"},
            if (fieldInfo.Name == "RPM") return "F0"; //&ecu_parameters.RPM, .title = "%s%0.0f"},
            if (fieldInfo.Name == "Speed") return "F1"; //&ecu_parameters.Speed, .title = "%s%0.1f"},
            if (fieldInfo.Name == "Acceleration") return "F2"; //&ecu_parameters.Acceleration, .title = "%s%0.2f"},
            if (fieldInfo.Name == "MassAirFlow") return "F1"; //&ecu_parameters.MassAirFlow, .title = "%s%0.1f"},
            if (fieldInfo.Name == "CyclicAirFlow") return "F1"; //&ecu_parameters.CyclicAirFlow, .title = "%s%0.1f"},
            if (fieldInfo.Name == "EffectiveVolume") return "F1"; //&ecu_parameters.EffectiveVolume, .title = "%s%0.1f"},
            if (fieldInfo.Name == "AirDestiny") return "F3"; //&ecu_parameters.AirDestiny, .title = "%s%0.3f"},
            if (fieldInfo.Name == "WishFuelRatio") return "F2"; //&ecu_parameters.WishFuelRatio, .title = "%s%0.2f"},
            if (fieldInfo.Name == "IdleValvePosition") return "F0"; //&ecu_parameters.IdleValvePosition, .title = "%s%0.0f"},
            if (fieldInfo.Name == "WishIdleRPM") return "F0"; //&ecu_parameters.WishIdleRPM, .title = "%s%0.0f"},
            if (fieldInfo.Name == "WishIdleMassAirFlow") return "F1"; //&ecu_parameters.WishIdleMassAirFlow, .title = "%s%0.1f"},
            if (fieldInfo.Name == "WishIdleValvePosition") return "F0"; //&ecu_parameters.WishIdleValvePosition, .title = "%s%0.0f"},
            if (fieldInfo.Name == "WishIdleIgnitionAngle") return "F0"; //&ecu_parameters.WishIdleIgnitionAngle, .title = "%s%0.0f"},
            if (fieldInfo.Name == "IgnitionAngle") return "F1"; //&ecu_parameters.IgnitionAngle, .title = "%s%0.0f"},
            if (fieldInfo.Name == "InjectionPhase") return "F0"; //&ecu_parameters.InjectionPhase, .title = "%s%0.0f"},
            if (fieldInfo.Name == "InjectionPhaseDuration") return "F0"; //&ecu_parameters.InjectionPhaseDuration, .title = "%s%0.0f"},
            if (fieldInfo.Name == "InjectionPulse") return "F2"; //&ecu_parameters.InjectionPulse, .title = "%s%0.2f"},
            if (fieldInfo.Name == "InjectionDutyCycle") return "F2"; //&ecu_parameters.InjectionDutyCycle, .title = "%s%0.2f"},
            if (fieldInfo.Name == "InjectionEnrichment") return "F2"; //&ecu_parameters.InjectionEnrichment, .title = "%s%0.2f"},
            if (fieldInfo.Name == "IgnitionPulse") return "F1"; //&ecu_parameters.IgnitionPulse, .title = "%s%0.2f"},
            if (fieldInfo.Name == "IdleSpeedShift") return "F0"; //&ecu_parameters.IdleSpeedShift, .title = "%s%0.0f"},
            if (fieldInfo.Name == "DrivenKilometers") return "F2"; //&ecu_parameters.DrivenKilometers, .title = "%s%0.1f"},
            if (fieldInfo.Name == "FuelConsumed") return "F3"; //&ecu_parameters.FuelConsumed, .title = "%s%0.1f"},
            if (fieldInfo.Name == "FuelConsumption") return "F1"; //&ecu_parameters.FuelConsumption, .title = "%s%0.1f"},
            return "F0";

        }

        private void UpdateChartsSetup()
        {
            this.SuspendLayout();
            tlpCharts.Controls.Clear();
            tlpCharts.RowStyles.Clear();
            tlpCharts.ColumnStyles.Clear();
            lblValues.Clear();
            chartValues.Clear();

            foreach (string obj in lbUsed.Items)
            {
                TableLayoutPanel tlp = new TableLayoutPanel();
                Label labelValue = new Label();
                Label labelTitle = new Label();
                Chart chart = new Chart();
                object tag = fieldInfos.Where(f => f.Name.Equals(obj)).FirstOrDefault();

                labelTitle.Name = "labelTitle" + obj.ToString();
                labelTitle.Font = new Font(this.Font.FontFamily, 10, FontStyle.Bold);
                labelTitle.Text = obj.ToString();
                labelTitle.Dock = DockStyle.Fill;
                labelTitle.TextAlign = ContentAlignment.MiddleCenter;
                labelTitle.Margin = new Padding(0, 0, 0, 0);
                labelTitle.Padding = new Padding(0, 0, 0, 0);
                labelTitle.Tag = tag;

                labelValue.Name = "labelValue" + obj.ToString();
                labelValue.Font = new Font(this.Font.FontFamily, 24, FontStyle.Regular);
                labelValue.Text = "0";
                labelValue.Dock = DockStyle.Fill;
                labelValue.TextAlign = ContentAlignment.TopCenter;
                labelValue.Margin = new Padding(0, 0, 0, 0);
                labelValue.Padding = new Padding(0, 0, 0, 0);
                labelValue.Tag = tag;

                chart.Dock = DockStyle.Fill;
                chart.Margin = new Padding(0, 0, 0, 0);
                chart.Padding = new Padding(0, 0, 0, 0);
                chart.BackColor = Color.Transparent;
                chart.BackGradientStyle = GradientStyle.HorizontalCenter;
                chart.BackSecondaryColor = Color.FromArgb(24, 64, 0);
                chart.Text = "chart" + obj.ToString();
                chart.Tag = tag;

                ChartArea area = chart.ChartAreas.Add("ChartArea" + obj.ToString());
                area.BackColor = Color.Transparent;
                area.Position = new ElementPosition(0, 0, 99, 100);
                area.AxisX.LineColor = Color.DimGray;
                area.AxisX.LabelStyle.ForeColor = Color.White;
                area.AxisX.LabelStyle.Font = new Font(area.AxisX.LabelStyle.Font.FontFamily, 8F, FontStyle.Regular);
                area.AxisY.LineColor = Color.DimGray;
                area.AxisY.LabelStyle.ForeColor = Color.White;
                area.AxisY.LabelStyle.Font = new Font(area.AxisX.LabelStyle.Font.FontFamily, 8F, FontStyle.Regular);

                Series series = chart.Series.Add("ChartSeries" + obj.ToString());
                series.LabelForeColor = Color.White;
                series.MarkerSize = 7;
                series.MarkerStyle = MarkerStyle.Square;
                series.ChartType = SeriesChartType.Line;
                series.Color = Color.Gold;
                series.BorderWidth = 3;
               
                lblValues.Add(labelValue);
                chartValues.Add(chart);

                tlp.ColumnStyles.Clear();
                tlp.RowStyles.Clear();
                
                tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
                tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));

                tlp.Dock = DockStyle.Fill;
                tlp.Name = "tlp" + obj.ToString();
                tlp.Margin = new Padding(0, 0, 0, 0);
                tlp.Padding = new Padding(0, 0, 0, 0);
                tlp.Controls.Add(chart, 0, 0);
                tlp.Controls.Add(labelTitle, 1, 0);
                tlp.Controls.Add(labelValue, 1, 1);
                tlp.SetRowSpan(chart, 2);
                tlp.Tag = tag;

                tlpCharts.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                tlpCharts.Controls.Add(tlp, 0, tlpCharts.Controls.Count);


            }
            UpdateCharts();
            this.ResumeLayout();
        }

        private void UpdateCharts()
        {
            FieldInfo fieldInfo;
            float valuef;
            int valuei;

            if (points.Count > 0)
            {
                PointData current = points.LastOrDefault();
                if (current != null)
                {
                    foreach (Label label in lblValues)
                    {
                        if (label.Tag != null)
                        {
                            fieldInfo = (FieldInfo)label.Tag;
                            if (fieldInfo.FieldType == typeof(float))
                            {
                                valuef = (float)fieldInfo.GetValue(current.Parameters);
                                label.Text = valuef.ToString(GetFormat(fieldInfo));
                            }
                            else
                            {
                                label.Text = fieldInfo.GetValue(current.Parameters).ToString();
                            }
                        }
                    }
                }


                if (cbLiveView.Checked)
                {
                    dTimeFrom = current.Seconds - TimeScaleTime[(int)TimeScale];
                    dTimeTo = current.Seconds;
                }
                else
                {
                    dTimeFrom = hScrollBar1.Value / 1000.0D;
                    dTimeTo = dTimeFrom + TimeScaleTime[(int)TimeScale];
                }

                if (dTimeFrom < 0)
                {
                    dTimeFrom = 0;
                    dTimeTo = TimeScaleTime[(int)TimeScale];
                }

                int index_first = 0;
                int index_last = 0;
                double[] seconds = points.Select(p => p.Seconds).ToArray();

                if (dTimeFrom <= points.First().Seconds)
                {
                    index_first = 0;
                }
                else
                {
                    index_first = BinarySearch<double>.Find(seconds, 0, points.Count - 1, dTimeFrom);
                    if (index_first < 0)
                        index_first = 0;
                }

                if (dTimeFrom >= points.Last().Seconds)
                {
                    index_last = 0;
                }
                else
                {
                    index_last = BinarySearch<double>.Find(seconds, 0, points.Count - 1, dTimeTo);
                    if (index_last < 0)
                        index_last = index_first;
                    else if(index_last + 1 < seconds.Count())
                        index_last++;
                    
                }
                
                hScrollBar1.Minimum = 0;
                hScrollBar1.Maximum = (int)stopwatch.ElapsedMilliseconds;
                hScrollBar1.LargeChange = TimeScaleTime[(int)TimeScale] * 1000;
                hScrollBar1.SmallChange = 10;

                if (cbLiveView.Checked)
                {
                    hScrollBar1.Enabled = false;
                    hScrollBar1.Value = (int)(Math.Ceiling(dTimeFrom) * 1000.0D);
                }
                else
                {
                    hScrollBar1.Enabled = true;
                }


                double posmin, posmax;

                if (cbLiveView.Checked)
                {
                    posmin = Math.Ceiling(dTimeFrom);
                    posmax = Math.Ceiling(dTimeTo);

                    while (posmax > dTimeTo && posmin >= 0.2D)
                    {
                        posmin -= 0.2D;
                        posmax -= 0.2D;
                    }
                }
                else
                {
                    posmin = dTimeFrom;
                    posmax = dTimeTo;
                }

                lblTimePos.Text = posmin.ToString() + "s";
                lblTimeScale.Text = "+" + TimeScaleTime[(int)TimeScale] + "s";

                foreach (Chart chart in chartValues)
                {
                    chart.SuspendLayout();
                    
                    chart.ChartAreas[0].AxisX.Minimum = posmin;
                    chart.ChartAreas[0].AxisX.Maximum = posmax;
                    
                    for (int i = index_first; i <= index_last; i++)
                    {
                        if (chart.Series[0].Points.Count == 0 || chart.Series[0].Points.Last().XValue < points[i].Seconds)
                        {
                            if (chart.Tag != null)
                            {
                                fieldInfo = (FieldInfo)chart.Tag;

                                if (fieldInfo.FieldType == typeof(float))
                                {
                                    valuef = (float)fieldInfo.GetValue(points[i].Parameters);
                                    chart.Series[0].Points.AddXY(points[i].Seconds, valuef);
                                }
                                else
                                {
                                    valuei = (int)fieldInfo.GetValue(points[i].Parameters);
                                    chart.Series[0].Points.AddXY(points[i].Seconds, valuei);
                                }
                            }
                        }


                    }
                    double min = Math.Floor(chart.Series[0].Points.Select(p => p.YValues[0]).Min());
                    double max = Math.Ceiling(chart.Series[0].Points.Select(p => p.YValues[0]).Max());

                    if (min == max)
                    {
                        min -= 0.5D;
                        max += 0.5D;
                    }
                    
                    chart.ChartAreas[0].AxisX.Interval = TimeScaleTime[(int)TimeScale] / 10.0D;
                    chart.ChartAreas[0].AxisY.Minimum = min;
                    chart.ChartAreas[0].AxisY.Maximum = max;

                    if (chart.Series[0].Points.Count > chart.Width / 34)
                        chart.Series[0].ChartType = SeriesChartType.FastLine;
                    else chart.Series[0].ChartType = SeriesChartType.Line;
                    chart.ResumeLayout();
                }
            }
        }

        public void UpdateParametersEvent(EcuParameters parameters)
        {
            Action action = new Action(() => { this.UpdateParameters(parameters); });
            if (this.InvokeRequired)
                this.BeginInvoke(action);
            else action.Invoke();
        }

        public void DragStartAckEvent(PK_DragStartAcknowledge dsaa)
        {
        }

        public void SynchronizedEvent(int errorCode)
        {
        }

        public void DragStopAckEvent(PK_DragStopAcknowledge dsta)
        {
        }

        public void UpdateDragPointEvent(PK_DragPointResponse dpr)
        {
        }

        public void UpdateDragStatusEvent(PK_DragUpdateResponse dur)
        {
        }

        private void UpdateParameters(EcuParameters parameters)
        {
            points.Add(new PointData { Parameters = parameters, Seconds = stopwatch.ElapsedMilliseconds * 0.001D });
        }

        private void DiagForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void DiagForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.Size.Width >= 600)
            {
                if (!widthBigPrev)
                {
                    widthBigPrev = true;
                    tabControl1.TabPages.Clear();

                    splitContainer1.Panel1.Controls.Add(panelChart);
                    splitContainer1.Panel2.Controls.Add(panelSetup);
                    splitContainer1.Visible = true;
                    tabControl1.Visible = false;
                }
                splitContainer1.SplitterDistance = splitContainer1.Size.Width - splitContainer1.Panel2MinSize;
            }
            else if(this.Size.Width < 600 && widthBigPrev)
            {
                widthBigPrev = false;
                
                splitContainer1.Panel1.Controls.Clear();
                splitContainer1.Panel2.Controls.Clear();
                splitContainer1.Visible = false;
                tabControl1.Visible = true;

                TabPage tabPage = new TabPage();
                tabPage.Text = "Chart";
                tabPage.Margin = new Padding(0, 0, 0, 0);
                tabPage.Padding = new Padding(0, 0, 0, 0);
                tabPage.Controls.Add(panelChart);
                tabControl1.TabPages.Add(tabPage);

                tabPage = new TabPage();
                tabPage.Text = "Setup";
                tabPage.Margin = new Padding(0, 0, 0, 0);
                tabPage.Padding = new Padding(0, 0, 0, 0);
                tabPage.Controls.Add(panelSetup);
                tabControl1.TabPages.Add(tabPage);

            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int index = lbUsed.SelectedIndex;
            string selected = (string)lbAvailable.SelectedItem;
            if (index >= 0)
            {
                lbUsed.Items.RemoveAt(index);
                lbAvailable.Items.Clear();

                FieldInfo[] fieldInfos = typeof(EcuParameters).GetFields();
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    lbAvailable.Items.Add(fieldInfo.Name);
                }
                foreach (string item in lbUsed.Items)
                {
                    lbAvailable.Items.Remove(item);
                }
                if(!string.IsNullOrWhiteSpace(selected))
                {
                    lbAvailable.SelectedItem = selected;
                }
                UpdateChartsSetup();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int index = lbAvailable.SelectedIndex;
            string item = (string)lbAvailable.SelectedItem;
            if (index >= 0)
            {
                lbAvailable.Items.RemoveAt(index);
                lbUsed.Items.Add(item);
                UpdateChartsSetup();
            }
        }

        private void btnUsedMoveUp_Click(object sender, EventArgs e)
        {
            int index = lbUsed.SelectedIndex;
            string item = (string)lbUsed.SelectedItem;
            if (index > 0)
            {
                lbUsed.Items.RemoveAt(index);
                lbUsed.Items.Insert(index - 1, item);
                lbUsed.SelectedIndex = index - 1;
                UpdateChartsSetup();
            }
        }

        private void btnUsedMoveDown_Click(object sender, EventArgs e)
        {
            int index = lbUsed.SelectedIndex;
            string item = (string)lbUsed.SelectedItem;
            if (index >= 0 && index < lbUsed.Items.Count - 1)
            {
                lbUsed.Items.RemoveAt(index);
                lbUsed.Items.Insert(index + 1, item);
                lbUsed.SelectedIndex = index + 1;
                UpdateChartsSetup();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateCharts();
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            if((int)TimeScale > 0)
            {
                TimeScale--;
            }
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            if ((int)TimeScale + 1 < (int)TimeScaleEnum.TimeScaleCount)
            {
                TimeScale++;
            }
        }
    }
}
 