using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ECU_Manager.Packets;
using ECU_Manager.Tables;
using ECU_Manager.Tools;

namespace ECU_Manager
{
    public partial class MainForm : Form
    {
        MiddleLayer middleLayer;
        SyncForm syncForm;
        DateTime lastReceivedGeneralStatus = DateTime.Now;
        int iCurrentTable = 0;
        bool generalStatusReceived = false;

        List<int> tlpRows = new List<int>();
        List<int> tlpColumns = new List<int>();
        int tlpInfoRow = 0;
        int tlpInfoColumn = 0;

        class DragPoint
        {
            public double RPM;
            public double Pressure;
            public double Load;
            public double Ignition;
            public double Time;
        }

        class DragRun
        {
            public List<DragPoint> Points;
            public int TotalPoints;
            public float FromRPM;
            public float ToRPM;
            public double Time;
            public DateTime DateTime;
            public string Name;
            public string Label;
        }

        public enum DragStatusType
        {
            Ready = 0,
            Set,
            Go,
            Done,
            Fail

        }

        DragRun drCurrentRun = null;
        List<DragRun> lDragRuns = new List<DragRun>();
        DragStatusType eDragStatus = DragStatusType.Ready;
        float fDragFromRPM = 2000;
        float fDragToRPM = 3000;
        PK_GeneralStatusResponse GeneralStatus;

        public MainForm(string portname)
        {
            InitializeComponent();
            middleLayer = new MiddleLayer(this, portname);

            tlpIgnitions.RowStyles.Clear();
            tlpIgnitions.ColumnStyles.Clear();

            tlpInfoColumn = tlpIgnitions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 0.0f));
            tlpInfoRow = tlpIgnitions.RowStyles.Add(new RowStyle(SizeType.Absolute, 0.0f));
            for (int y = 0; y < Consts.TABLE_PRESSURES_MAX; y++)
                tlpRows.Add(tlpIgnitions.RowStyles.Add(new RowStyle(SizeType.Absolute, 0.0f)));
            for (int x = 0; x < Consts.TABLE_ROTATES_MAX; x++)
                tlpColumns.Add(tlpIgnitions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 0.0f)));
            tlpIgnitions.ColumnCount = Consts.TABLE_ROTATES_MAX + 1;
            tlpIgnitions.RowCount = Consts.TABLE_PRESSURES_MAX + 1;
            

            for (int y = -1; y < Consts.TABLE_PRESSURES_MAX; y++)
            {
                for (int x = -1; x < Consts.TABLE_ROTATES_MAX; x++)
                {
                    if (x >= 0 && y >= 0)
                    {
                        NumericUpDown nud = new NumericUpDown();
                        nud.Margin = new Padding(0);
                        nud.Minimum = (decimal)-10.0f;
                        nud.Maximum = (decimal)60.0f;
                        nud.DecimalPlaces = 1;
                        nud.Increment = (decimal)0.5f;
                        nud.Tag = y * Consts.TABLE_ROTATES_MAX + x;
                        nud.Value = 0;
                        nud.ValueChanged += nudIgnition_ValueChanged;
                        nud.Location = new Point(0, 0);
                        nud.Dock = DockStyle.Fill;
                        nud.Visible = false;
                        tlpIgnitions.Controls.Add(nud, tlpColumns[x], tlpRows[y]);
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
                                lbl.Tag = 0 * (Consts.TABLE_ROTATES_MAX + 1) + (x + 1);
                                tlpIgnitions.Controls.Add(lbl, tlpColumns[x], tlpInfoRow);
                            }
                            else if (y != -1)
                            {
                                lbl.Tag = (y + 1) * (Consts.TABLE_ROTATES_MAX + 1) + 0;
                                tlpIgnitions.Controls.Add(lbl, tlpInfoColumn, tlpRows[y]);
                            }
                        }
                        else if(x == -1 && y == -1)
                        {
                            lbl.Text = @"Pa\RPM";
                            lbl.Visible = true;
                            lbl.Tag = null;
                            tlpIgnitions.Controls.Add(lbl, tlpInfoColumn, tlpInfoRow);
                        }
                    }
                }
            }

            middleLayer.SyncLoad(false);

            syncForm = new SyncForm();
            syncForm.ShowDialog();
            
        }

        public void SynchronizedEvent()
        {
            if (syncForm.InvokeRequired)
                syncForm.Invoke(new Action(() => syncForm.CloseForm()));
            else syncForm.Close();

            rbCutoffMode1.Checked = false;
            rbCutoffMode2.Checked = false;
            rbCutoffMode3.Checked = false;
            rbCutoffMode4.Checked = false;
            rbCutoffMode5.Checked = false;
            rbCutoffMode6.Checked = false;
            rbCutoffMode7.Checked = false;
            rbCutoffMode8.Checked = false;

            cbCutoffEnabled.Checked = middleLayer.Config.parameters.isCutoffEnabled > 0;
            switch (middleLayer.Config.parameters.CutoffMode)
            {
                case 0: rbCutoffMode1.Checked = true; break;
                case 1: rbCutoffMode2.Checked = true; break;
                case 2: rbCutoffMode3.Checked = true; break;
                case 3: rbCutoffMode4.Checked = true; break;
                case 4: rbCutoffMode5.Checked = true; break;
                case 5: rbCutoffMode6.Checked = true; break;
                case 6: rbCutoffMode7.Checked = true; break;
                case 7: rbCutoffMode8.Checked = true; break;
                default: break;
            }

            lblCutoffRPM.Text = middleLayer.Config.parameters.CutoffRPM.ToString("F0");
            tbCutoffRPM.Value = Convert.ToInt32(middleLayer.Config.parameters.CutoffRPM);

            lblCutoffAngle.Text = middleLayer.Config.parameters.CutoffAngle.ToString("F0");
            tbCutoffAngle.Value = Convert.ToInt32(middleLayer.Config.parameters.CutoffAngle);

            cbEconEnabled.Checked = middleLayer.Config.parameters.isEconomEnabled > 0;
            cbEconStrobe.Checked = middleLayer.Config.parameters.isEconOutAsStrobe > 0;
            cbEconIgnitionBreak.Checked = middleLayer.Config.parameters.isEconIgnitionOff > 0;

            lblEconRPM.Text = middleLayer.Config.parameters.EconRpmThreshold.ToString("F0");
            tbEconRPM.Value = Convert.ToInt32(middleLayer.Config.parameters.EconRpmThreshold);

            nudSwPos1.Value = middleLayer.Config.parameters.switchPos1Table + 1;
            nudSwPos0.Value = middleLayer.Config.parameters.switchPos0Table + 1;
            nudSwPos2.Value = middleLayer.Config.parameters.switchPos2Table + 1;
            nudFuelForce.Value = middleLayer.Config.parameters.forceTableNumber + 1;
            nudEngVol.Value = middleLayer.Config.parameters.engineVolume;
            nudForceIgnitionAngle.Value = middleLayer.Config.parameters.forceIgnitionAngle;

            cbTempEnabled.Checked = middleLayer.Config.parameters.isTemperatureEnabled > 0;
            cbAutostartEnabled.Checked = middleLayer.Config.parameters.isAutostartEnabled > 0;
            cbHallIgnition.Checked = middleLayer.Config.parameters.isIgnitionByHall > 0;
            cbHallLearn.Checked = middleLayer.Config.parameters.isHallLearningMode > 0;
            cbForceIgnition.Checked = middleLayer.Config.parameters.isForceIgnition > 0;
            cbFuelForce.Checked = middleLayer.Config.parameters.isForceTable > 0;
            cbFuelExtSw.Checked = middleLayer.Config.parameters.isSwitchByExternal > 0;

            nudToolsCurTable.Minimum = 1;
            nudToolsCurTable.Maximum = Consts.TABLE_SETUPS_MAX;
            nudToolsCurTable.Value = iCurrentTable + 1;

            UpdateIgnitionsGui();

            middleLayer.PacketHandler.SendGeneralStatusRequest();
        }

        private void UpdateIgnitionsGui()
        {
            Series series;
            tbParamsName.MaxLength = Consts.TABLE_STRING_MAX - 1;
            tbParamsName.Text = middleLayer.Config.tables[iCurrentTable].name;
            rbParamsValve0.Checked = false;
            rbParamsValve1.Checked = false;
            rbParamsValve2.Checked = false;

            switch(middleLayer.Config.tables[iCurrentTable].valve_channel)
            {
                case 0: rbParamsValve0.Checked = true; break;
                case 1: rbParamsValve1.Checked = true; break;
                case 2: rbParamsValve2.Checked = true; break;
                default: break;
            }
            nudParamsValveTimeout.Value = middleLayer.Config.tables[iCurrentTable].valve_timeout;
            nudParamsOctane.Value = (decimal)middleLayer.Config.tables[iCurrentTable].octane_corrector;
            nudParamsInitial.Value = (decimal)middleLayer.Config.tables[iCurrentTable].initial_ignition;

            nudParamsFuelRate.Value = (decimal)middleLayer.Config.tables[iCurrentTable].fuel_rate;
            nudParamsFuelVolume.Value = (decimal)middleLayer.Config.tables[iCurrentTable].fuel_volume;

            nudParamsCntPress.Maximum = Consts.TABLE_PRESSURES_MAX;
            nudParamsCntRPMs.Maximum = Consts.TABLE_ROTATES_MAX;
            nudParamsCntIdles.Maximum = Consts.TABLE_ROTATES_MAX;
            nudParamsCntTemps.Maximum = Consts.TABLE_TEMPERATURES_MAX;

            nudParamsCntPress.Value = middleLayer.Config.tables[iCurrentTable].pressures_count;
            nudParamsCntRPMs.Value = middleLayer.Config.tables[iCurrentTable].rotates_count;
            nudParamsCntIdles.Value = middleLayer.Config.tables[iCurrentTable].idles_count;
            nudParamsCntTemps.Value = middleLayer.Config.tables[iCurrentTable].temperatures_count;

            chartIgnPressures.Series[0].Points.Clear();
            chartIgnPressures.ChartAreas[0].AxisX.Minimum = 1;
            chartIgnPressures.ChartAreas[0].AxisX.Maximum = middleLayer.Config.tables[iCurrentTable].pressures_count;
            nudIgnPressItem.Minimum = 1;
            nudIgnPressItem.Maximum = Consts.TABLE_PRESSURES_MAX;
            nudIgnPressValue.Value = (decimal)middleLayer.Config.tables[iCurrentTable].pressures[(int)nudIgnPressItem.Value - 1];
            for (int i = 0; i < middleLayer.Config.tables[iCurrentTable].pressures_count; i++)
                chartIgnPressures.Series[0].Points[chartIgnPressures.Series[0].Points.AddXY(i + 1, middleLayer.Config.tables[iCurrentTable].pressures[i])].Tag = i;

            chartIgnRPMs.Series[0].Points.Clear();
            chartIgnRPMs.ChartAreas[0].AxisX.Minimum = 1;
            chartIgnRPMs.ChartAreas[0].AxisX.Maximum = middleLayer.Config.tables[iCurrentTable].rotates_count;
            nudIgnRPMItem.Minimum = 1;
            nudIgnRPMItem.Maximum = Consts.TABLE_ROTATES_MAX;
            nudIgnRPMValue.Value = (decimal)middleLayer.Config.tables[iCurrentTable].rotates[(int)nudIgnRPMItem.Value - 1];
            for (int i = 0; i < middleLayer.Config.tables[iCurrentTable].rotates_count; i++)
                chartIgnRPMs.Series[0].Points[chartIgnRPMs.Series[0].Points.AddXY(i + 1, middleLayer.Config.tables[iCurrentTable].rotates[i])].Tag = i;

            chartIdleRPMs.Series[0].Points.Clear();
            chartIdleRPMs.ChartAreas[0].AxisX.Minimum = 1;
            chartIdleRPMs.ChartAreas[0].AxisX.Maximum = middleLayer.Config.tables[iCurrentTable].idles_count;
            nudIdleRPMItem.Minimum = 1;
            nudIdleRPMItem.Maximum = Consts.TABLE_ROTATES_MAX;
            nudIdleRPMValue.Value = (decimal)middleLayer.Config.tables[iCurrentTable].idle_rotates[(int)nudIdleRPMItem.Value - 1];
            for (int i = 0; i < middleLayer.Config.tables[iCurrentTable].idles_count; i++)
                chartIdleRPMs.Series[0].Points[chartIdleRPMs.Series[0].Points.AddXY(i + 1, middleLayer.Config.tables[iCurrentTable].idle_rotates[i])].Tag = i;

            chartIdleAngles.Series[0].Points.Clear();
            chartIdleAngles.ChartAreas[0].AxisX.Minimum = middleLayer.Config.tables[iCurrentTable].idle_rotates[0] - (middleLayer.Config.tables[iCurrentTable].idle_rotates[0] % 50);
            chartIdleAngles.ChartAreas[0].AxisX.Maximum = middleLayer.Config.tables[iCurrentTable].idle_rotates[middleLayer.Config.tables[iCurrentTable].idles_count - 1] + (50 - (middleLayer.Config.tables[iCurrentTable].idle_rotates[middleLayer.Config.tables[iCurrentTable].idles_count - 1] % 50));
            nudIdleAngleItem.Minimum = 1;
            nudIdleAngleItem.Maximum = Consts.TABLE_ROTATES_MAX;
            nudIdleAngleValue.Value = (decimal)middleLayer.Config.tables[iCurrentTable].idle_ignitions[(int)nudIdleAngleItem.Value - 1];
            lblIdleRPM.Text = middleLayer.Config.tables[iCurrentTable].idle_rotates[(int)nudIdleAngleItem.Value - 1].ToString("F0");
            for (int i = 0; i < middleLayer.Config.tables[iCurrentTable].idles_count; i++)
                chartIdleAngles.Series[0].Points[(int)chartIdleAngles.Series[0].Points.AddXY(middleLayer.Config.tables[iCurrentTable].idle_rotates[i], middleLayer.Config.tables[iCurrentTable].idle_ignitions[i])].Tag = i;

            chartTemps.Series[0].Points.Clear();
            chartTemps.ChartAreas[0].AxisX.Minimum = 1;
            chartTemps.ChartAreas[0].AxisX.Maximum = middleLayer.Config.tables[iCurrentTable].temperatures_count;
            nudTempItem.Minimum = 1;
            nudTempItem.Maximum = Consts.TABLE_TEMPERATURES_MAX;
            nudTempValue.Value = (decimal)middleLayer.Config.tables[iCurrentTable].temperatures[(int)nudTempItem.Value - 1];
            for (int i = 0; i < middleLayer.Config.tables[iCurrentTable].temperatures_count; i++)
                chartTemps.Series[0].Points[chartTemps.Series[0].Points.AddXY(i + 1, middleLayer.Config.tables[iCurrentTable].temperatures[i])].Tag = i;

            chartTempAngles.Series[0].Points.Clear();
            chartTempAngles.ChartAreas[0].AxisX.Minimum = middleLayer.Config.tables[iCurrentTable].temperatures[0] - (middleLayer.Config.tables[iCurrentTable].temperatures[0] % 10);
            chartTempAngles.ChartAreas[0].AxisX.Maximum = middleLayer.Config.tables[iCurrentTable].temperatures[middleLayer.Config.tables[iCurrentTable].temperatures_count - 1] + (10 - (middleLayer.Config.tables[iCurrentTable].temperatures[middleLayer.Config.tables[iCurrentTable].temperatures_count - 1] % 10));
            nudTempAngleItem.Minimum = 1;
            nudTempAngleItem.Maximum = Consts.TABLE_TEMPERATURES_MAX;
            nudTempAngleValue.Value = (decimal)middleLayer.Config.tables[iCurrentTable].temperature_ignitions[(int)nudTempAngleItem.Value - 1];
            lblTemp.Text = middleLayer.Config.tables[iCurrentTable].temperatures[(int)nudTempAngleItem.Value - 1].ToString("F0");
            for (int i = 0; i < middleLayer.Config.tables[iCurrentTable].temperatures_count; i++)
                chartTempAngles.Series[0].Points[(int)chartTempAngles.Series[0].Points.AddXY(middleLayer.Config.tables[iCurrentTable].temperatures[i], middleLayer.Config.tables[iCurrentTable].temperature_ignitions[i])].Tag = i;

            chartTempServoAcc.Series[0].Points.Clear();
            chartTempServoAcc.ChartAreas[0].AxisX.Minimum = middleLayer.Config.tables[iCurrentTable].temperatures[0] - (middleLayer.Config.tables[iCurrentTable].temperatures[0] % 10);
            chartTempServoAcc.ChartAreas[0].AxisX.Maximum = middleLayer.Config.tables[iCurrentTable].temperatures[middleLayer.Config.tables[iCurrentTable].temperatures_count - 1] + (10 - (middleLayer.Config.tables[iCurrentTable].temperatures[middleLayer.Config.tables[iCurrentTable].temperatures_count - 1] % 10));
            nudTempServoAccItem.Minimum = 1;
            nudTempServoAccItem.Maximum = Consts.TABLE_TEMPERATURES_MAX;
            nudTempServoAccValue.Value = (decimal)middleLayer.Config.tables[iCurrentTable].servo_acc[(int)nudTempServoAccItem.Value - 1];
            lblTemp.Text = middleLayer.Config.tables[iCurrentTable].temperatures[(int)nudTempServoAccItem.Value - 1].ToString("F0");
            for (int i = 0; i < middleLayer.Config.tables[iCurrentTable].temperatures_count; i++)
                chartTempServoAcc.Series[0].Points[(int)chartTempServoAcc.Series[0].Points.AddXY(middleLayer.Config.tables[iCurrentTable].temperatures[i], middleLayer.Config.tables[iCurrentTable].servo_acc[i])].Tag = i;

            chartTempServoChoke.Series[0].Points.Clear();
            chartTempServoChoke.ChartAreas[0].AxisX.Minimum = middleLayer.Config.tables[iCurrentTable].temperatures[0] - (middleLayer.Config.tables[iCurrentTable].temperatures[0] % 10);
            chartTempServoChoke.ChartAreas[0].AxisX.Maximum = middleLayer.Config.tables[iCurrentTable].temperatures[middleLayer.Config.tables[iCurrentTable].temperatures_count - 1] + (10 - (middleLayer.Config.tables[iCurrentTable].temperatures[middleLayer.Config.tables[iCurrentTable].temperatures_count - 1] % 10));
            nudTempServoChokeItem.Minimum = 1;
            nudTempServoChokeItem.Maximum = Consts.TABLE_TEMPERATURES_MAX;
            nudTempServoChokeValue.Value = (decimal)middleLayer.Config.tables[iCurrentTable].servo_choke[(int)nudTempServoChokeItem.Value - 1];
            lblTemp.Text = middleLayer.Config.tables[iCurrentTable].temperatures[(int)nudTempServoChokeItem.Value - 1].ToString("F0");
            for (int i = 0; i < middleLayer.Config.tables[iCurrentTable].temperatures_count; i++)
                chartTempServoChoke.Series[0].Points[(int)chartTempServoChoke.Series[0].Points.AddXY(middleLayer.Config.tables[iCurrentTable].temperatures[i], middleLayer.Config.tables[iCurrentTable].servo_choke[i])].Tag = i;


            tlpIgnitions.SuspendLayout();
            tlpIgnitions.RowStyles[tlpInfoRow] = new RowStyle(SizeType.AutoSize);
            tlpIgnitions.ColumnStyles[tlpInfoColumn] = new ColumnStyle(SizeType.AutoSize);

            for (int y = 0; y < middleLayer.Config.tables[iCurrentTable].pressures_count; y++)
                tlpIgnitions.RowStyles[tlpRows[y]] = new RowStyle(SizeType.AutoSize);
            for (int y = middleLayer.Config.tables[iCurrentTable].pressures_count; y < Consts.TABLE_PRESSURES_MAX; y++)
                tlpIgnitions.RowStyles[tlpRows[y]] = new RowStyle(SizeType.Absolute, 0.0f);
            for (int x = 0; x < middleLayer.Config.tables[iCurrentTable].rotates_count; x++)
                tlpIgnitions.ColumnStyles[tlpColumns[x]] = new ColumnStyle(SizeType.Percent, 10.0f);
            for (int x = middleLayer.Config.tables[iCurrentTable].rotates_count; x < Consts.TABLE_ROTATES_MAX; x++)
                tlpIgnitions.ColumnStyles[tlpColumns[x]] = new ColumnStyle(SizeType.Absolute, 0.0f);

            foreach (Control control in tlpIgnitions.Controls)
            {
                if (control.Tag != null)
                {

                    if (control.GetType() == typeof(NumericUpDown))
                    {
                        int x = ((int)control.Tag) % (Consts.TABLE_ROTATES_MAX);
                        int y = ((int)control.Tag) / (Consts.TABLE_ROTATES_MAX);
                        control.Visible = x < middleLayer.Config.tables[iCurrentTable].rotates_count && y < middleLayer.Config.tables[iCurrentTable].pressures_count;

                        NumericUpDown nud = (NumericUpDown)control;
                        nud.Value = (decimal)middleLayer.Config.tables[iCurrentTable].ignitions[(int)nud.Tag];
                    }
                    else if (control.GetType() == typeof(Label))
                    {
                        int x = ((int)control.Tag) % (Consts.TABLE_ROTATES_MAX + 1);
                        int y = ((int)control.Tag) / (Consts.TABLE_ROTATES_MAX + 1);
                        control.Visible = (x - 1) < middleLayer.Config.tables[iCurrentTable].rotates_count && (y - 1) < middleLayer.Config.tables[iCurrentTable].pressures_count;

                        Label lbl = (Label)control;
                        if (x == 0 && y != 0) lbl.Text = middleLayer.Config.tables[iCurrentTable].pressures[y - 1].ToString("F0");
                        else if (y == 0 && x != 0) lbl.Text = middleLayer.Config.tables[iCurrentTable].rotates[x - 1].ToString("F0");
                    }
                }
            }
            

            tlpIgnitions.ResumeLayout();

            Color min = Color.SpringGreen;
            Color max = Color.IndianRed;
            int ignMin = int.MaxValue;
            int ignMax = int.MinValue;

            chartIgnitions.Series.Clear();
            chartIgnitions.ChartAreas[0].AxisX.Minimum = middleLayer.Config.tables[iCurrentTable].rotates[0] - (middleLayer.Config.tables[iCurrentTable].rotates[0] % 100);
            chartIgnitions.ChartAreas[0].AxisX.Maximum = middleLayer.Config.tables[iCurrentTable].rotates[middleLayer.Config.tables[iCurrentTable].rotates_count - 1] + (100 - (middleLayer.Config.tables[iCurrentTable].rotates[middleLayer.Config.tables[iCurrentTable].rotates_count - 1] % 100));
            
            if (middleLayer.Config.tables[iCurrentTable].pressures_count > 0 && middleLayer.Config.tables[iCurrentTable].rotates_count > 0)
            {
                for (int i = 0; i < middleLayer.Config.tables[iCurrentTable].pressures_count; i++)
                {
                    series = chartIgnitions.Series.Add(middleLayer.Config.tables[iCurrentTable].pressures[i].ToString("F0"));
                    series.Tag = i;
                    series.ChartType = SeriesChartType.Line;
                    series.XAxisType = AxisType.Primary;
                    series.XValueType = ChartValueType.Single;
                    series.YAxisType = AxisType.Primary;
                    series.YValueType = ChartValueType.Single;
                    series.BorderWidth = 2;
                    float trans = (float)i / (float)(middleLayer.Config.tables[iCurrentTable].pressures_count - 1);
                    series.Color = Color.FromArgb((int)(min.R * (1.0f - trans) + max.R * trans), (int)(min.G * (1.0f - trans) + max.G * trans), (int)(min.B * (1.0f - trans) + max.B * trans));
                    for (int j = 0; j < middleLayer.Config.tables[iCurrentTable].rotates_count; j++)
                    {
                        float ign = middleLayer.Config.tables[iCurrentTable].ignitions[i * Consts.TABLE_ROTATES_MAX + j];
                        int point = series.Points.AddXY(middleLayer.Config.tables[iCurrentTable].rotates[j], ign);
                        series.Points[point].Tag = j;
                        if (ign > ignMax)
                            ignMax = (int)ign;
                        if (ign < ignMin)
                            ignMin = (int)ign;
                    }
                }
                chartIgnitions.ChartAreas[0].AxisY.Minimum = (float)(ignMin - (ignMin % 10));
                chartIgnitions.ChartAreas[0].AxisY.Maximum = (float)(ignMax + (10 - (ignMax % 10)));
                
            }

            series = chartIgnitions.Series.Add("Current");
            series.ChartType = SeriesChartType.Point;
            series.XAxisType = AxisType.Primary;
            series.XValueType = ChartValueType.Single;
            series.YAxisType = AxisType.Primary;
            series.YValueType = ChartValueType.Single;
            series.BorderWidth = 8;
            series.Color = Color.Red;
            series.MarkerStyle = MarkerStyle.Circle;
            series.Points.AddXY(GeneralStatus.RealRPM, GeneralStatus.IgnitionAngle);


        }

        private void UpdateIgnitionChartPoint()
        {
            int seriescount = chartIgnitions.Series.Count;
            if (seriescount > 0 && chartIgnitions.Series[0].Points.Count > 0)
            {
                chartIgnitions.Series[seriescount - 1].Points[0].XValue = GeneralStatus.RealRPM;
                chartIgnitions.Series[seriescount - 1].Points[0].YValues[0] = GeneralStatus.IgnitionAngle;
            }

            seriescount = chartIdleAngles.Series.Count;
            if (seriescount > 0 && chartIdleAngles.Series[0].Points.Count > 0)
            {
                chartIdleAngles.Series[seriescount - 1].Points[0].XValue = GeneralStatus.RealRPM;
                chartIdleAngles.Series[seriescount - 1].Points[0].YValues[0] = GeneralStatus.IgnitionAngle;
            }

            float pressure = GeneralStatus.Pressure;
            float rpm = GeneralStatus.RPM;
            int rpmindex = -1;
            int pressindex = -1;

            if (middleLayer.Config.tables[iCurrentTable].rotates_count > 0 && middleLayer.Config.tables[iCurrentTable].pressures_count > 0)
            {

                if (rpm < middleLayer.Config.tables[iCurrentTable].rotates[0])
                {
                    rpmindex = 0;
                }
                else if (rpm > middleLayer.Config.tables[iCurrentTable].rotates[middleLayer.Config.tables[iCurrentTable].rotates_count - 1])
                {
                    rpmindex = middleLayer.Config.tables[iCurrentTable].rotates_count - 1;
                }
                else
                {
                    for (int i = 1; i < middleLayer.Config.tables[iCurrentTable].rotates_count; i++)
                    {
                        if (middleLayer.Config.tables[iCurrentTable].rotates[i - 1] <= rpm && middleLayer.Config.tables[iCurrentTable].rotates[i] > rpm)
                        {
                            if (rpm - middleLayer.Config.tables[iCurrentTable].rotates[i - 1] <
                                (middleLayer.Config.tables[iCurrentTable].rotates[i] - middleLayer.Config.tables[iCurrentTable].rotates[i - 1]) / 2)
                            {
                                rpmindex = i - 1;
                            }
                            else
                            {
                                rpmindex = i;
                            }
                        }
                    }
                }

                if (pressure < middleLayer.Config.tables[iCurrentTable].pressures[0])
                {
                    pressindex = 0;
                }
                else if (pressure > middleLayer.Config.tables[iCurrentTable].pressures[middleLayer.Config.tables[iCurrentTable].pressures_count - 1])
                {
                    pressindex = middleLayer.Config.tables[iCurrentTable].pressures_count - 1;
                }
                else
                {
                    for (int i = 1; i < middleLayer.Config.tables[iCurrentTable].pressures_count; i++)
                    {
                        if (middleLayer.Config.tables[iCurrentTable].pressures[i - 1] <= pressure && middleLayer.Config.tables[iCurrentTable].pressures[i] > pressure)
                        {
                            if (pressure - middleLayer.Config.tables[iCurrentTable].pressures[i - 1] <
                                (middleLayer.Config.tables[iCurrentTable].pressures[i] - middleLayer.Config.tables[iCurrentTable].pressures[i - 1]) / 2)
                            {
                                pressindex = i - 1;
                            }
                            else
                            {
                                pressindex = i;
                            }
                        }
                    }
                }

                for (int i = 0; i < tlpIgnitions.Controls.Count; i++)
                {
                    if (tlpIgnitions.Controls[i] is NumericUpDown)
                    {
                        NumericUpDown nud = (NumericUpDown)tlpIgnitions.Controls[i];
                        if ((int)nud.Tag == pressindex * Consts.TABLE_ROTATES_MAX + rpmindex)
                        {
                            nud.BackColor = Color.DarkGray;
                            nud.ForeColor = Color.White;
                            nud.Font = new Font(nud.Font, FontStyle.Bold);
                        }
                        else
                        {
                            if (nud.BackColor == Color.DarkGray)
                            {
                                nudIgnition_ValueChanged(nud, new EventArgs());
                            }
                        }
                    }
                }
            }

        }

        private void ClearDragCharts()
        {
            chartDragTime.Series.Clear();
            chartDragAccel.Series.Clear();
            lvDragTable.Items.Clear();
            for (int i = lvDragTable.Columns.Count - 1; i >= 2; i--)
            {
                lvDragTable.Columns.RemoveAt(i);
            }
        }

        private void UpdateDragCharts()
        {
            const int pointperiod = 50;
            double FromRPM = fDragFromRPM;
            double ToRPM = fDragToRPM;
            int DeltaMs = (int)nudDragAccelStep.Value;
            int TableSplit = (int)nudDragTableSplit.Value;

            ClearDragCharts();

            //Color min = Color.SpringGreen;
            //Color max = Color.IndianRed;
            double timeMax = 1;
            double deltaRpmMax = 1;

            chartDragTime.Series.Clear();
            chartDragAccel.Series.Clear();
            lvDragTable.Items.Clear();
            for(int i = lvDragTable.Columns.Count - 1; i >= 2; i--)
            {
                lvDragTable.Columns.RemoveAt(i);
            }

            if (lDragRuns != null && lDragRuns.Count > 0)
            {
                for (int i = 0; i < lDragRuns.Count; i++)
                {
                    if (lDragRuns[i] != null && lDragRuns[i].Points != null && lDragRuns[i].Points.Count > 0)
                    {
                        Series series = chartDragTime.Series.Add(lDragRuns[i].Label);
                        series.ChartType = SeriesChartType.Line;
                        series.XAxisType = AxisType.Primary;
                        series.XValueType = ChartValueType.Single;
                        series.YAxisType = AxisType.Primary;
                        series.YValueType = ChartValueType.Single;
                        series.BorderWidth = 2;
                        series.Tag = lDragRuns[i];
                        //float trans = (float)i / (float)(lDragRuns.Count - 1);
                        //series.Color = Color.FromArgb((int)(min.R * (1.0f - trans) + max.R * trans), (int)(min.G * (1.0f - trans) + max.G * trans), (int)(min.B * (1.0f - trans) + max.B * trans));
                        for (int j = 0; j < lDragRuns[i].Points.Count; j++)
                        {
                            series.Points.AddXY(lDragRuns[i].Points[j].Time, lDragRuns[i].Points[j].RPM);
                        }
                        if (timeMax < lDragRuns[i].Time)
                        {
                            timeMax = lDragRuns[i].Time;
                        }

                        series = chartDragAccel.Series.Add(lDragRuns[i].Label);
                        series.ChartType = SeriesChartType.Line;
                        series.XAxisType = AxisType.Primary;
                        series.XValueType = ChartValueType.Single;
                        series.YAxisType = AxisType.Primary;
                        series.YValueType = ChartValueType.Single;
                        series.BorderWidth = 2;
                        series.Tag = lDragRuns[i];
                        //float trans = (float)i / (float)(lDragRuns.Count - 1);
                        //series.Color = Color.FromArgb((int)(min.R * (1.0f - trans) + max.R * trans), (int)(min.G * (1.0f - trans) + max.G * trans), (int)(min.B * (1.0f - trans) + max.B * trans));

                        double rpmPrev = lDragRuns[i].Points[0].RPM;
                        for (int j = 0, n = 0; j < lDragRuns[i].Points.Count; j += DeltaMs / pointperiod, n++)
                        {
                            double rpm = 0;
                            double delta = 0.0;

                            for (int k = j; k < j + (DeltaMs / pointperiod); k++)
                            {
                                if (k < lDragRuns[i].Points.Count)
                                {
                                    rpm = lDragRuns[i].Points[k].RPM;
                                    delta += rpm - rpmPrev;
                                    rpmPrev = rpm;
                                }
                                else
                                {
                                    delta = double.NaN;
                                }
                            }
                            if (!double.IsNaN(delta))
                            {
                                delta /= (DeltaMs / pointperiod);

                                series.Points.AddXY(n * pointperiod / 1000.0, delta);

                                if (deltaRpmMax < delta)
                                {
                                    deltaRpmMax = delta;
                                }
                            }
                        }

                    }
                }
                
                chartDragTime.ChartAreas[0].AxisX.Minimum = 0;
                //chartDragTime.ChartAreas[0].AxisX.Maximum = timeMax;

                chartDragTime.ChartAreas[0].AxisY.Minimum = FromRPM;
                chartDragTime.ChartAreas[0].AxisY.Maximum = ToRPM;
                
                chartDragAccel.ChartAreas[0].AxisX.Minimum = 0;
                //chartDragAccel.ChartAreas[0].AxisX.Maximum = timeMax;

                chartDragAccel.ChartAreas[0].AxisY.Minimum = 0;
                //chartDragAccel.ChartAreas[0].AxisY.Maximum = deltaRpmMax;


                if(TableSplit >= 2)
                {
                    double[] rpms = new double[TableSplit + 1];
                    for(int i = 0; i <= TableSplit; i++)
                    {
                        rpms[i] = ((ToRPM - FromRPM) / TableSplit * i) + FromRPM;
                    }

                    for (int i = 0; i < TableSplit; i++)
                    {
                        lvDragTable.Columns.Add($"{rpms[i]}-{rpms[i + 1]}");
                    }

                    for (int j = 0; j < lDragRuns.Count; j++)
                    {
                        ListViewItem item = lvDragTable.Items.Add(lDragRuns[j].Label);
                        item.SubItems.Add($"{lDragRuns[j].Time.ToString("F2")}s");
                        item.Tag = lDragRuns[j];

                        for (int i = 0; i < TableSplit; i++)
                        {
                            double timemin = double.MaxValue;
                            double timemax = double.MinValue;
                            for (int p = 0; p < lDragRuns[j].Points.Count; p++)
                            {
                                if(lDragRuns[j].Points[p].RPM >= rpms[i] && lDragRuns[j].Points[p].RPM < rpms[i + 1])
                                {
                                    if (lDragRuns[j].Points[p].Time < timemin)
                                    {
                                        timemin = lDragRuns[j].Points[p].Time;
                                    }
                                    if (lDragRuns[j].Points[p].Time > timemax)
                                    {
                                        timemax = lDragRuns[j].Points[p].Time;
                                    }
                                }
                            }
                            if (timemax >= timemin)
                            {
                                item.SubItems.Add($"{(timemax - timemin).ToString("F2")}s");
                            }
                            else
                            {
                                item.SubItems.Add($"----");
                            }
                        }
                    }


                    for (int i = 0; i < TableSplit; i++)
                    {
                        lvDragTable.Columns[i + 2].Width = -2;
                    }
                }

            }
        }

        internal void UpdateDragStatus(PK_DragUpdateResponse dur)
        {
            lblDragRpm.Text = dur.CurrentRPM.ToString("F0");
            lblDragTime.Text = (dur.Time / 1000000.0).ToString("F2") + "s";
            if (eDragStatus == DragStatusType.Set)
            {
                if (dur.Started > 0)
                {
                    eDragStatus = DragStatusType.Go;
                    lblDragStatus.Text = "GO!";
                }
            }
            if(eDragStatus == DragStatusType.Set || eDragStatus == DragStatusType.Go)
            {
                if(dur.ErrorCode > 0)
                {
                    eDragStatus = DragStatusType.Fail;
                    lblDragStatus.Text = "Fail!";

                    nudDragRPMFrom.Enabled = false;
                    nudDragRPMTo.Enabled = false;
                    btnDragStart.Enabled = true;
                    btnDragClear.Enabled = true;
                    nudDragTableSplit.Enabled = false;
                    nudDragAccelStep.Enabled = false;
                    btnDragStop.Enabled = false;
                }
                else
                {
                    if(dur.Completed > 0)
                    {
                        eDragStatus = DragStatusType.Done;
                        lblDragStatus.Text = "Wait...";

                        drCurrentRun = new DragRun
                        {
                            FromRPM = dur.FromRPM,
                            ToRPM = dur.ToRPM,
                            Time = dur.Time / 1000000.0,
                            Points = new List<DragPoint>(),
                            DateTime = DateTime.Now,
                            Name = tbDragName.Text,
                            TotalPoints = (int)dur.TotalPoints
                        };
                        drCurrentRun.Label = drCurrentRun.Name + " " + (lDragRuns.Where(t => t.Name == drCurrentRun.Name).Count() + 1).ToString();

                        lDragRuns.Add(drCurrentRun);

                        if (drCurrentRun.TotalPoints > drCurrentRun.Points.Count)
                        {
                            middleLayer.PacketHandler.SendDragPointRequest(drCurrentRun.FromRPM, drCurrentRun.ToRPM, drCurrentRun.Points.Count);
                        }
                        else
                        {
                            lblDragStatus.Text = "Empty...";

                            nudDragRPMFrom.Enabled = false;
                            nudDragRPMTo.Enabled = false;
                            btnDragStart.Enabled = true;
                            btnDragClear.Enabled = true;
                            nudDragTableSplit.Enabled = false;
                            nudDragAccelStep.Enabled = false;
                            btnDragStop.Enabled = false;
                        }
                    }
                }
            }
        }

        internal void UpdateDragPoint(PK_DragPointResponse dpr)
        {
            if (dpr.ErrorCode == 0)
            {
                drCurrentRun.Points.Add(new DragPoint
                {
                    Ignition = dpr.Ignition,
                    Load = dpr.Load,
                    Pressure = dpr.Pressure,
                    RPM = dpr.RPM,
                    Time = dpr.Time / 1000000.0f
                });

                if (drCurrentRun.TotalPoints > drCurrentRun.Points.Count)
                {
                    middleLayer.PacketHandler.SendDragPointRequest(drCurrentRun.FromRPM, drCurrentRun.ToRPM, drCurrentRun.Points.Count);
                }
                else
                {
                    UpdateDragCharts();

                    lblDragStatus.Text = "Done!";

                    nudDragRPMFrom.Enabled = false;
                    nudDragRPMTo.Enabled = false;
                    btnDragStart.Enabled = true;
                    btnDragClear.Enabled = true;
                    nudDragTableSplit.Enabled = false;
                    nudDragAccelStep.Enabled = false;
                    btnDragStop.Enabled = false;
                }
            }
            else
            {
                lblDragStatus.Text = "Error code: " + dpr.ErrorCode;

                nudDragRPMFrom.Enabled = false;
                nudDragRPMTo.Enabled = false;
                btnDragStart.Enabled = true;
                btnDragClear.Enabled = true;
                nudDragTableSplit.Enabled = false;
                nudDragAccelStep.Enabled = false;
                btnDragStop.Enabled = false;
            }
        }

        internal void DragStartAck(PK_DragStartAcknowledge dsaa)
        {
            if(eDragStatus == DragStatusType.Ready)
            {
                eDragStatus = DragStatusType.Set;
                lblDragStatus.Text = "SET";

                nudDragRPMFrom.Enabled = false;
                nudDragRPMTo.Enabled = false;
                btnDragStart.Enabled = false;
                btnDragClear.Enabled = false;
                nudDragTableSplit.Enabled = false;
                nudDragAccelStep.Enabled = false;
                btnDragStop.Enabled = true;
            }
        }

        internal void DragStopAck(PK_DragStopAcknowledge dsta)
        {
            eDragStatus = DragStatusType.Ready;
            lblDragStatus.Text = "Aborted";

            nudDragRPMFrom.Enabled = false;
            nudDragRPMTo.Enabled = false;
            btnDragStart.Enabled = true;
            btnDragClear.Enabled = true;
            nudDragTableSplit.Enabled = false;
            nudDragAccelStep.Enabled = false;
            btnDragStop.Enabled = false;
        }

        private void nudIgnition_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown nud = (NumericUpDown)sender;
            int index = (int)nud.Tag;
            int x = index % Consts.TABLE_ROTATES_MAX;
            int y = index / Consts.TABLE_ROTATES_MAX;
            float value = (float)nud.Value;
            Color min = Color.DeepSkyBlue;
            Color mid = Color.Orange;
            Color max = Color.Red;
            Color text = Color.White;
            float trans = (value + 10.0f) / 70.0f;
            if (chartIgnitions.Series.Count > y && chartIgnitions.Series[y].Points.Count > x)
            {
                chartIgnitions.Series[y].Points[x].YValues = new double[1] { value };
            }
            int r = 0;
            int g = 0;
            int b = 0;

            if (trans < 0.5f)
            {
                trans /= 0.5f;
                r = (int)(min.R * (1.0f - trans) + mid.R * trans);
                g = (int)(min.G * (1.0f - trans) + mid.G * trans);
                b = (int)(min.B * (1.0f - trans) + mid.B * trans);
            }
            else if (trans >= 0.5f)
            {
                trans -= 0.5f;
                trans /= 0.5f;
                r = (int)(mid.R * (1.0f - trans) + max.R * trans);
                g = (int)(mid.G * (1.0f - trans) + max.G * trans);
                b = (int)(mid.B * (1.0f - trans) + max.B * trans);
            }

            Color back = Color.FromArgb(r, g, b);
            if (!middleLayer.IsSynchronizing)
            {
                if (middleLayer.Config.tables[iCurrentTable].ignitions[index] != value)
                {
                    middleLayer.Config.tables[iCurrentTable].ignitions[index] = value;
                    if (!middleLayer.IsSynchronizing && cbLive.Checked)
                    {
                        middleLayer.UpdateTable(iCurrentTable);
                    }
                }
            }
            nud.ForeColor = text;
            nud.BackColor = back;
            nud.Font = new Font(nud.Font, FontStyle.Regular);

            int ignMin = int.MaxValue;
            int ignMax = int.MinValue;
            
            for (int i = 0; i < chartIgnitions.Series.Count; i++)
            {
                for (int j = 0; j < chartIgnitions.Series[i].Points.Count; j++)
                {
                    value = (float)chartIgnitions.Series[i].Points[j].YValues[0];
                    if (value > ignMax)
                        ignMax = (int)value;
                    if (value < ignMin)
                        ignMin = (int)value;
                }
            }
            if (ignMin != int.MaxValue && ignMax != int.MinValue)
            {
                chartIgnitions.ChartAreas[0].AxisY.Minimum = (float)(ignMin - (ignMin % 10));
                chartIgnitions.ChartAreas[0].AxisY.Maximum = (float)(ignMax + (10 - (ignMax % 10)));
            }


        }

        public void UpdateGeneralStatus(PK_GeneralStatusResponse status)
        {
            GeneralStatus = status;
            //if (tableLayoutPanel3.Visible)
            {
                mGenIgn.NeedleVal = status.IgnitionAngle;
                mGenPress.NeedleVal = status.Pressure;
                mGenRPM.NeedleVal = status.RPM;
                mGenTemp.NeedleVal = status.Temperature;
                mGenFuelUsage.NeedleVal = status.FuelUsage;
                label7.Text = (status.tablenum + 1).ToString();
                label8.Text = status.tablename;
                label10.Text = status.valvenum == 0 ? "All Closed" : status.valvenum == 1 ? "Petrol" : status.valvenum == 2 ? "Propane" : "Invalid";
                label16.Text = status.Voltage.ToString("F2") + "V";
            }
            if(panel11.Visible)
            {
                lblSetupRPM.Text = status.RPM.ToString("F0");
                lblSetupPressure.Text = status.Pressure.ToString("F0");
                lblSetupIgnition.Text = status.IgnitionAngle.ToString("F1") + "°";
                lblSetupTemperature.Text = status.Temperature.ToString("F1") + "°";
            }
            UpdateIgnitionChartPoint();
            generalStatusReceived = true;
            lastReceivedGeneralStatus = DateTime.Now;
        }
        
        private void tmr1sec_Tick(object sender, EventArgs e)
        {
            if(middleLayer.Config.parameters.isHallLearningMode > 0)
            {
                middleLayer.SyncLoad(false);
            }
        }

        private void tmr50ms_Tick(object sender, EventArgs e)
        {
            tableLayoutPanel1.Enabled = !middleLayer.IsSynchronizing;
            toolStripProgressBar1.Style = middleLayer.IsSynchronizing ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks;
            if (generalStatusReceived || (DateTime.Now - lastReceivedGeneralStatus).TotalMilliseconds > 200)
            {
                generalStatusReceived = false;
                lastReceivedGeneralStatus = DateTime.Now;
                middleLayer.PacketHandler.SendGeneralStatusRequest();
            }
            if((tabControl1.Visible && tabControl1.SelectedTab == tabPage18) || eDragStatus == DragStatusType.Set || eDragStatus == DragStatusType.Go)
            {
                middleLayer.PacketHandler.SendDragUpdateRequest();
            }
        }
        
        private void cbForceIgnition_CheckedChanged(object sender, EventArgs e)
        {
            middleLayer.Config.parameters.isForceIgnition = ((CheckBox)sender).Checked ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudForceIgnitionAngle_ValueChanged(object sender, EventArgs e)
        {
            middleLayer.Config.parameters.forceIgnitionAngle = (int)((NumericUpDown)sender).Value - 1;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbCutoffEnabled_CheckedChanged(object sender, EventArgs e)
        {
            middleLayer.Config.parameters.isCutoffEnabled = ((CheckBox)sender).Checked ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }

        }

        private void rbCutoffMode1_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                middleLayer.Config.parameters.CutoffMode = 0;
                if (!middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateConfig();
                }
            }
        }

        private void rbCutoffMode2_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                middleLayer.Config.parameters.CutoffMode = 1;
                if (!middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateConfig();
                }
            }
        }

        private void rbCutoffMode3_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                middleLayer.Config.parameters.CutoffMode = 2;
                if (!middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateConfig();
                }
            }
        }

        private void rbCutoffMode4_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                middleLayer.Config.parameters.CutoffMode = 3;
                if (!middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateConfig();
                }
            }
        }

        private void rbCutoffMode5_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                middleLayer.Config.parameters.CutoffMode = 4;
                if (!middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateConfig();
                }
            }
        }

        private void rbCutoffMode6_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                middleLayer.Config.parameters.CutoffMode = 5;
                if (!middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateConfig();
                }
            }
        }

        private void rbCutoffMode7_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                middleLayer.Config.parameters.CutoffMode = 6;
                if (!middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateConfig();
                }
            }
        }

        private void rbCutoffMode8_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                middleLayer.Config.parameters.CutoffMode = 7;
                if (!middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateConfig();
                }
            }
        }

        private void tbCutoffRPM_Scroll(object sender, EventArgs e)
        {
            ((TrackBar)sender).Value -= ((TrackBar)sender).Value % 100;
            lblCutoffRPM.Text = ((TrackBar)sender).Value.ToString();
            middleLayer.Config.parameters.CutoffRPM = ((TrackBar)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void tbCutoffAngle_Scroll(object sender, EventArgs e)
        {
            lblCutoffAngle.Text = ((TrackBar)sender).Value.ToString();
            middleLayer.Config.parameters.CutoffAngle = ((TrackBar)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbEconEnabled_CheckedChanged(object sender, EventArgs e)
        {
            middleLayer.Config.parameters.isEconomEnabled = ((CheckBox)sender).Checked ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbEconStrobe_CheckedChanged(object sender, EventArgs e)
        {
            middleLayer.Config.parameters.isEconOutAsStrobe = ((CheckBox)sender).Checked ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbEconIgnitionBreak_CheckedChanged(object sender, EventArgs e)
        {
            middleLayer.Config.parameters.isEconIgnitionOff = ((CheckBox)sender).Checked ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void tbEconRPM_Scroll(object sender, EventArgs e)
        {
            ((TrackBar)sender).Value -= ((TrackBar)sender).Value % 100;
            lblEconRPM.Text = ((TrackBar)sender).Value.ToString();
            middleLayer.Config.parameters.EconRpmThreshold = ((TrackBar)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbFuelExtSw_CheckedChanged(object sender, EventArgs e)
        {
            middleLayer.Config.parameters.isSwitchByExternal = ((CheckBox)sender).Checked ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudSwPos1_ValueChanged(object sender, EventArgs e)
        {
            middleLayer.Config.parameters.switchPos1Table = (int)((NumericUpDown)sender).Value - 1;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudEngVol_ValueChanged(object sender, EventArgs e)
        {
            middleLayer.Config.parameters.engineVolume = (int)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudSwPos0_ValueChanged(object sender, EventArgs e)
        {
            middleLayer.Config.parameters.switchPos0Table = (int)((NumericUpDown)sender).Value - 1;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudSwPos2_ValueChanged(object sender, EventArgs e)
        {
            middleLayer.Config.parameters.switchPos2Table = (int)((NumericUpDown)sender).Value - 1;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbFuelForce_CheckedChanged(object sender, EventArgs e)
        {
            middleLayer.Config.parameters.isForceTable = ((CheckBox)sender).Checked ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudFuelForce_ValueChanged(object sender, EventArgs e)
        {
            middleLayer.Config.parameters.forceTableNumber = (int)((NumericUpDown)sender).Value - 1;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbTempEnabled_CheckedChanged(object sender, EventArgs e)
        {
            middleLayer.Config.parameters.isTemperatureEnabled = ((CheckBox)sender).Checked ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbAutostartEnabled_CheckedChanged(object sender, EventArgs e)
        {
            middleLayer.Config.parameters.isAutostartEnabled = ((CheckBox)sender).Checked ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbHallIgnition_CheckedChanged(object sender, EventArgs e)
        {
            middleLayer.Config.parameters.isIgnitionByHall = ((CheckBox)sender).Checked ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbHallLearn_CheckedChanged(object sender, EventArgs e)
        {
            bool learningmode = ((CheckBox)sender).Checked;
            middleLayer.Config.parameters.isHallLearningMode = learningmode ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }

            tlpIgnitions.Enabled = !learningmode;


        }

        private void cbForceIdle_CheckedChanged(object sender, EventArgs e)
        {
            middleLayer.Config.parameters.isForceIdle = ((CheckBox)sender).Checked ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbLive_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
                middleLayer.SyncSave(false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            middleLayer.SyncSave(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            middleLayer.SyncLoad(true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            middleLayer.SyncLoad(false);
        }

        private void nudIgnPressItem_ValueChanged(object sender, EventArgs e)
        {
            nudIgnPressValue.Value = (decimal)middleLayer.Config.tables[iCurrentTable].pressures[(int)((NumericUpDown)sender).Value - 1];
        }

        private void nudIgnPressValue_ValueChanged(object sender, EventArgs e)
        {
            float value = (float)((NumericUpDown)sender).Value;
            int index = (int)nudIgnPressItem.Value - 1;
            if (index > 0 && value - 500 < middleLayer.Config.tables[iCurrentTable].pressures[index - 1])
            {
                value = middleLayer.Config.tables[iCurrentTable].pressures[index - 1] + 500;
                ((NumericUpDown)sender).Value = (decimal)value;
            }
            else if (index < middleLayer.Config.tables[iCurrentTable].pressures_count - 1 && value + 500 > middleLayer.Config.tables[iCurrentTable].pressures[index + 1])
            {
                value = middleLayer.Config.tables[iCurrentTable].pressures[index + 1] - 500;
                ((NumericUpDown)sender).Value = (decimal)value;
            }
            middleLayer.Config.tables[iCurrentTable].pressures[index] = value;
            var result = chartIgnPressures.Series[0].Points.Where(n => (int)n.Tag == index);
            if(result.FirstOrDefault() != null) result.FirstOrDefault().YValues = new double[1] { value };
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
                middleLayer.UpdateTable(iCurrentTable);
        }

        private void btnIgnPressApply_Click(object sender, EventArgs e)
        {
            middleLayer.UpdateTable(iCurrentTable);
        }

        private void nudIgnRPMItem_ValueChanged(object sender, EventArgs e)
        {
            nudIgnRPMValue.Value = (decimal)middleLayer.Config.tables[iCurrentTable].rotates[(int)((NumericUpDown)sender).Value - 1];
        }

        private void nudIgnRPMValue_ValueChanged(object sender, EventArgs e)
        {
            float value = (float)((NumericUpDown)sender).Value;
            int index = (int)nudIgnRPMItem.Value - 1;
            if (index > 0 && value - 20 < middleLayer.Config.tables[iCurrentTable].rotates[index - 1])
            {
                value = middleLayer.Config.tables[iCurrentTable].rotates[index - 1] + 20;
                ((NumericUpDown)sender).Value = (decimal)value;
            }
            else if (index < middleLayer.Config.tables[iCurrentTable].rotates_count - 1 && value + 20 > middleLayer.Config.tables[iCurrentTable].rotates[index + 1])
            {
                value = middleLayer.Config.tables[iCurrentTable].rotates[index + 1] - 20;
                ((NumericUpDown)sender).Value = (decimal)value;
            }
            middleLayer.Config.tables[iCurrentTable].rotates[index] = value;
            var result = chartIgnRPMs.Series[0].Points.Where(n => (int)n.Tag == index);
            if (result.FirstOrDefault() != null) result.FirstOrDefault().YValues = new double[1] { value };
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
                middleLayer.UpdateTable(iCurrentTable);
        }

        private void btnIgnRPMApply_Click(object sender, EventArgs e)
        {
            middleLayer.UpdateTable(iCurrentTable);
        }

        private void nudToolsCurTable_ValueChanged(object sender, EventArgs e)
        {
            iCurrentTable = (int)nudToolsCurTable.Value - 1;
            UpdateIgnitionsGui();
        }

        private void nudIdleRPMItem_ValueChanged(object sender, EventArgs e)
        {
            nudIdleRPMValue.Value = (decimal)middleLayer.Config.tables[iCurrentTable].idle_rotates[(int)((NumericUpDown)sender).Value - 1];
        }

        private void nudIdleRPMValue_ValueChanged(object sender, EventArgs e)
        {
            float value = (float)((NumericUpDown)sender).Value;
            int index = (int)nudIdleRPMItem.Value - 1;
            if (index > 0 && value - 5 < middleLayer.Config.tables[iCurrentTable].idle_rotates[index - 1])
            {
                value = middleLayer.Config.tables[iCurrentTable].idle_rotates[index - 1] + 5;
                ((NumericUpDown)sender).Value = (decimal)value;
            }
            else if (index < middleLayer.Config.tables[iCurrentTable].idles_count - 1 && value + 5 > middleLayer.Config.tables[iCurrentTable].idle_rotates[index + 1])
            {
                value = middleLayer.Config.tables[iCurrentTable].idle_rotates[index + 1] - 5;
                ((NumericUpDown)sender).Value = (decimal)value;
            }
            middleLayer.Config.tables[iCurrentTable].idle_rotates[index] = value;
            var result = chartIdleRPMs.Series[0].Points.Where(n => (int)n.Tag == index);
            if (result.FirstOrDefault() != null) result.FirstOrDefault().YValues = new double[1] { value };
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
                middleLayer.UpdateTable(iCurrentTable);
        }

        private void btnIdleRPMApply_Click(object sender, EventArgs e)
        {
            middleLayer.UpdateTable(iCurrentTable);
        }

        private void nudIdleAngleItem_ValueChanged(object sender, EventArgs e)
        {
            nudIdleAngleValue.Value = (decimal)middleLayer.Config.tables[iCurrentTable].idle_ignitions[(int)((NumericUpDown)sender).Value - 1];
            lblIdleRPM.Text = middleLayer.Config.tables[iCurrentTable].idle_rotates[(int)((NumericUpDown)sender).Value - 1].ToString("F0");
        }

        private void nudIdleAngleValue_ValueChanged(object sender, EventArgs e)
        {
            float value = (float)((NumericUpDown)sender).Value;
            int index = (int)nudIdleAngleItem.Value - 1;
            middleLayer.Config.tables[iCurrentTable].idle_ignitions[index] = value;
            var result = chartIdleAngles.Series[0].Points.Where(n => (int)n.Tag == index);
            if (result.FirstOrDefault() != null) result.FirstOrDefault().YValues = new double[1] { value };
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
                middleLayer.UpdateTable(iCurrentTable);
        }

        private void btnIdleAngleApply_Click(object sender, EventArgs e)
        {
            middleLayer.UpdateTable(iCurrentTable);
        }

        private void nudTempAngleItem_ValueChanged(object sender, EventArgs e)
        {
            nudTempAngleValue.Value = (decimal)middleLayer.Config.tables[iCurrentTable].temperature_ignitions[(int)((NumericUpDown)sender).Value - 1];
            lblTemp.Text = middleLayer.Config.tables[iCurrentTable].temperatures[(int)((NumericUpDown)sender).Value - 1].ToString("F0");
        }

        private void nudTempAngleValue_ValueChanged(object sender, EventArgs e)
        {
            float value = (float)((NumericUpDown)sender).Value;
            int index = (int)nudTempAngleItem.Value - 1;
            middleLayer.Config.tables[iCurrentTable].temperature_ignitions[index] = value;
            var result = chartTempAngles.Series[0].Points.Where(n => (int)n.Tag == index);
            if (result.FirstOrDefault() != null) result.FirstOrDefault().YValues = new double[1] { value };
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
                middleLayer.UpdateTable(iCurrentTable);
        }

        private void btnTempAngleApply_Click(object sender, EventArgs e)
        {
            middleLayer.UpdateTable(iCurrentTable);
        }

        private void nudTempItem_ValueChanged(object sender, EventArgs e)
        {
            nudTempValue.Value = (decimal)middleLayer.Config.tables[iCurrentTable].temperatures[(int)((NumericUpDown)sender).Value - 1];
            lblIdleRPM.Text = middleLayer.Config.tables[iCurrentTable].temperatures[(int)((NumericUpDown)sender).Value - 1].ToString("F0");
        }

        private void nudTempValue_ValueChanged(object sender, EventArgs e)
        {
            float value = (float)((NumericUpDown)sender).Value;
            int index = (int)nudTempItem.Value - 1;

            if (index > 0 && value - 1 < middleLayer.Config.tables[iCurrentTable].temperatures[index - 1])
            {
                value = middleLayer.Config.tables[iCurrentTable].temperatures[index - 1] + 1;
                ((NumericUpDown)sender).Value = (decimal)value;
            }
            else if (index < middleLayer.Config.tables[iCurrentTable].idles_count - 1 && value + 1 > middleLayer.Config.tables[iCurrentTable].temperatures[index + 1])
            {
                value = middleLayer.Config.tables[iCurrentTable].temperatures[index + 1] - 1;
                ((NumericUpDown)sender).Value = (decimal)value;
            }

            middleLayer.Config.tables[iCurrentTable].temperatures[index] = value;
            var result = chartTemps.Series[0].Points.Where(n => (int)n.Tag == index);
            if (result.FirstOrDefault() != null) result.FirstOrDefault().YValues = new double[1] { value };
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
                middleLayer.UpdateTable(iCurrentTable);
        }

        private void btnTempApply_Click(object sender, EventArgs e)
        {
            middleLayer.UpdateTable(iCurrentTable);
        }

        private void nudTempServoAccItem_ValueChanged(object sender, EventArgs e)
        {
            nudTempServoAccValue.Value = (decimal)middleLayer.Config.tables[iCurrentTable].servo_acc[(int)((NumericUpDown)sender).Value - 1];
            lblTempServoAcc.Text = middleLayer.Config.tables[iCurrentTable].temperatures[(int)((NumericUpDown)sender).Value - 1].ToString("F0");
        }

        private void nudTempServoAccValue_ValueChanged(object sender, EventArgs e)
        {
            float value = (float)((NumericUpDown)sender).Value;
            int index = (int)nudTempServoAccItem.Value - 1;
            middleLayer.Config.tables[iCurrentTable].servo_acc[index] = value;
            var result = chartTempServoAcc.Series[0].Points.Where(n => (int)n.Tag == index);
            if (result.FirstOrDefault() != null) result.FirstOrDefault().YValues = new double[1] { value };
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
                middleLayer.UpdateTable(iCurrentTable);
        }

        private void btnTempAccChokeApply_Click(object sender, EventArgs e)
        {
            middleLayer.UpdateTable(iCurrentTable);
        }

        private void nudTempServoChokeItem_ValueChanged(object sender, EventArgs e)
        {
            nudTempServoChokeValue.Value = (decimal)middleLayer.Config.tables[iCurrentTable].servo_choke[(int)((NumericUpDown)sender).Value - 1];
            lblTempServoChoke.Text = middleLayer.Config.tables[iCurrentTable].temperatures[(int)((NumericUpDown)sender).Value - 1].ToString("F0");
        }

        private void nudTempServoChokeValue_ValueChanged(object sender, EventArgs e)
        {
            float value = (float)((NumericUpDown)sender).Value;
            int index = (int)nudTempServoChokeItem.Value - 1;
            middleLayer.Config.tables[iCurrentTable].servo_choke[index] = value;
            var result = chartTempServoChoke.Series[0].Points.Where(n => (int)n.Tag == index);
            if (result.FirstOrDefault() != null) result.FirstOrDefault().YValues = new double[1] { value };
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
                middleLayer.UpdateTable(iCurrentTable);
        }

        private void btnTempServoChokeApply_Click(object sender, EventArgs e)
        {
            middleLayer.UpdateTable(iCurrentTable);
        }

        private void nudToolsCopyFrom_ValueChanged(object sender, EventArgs e)
        {
            btnToolsCopy.Enabled = nudToolsCopyFrom.Value != nudToolsCopyTo.Value;
        }

        private void nudToolsCopyTo_ValueChanged(object sender, EventArgs e)
        {
            btnToolsCopy.Enabled = nudToolsCopyFrom.Value != nudToolsCopyTo.Value;
        }

        private void btnToolsCopy_Click(object sender, EventArgs e)
        {
            int from = (int)nudToolsCopyFrom.Value - 1;
            int to = (int)nudToolsCopyTo.Value - 1;

            if (from != to && from >= 0 && from < 4 && to >= 0 && to < 4)
            {
                StructCopy<IgnitionTable> structCopy = new StructCopy<IgnitionTable>();
                byte[] data = structCopy.GetBytes(middleLayer.Config.tables[from]);
                middleLayer.Config.tables[to] = structCopy.FromBytes(data);
                middleLayer.SyncSave(false);
            }
        }

        private void tbParamsName_TextChanged(object sender, EventArgs e)
        {
            middleLayer.Config.tables[iCurrentTable].name = ((TextBox)sender).Text;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(iCurrentTable);
            }
        }

        private void rbParamsValve0_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                middleLayer.Config.tables[iCurrentTable].valve_channel = 0;
                if (!middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateTable(iCurrentTable);
                }
            }
        }

        private void rbParamsValve1_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                middleLayer.Config.tables[iCurrentTable].valve_channel = 1;
                if (!middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateTable(iCurrentTable);
                }
            }
        }

        private void rbParamsValve2_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                middleLayer.Config.tables[iCurrentTable].valve_channel = 2;
                if (!middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateTable(iCurrentTable);
                }
            }
        }

        private void nudParamsValveTimeout_ValueChanged(object sender, EventArgs e)
        {
            middleLayer.Config.tables[iCurrentTable].valve_timeout = (int)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(iCurrentTable);
            }
        }

        private void nudParamsOctane_ValueChanged(object sender, EventArgs e)
        {
            middleLayer.Config.tables[iCurrentTable].octane_corrector = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(iCurrentTable);
            }
        }

        private void nudParamsInitial_ValueChanged(object sender, EventArgs e)
        {
            middleLayer.Config.tables[iCurrentTable].initial_ignition = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(iCurrentTable);
            }
        }

        private void nudParamsCntPress_ValueChanged(object sender, EventArgs e)
        {
            middleLayer.Config.tables[iCurrentTable].pressures_count = (int)((NumericUpDown)sender).Value;
            for (int i = 0; i < middleLayer.Config.tables[iCurrentTable].pressures_count - 1; i++)
                if (middleLayer.Config.tables[iCurrentTable].pressures[i + 1] <= middleLayer.Config.tables[iCurrentTable].pressures[i])
                    middleLayer.Config.tables[iCurrentTable].pressures[i + 1] = middleLayer.Config.tables[iCurrentTable].pressures[i] + 1000;
            UpdateIgnitionsGui();
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(iCurrentTable);
            }
        }

        private void nudParamsCntRPMs_ValueChanged(object sender, EventArgs e)
        {
            middleLayer.Config.tables[iCurrentTable].rotates_count = (int)((NumericUpDown)sender).Value;
            for (int i = 0; i < middleLayer.Config.tables[iCurrentTable].rotates_count - 1; i++)
                if (middleLayer.Config.tables[iCurrentTable].rotates[i + 1] <= middleLayer.Config.tables[iCurrentTable].rotates[i])
                    middleLayer.Config.tables[iCurrentTable].rotates[i + 1] = middleLayer.Config.tables[iCurrentTable].rotates[i] + 100;
            UpdateIgnitionsGui();
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(iCurrentTable);
            }
        }

        private void nudParamsCntIdles_ValueChanged(object sender, EventArgs e)
        {
            middleLayer.Config.tables[iCurrentTable].idles_count = (int)((NumericUpDown)sender).Value;
            for (int i = 0; i < middleLayer.Config.tables[iCurrentTable].idles_count - 1; i++)
                if (middleLayer.Config.tables[iCurrentTable].idle_rotates[i + 1] <= middleLayer.Config.tables[iCurrentTable].idle_rotates[i])
                    middleLayer.Config.tables[iCurrentTable].idle_rotates[i + 1] = middleLayer.Config.tables[iCurrentTable].idle_rotates[i] + 50;
            UpdateIgnitionsGui();
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(iCurrentTable);
            }
        }

        private void nudParamsCntTemps_ValueChanged(object sender, EventArgs e)
        {
            middleLayer.Config.tables[iCurrentTable].temperatures_count = (int)((NumericUpDown)sender).Value;
            for (int i = 0; i < middleLayer.Config.tables[iCurrentTable].temperatures_count - 1; i++)
                if (middleLayer.Config.tables[iCurrentTable].temperatures[i + 1] <= middleLayer.Config.tables[iCurrentTable].temperatures[i])
                    middleLayer.Config.tables[iCurrentTable].temperatures[i + 1] = middleLayer.Config.tables[iCurrentTable].temperatures[i] + 10;
            UpdateIgnitionsGui();
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(iCurrentTable);
            }
        }

        private void nudParamsFuelRate_ValueChanged(object sender, EventArgs e)
        {
            middleLayer.Config.tables[iCurrentTable].fuel_rate = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(iCurrentTable);
            }
        }

        private void nudParamsFuelVolume_ValueChanged(object sender, EventArgs e)
        {
            middleLayer.Config.tables[iCurrentTable].fuel_volume = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(iCurrentTable);
            }
        }

        private void btnDragStart_Click(object sender, EventArgs e)
        {
            fDragFromRPM = (float)nudDragRPMFrom.Value;
            fDragToRPM = (float)nudDragRPMTo.Value;
            if (fDragFromRPM < fDragToRPM)
            {
                nudDragRPMFrom.Enabled = false;
                nudDragRPMTo.Enabled = false;
                btnDragStart.Enabled = false;
                btnDragClear.Enabled = false;
                nudDragTableSplit.Enabled = false;
                nudDragAccelStep.Enabled = false;
                btnDragStop.Enabled = false;
                eDragStatus = DragStatusType.Ready;

                lblDragStatus.Text = "Wait...";

                middleLayer.PacketHandler.SendDragStartRequest(fDragFromRPM, fDragToRPM);
            }
            else
            {
                lblDragStatus.Text = "Invalid RPMs";
            }
        }

        private void btnDragStop_Click(object sender, EventArgs e)
        {
            nudDragRPMFrom.Enabled = false;
            nudDragRPMTo.Enabled = false;
            btnDragStart.Enabled = false;
            btnDragClear.Enabled = false;
            nudDragTableSplit.Enabled = false;
            nudDragAccelStep.Enabled = false;
            btnDragStop.Enabled = false;
            lblDragStatus.Text = "Aborting...";

            middleLayer.PacketHandler.SendDragStopRequest(fDragFromRPM, fDragToRPM);
        }

        private void nudDragTableSplit_ValueChanged(object sender, EventArgs e)
        {
            UpdateDragCharts();
        }

        private void nudDragAccelStep_ValueChanged(object sender, EventArgs e)
        {
            UpdateDragCharts();
        }

        private void btnDragClear_Click(object sender, EventArgs e)
        {
            nudDragRPMFrom.Enabled = true;
            nudDragRPMTo.Enabled = true;
            nudDragAccelStep.Enabled = true;
            nudDragTableSplit.Enabled = true;
            lDragRuns.Clear();
            ClearDragCharts();
        }

        private void btnTableImport_Click(object sender, EventArgs e)
        {
            if (dlgImport.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    middleLayer.Config.tables[iCurrentTable] = Serializator<IgnitionTable>.Deserialize(dlgExport.FileName);
                    if (!middleLayer.IsSynchronizing && cbLive.Checked)
                    {
                        middleLayer.UpdateTable(iCurrentTable);
                    }
                    SynchronizedEvent();
                    MessageBox.Show($"Table import success.", "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Table import failed.\r\n{ex.Message}", "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnTableExport_Click(object sender, EventArgs e)
        {
            if(dlgExport.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Serializator<IgnitionTable>.Serialize(dlgExport.FileName, middleLayer.Config.tables[iCurrentTable]);
                    MessageBox.Show($"Table export success.", "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Table export failed.\r\n{ex.Message}","ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
