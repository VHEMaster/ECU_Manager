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
using ECU_Manager.Structs;
using ECU_Manager.Tools;

namespace ECU_Manager
{
    public partial class MainForm : Form, IEcuEventHandler
    {
        MiddleLayer middleLayer;
        SyncForm syncForm;
        DateTime lastReceivedarameters = DateTime.Now;
        bool parametersReceived = false;
        ComponentStructure cs;

        class DragRun
        {
            public List<DragPoint> Points;
            public int TotalPoints;
            public float FromSpeed;
            public float ToSpeed;
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
        float fDragFromSpeed = 0;
        float fDragToSpeed = 100;
        EcuParameters gParameters;

        public MainForm(MiddleLayer middleLayer)
        {
            InitializeComponent();
            middleLayer.RegisterEventHandler(this);

            this.middleLayer = middleLayer;
            this.cs = middleLayer.ComponentStructure;


            middleLayer.SyncLoad(false);

            syncForm = new SyncForm();
            syncForm.ShowDialog();

            this.DoubleBuffered = true;
            
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

        public void SynchronizedEvent(int errorCode)
        {
            Action action = new Action(() => { this.SynchronizedEventInternal(errorCode); });
            if (this.InvokeRequired)
                this.BeginInvoke(action);
            else action.Invoke();
        }

        private void SynchronizedEventInternal(int errorCode)
        {
            if (syncForm.InvokeRequired)
                syncForm.Invoke(new Action(() => syncForm.CloseForm()));
            else syncForm.Close();

            if(errorCode != 0)
            {
                MessageBox.Show("Error during sync.\r\n\r\nCode: " + errorCode, "Engine Control Unit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            rbCutoffMode1.Checked = false;
            rbCutoffMode2.Checked = false;
            rbCutoffMode3.Checked = false;
            rbCutoffMode4.Checked = false;
            rbCutoffMode5.Checked = false;
            rbCutoffMode6.Checked = false;
            rbCutoffMode7.Checked = false;
            rbCutoffMode8.Checked = false;
            
            switch (cs.ConfigStruct.parameters.cutoffMode)
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

            lblCutoffRPM.Text = cs.ConfigStruct.parameters.cutoffRPM.ToString("F0");
            tbCutoffRPM.Value = Convert.ToInt32(cs.ConfigStruct.parameters.cutoffRPM);

            lblCutoffAngle.Text = cs.ConfigStruct.parameters.cutoffAngle.ToString("F1");
            tbCutoffAngle.Value = Convert.ToInt32(cs.ConfigStruct.parameters.cutoffAngle * 10.0f);

            lblCutoffMixture.Text = cs.ConfigStruct.parameters.cutoffMixture.ToString("F1");
            tbCutoffMixture.Value = Convert.ToInt32(cs.ConfigStruct.parameters.cutoffMixture * 10.0f);

            nudSwPos1.Value = cs.ConfigStruct.parameters.switchPos1Table + 1;
            nudSwPos0.Value = cs.ConfigStruct.parameters.switchPos0Table + 1;
            nudSwPos2.Value = cs.ConfigStruct.parameters.switchPos2Table + 1;
            nudFuelForce.Value = cs.ConfigStruct.parameters.forceTable + 1;
            nudEngVol.Value = (decimal)cs.ConfigStruct.parameters.engineVolume;
            nudSpeedCorr.Value = (decimal)cs.ConfigStruct.parameters.speedCorrection;

            cbUseTSPS.Checked = cs.ConfigStruct.parameters.useTSPS > 0;
            cbUseKnock.Checked = cs.ConfigStruct.parameters.useKnockSensor > 0;
            cbUseLambda.Checked = cs.ConfigStruct.parameters.useLambdaSensor > 0;
            cbPerformCorrs.Checked = cs.ConfigStruct.parameters.performAdaptation > 0;
            cbFuelForce.Checked = cs.ConfigStruct.parameters.isForceTable > 0;
            cbFuelExtSw.Checked = cs.ConfigStruct.parameters.isSwitchByExternal > 0;

            nudToolsCurTable.Minimum = 1;
            nudToolsCurTable.Maximum = Consts.TABLE_SETUPS_MAX;
            nudToolsCurTable.Value = cs.CurrentTable + 1;

            UpdateEcuTableValues();

            if (parametersReceived || (DateTime.Now - lastReceivedarameters).TotalMilliseconds > 200)
            {
                parametersReceived = false;
                lastReceivedarameters = DateTime.Now;
                middleLayer.PacketHandler.SendParametersRequest();
            }
        }

        private void UpdateEcuTableValues()
        {
            tbParamsName.MaxLength = Consts.TABLE_STRING_MAX - 1;
            tbParamsName.Text = cs.ConfigStruct.tables[cs.CurrentTable].name;
            rbInjCh1.Checked = false;
            rbInjCh2.Checked = false;

            switch(cs.ConfigStruct.tables[cs.CurrentTable].inj_channel)
            {
                case 0: rbInjCh1.Checked = true; break;
                case 1: rbInjCh2.Checked = true; break;
                default: break;
            }

            nudParamsCntPress.Maximum = Consts.TABLE_PRESSURES_MAX;
            nudParamsCntRPMs.Maximum = Consts.TABLE_ROTATES_MAX;
            nudParamsCntThrottles.Maximum = Consts.TABLE_THROTTLES_MAX;
            nudParamsCntVoltages.Maximum = Consts.TABLE_VOLTAGES_MAX;
            nudParamsCntFillings.Maximum = Consts.TABLE_FILLING_MAX;
            nudParamsCntEngineTemps.Maximum = Consts.TABLE_TEMPERATURES_MAX;

            nudParamsCntPress.Value = cs.ConfigStruct.tables[cs.CurrentTable].pressures_count;
            nudParamsCntRPMs.Value = cs.ConfigStruct.tables[cs.CurrentTable].rotates_count;
            nudParamsCntThrottles.Value = cs.ConfigStruct.tables[cs.CurrentTable].throttles_count;
            nudParamsCntVoltages.Value = cs.ConfigStruct.tables[cs.CurrentTable].voltages_count;
            nudParamsCntFillings.Value = cs.ConfigStruct.tables[cs.CurrentTable].fillings_count;
            nudParamsCntEngineTemps.Value = cs.ConfigStruct.tables[cs.CurrentTable].engine_temp_count;

            nudParamsFuelPressure.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].fuel_pressure;
            nudParamsInitialIgnition.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].ignition_initial;
            nudParamsInjPerformance.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].injector_performance;
            nudParamsFuelKgL.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].fuel_mass_per_cc;

            nudParamsPidIdleValveP.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].idle_valve_to_massair_pid_p;
            nudParamsPidIdleValveI.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].idle_valve_to_massair_pid_i;
            nudParamsPidIdleValveD.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].idle_valve_to_massair_pid_d;
            nudParamsPidIdleIgnP.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].idle_ign_to_rpm_pid_p;
            nudParamsPidIdleIgnI.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].idle_ign_to_rpm_pid_i;
            nudParamsPidIdleIgnD.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].idle_ign_to_rpm_pid_d;
            
            nudParamsEnrPMapTps.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].enrichment_proportion_map_vs_thr;
            nudParamsIdleIgnDevMin.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].idle_ign_deviation_min;
            nudParamsIdleIgnDevMax.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].idle_ign_deviation_max;
            nudParamsIdleIgnFanCorr.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].idle_ign_fan_corr;

            nudParamsCorrInjCy1.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_injection[0];
            nudParamsCorrInjCy2.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_injection[1];
            nudParamsCorrInjCy3.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_injection[2];
            nudParamsCorrInjCy4.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_injection[3];

            nudParamsCorrIgnCy1.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_ignition[0];
            nudParamsCorrIgnCy2.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_ignition[1];
            nudParamsCorrIgnCy3.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_ignition[2];
            nudParamsCorrIgnCy4.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_ignition[3];

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
            double FromSpeed = fDragFromSpeed;
            double ToSpeed = fDragToSpeed;
            int TableSplit = (int)nudDragTableSplit.Value;

            ClearDragCharts();

            //Color min = Color.SpringGreen;
            //Color max = Color.IndianRed;
            double timeMax = 1;

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
                            series.Points.AddXY(lDragRuns[i].Points[j].Time, lDragRuns[i].Points[j].Speed);
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
                        
                        for (int j = 0; j < lDragRuns[i].Points.Count; j++)
                        {
                            series.Points.AddXY(lDragRuns[i].Points[j].Time, lDragRuns[i].Points[j].Acceleration);
                        }

                    }
                }
                
                chartDragTime.ChartAreas[0].AxisX.Minimum = 0;
                //chartDragTime.ChartAreas[0].AxisX.Maximum = timeMax;

                chartDragTime.ChartAreas[0].AxisY.Minimum = FromSpeed;
                chartDragTime.ChartAreas[0].AxisY.Maximum = ToSpeed;
                
                chartDragAccel.ChartAreas[0].AxisX.Minimum = 0;
                //chartDragAccel.ChartAreas[0].AxisX.Maximum = timeMax;

                chartDragAccel.ChartAreas[0].AxisY.Minimum = 0;
                //chartDragAccel.ChartAreas[0].AxisY.Maximum = deltaRpmMax;


                if(TableSplit >= 2)
                {
                    double[] speeds = new double[TableSplit + 1];
                    for(int i = 0; i <= TableSplit; i++)
                    {
                        speeds[i] = ((ToSpeed - FromSpeed) / TableSplit * i) + FromSpeed;
                    }

                    for (int i = 0; i < TableSplit; i++)
                    {
                        lvDragTable.Columns.Add($"{speeds[i]}-{speeds[i + 1]}");
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
                                if(lDragRuns[j].Points[p].Speed >= speeds[i] && lDragRuns[j].Points[p].Speed < speeds[i + 1])
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
            lblDragSpeed.Text = dur.Point.Speed.ToString("F0") + " km/h";
            lblDragTime.Text = (dur.Point.Time / 1000000.0).ToString("F2") + "s";
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

                    nudDragSpeedFrom.Enabled = false;
                    nudDragSpeedTo.Enabled = false;
                    btnDragStart.Enabled = true;
                    btnDragClear.Enabled = true;
                    nudDragTableSplit.Enabled = false;
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
                            FromSpeed = dur.FromSpeed,
                            ToSpeed = dur.ToSpeed,
                            Time = dur.Point.Time / 1000000.0,
                            Points = new List<DragPoint>(),
                            DateTime = DateTime.Now,
                            Name = tbDragName.Text,
                            TotalPoints = (int)dur.TotalPoints
                        };
                        drCurrentRun.Label = drCurrentRun.Name + " " + (lDragRuns.Where(t => t.Name == drCurrentRun.Name).Count() + 1).ToString();

                        lDragRuns.Add(drCurrentRun);

                        if (drCurrentRun.TotalPoints > drCurrentRun.Points.Count)
                        {
                            middleLayer.PacketHandler.SendDragPointRequest(drCurrentRun.FromSpeed, drCurrentRun.ToSpeed, drCurrentRun.Points.Count);
                        }
                        else
                        {
                            lblDragStatus.Text = "Empty...";

                            nudDragSpeedFrom.Enabled = false;
                            nudDragSpeedTo.Enabled = false;
                            btnDragStart.Enabled = true;
                            btnDragClear.Enabled = true;
                            nudDragTableSplit.Enabled = false;
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
                drCurrentRun.Points.Add(dpr.Point);

                if (drCurrentRun.TotalPoints > drCurrentRun.Points.Count)
                {
                    middleLayer.PacketHandler.SendDragPointRequest(drCurrentRun.FromSpeed, drCurrentRun.ToSpeed, drCurrentRun.Points.Count);
                }
                else
                {
                    UpdateDragCharts();

                    lblDragStatus.Text = "Done!";

                    nudDragSpeedFrom.Enabled = false;
                    nudDragSpeedTo.Enabled = false;
                    btnDragStart.Enabled = true;
                    btnDragClear.Enabled = true;
                    nudDragTableSplit.Enabled = false;
                    btnDragStop.Enabled = false;
                }
            }
            else
            {
                lblDragStatus.Text = "Error code: " + dpr.ErrorCode;

                nudDragSpeedFrom.Enabled = false;
                nudDragSpeedTo.Enabled = false;
                btnDragStart.Enabled = true;
                btnDragClear.Enabled = true;
                nudDragTableSplit.Enabled = false;
                btnDragStop.Enabled = false;
            }
        }

        private void DragStartAck(PK_DragStartAcknowledge dsaa)
        {
            if(eDragStatus == DragStatusType.Ready)
            {
                eDragStatus = DragStatusType.Set;
                lblDragStatus.Text = "SET";

                nudDragSpeedFrom.Enabled = false;
                nudDragSpeedTo.Enabled = false;
                btnDragStart.Enabled = false;
                btnDragClear.Enabled = false;
                nudDragTableSplit.Enabled = false;
                btnDragStop.Enabled = true;
            }
        }

        private void DragStopAck(PK_DragStopAcknowledge dsta)
        {
            eDragStatus = DragStatusType.Ready;
            lblDragStatus.Text = "Aborted";

            nudDragSpeedFrom.Enabled = false;
            nudDragSpeedTo.Enabled = false;
            btnDragStart.Enabled = true;
            btnDragClear.Enabled = true;
            nudDragTableSplit.Enabled = false;
            btnDragStop.Enabled = false;
        }
        

        private void UpdateParameters(EcuParameters parameters)
        {
            gParameters = parameters;
            //if (tableLayoutPanel3.Visible)
            {
                mGenIgn.NeedleVal = parameters.IgnitionAngle;
                mGenPress.NeedleVal = parameters.ManifoldAirPressure;
                mGenRPM.NeedleVal = parameters.RPM;
                mGenTemp.NeedleVal = parameters.EngineTemp;
                mGenFuelUsage.NeedleVal = parameters.FuelConsumption;
                label7.Text = (parameters.CurrentTable + 1).ToString();
                label8.Text = parameters.CurrentTableName;
                label10.Text = parameters.InjectorChannel == 0 ? "Channel 1" : parameters.InjectorChannel == 1 ? "Channel 2" : "Invalid";
                label16.Text = parameters.PowerVoltage.ToString("F2") + "V";
            }
            parametersReceived = true;
            lastReceivedarameters = DateTime.Now;
        }
        
        private void tmr1sec_Tick(object sender, EventArgs e)
        {
            middleLayer.SyncFast();
        }

        private void tmr50ms_Tick(object sender, EventArgs e)
        {
            tableLayoutPanel1.Enabled = !middleLayer.IsSynchronizing;
            toolStripProgressBar1.Style = middleLayer.IsSynchronizing ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks;
            if (parametersReceived || (DateTime.Now - lastReceivedarameters).TotalMilliseconds > 200)
            {
                parametersReceived = false;
                lastReceivedarameters = DateTime.Now;
                middleLayer.PacketHandler.SendParametersRequest();
            }
            if((tabControl1.Visible && tabControl1.SelectedTab == tabPage18) || eDragStatus == DragStatusType.Set || eDragStatus == DragStatusType.Go)
            {
                middleLayer.PacketHandler.SendDragUpdateRequest();
            }
        }

        private void rbCutoffMode1_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                cs.ConfigStruct.parameters.cutoffMode = 0;
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
                cs.ConfigStruct.parameters.cutoffMode = 1;
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
                cs.ConfigStruct.parameters.cutoffMode = 2;
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
                cs.ConfigStruct.parameters.cutoffMode = 3;
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
                cs.ConfigStruct.parameters.cutoffMode = 4;
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
                cs.ConfigStruct.parameters.cutoffMode = 5;
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
                cs.ConfigStruct.parameters.cutoffMode = 6;
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
                cs.ConfigStruct.parameters.cutoffMode = 7;
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
            cs.ConfigStruct.parameters.cutoffRPM = ((TrackBar)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void tbCutoffAngle_Scroll(object sender, EventArgs e)
        {
            lblCutoffAngle.Text = ((float)((TrackBar)sender).Value / 10.0f).ToString("F1");
            cs.ConfigStruct.parameters.cutoffAngle = ((float)((TrackBar)sender).Value / 10.0f);
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void tbCutoffMixture_Scroll(object sender, EventArgs e)
        {
            lblCutoffMixture.Text = ((float)((TrackBar)sender).Value / 10.0f).ToString("F1");
            cs.ConfigStruct.parameters.cutoffMixture = ((float)((TrackBar)sender).Value / 10.0f);
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
            cs.ConfigStruct.parameters.engineVolume = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudSpeedCorr_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.speedCorrection = (float)((NumericUpDown)sender).Value;
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
            cs.ConfigStruct.parameters.forceTable = (int)((NumericUpDown)sender).Value - 1;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbUseTSPS_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.useTSPS = ((CheckBox)sender).Checked ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbUseKnock_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.useKnockSensor = ((CheckBox)sender).Checked ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbUseLambda_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.useLambdaSensor = ((CheckBox)sender).Checked ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbPerformCorrs_CheckedChanged(object sender, EventArgs e)
        {
            bool learningmode = ((CheckBox)sender).Checked;
            cs.ConfigStruct.parameters.performAdaptation = learningmode ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }

        }

        private void cbIsIndivCoils_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.isIndividualCoils = ((CheckBox)sender).Checked ? 1 : 0;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbIsSingleCoil_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.isSingleCoil = ((CheckBox)sender).Checked ? 1 : 0;
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
            while (!middleLayer.SyncSave(true));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            while(!middleLayer.SyncLoad(true));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            while (!middleLayer.SyncLoad(false));
        }

        private void nudToolsCurTable_ValueChanged(object sender, EventArgs e)
        {
            cs.CurrentTable = (int)nudToolsCurTable.Value - 1;
            UpdateEcuTableValues();
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

        private void btnDragStart_Click(object sender, EventArgs e)
        {
            fDragFromSpeed = (float)nudDragSpeedFrom.Value;
            fDragToSpeed = (float)nudDragSpeedTo.Value;
            if (fDragFromSpeed < fDragToSpeed)
            {
                nudDragSpeedFrom.Enabled = false;
                nudDragSpeedTo.Enabled = false;
                btnDragStart.Enabled = false;
                btnDragClear.Enabled = false;
                nudDragTableSplit.Enabled = false;
                btnDragStop.Enabled = false;
                eDragStatus = DragStatusType.Ready;

                lblDragStatus.Text = "Wait...";

                middleLayer.PacketHandler.SendDragStartRequest(fDragFromSpeed, fDragToSpeed);
            }
            else
            {
                lblDragStatus.Text = "Invalid RPMs";
            }
        }

        private void btnDragStop_Click(object sender, EventArgs e)
        {
            nudDragSpeedFrom.Enabled = false;
            nudDragSpeedTo.Enabled = false;
            btnDragStart.Enabled = false;
            btnDragClear.Enabled = false;
            nudDragTableSplit.Enabled = false;
            btnDragStop.Enabled = false;
            lblDragStatus.Text = "Aborting...";

            middleLayer.PacketHandler.SendDragStopRequest(fDragFromSpeed, fDragToSpeed);
        }

        private void nudDragTableSplit_ValueChanged(object sender, EventArgs e)
        {
            UpdateDragCharts();
        }

        private void btnDragClear_Click(object sender, EventArgs e)
        {
            nudDragSpeedFrom.Enabled = true;
            nudDragSpeedTo.Enabled = true;
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
                    cs.ConfigStruct.tables[cs.CurrentTable] = Serializator<EcuTable>.Deserialize(dlgExport.FileName);
                    if (!middleLayer.IsSynchronizing && cbLive.Checked)
                    {
                        middleLayer.UpdateTable(cs.CurrentTable);
                    }
                    SynchronizedEvent(0);
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
                    Serializator<EcuTable>.Serialize(dlgExport.FileName, cs.ConfigStruct.tables[cs.CurrentTable]);
                    MessageBox.Show($"Table export success.", "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Table export failed.\r\n{ex.Message}","ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tbParamsName_TextChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].name = ((TextBox)sender).Text;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void rbInjCh1_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                cs.ConfigStruct.tables[cs.CurrentTable].inj_channel = 0;
                if (!middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateTable(cs.CurrentTable);
                }
            }
        }

        private void rbInjCh2_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                cs.ConfigStruct.tables[cs.CurrentTable].inj_channel = 1;
                if (!middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateTable(cs.CurrentTable);
                }
            }
        }

        private void nudParamsFuelPressure_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].fuel_pressure = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsFuelKgL_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].fuel_mass_per_cc = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsInjPerformance_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].injector_performance = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsInitialIgnition_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].ignition_initial = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCntPress_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].pressures_count = (int)((NumericUpDown)sender).Value;
            for (int i = 0; i < cs.ConfigStruct.tables[cs.CurrentTable].pressures_count - 1; i++)
                if (cs.ConfigStruct.tables[cs.CurrentTable].pressures[i + 1] <= cs.ConfigStruct.tables[cs.CurrentTable].pressures[i])
                    cs.ConfigStruct.tables[cs.CurrentTable].pressures[i + 1] = cs.ConfigStruct.tables[cs.CurrentTable].pressures[i] + 1000;
            UpdateEcuTableValues();
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCntRPMs_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].rotates_count = (int)((NumericUpDown)sender).Value;
            for (int i = 0; i < cs.ConfigStruct.tables[cs.CurrentTable].rotates_count - 1; i++)
                if (cs.ConfigStruct.tables[cs.CurrentTable].rotates[i + 1] <= cs.ConfigStruct.tables[cs.CurrentTable].rotates[i])
                    cs.ConfigStruct.tables[cs.CurrentTable].rotates[i + 1] = cs.ConfigStruct.tables[cs.CurrentTable].rotates[i] + 100;
            UpdateEcuTableValues();
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCntThrottles_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].throttles_count = (int)((NumericUpDown)sender).Value;
            for (int i = 0; i < cs.ConfigStruct.tables[cs.CurrentTable].throttles_count - 1; i++)
                if (cs.ConfigStruct.tables[cs.CurrentTable].throttles[i + 1] <= cs.ConfigStruct.tables[cs.CurrentTable].throttles[i])
                    cs.ConfigStruct.tables[cs.CurrentTable].throttles[i + 1] = cs.ConfigStruct.tables[cs.CurrentTable].throttles[i] + 2;
            UpdateEcuTableValues();
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCntVoltages_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].voltages_count = (int)((NumericUpDown)sender).Value;
            for (int i = 0; i < cs.ConfigStruct.tables[cs.CurrentTable].voltages_count - 1; i++)
                if (cs.ConfigStruct.tables[cs.CurrentTable].voltages[i + 1] <= cs.ConfigStruct.tables[cs.CurrentTable].rotates[i])
                    cs.ConfigStruct.tables[cs.CurrentTable].voltages[i + 1] = cs.ConfigStruct.tables[cs.CurrentTable].rotates[i] + 1;
            UpdateEcuTableValues();
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCntFillings_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].fillings_count = (int)((NumericUpDown)sender).Value;
            for (int i = 0; i < cs.ConfigStruct.tables[cs.CurrentTable].fillings_count - 1; i++)
                if (cs.ConfigStruct.tables[cs.CurrentTable].fillings[i + 1] <= cs.ConfigStruct.tables[cs.CurrentTable].fillings[i])
                    cs.ConfigStruct.tables[cs.CurrentTable].fillings[i + 1] = cs.ConfigStruct.tables[cs.CurrentTable].fillings[i] + 10;
            UpdateEcuTableValues();
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCntEngineTemps_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].engine_temp_count = (int)((NumericUpDown)sender).Value;
            for (int i = 0; i < cs.ConfigStruct.tables[cs.CurrentTable].engine_temp_count - 1; i++)
                if (cs.ConfigStruct.tables[cs.CurrentTable].engine_temps[i + 1] <= cs.ConfigStruct.tables[cs.CurrentTable].engine_temps[i])
                    cs.ConfigStruct.tables[cs.CurrentTable].engine_temps[i + 1] = cs.ConfigStruct.tables[cs.CurrentTable].engine_temps[i] + 10;
            UpdateEcuTableValues();
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsPidIdleValveP_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].idle_valve_to_massair_pid_p = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsPidIdleValveI_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].idle_valve_to_massair_pid_i = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsPidIdleValveD_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].idle_valve_to_massair_pid_d = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsPidIdleIgnP_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].idle_ign_to_rpm_pid_p = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsPidIdleIgnI_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].idle_ign_to_rpm_pid_i = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsPidIdleIgnD_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].idle_ign_to_rpm_pid_d = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCorrInjCy1_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_injection[0] = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCorrInjCy2_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_injection[1] = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCorrInjCy3_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_injection[2] = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCorrInjCy4_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_injection[3] = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCorrIgnCy1_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_ignition[0] = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCorrIgnCy2_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_ignition[1] = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCorrIgnCy3_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_ignition[2] = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCorrIgnCy4_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_ignition[3] = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsEnrPMapTps_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].enrichment_proportion_map_vs_thr = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsIdleIgnDevMin_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].idle_ign_deviation_min = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsIdleIgnDevMax_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].idle_ign_deviation_max = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsIdleIgnFanCorr_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].idle_ign_fan_corr = (float)((NumericUpDown)sender).Value;
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
