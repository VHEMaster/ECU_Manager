﻿using System;
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
using ECU_Manager.Structs;
using ECU_Manager.Tools;

namespace ECU_Manager
{
    public partial class MainForm : Form, IEcuEventHandler
    {
        MiddleLayer middleLayer;
        SyncForm syncForm;
        DateTime lastReceivedGeneralStatus = DateTime.Now;
        int iCurrentTable = 0;
        bool generalStatusReceived = false;
        ComponentStructure cs;

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
        EcuParameters gParameters;

        public MainForm(MiddleLayer middleLayer)
        {
            InitializeComponent();
            this.middleLayer = middleLayer;
            this.cs = middleLayer.ComponentStructure;


            middleLayer.SyncLoad(false);

            syncForm = new SyncForm();
            syncForm.ShowDialog();
            
        }

        public void UpdateParametersEvent(EcuParameters parameters)
        {
            Action action = new Action(() => { this.UpdateParameters(parameters); });
            if (this.InvokeRequired)
                this.BeginInvoke(action);
            else action.Invoke();
        }

        public void UpdateDragStatusEvent(PK_DragUpdateResponse dur)
        {
            Action action = new Action(() => { this.UpdateDragStatus(dur); });
            if (this.InvokeRequired)
                this.BeginInvoke(action);
            else action.Invoke();
        }

        public void UpdateDragPointEvent(PK_DragPointResponse dpr)
        {
            Action action = new Action(() => { this.UpdateDragPoint(dpr); });
            if (this.InvokeRequired)
                this.BeginInvoke(action);
            else action.Invoke();
        }

        public void DragStartAckEvent(PK_DragStartAcknowledge dsaa)
        {
            Action action = new Action(() => { this.DragStartAck(dsaa); });
            if (this.InvokeRequired)
                this.BeginInvoke(action);
            else action.Invoke();
        }

        public void DragStopAckEvent(PK_DragStopAcknowledge dsta)
        {
            Action action = new Action(() => { this.DragStopAck(dsta); });
            if (this.InvokeRequired)
                this.BeginInvoke(action);
            else action.Invoke();
        }

        public void SynchronizedEvent()
        {
            Action action = new Action(() => { this.SynchronizedEventInternal(); });
            if (this.InvokeRequired)
                this.BeginInvoke(action);
            else action.Invoke();
        }

        private void SynchronizedEventInternal()
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

            cbCutoffEnabled.Checked = cs.ConfigStruct.parameters.isCutoffEnabled > 0;
            switch (cs.ConfigStruct.parameters.CutoffMode)
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

            lblCutoffRPM.Text = cs.ConfigStruct.parameters.CutoffRPM.ToString("F0");
            tbCutoffRPM.Value = Convert.ToInt32(cs.ConfigStruct.parameters.CutoffRPM);

            lblCutoffAngle.Text = cs.ConfigStruct.parameters.CutoffAngle.ToString("F0");
            tbCutoffAngle.Value = Convert.ToInt32(cs.ConfigStruct.parameters.CutoffAngle);

            cbEconEnabled.Checked = cs.ConfigStruct.parameters.isEconomEnabled > 0;
            cbEconStrobe.Checked = cs.ConfigStruct.parameters.isEconOutAsStrobe > 0;
            cbEconIgnitionBreak.Checked = cs.ConfigStruct.parameters.isEconIgnitionOff > 0;

            lblEconRPM.Text = cs.ConfigStruct.parameters.EconRpmThreshold.ToString("F0");
            tbEconRPM.Value = Convert.ToInt32(cs.ConfigStruct.parameters.EconRpmThreshold);

            nudSwPos1.Value = cs.ConfigStruct.parameters.switchPos1Table + 1;
            nudSwPos0.Value = cs.ConfigStruct.parameters.switchPos0Table + 1;
            nudSwPos2.Value = cs.ConfigStruct.parameters.switchPos2Table + 1;
            nudFuelForce.Value = cs.ConfigStruct.parameters.forceTableNumber + 1;
            nudEngVol.Value = cs.ConfigStruct.parameters.engineVolume;
            nudForceIgnitionAngle.Value = cs.ConfigStruct.parameters.forceIgnitionAngle;

            cbTempEnabled.Checked = cs.ConfigStruct.parameters.isTemperatureEnabled > 0;
            cbAutostartEnabled.Checked = cs.ConfigStruct.parameters.isAutostartEnabled > 0;
            cbHallIgnition.Checked = cs.ConfigStruct.parameters.isIgnitionByHall > 0;
            cbHallLearn.Checked = cs.ConfigStruct.parameters.isHallLearningMode > 0;
            cbForceIgnition.Checked = cs.ConfigStruct.parameters.isForceIgnition > 0;
            cbFuelForce.Checked = cs.ConfigStruct.parameters.isForceTable > 0;
            cbFuelExtSw.Checked = cs.ConfigStruct.parameters.isSwitchByExternal > 0;

            nudToolsCurTable.Minimum = 1;
            nudToolsCurTable.Maximum = Consts.TABLE_SETUPS_MAX;
            nudToolsCurTable.Value = iCurrentTable + 1;

            UpdateIgnitionsGui();

            middleLayer.PacketHandler.SendGeneralStatusRequest();
        }

        private void UpdateIgnitionsGui()
        {
            tbParamsName.MaxLength = Consts.TABLE_STRING_MAX - 1;
            tbParamsName.Text = cs.ConfigStruct.tables[iCurrentTable].name;
            rbParamsValve0.Checked = false;
            rbParamsValve1.Checked = false;
            rbParamsValve2.Checked = false;

            switch(cs.ConfigStruct.tables[iCurrentTable].valve_channel)
            {
                case 0: rbParamsValve0.Checked = true; break;
                case 1: rbParamsValve1.Checked = true; break;
                case 2: rbParamsValve2.Checked = true; break;
                default: break;
            }
            nudParamsValveTimeout.Value = cs.ConfigStruct.tables[iCurrentTable].valve_timeout;
            nudParamsOctane.Value = (decimal)cs.ConfigStruct.tables[iCurrentTable].octane_corrector;
            nudParamsInitial.Value = (decimal)cs.ConfigStruct.tables[iCurrentTable].initial_ignition;

            nudParamsFuelRate.Value = (decimal)cs.ConfigStruct.tables[iCurrentTable].fuel_rate;
            nudParamsFuelVolume.Value = (decimal)cs.ConfigStruct.tables[iCurrentTable].fuel_volume;

            nudParamsCntPress.Maximum = Consts.TABLE_PRESSURES_MAX;
            nudParamsCntRPMs.Maximum = Consts.TABLE_ROTATES_MAX;
            nudParamsCntIdles.Maximum = Consts.TABLE_ROTATES_MAX;
            nudParamsCntTemps.Maximum = Consts.TABLE_TEMPERATURES_MAX;

            nudParamsCntPress.Value = cs.ConfigStruct.tables[iCurrentTable].pressures_count;
            nudParamsCntRPMs.Value = cs.ConfigStruct.tables[iCurrentTable].rotates_count;
            nudParamsCntIdles.Value = cs.ConfigStruct.tables[iCurrentTable].idles_count;
            nudParamsCntTemps.Value = cs.ConfigStruct.tables[iCurrentTable].temperatures_count;
            
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

        private void UpdateDragStatus(PK_DragUpdateResponse dur)
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

        private void UpdateDragPoint(PK_DragPointResponse dpr)
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

        private void DragStartAck(PK_DragStartAcknowledge dsaa)
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

        private void DragStopAck(PK_DragStopAcknowledge dsta)
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
                if (cs.ConfigStruct.tables[iCurrentTable].ignitions[index] != value)
                {
                    cs.ConfigStruct.tables[iCurrentTable].ignitions[index] = value;
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

        private void UpdateParameters(EcuParameters parameters)
        {
            gParameters = parameters;
            //if (tableLayoutPanel3.Visible)
            {
                mGenIgn.NeedleVal = parameters.IgnitionAngle;
                mGenPress.NeedleVal = parameters.Pressure;
                mGenRPM.NeedleVal = parameters.RPM;
                mGenTemp.NeedleVal = parameters.Temperature;
                mGenFuelUsage.NeedleVal = parameters.FuelUsage;
                label7.Text = (parameters.tablenum + 1).ToString();
                label8.Text = parameters.tablename;
                label10.Text = parameters.valvenum == 0 ? "All Closed" : parameters.valvenum == 1 ? "Petrol" : parameters.valvenum == 2 ? "Propane" : "Invalid";
                label16.Text = parameters.Voltage.ToString("F2") + "V";
            }
            if(panel11.Visible)
            {
                lblSetupRPM.Text = parameters.RPM.ToString("F0");
                lblSetupPressure.Text = parameters.Pressure.ToString("F0");
                lblSetupIgnition.Text = parameters.IgnitionAngle.ToString("F1") + "°";
                lblSetupTemperature.Text = parameters.Temperature.ToString("F1") + "°";
            }
            UpdateIgnitionChartPoint();
            generalStatusReceived = true;
            lastReceivedGeneralStatus = DateTime.Now;
        }
        
        private void tmr1sec_Tick(object sender, EventArgs e)
        {
            if(cs.ConfigStruct.parameters.isHallLearningMode > 0)
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
            cs.ConfigStruct.parameters.isForceIgnition = ((CheckBox)sender).Checked ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudForceIgnitionAngle_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.forceIgnitionAngle = (int)((NumericUpDown)sender).Value - 1;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbCutoffEnabled_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.isCutoffEnabled = ((CheckBox)sender).Checked ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }

        }

        private void rbCutoffMode1_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                cs.ConfigStruct.parameters.CutoffMode = 0;
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
                cs.ConfigStruct.parameters.CutoffMode = 1;
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
                cs.ConfigStruct.parameters.CutoffMode = 2;
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
                cs.ConfigStruct.parameters.CutoffMode = 3;
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
                cs.ConfigStruct.parameters.CutoffMode = 4;
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
                cs.ConfigStruct.parameters.CutoffMode = 5;
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
                cs.ConfigStruct.parameters.CutoffMode = 6;
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
                cs.ConfigStruct.parameters.CutoffMode = 7;
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
            cs.ConfigStruct.parameters.CutoffRPM = ((TrackBar)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void tbCutoffAngle_Scroll(object sender, EventArgs e)
        {
            lblCutoffAngle.Text = ((TrackBar)sender).Value.ToString();
            cs.ConfigStruct.parameters.CutoffAngle = ((TrackBar)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbEconEnabled_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.isEconomEnabled = ((CheckBox)sender).Checked ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbEconStrobe_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.isEconOutAsStrobe = ((CheckBox)sender).Checked ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbEconIgnitionBreak_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.isEconIgnitionOff = ((CheckBox)sender).Checked ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void tbEconRPM_Scroll(object sender, EventArgs e)
        {
            ((TrackBar)sender).Value -= ((TrackBar)sender).Value % 100;
            lblEconRPM.Text = ((TrackBar)sender).Value.ToString();
            cs.ConfigStruct.parameters.EconRpmThreshold = ((TrackBar)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbFuelExtSw_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.isSwitchByExternal = ((CheckBox)sender).Checked ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudSwPos1_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.switchPos1Table = (int)((NumericUpDown)sender).Value - 1;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudEngVol_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.engineVolume = (int)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudSwPos0_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.switchPos0Table = (int)((NumericUpDown)sender).Value - 1;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudSwPos2_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.switchPos2Table = (int)((NumericUpDown)sender).Value - 1;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbFuelForce_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.isForceTable = ((CheckBox)sender).Checked ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudFuelForce_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.forceTableNumber = (int)((NumericUpDown)sender).Value - 1;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbTempEnabled_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.isTemperatureEnabled = ((CheckBox)sender).Checked ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbAutostartEnabled_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.isAutostartEnabled = ((CheckBox)sender).Checked ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbHallIgnition_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.isIgnitionByHall = ((CheckBox)sender).Checked ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbHallLearn_CheckedChanged(object sender, EventArgs e)
        {
            bool learningmode = ((CheckBox)sender).Checked;
            cs.ConfigStruct.parameters.isHallLearningMode = learningmode ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }

            tlpIgnitions.Enabled = !learningmode;


        }

        private void cbForceIdle_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.isForceIdle = ((CheckBox)sender).Checked ? 1 : 0;
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

        private void nudToolsCurTable_ValueChanged(object sender, EventArgs e)
        {
            iCurrentTable = (int)nudToolsCurTable.Value - 1;
            UpdateIgnitionsGui();
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
                StructCopy<EcuTable> structCopy = new StructCopy<EcuTable>();
                byte[] data = structCopy.GetBytes(cs.ConfigStruct.tables[from]);
                cs.ConfigStruct.tables[to] = structCopy.FromBytes(data);
                middleLayer.SyncSave(false);
            }
        }

        private void tbParamsName_TextChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[iCurrentTable].name = ((TextBox)sender).Text;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(iCurrentTable);
            }
        }

        private void rbParamsValve0_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                cs.ConfigStruct.tables[iCurrentTable].valve_channel = 0;
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
                cs.ConfigStruct.tables[iCurrentTable].valve_channel = 1;
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
                cs.ConfigStruct.tables[iCurrentTable].valve_channel = 2;
                if (!middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateTable(iCurrentTable);
                }
            }
        }

        private void nudParamsValveTimeout_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[iCurrentTable].valve_timeout = (int)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(iCurrentTable);
            }
        }

        private void nudParamsOctane_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[iCurrentTable].octane_corrector = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(iCurrentTable);
            }
        }

        private void nudParamsInitial_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[iCurrentTable].initial_ignition = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(iCurrentTable);
            }
        }

        private void nudParamsCntPress_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[iCurrentTable].pressures_count = (int)((NumericUpDown)sender).Value;
            for (int i = 0; i < cs.ConfigStruct.tables[iCurrentTable].pressures_count - 1; i++)
                if (cs.ConfigStruct.tables[iCurrentTable].pressures[i + 1] <= cs.ConfigStruct.tables[iCurrentTable].pressures[i])
                    cs.ConfigStruct.tables[iCurrentTable].pressures[i + 1] = cs.ConfigStruct.tables[iCurrentTable].pressures[i] + 1000;
            UpdateIgnitionsGui();
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(iCurrentTable);
            }
        }

        private void nudParamsCntRPMs_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[iCurrentTable].rotates_count = (int)((NumericUpDown)sender).Value;
            for (int i = 0; i < cs.ConfigStruct.tables[iCurrentTable].rotates_count - 1; i++)
                if (cs.ConfigStruct.tables[iCurrentTable].rotates[i + 1] <= cs.ConfigStruct.tables[iCurrentTable].rotates[i])
                    cs.ConfigStruct.tables[iCurrentTable].rotates[i + 1] = cs.ConfigStruct.tables[iCurrentTable].rotates[i] + 100;
            UpdateIgnitionsGui();
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(iCurrentTable);
            }
        }

        private void nudParamsCntIdles_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[iCurrentTable].idles_count = (int)((NumericUpDown)sender).Value;
            for (int i = 0; i < cs.ConfigStruct.tables[iCurrentTable].idles_count - 1; i++)
                if (cs.ConfigStruct.tables[iCurrentTable].idle_rotates[i + 1] <= cs.ConfigStruct.tables[iCurrentTable].idle_rotates[i])
                    cs.ConfigStruct.tables[iCurrentTable].idle_rotates[i + 1] = cs.ConfigStruct.tables[iCurrentTable].idle_rotates[i] + 50;
            UpdateIgnitionsGui();
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(iCurrentTable);
            }
        }

        private void nudParamsCntTemps_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[iCurrentTable].temperatures_count = (int)((NumericUpDown)sender).Value;
            for (int i = 0; i < cs.ConfigStruct.tables[iCurrentTable].temperatures_count - 1; i++)
                if (cs.ConfigStruct.tables[iCurrentTable].temperatures[i + 1] <= cs.ConfigStruct.tables[iCurrentTable].temperatures[i])
                    cs.ConfigStruct.tables[iCurrentTable].temperatures[i + 1] = cs.ConfigStruct.tables[iCurrentTable].temperatures[i] + 10;
            UpdateIgnitionsGui();
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(iCurrentTable);
            }
        }

        private void nudParamsFuelRate_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[iCurrentTable].fuel_rate = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(iCurrentTable);
            }
        }

        private void nudParamsFuelVolume_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[iCurrentTable].fuel_volume = (float)((NumericUpDown)sender).Value;
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
                    cs.ConfigStruct.tables[iCurrentTable] = Serializator<EcuTable>.Deserialize(dlgExport.FileName);
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
                    Serializator<EcuTable>.Serialize(dlgExport.FileName, cs.ConfigStruct.tables[iCurrentTable]);
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
