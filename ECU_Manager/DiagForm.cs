using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ECU_Framework;
using ECU_Framework.Packets;
using ECU_Framework.Structs;
using ECU_Framework.Tools;

namespace ECU_Manager
{
    public partial class DiagForm : Form, IEcuEventHandler
    {
        MiddleLayer middleLayer;
        
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
        List<PointData> dataPoints = new List<PointData>();
        Mutex dataMutex = new Mutex();
        Stopwatch stopwatch = new Stopwatch();
        bool widthBigPrev = false;

        List<Label> lblValues = new List<Label>();
        List<Chart> chartValues = new List<Chart>();

        public DiagForm(FileInfo fileInfo)
        {
            InitializeComponent();

            cbLiveView.Visible = false;
            cbLiveView.Enabled = false;
            cbLiveView.Checked = false;

            Initialize();

            try
            {
                IEnumerable<PointData> points = EcuLog.ParseLog(fileInfo);
                dataMutex.WaitOne();
                foreach(PointData point in points)
                {
                    dataPoints.Add(point);
                }
                dataMutex.ReleaseMutex();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to import log.\r\n" + ex.Message, "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            UpdateCharts();
        }
        
        public DiagForm(MiddleLayer middleLayer)
        {
            InitializeComponent();

            middleLayer.RegisterEventHandler(this);
            this.middleLayer = middleLayer;

            Initialize();
        }

        private void Initialize()
        {
            InitParametersList();

            lbParamsAvailable.Items.Clear();
            lbParamsUsed.Items.Clear();

            foreach (Parameter parameter in ChartParameters)
            {
                lbParamsAvailable.Items.Add(parameter);
            }

            lbParamsUsed.Items.Add(ChartParameters.Where(p => p.Name == "RPM").First());
            lbParamsUsed.Items.Add(ChartParameters.Where(p => p.Name == "ManifoldAirPressure").First());
            lbParamsUsed.Items.Add(ChartParameters.Where(p => p.Name == "ThrottlePosition").First());
            lbParamsUsed.Items.Add(ChartParameters.Where(p => p.Name == "IgnitionAdvance").First());
            lbParamsUsed.Items.Add(ChartParameters.Where(p => p.Name == "FuelRatio").First());
            lbParamsUsed.Items.Add(ChartParameters.Where(p => p.Name == "CyclicAirFlow").First());
            lbParamsUsed.Items.Add(ChartParameters.Where(p => p.Name == "InjectionPulse").First());

            foreach (Parameter item in lbParamsUsed.Items)
            {
                lbParamsAvailable.Items.Remove(item);
            }

            GroupBox gpPrevious = gpForceTemplate;
            gpPrevious.Location = new Point(gpPrevious.Location.X, gpPrevious.Location.Y - gpPrevious.Size.Height);
            if (middleLayer != null)
            {
                foreach (Parameter parameter in ForceParameters)
                {
                    GroupBox groupBox = new GroupBox();
                    groupBox.ForeColor = gpPrevious.ForeColor;
                    groupBox.Size = gpPrevious.Size;
                    groupBox.Location = gpPrevious.Location;
                    groupBox.Font = gpPrevious.Font;
                    groupBox.Anchor = gpPrevious.Anchor;
                    groupBox.Padding = gpPrevious.Padding;
                    groupBox.Margin = gpPrevious.Margin;
                    groupBox.Tag = parameter;
                    groupBox.Name = "groupBoxForce" + parameter.Name;
                    groupBox.Text = parameter.Name;
                    groupBox.Location = new Point(gpPrevious.Location.X, gpPrevious.Location.Y + gpPrevious.Size.Height);

                    CheckBox checkBox = new CheckBox();
                    checkBox.AutoSize = cbForceTemplate.AutoSize;
                    checkBox.Location = cbForceTemplate.Location;
                    checkBox.Size = cbForceTemplate.Size;
                    checkBox.Name = "checkBoxForceEnable" + parameter.Name;
                    checkBox.Checked = false;
                    checkBox.Tag = parameter;
                    checkBox.CheckedChanged += CheckBoxForceParam_CheckedChanged;

                    TrackBar trackBar = new TrackBar();
                    trackBar.Anchor = tbForceTemplate.Anchor;
                    trackBar.AutoSize = tbForceTemplate.AutoSize;
                    trackBar.Location = tbForceTemplate.Location;
                    trackBar.Size = tbForceTemplate.Size;
                    trackBar.Name = "trackBarForceEnable" + parameter.Name;
                    trackBar.Enabled = checkBox.Checked;
                    trackBar.Minimum = (int)(parameter.Min / parameter.Step);
                    trackBar.Maximum = (int)(parameter.Max / parameter.Step);
                    trackBar.LargeChange = (int)((parameter.Max - parameter.Min) / parameter.Step / 10.0F);
                    trackBar.TickFrequency = trackBar.LargeChange;
                    trackBar.SmallChange = 1;
                    if (parameter.Type == typeof(float))
                    {
                        if ((float)parameter.Value / parameter.Step > trackBar.Maximum)
                            trackBar.Value = trackBar.Maximum;
                        else if ((float)parameter.Value / parameter.Step < trackBar.Minimum)
                            trackBar.Value = trackBar.Minimum;
                        else
                            trackBar.Value = (int)((float)parameter.Value / parameter.Step);
                    }
                    else
                    {
                        if ((int)((int)parameter.Value / parameter.Step) > trackBar.Maximum)
                            trackBar.Value = trackBar.Maximum;
                        else if ((int)((int)parameter.Value / parameter.Step) < trackBar.Minimum)
                            trackBar.Value = trackBar.Minimum;
                        else
                            trackBar.Value = (int)((int)parameter.Value / parameter.Step);
                    }
                    trackBar.Tag = parameter;
                    trackBar.Scroll += TrackBarForceParam_Scroll;

                    NumericUpDown nud = new NumericUpDown();
                    nud.Anchor = nudForceTemplate.Anchor;
                    nud.AutoSize = nudForceTemplate.AutoSize;
                    nud.Location = nudForceTemplate.Location;
                    nud.Size = nudForceTemplate.Size;
                    nud.Name = "nudForceEnable" + parameter.Name;
                    nud.Enabled = checkBox.Checked;
                    nud.Minimum = (decimal)parameter.Min;
                    nud.Maximum = (decimal)parameter.Max;
                    nud.DecimalPlaces = parameter.Step < 1.0F ? (int)Math.Ceiling(0 - Math.Log10(parameter.Step)) : 0;
                    nud.Increment = (decimal)parameter.Step;
                    if (parameter.Type == typeof(float))
                    {
                        if ((decimal)(float)parameter.Value > nud.Maximum)
                            nud.Value = nud.Maximum;
                        else if ((decimal)(float)parameter.Value < nud.Minimum)
                            nud.Value = nud.Minimum;
                        else
                            nud.Value = (decimal)(float)parameter.Value;
                    }
                    else
                    {
                        if ((decimal)(int)parameter.Value > nud.Maximum)
                            nud.Value = nud.Maximum;
                        else if ((decimal)(int)parameter.Value < nud.Minimum)
                            nud.Value = nud.Minimum;
                        else
                            nud.Value = (decimal)(int)parameter.Value;
                    }
                    nud.Tag = parameter;
                    nud.ValueChanged += NudForceParam_ValueChanged;


                    parameter.NumericUpDown = nud;
                    parameter.TrackBar = trackBar;

                    groupBox.Controls.Add(checkBox);
                    groupBox.Controls.Add(trackBar);
                    groupBox.Controls.Add(nud);
                    gpForceTemplate.Parent.Controls.Add(groupBox);

                    gpPrevious = groupBox;

                }
            }
            else
            {
                label5.Visible = false;
            }

            gpForceTemplate.Parent.Controls.Remove(gpForceTemplate);

            UpdateChartsSetup();

            stopwatch.Restart();
            this.DoubleBuffered = true;

            dTimeFrom = 0;
            dTimeTo = TimeScaleTime[(int)TimeScale];
        }

        private void CheckBoxForceParam_CheckedChanged(object sender, EventArgs e)
        {
            if (middleLayer != null)
            {
                Parameter parameter = (Parameter)((Control)sender).Tag;
                if (parameter != null)
                {
                    parameter.Enabled = ((CheckBox)sender).Checked;
                    parameter.NumericUpDown.Enabled = parameter.Enabled;
                    parameter.TrackBar.Enabled = parameter.Enabled;
                    UpdateForceElements(middleLayer.ComponentStructure.EcuParameters);
                    middleLayer.UpdateForceParameters();
                }
            }
            else
            {
                if(((CheckBox)sender).Checked != false)
                    ((CheckBox)sender).Checked = false;
            }
        }

        private void NudForceParam_ValueChanged(object sender, EventArgs e)
        {
            Parameter parameter = (Parameter)((Control)sender).Tag;

            if (middleLayer != null && parameter != null)
            {
                if (parameter.Type == typeof(float))
                {
                    float value = (float)parameter.NumericUpDown.Value;

                    if ((decimal)value > parameter.NumericUpDown.Maximum)
                        value = (float)parameter.NumericUpDown.Maximum;
                    else if ((decimal)value < parameter.NumericUpDown.Minimum)
                        value = (float)parameter.NumericUpDown.Minimum;

                    parameter.Value = value;

                    value /= parameter.Step;
                    if (value > parameter.TrackBar.Maximum)
                        value = parameter.TrackBar.Maximum;
                    else if (value < parameter.TrackBar.Minimum)
                        value = parameter.TrackBar.Minimum;
                    parameter.TrackBar.Value = (int)value;
                }
                else
                {
                    int value = (int)parameter.NumericUpDown.Value;

                    if ((decimal)value > parameter.NumericUpDown.Maximum)
                        value = (int)parameter.NumericUpDown.Maximum;
                    else if ((decimal)value < parameter.NumericUpDown.Minimum)
                        value = (int)parameter.NumericUpDown.Minimum;

                    parameter.Value = value;

                    value = (int)((float)value / parameter.Step);
                    if (value > parameter.TrackBar.Maximum)
                        value = parameter.TrackBar.Maximum;
                    else if (value < parameter.TrackBar.Minimum)
                        value = parameter.TrackBar.Minimum;
                    parameter.TrackBar.Value = (int)value;
                }
            }
        }

        private void TrackBarForceParam_Scroll(object sender, EventArgs e)
        {
            Parameter parameter = (Parameter)((Control)sender).Tag;

            if(parameter != null)
            {
                if (parameter.Type == typeof(float))
                {
                    float value = (float)parameter.TrackBar.Value * parameter.Step;

                    if ((decimal)value > parameter.NumericUpDown.Maximum)
                        value = (float)parameter.NumericUpDown.Maximum;
                    else if ((decimal)value < parameter.NumericUpDown.Minimum)
                        value = (float)parameter.NumericUpDown.Minimum;

                    parameter.NumericUpDown.Value = (decimal)value;
                    parameter.Value = value;
                }
                else
                {
                    int value = (int)((float)parameter.TrackBar.Value * parameter.Step);

                    if ((decimal)value > parameter.NumericUpDown.Maximum)
                        value = (int)parameter.NumericUpDown.Maximum;
                    else if ((decimal)value < parameter.NumericUpDown.Minimum)
                        value = (int)parameter.NumericUpDown.Minimum;

                    parameter.NumericUpDown.Value = (decimal)value;
                    parameter.Value = value;
                }
            }
        }

        private List<Parameter> ChartParameters = new List<Parameter>();
        private List<Parameter> ForceParameters = new List<Parameter>();

        private void InitParametersList()
        {
            FieldInfo[] parametersFields = typeof(EcuParameters).GetFields();
            FieldInfo[] forceFields = typeof(EcuForceParameters).GetFields();

            foreach (FieldInfo fieldInfo in parametersFields)
            {
                Parameter parameter = new Parameter(middleLayer);

                parameter.FieldInfo = fieldInfo;
                parameter.Name = fieldInfo.Name;
                parameter.Type = fieldInfo.FieldType;

                if (fieldInfo.Name == "AdcKnockVoltage") parameter.FloatFormat = "F2"; //&ecu_parameters.AdcKnockVoltage, .title = "%s%0.2f"},
                if (fieldInfo.Name == "AdcAirTemp") parameter.FloatFormat = "F2"; //&ecu_parameters.AdcAirTemp, .title = "%s%0.2f"},
                if (fieldInfo.Name == "AdcEngineTemp") parameter.FloatFormat = "F2"; //&ecu_parameters.AdcEngineTemp, .title = "%s%0.2f"},
                if (fieldInfo.Name == "AdcPressure") parameter.FloatFormat = "F2"; //&ecu_parameters.AdcManifoldAirPressure, .title = "%s%0.2f"},
                if (fieldInfo.Name == "AdcThrottlePosition") parameter.FloatFormat = "F2"; //&ecu_parameters.AdcThrottlePosition, .title = "%s%0.2f"},
                if (fieldInfo.Name == "AdcPowerVoltage") parameter.FloatFormat = "F2"; //&ecu_parameters.AdcPowerVoltage, .title = "%s%0.2f"},
                if (fieldInfo.Name == "AdcReferenceVoltage") parameter.FloatFormat = "F2"; //&ecu_parameters.AdcReferenceVoltage, .title = "%s%0.2f"},
                if (fieldInfo.Name == "AdcLambdaUR") parameter.FloatFormat = "F2"; //&ecu_parameters.AdcLambdaUR, .title = "%s%0.2f"},
                if (fieldInfo.Name == "AdcLambdaUA") parameter.FloatFormat = "F2"; //&ecu_parameters.AdcLambdaUA, .title = "%s%0.2f"},
                if (fieldInfo.Name == "KnockSensor") parameter.FloatFormat = "F2"; //&ecu_parameters.KnockSensor, .title = "%s%0.2f"},
                if (fieldInfo.Name == "KnockSensorFiltered") parameter.FloatFormat = "F2"; //&ecu_parameters.KnockSensorFiltered, .title = "%s%0.2f"},
                if (fieldInfo.Name == "KnockSensorDetonate") parameter.FloatFormat = "F2"; //&ecu_parameters.KnockSensorDetonate, .title = "%s%0.2f"},
                if (fieldInfo.Name == "KnockZone") parameter.FloatFormat = "F2"; //&ecu_parameters.KnockSensorFiltered, .title = "%s%0.2f"},
                if (fieldInfo.Name == "KnockAdvance") parameter.FloatFormat = "F2"; //&ecu_parameters.KnockSensorFiltered, .title = "%s%0.2f"},
                if (fieldInfo.Name == "AirTemp") parameter.FloatFormat = "F1"; //&ecu_parameters.AirTemp, .title = "%s%0.1f"},
                if (fieldInfo.Name == "EngineTemp") parameter.FloatFormat = "F1"; //&ecu_parameters.EngineTemp, .title = "%s%0.1f"},
                if (fieldInfo.Name == "CalculatedAirTemp") parameter.FloatFormat = "F1"; //&ecu_parameters.CalculatedAirTemp, .title = "%s%0.1f"},
                if (fieldInfo.Name == "Pressure") parameter.FloatFormat = "F0"; //&ecu_parameters.ManifoldAirPressure, .title = "%s%0.0f"},
                if (fieldInfo.Name == "ThrottlePosition") parameter.FloatFormat = "F1"; //&ecu_parameters.ThrottlePosition, .title = "%s%0.1f"},
                if (fieldInfo.Name == "ReferenceVoltage") parameter.FloatFormat = "F2"; //&ecu_parameters.ReferenceVoltage, .title = "%s%0.2f"},
                if (fieldInfo.Name == "PowerVoltage") parameter.FloatFormat = "F2"; //&ecu_parameters.PowerVoltage, .title = "%s%0.2f"},
                if (fieldInfo.Name == "FuelRatio") parameter.FloatFormat = "F2"; //&ecu_parameters.FuelRatio, .title = "%s%0.2f"},
                if (fieldInfo.Name == "FuelRatioDiff") parameter.FloatFormat = "F3"; //&ecu_parameters.FuelRatioDiff, .title = "%s%0.3f"},
                if (fieldInfo.Name == "LambdaValue") parameter.FloatFormat = "F2"; //&ecu_parameters.LambdaValue, .title = "%s%0.2f"},
                if (fieldInfo.Name == "LambdaTemperature") parameter.FloatFormat = "F0"; //&ecu_parameters.LambdaTemperature, .title = "%s%0.0f"},
                if (fieldInfo.Name == "LambdaHeaterVoltage") parameter.FloatFormat = "F2"; //&ecu_parameters.LambdaHeaterVoltage, .title = "%s%0.2f"},
                if (fieldInfo.Name == "LambdaTemperatureVoltage") parameter.FloatFormat = "F2"; //&ecu_parameters.LambdaTemperatureVoltage, .title = "%s%0.2f"},
                if (fieldInfo.Name == "LongTermCorrection") parameter.FloatFormat = "F3"; //&ecu_parameters.LongTermCorrection, .title = "%s%0.3f"},
                if (fieldInfo.Name == "ShortTermCorrection") parameter.FloatFormat = "F3"; //&ecu_parameters.ShortTermCorrection, .title = "%s%0.3f"},
                if (fieldInfo.Name == "IdleCorrection") parameter.FloatFormat = "F3"; //&ecu_parameters.LongTermCorrection, .title = "%s%0.3f"},
                if (fieldInfo.Name == "RPM") parameter.FloatFormat = "F0"; //&ecu_parameters.RPM, .title = "%s%0.0f"},
                if (fieldInfo.Name == "Speed") parameter.FloatFormat = "F1"; //&ecu_parameters.Speed, .title = "%s%0.1f"},
                if (fieldInfo.Name == "Acceleration") parameter.FloatFormat = "F2"; //&ecu_parameters.Acceleration, .title = "%s%0.2f"},
                if (fieldInfo.Name == "MassAirFlow") parameter.FloatFormat = "F1"; //&ecu_parameters.MassAirFlow, .title = "%s%0.1f"},
                if (fieldInfo.Name == "CyclicAirFlow") parameter.FloatFormat = "F1"; //&ecu_parameters.CyclicAirFlow, .title = "%s%0.1f"},
                if (fieldInfo.Name == "EffectiveVolume") parameter.FloatFormat = "F1"; //&ecu_parameters.EffectiveVolume, .title = "%s%0.1f"},
                if (fieldInfo.Name == "AirDensity") parameter.FloatFormat = "F3"; //&ecu_parameters.AirDensity, .title = "%s%0.3f"},
                if (fieldInfo.Name == "EngineLoad") parameter.FloatFormat = "F1"; //&ecu_parameters.EngineLoad, .title = "%s%0.1f"},
                if (fieldInfo.Name == "EstimatedPower") parameter.FloatFormat = "F1"; //&ecu_parameters.EstimatedPower, .title = "%s%0.1f"},
                if (fieldInfo.Name == "EstimatedTorque") parameter.FloatFormat = "F1"; //&ecu_parameters.EstimatedTorque, .title = "%s%0.1f"},
                if (fieldInfo.Name == "WishFuelRatio") parameter.FloatFormat = "F2"; //&ecu_parameters.WishFuelRatio, .title = "%s%0.2f"},
                if (fieldInfo.Name == "IdleValvePosition") parameter.FloatFormat = "F0"; //&ecu_parameters.IdleValvePosition, .title = "%s%0.0f"},
                if (fieldInfo.Name == "IdleRegThrRPM") parameter.FloatFormat = "F0"; //&ecu_parameters.IdleRegThrRPM, .title = "%s%0.0f"},
                if (fieldInfo.Name == "WishIdleRPM") parameter.FloatFormat = "F0"; //&ecu_parameters.WishIdleRPM, .title = "%s%0.0f"},
                if (fieldInfo.Name == "WishIdleMassAirFlow") parameter.FloatFormat = "F1"; //&ecu_parameters.WishIdleMassAirFlow, .title = "%s%0.1f"},
                if (fieldInfo.Name == "WishIdleValvePosition") parameter.FloatFormat = "F0"; //&ecu_parameters.WishIdleValvePosition, .title = "%s%0.0f"},
                if (fieldInfo.Name == "WishIdleIgnitionAdvance") parameter.FloatFormat = "F0"; //&ecu_parameters.WishIdleIgnitionAdvance, .title = "%s%0.0f"},
                if (fieldInfo.Name == "IgnitionAdvance") parameter.FloatFormat = "F1"; //&ecu_parameters.IgnitionAdvance, .title = "%s%0.0f"},
                if (fieldInfo.Name == "InjectionPhase") parameter.FloatFormat = "F0"; //&ecu_parameters.InjectionPhase, .title = "%s%0.0f"},
                if (fieldInfo.Name == "InjectionPhaseDuration") parameter.FloatFormat = "F0"; //&ecu_parameters.InjectionPhaseDuration, .title = "%s%0.0f"},
                if (fieldInfo.Name == "InjectionPulse") parameter.FloatFormat = "F0"; //&ecu_parameters.InjectionPulse, .title = "%s%0.0f"},
                if (fieldInfo.Name == "InjectionDutyCycle") parameter.FloatFormat = "F2"; //&ecu_parameters.InjectionDutyCycle, .title = "%s%0.2f"},
                if (fieldInfo.Name == "InjectionEnrichment") parameter.FloatFormat = "F3"; //&ecu_parameters.InjectionEnrichment, .title = "%s%0.3f"},
                if (fieldInfo.Name == "IgnitionPulse") parameter.FloatFormat = "F1"; //&ecu_parameters.IgnitionPulse, .title = "%s%0.2f"},
                if (fieldInfo.Name == "IdleSpeedShift") parameter.FloatFormat = "F0"; //&ecu_parameters.IdleSpeedShift, .title = "%s%0.0f"},
                if (fieldInfo.Name == "EnrichmentSyncAmount") parameter.FloatFormat = "F3"; //&ecu_parameters.EnrichmentSyncAmount, .title = "%s%0.3f"},
                if (fieldInfo.Name == "EnrichmentAsyncAmount") parameter.FloatFormat = "F3"; //&ecu_parameters.EnrichmentAsyncAmount, .title = "%s%0.3f"},
                if (fieldInfo.Name == "EnrichmentStartLoad") parameter.FloatFormat = "F1"; //&ecu_parameters.EnrichmentStartLoad, .title = "%s%0.1f"},
                if (fieldInfo.Name == "EnrichmentLoadDerivative") parameter.FloatFormat = "F0"; //&ecu_parameters.EnrichmentLoadDerivative, .title = "%s%00f"},
                if (fieldInfo.Name == "DrivenKilometers") parameter.FloatFormat = "F2"; //&ecu_parameters.DrivenKilometers, .title = "%s%0.1f"},
                if (fieldInfo.Name == "FuelConsumed") parameter.FloatFormat = "F3"; //&ecu_parameters.FuelConsumed, .title = "%s%0.1f"},
                if (fieldInfo.Name == "FuelConsumption") continue;
                if (fieldInfo.Name == "FuelHourly") parameter.FloatFormat = "F1"; //&ecu_parameters.FuelHourly, .title = "%s%0.1f"},
                if (fieldInfo.Name == "TspsRelativePosition") parameter.FloatFormat = "F2"; //&ecu_parameters.TspsRelativePosition, .title = "%s%0.2f"},

                ChartParameters.Add(parameter);
            }

            foreach (FieldInfo fieldInfo in forceFields)
            {
                Parameter parameter = new Parameter(middleLayer);

                parameter.DepFieldInfo = parametersFields.Where(f => f.Name.Equals(fieldInfo.Name)).FirstOrDefault();
                parameter.EnableFieldInfo = forceFields.Where(f => f.Name.Equals("Enable" + fieldInfo.Name)).FirstOrDefault();
                
                parameter.FieldInfo = fieldInfo;
                parameter.Name = fieldInfo.Name;
                parameter.Type = fieldInfo.FieldType;

                if (parameter.Type != typeof(byte))
                {
                    if (parameter.Name == "IgnitionAdvance") { parameter.Min = -15; parameter.Max = 60; parameter.Step = 0.2F; }
                    else if (parameter.Name == "InjectionPhase") { parameter.Min = 0; parameter.Max = 720; parameter.Step = 1.0F; }
                    else if (parameter.Name == "IgnitionOctane") { parameter.Min = -15; parameter.Max = 60; parameter.Step = 0.2F; }
                    else if (parameter.Name == "IgnitionPulse") { parameter.Min = 300; parameter.Max = 10000; parameter.Step = 100F; }
                    else if (parameter.Name == "InjectionPulse") { parameter.Min = 0; parameter.Max = 100000; parameter.Step = 10F; }
                    else if (parameter.Name == "WishFuelRatio") { parameter.Min = 1; parameter.Max = 20; parameter.Step = 0.1F; }
                    else if (parameter.Name == "WishIdleRPM") { parameter.Min = 200; parameter.Max = 5000; parameter.Step = 20.0F; }
                    else if (parameter.Name == "WishIdleValvePosition") { parameter.Min = 0; parameter.Max = Consts.IDLE_VALVE_POS_MAX; parameter.Step = 1.0F; }
                    else if (parameter.Name == "WishIdleIgnitionAdvance") { parameter.Min = -15; parameter.Max = 60; parameter.Step = 0.2F; }
                    else if (parameter.Name == "WishIdleMassAirFlow") { parameter.Min = 0; parameter.Max = 500; parameter.Step = 0.1F; }
                    else if (parameter.Name == "FanRelay") { parameter.Min = 0; parameter.Max = 1; parameter.Step = 1; }
                    else if (parameter.Name == "FanSwitch") { parameter.Min = 0; parameter.Max = 1; parameter.Step = 1; }
                    else if (parameter.Name == "FuelPumpRelay") { parameter.Min = 0; parameter.Max = 1; parameter.Step = 1; }
                    else if (parameter.Name == "CheckEngine") { parameter.Min = 0; parameter.Max = 1; parameter.Step = 1; }
                    else continue;

                    ForceParameters.Add(parameter);
                }
            }
        }
        private void UpdateForceElements(EcuParameters parameters)
        {
            foreach (Parameter parameter in ForceParameters)
            {
                if (parameter.Type == typeof(float))
                {
                    float value = (float)parameter.Value;

                    if ((decimal)value > parameter.NumericUpDown.Maximum)
                        value = (float)parameter.NumericUpDown.Maximum;
                    else if ((decimal)value < parameter.NumericUpDown.Minimum)
                        value = (float)parameter.NumericUpDown.Minimum;
                    parameter.NumericUpDown.Value = (decimal)value;

                    value /= parameter.Step;
                    if (value > parameter.TrackBar.Maximum)
                        value = parameter.TrackBar.Maximum;
                    else if (value < parameter.TrackBar.Minimum)
                        value = parameter.TrackBar.Minimum;
                    parameter.TrackBar.Value = (int)value;
                }
                else
                {
                    int value = (int)parameter.Value;

                    if ((decimal)value > parameter.NumericUpDown.Maximum)
                        value = (int)parameter.NumericUpDown.Maximum;
                    else if ((decimal)value < parameter.NumericUpDown.Minimum)
                        value = (int)parameter.NumericUpDown.Minimum;
                    parameter.NumericUpDown.Value = (decimal)value;

                    value = (int)((float)value / parameter.Step);
                    if (value > parameter.TrackBar.Maximum)
                        value = parameter.TrackBar.Maximum;
                    else if (value < parameter.TrackBar.Minimum)
                        value = parameter.TrackBar.Minimum;
                    parameter.TrackBar.Value = (int)value;
                }

            }
        }

        private void SendUpdatedParameters()
        {
            int updated = 0;
            foreach (Parameter parameter in ForceParameters)
            {
                if (parameter.Enabled)
                {
                    if (parameter.Type == typeof(float))
                    {
                        float value = (float)parameter.NumericUpDown.Value;
                        parameter.Value = value;
                        if (parameter.ValueOld == null || (float)parameter.ValueOld != value)
                        {
                            parameter.ValueOld = value;
                            updated++;
                        }
                    }
                    else
                    {
                        int value = (int)parameter.NumericUpDown.Value;
                        parameter.Value = value;
                        if (parameter.ValueOld == null || (int)parameter.ValueOld != value)
                        {
                            parameter.ValueOld = value;
                            updated++;
                        }
                    }
                }
            }
            if(middleLayer != null && updated > 0)
            {
                middleLayer.UpdateForceParameters();
            }
        }

        private void UpdateChartsSetup()
        {
            this.SuspendLayout();
            tlpCharts.Controls.Clear();
            tlpCharts.RowStyles.Clear();
            tlpCharts.ColumnStyles.Clear();
            lblValues.Clear();
            chartValues.Clear();

            foreach (Parameter obj in lbParamsUsed.Items)
            {
                TableLayoutPanel tlp = new TableLayoutPanel();
                Label labelValue = new Label();
                Label labelTitle = new Label();
                Chart chart = new Chart();
                object tag = obj;

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
                labelValue.Size = new Size(200, 100);
                labelValue.Tag = tag;

                chart.Dock = DockStyle.Fill;
                chart.Margin = new Padding(0, 0, 0, 0);
                chart.Padding = new Padding(0, 0, 0, 0);
                chart.BackColor = Color.Transparent;
                chart.BackGradientStyle = GradientStyle.HorizontalCenter;
                chart.BackSecondaryColor = Color.FromArgb(24, 64, 0);
                chart.Text = "chart" + obj.ToString();
                chart.Tag = tag;
                chart.MouseMove += chart_MouseMove;

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

        Point? chart_point_prev;

        private void chart_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (!cbLiveView.Checked)
                {
                    Chart chart = ((Chart)sender);

                    if (chart_point_prev.HasValue && e.X == chart_point_prev.Value.X && e.Y == chart_point_prev.Value.Y)
                        return;

                    chart_point_prev = new Point(e.X, e.Y);

                    double time_nrst = chart.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
                    dataMutex.WaitOne();
                    PointData closest = dataPoints.OrderBy(p => Math.Abs(p.Seconds - time_nrst)).FirstOrDefault();
                    dataMutex.ReleaseMutex();
                    if (closest != null)
                    {
                        Parameter parameter = chart.Tag as Parameter;

                        this.SuspendLayout();
                        foreach (Label label in lblValues)
                        {
                            if (label.Tag != null)
                            {
                                parameter = (Parameter)label.Tag;
                                if (parameter.Type == typeof(float))
                                {
                                    float valuef = (float)parameter.FieldInfo.GetValue(closest.Parameters);
                                    label.Text = valuef.ToString(ChartParameters.Where(p => p.FieldInfo == parameter.FieldInfo).First().FloatFormat);
                                }
                                else if(parameter.Type == typeof(int))
                                {
                                    label.Text = parameter.FieldInfo.GetValue(closest.Parameters).ToString();
                                }
                            }
                        }
                        this.ResumeLayout(true);
                    }
                }
            }
            catch
            {

            }
            
        }

        private void UpdateCharts()
        {
            Parameter parameter;
            float valuef;
            int valuei;

            dataMutex.WaitOne();
            if (dataPoints.Count > 0)
            {
                PointData current = dataPoints.LastOrDefault();
                dataMutex.ReleaseMutex();
                if (current != null && cbLiveView.Checked)
                {
                    foreach (Label label in lblValues)
                    {
                        if (label.Tag != null)
                        {
                            parameter = (Parameter)label.Tag;
                            if (parameter.Type == typeof(float))
                            {
                                valuef = (float)parameter.FieldInfo.GetValue(current.Parameters);
                                label.Text = valuef.ToString(ChartParameters.Where(p => p.FieldInfo == parameter.FieldInfo).First().FloatFormat);
                            }
                            else
                            {
                                label.Text = parameter.FieldInfo.GetValue(current.Parameters).ToString();
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
                dataMutex.WaitOne();
                double[] seconds = dataPoints.Select(p => p.Seconds).ToArray();

                if (dTimeFrom <= dataPoints.First().Seconds)
                {
                    index_first = 0;
                }
                else
                {
                    index_first = BinarySearch<double>.Find(seconds, 0, dataPoints.Count - 1, dTimeFrom);
                    if (index_first < 0)
                        index_first = 0;
                }

                if (dTimeFrom >= dataPoints.Last().Seconds)
                {
                    index_last = 0;
                }
                else
                {
                    index_last = BinarySearch<double>.Find(seconds, 0, dataPoints.Count - 1, dTimeTo);
                    if (index_last < 0)
                        index_last = index_first;
                    else if(index_last + 1 < seconds.Count())
                        index_last++;

                }
                dataMutex.ReleaseMutex();

                if (middleLayer == null)
                {
                    hScrollBar1.Minimum = 0;
                    hScrollBar1.Maximum = (int)(current.Seconds * 1000);
                }
                else
                {
                    hScrollBar1.Minimum = 0;
                    hScrollBar1.Maximum = (int)stopwatch.ElapsedMilliseconds;
                }
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

                    IEnumerable<DataPoint> toberemoved_min = chart.Series[0].Points.Where(p => p.XValue < chart.ChartAreas[0].AxisX.Minimum).ToArray();
                    foreach (DataPoint point in toberemoved_min)
                        chart.Series[0].Points.Remove(point);

                    IEnumerable<DataPoint> toberemoved_max = chart.Series[0].Points.Where(p => p.XValue > chart.ChartAreas[0].AxisX.Maximum).ToArray();
                    foreach (DataPoint point in toberemoved_max)
                        chart.Series[0].Points.Remove(point);

                    for (int i = index_first; i <= index_last; i++)
                    {
                        if (chart.Series[0].Points.Count == 0 || chart.Series[0].Points.Last().XValue < dataPoints[i].Seconds)
                        {
                            if (chart.Tag != null)
                            {
                                parameter = (Parameter)chart.Tag;

                                if (parameter.Type == typeof(float))
                                {
                                    valuef = (float)parameter.FieldInfo.GetValue(dataPoints[i].Parameters);
                                    chart.Series[0].Points.AddXY(dataPoints[i].Seconds, valuef);
                                }
                                else if(parameter.Type == typeof(int))
                                {
                                    valuei = (int)parameter.FieldInfo.GetValue(dataPoints[i].Parameters);
                                    chart.Series[0].Points.AddXY(dataPoints[i].Seconds, valuei);
                                }
                            }
                        }
                    }

                    for (int i = index_last; i >= index_first; i--)
                    {
                        if (chart.Series[0].Points.First().XValue > dataPoints[i].Seconds)
                        {
                            if (chart.Tag != null)
                            {
                                parameter = (Parameter)chart.Tag;

                                if (parameter.Type == typeof(float))
                                {
                                    valuef = (float)parameter.FieldInfo.GetValue(dataPoints[i].Parameters);
                                    chart.Series[0].Points.InsertXY(0, dataPoints[i].Seconds, valuef);
                                }
                                else
                                {
                                    valuei = (int)parameter.FieldInfo.GetValue(dataPoints[i].Parameters);
                                    chart.Series[0].Points.InsertXY(0, dataPoints[i].Seconds, valuei);
                                }
                            }
                        }
                    }

                    double min = 0;
                    double max = 0;
                    int count = 0;
                    if (chart.Series[0].Points.Count > 0)
                    {
                        IEnumerable<DataPoint> sequence = chart.Series[0].Points.Where(p => p.XValue > posmin && p.XValue < posmax);
                        count = sequence.Count();
                        if (sequence.Count() > 0)
                        {
                            min = Math.Floor(sequence.Select(p => p.YValues[0]).Min());
                            max = Math.Ceiling(sequence.Select(p => p.YValues[0]).Max());
                        }
                    }

                    if (min == max)
                    {
                        min -= 0.5D;
                        max += 0.5D;
                    }
                    
                    chart.ChartAreas[0].AxisX.Interval = TimeScaleTime[(int)TimeScale] / 10.0D;
                    chart.ChartAreas[0].AxisY.Minimum = min;
                    chart.ChartAreas[0].AxisY.Maximum = max;

                    if (count > chart.Width / 34)
                        chart.Series[0].ChartType = SeriesChartType.FastLine;
                    else chart.Series[0].ChartType = SeriesChartType.Line;
                    chart.ResumeLayout();
                }
            }
            else
            {
                dataMutex.ReleaseMutex();
            }
        }

        public void UpdateParametersEvent(EcuParameters parameters)
        {
            dataMutex.WaitOne();
            dataPoints.Add(new PointData { Parameters = parameters, Seconds = stopwatch.ElapsedMilliseconds * 0.001D });
            dataMutex.ReleaseMutex();
            Action action = new Action(() => { this.UpdateParameters(parameters); });
            if (this.InvokeRequired)
                this.BeginInvoke(action);
            else action.Invoke();
        }

        public void UpdateStatusEvent(IEnumerable<CheckDataItem> checkDataList)
        {

        }

        public void DragStartAckEvent(PK_DragStartAcknowledge dsaa)
        {
        }

        public void SynchronizedEvent(int errorCode, bool fast)
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
            UpdateForceElements(parameters);
        }

        private void DiagForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //TODO: do it synchroniously
            if (middleLayer != null)
            {
                foreach (Parameter parameter in ForceParameters)
                {
                    parameter.Enabled = false;
                }
                middleLayer.UpdateForceParameters();
            }
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
            int index = lbParamsUsed.SelectedIndex;
            Parameter selected = (Parameter)lbParamsAvailable.SelectedItem;
            if (index >= 0)
            {
                lbParamsUsed.Items.RemoveAt(index);
                lbParamsAvailable.Items.Clear();
                
                foreach (Parameter param in ChartParameters)
                {
                    lbParamsAvailable.Items.Add(param);
                }
                foreach (Parameter item in lbParamsUsed.Items)
                {
                    lbParamsAvailable.Items.Remove(item);
                }
                if(selected != null && !string.IsNullOrWhiteSpace(selected.Name))
                {
                    lbParamsAvailable.SelectedItem = selected;
                }
                UpdateChartsSetup();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int index = lbParamsAvailable.SelectedIndex;
            Parameter item = (Parameter)lbParamsAvailable.SelectedItem;
            if (index >= 0)
            {
                lbParamsAvailable.Items.RemoveAt(index);
                lbParamsUsed.Items.Add(item);
                UpdateChartsSetup();
            }
        }

        private void btnUsedMoveUp_Click(object sender, EventArgs e)
        {
            int index = lbParamsUsed.SelectedIndex;
            Parameter item = (Parameter)lbParamsUsed.SelectedItem;
            if (index > 0)
            {
                lbParamsUsed.Items.RemoveAt(index);
                lbParamsUsed.Items.Insert(index - 1, item);
                lbParamsUsed.SelectedIndex = index - 1;
                UpdateChartsSetup();
            }
        }

        private void btnUsedMoveDown_Click(object sender, EventArgs e)
        {
            int index = lbParamsUsed.SelectedIndex;
            Parameter item = (Parameter)lbParamsUsed.SelectedItem;
            if (index >= 0 && index < lbParamsUsed.Items.Count - 1)
            {
                lbParamsUsed.Items.RemoveAt(index);
                lbParamsUsed.Items.Insert(index + 1, item);
                lbParamsUsed.SelectedIndex = index + 1;
                UpdateChartsSetup();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateCharts();
            if (middleLayer != null)
            {
                SendUpdatedParameters();
            }
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

        private void gpForceTemplate_Enter(object sender, EventArgs e)
        {

        }

        private void btnExportLog_Click(object sender, EventArgs e)
        {
            DialogResult result = sfdExportLog.ShowDialog();
            if(result == DialogResult.OK)
            {
                Task task = new Task(new Action(() =>
                {
                    try
                    {
                        dataMutex.WaitOne();
                        EcuLog.SaveLog(sfdExportLog.FileName, dataPoints);
                        dataMutex.ReleaseMutex();
                        this.Invoke(new Action(() => MessageBox.Show(this, "Log exported successfully", "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Information)));
                    }
                    catch (Exception ex)
                    {
                        this.Invoke(new Action(() => MessageBox.Show(this, "Failed to export log.\r\n" + ex.Message, "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Error)));
                    }
                }));

                task.Start();
            }
        }

        private void btnSaveParams_Click(object sender, EventArgs e)
        {
            DialogResult result = sfdSaveParams.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    List<string> parametersUsed = new List<string>();
                    foreach (Parameter parameter in lbParamsUsed.Items)
                        parametersUsed.Add(parameter.Name);

                    Serializator<string[]>.Serialize(sfdSaveParams.FileName, parametersUsed.ToArray());
                    MessageBox.Show(this, "Log params saved successfully", "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Failed to save log params.\r\n" + ex.Message, "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } 
        }

        private void btnImportParams_Click(object sender, EventArgs e)
        {
            DialogResult result = ofdImportParams.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    string[] parametersUsed = Serializator<string[]>.Deserialize(ofdImportParams.FileName);

                    lbParamsAvailable.Items.Clear();
                    lbParamsUsed.Items.Clear();

                    foreach (Parameter parameter in ChartParameters)
                    {
                        lbParamsAvailable.Items.Add(parameter);
                    }

                    foreach (string parameter in parametersUsed)
                    {
                        object param = ChartParameters.Where(p => p.Name == parameter).FirstOrDefault();
                        if (param != null)
                            lbParamsUsed.Items.Add(param);
                    }

                    foreach (Parameter item in lbParamsUsed.Items)
                    {
                        lbParamsAvailable.Items.Remove(item);
                    }
                    UpdateChartsSetup();


                    MessageBox.Show(this, "Log params import successful", "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Failed to import log params.\r\n" + ex.Message, "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
 