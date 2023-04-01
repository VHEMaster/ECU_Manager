using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ECU_Manager.Controls;
using ECU_Manager.Packets;
using ECU_Manager.Structs;
using ECU_Manager.Tools;

namespace ECU_Manager
{
    public partial class MainForm : Form, IEcuEventHandler
    {
        FileInfo standaloneFileInfo;
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

        bool bLiveCheckOld = false;
        DragRun drCurrentRun = null;
        List<DragRun> lDragRuns = new List<DragRun>();
        DragStatusType eDragStatus = DragStatusType.Ready;
        float fDragFromSpeed = 0;
        float fDragToSpeed = 100;
        EcuParameters gParameters;

        private static Image MakeImageColored(Image bitmap, Color color)
        {
            Bitmap pic = new Bitmap(bitmap);
            for (int y = 0; (y <= (pic.Height - 1)); y++)
            {
                for (int x = 0; (x <= (pic.Width - 1)); x++)
                {
                    Color inv = pic.GetPixel(x, y);
                    inv = Color.FromArgb(inv.A, color.R, color.G, color.B);
                    pic.SetPixel(x, y, inv);
                }
            }
            return pic;
        }

        public MainForm(FileInfo fileInfo)
        {
            this.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            InitializeComponent();

            this.middleLayer = null;
            this.cs = new ComponentStructure();
            this.cs.ConfigStruct = Serializator<ConfigStruct>.Deserialize(fileInfo.FullName);

            cbLive.Enabled = false;
            cbLive.Checked = false;
            cbLive.Visible = false;

            tableLayoutPanel13.Enabled = false;
            btnCorrStart.Enabled = false;
            btnCorrStop.Enabled = false;
            btnResetFailures.Enabled = false;

            standaloneFileInfo = fileInfo;

            btnRestore.Text = "Reload File";
            btnSave.Text = "Save File";
            btnRedownload.Enabled = false;
            btnIITestRun.Enabled = false;
            groupBox8.Enabled = false;

            Initialize();

            SynchronizedEventInternal(0, false);
        }

        public MainForm(MiddleLayer middleLayer)
        {
            this.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);
            InitializeComponent();

            this.middleLayer = middleLayer;
            this.cs = middleLayer.ComponentStructure;


            this.middleLayer.RegisterEventHandler(this);
            this.middleLayer.SyncLoad(false);

            this.syncForm = new SyncForm();
            this.syncForm.ShowDialog();

            this.pbCheckEngine.Image = MakeImageColored(pbCheckEngine.Image, Color.FromArgb(255, 255, 0));

            Initialize();

        }

        private void Initialize()
        {
            ColorTransience colorTransience;

            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            eCyclicFilling.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].pressures_count,
                0.0D, 2.0D, 0.001D, 100.0D, 0.2D, 0.6D, 1.2D, 500, 0.1D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_PRESSURES_MAX, 3);

            eCyclicFilling.SetConfig("fill_by_map", "rotates_count", "pressures_count", "rotates", "pressures");
            eCyclicFilling.SetX("RPM", "RPM", "F0");
            eCyclicFilling.SetY("RelativeFilling", "Fill", "F2");
            eCyclicFilling.SetD("ManifoldAirPressure", "MAP", "F0");
            eCyclicFilling.SetTableEventHandler(ChartUpdateEvent);

            colorTransience = new ColorTransience(0.5F, 1.5F, Color.Gray);
            colorTransience.Add(Color.FromArgb(0, 128, 255), 0.5F);
            colorTransience.Add(Color.Blue, 0.7F);
            colorTransience.Add(Color.FromArgb(0, 92, 160), 0.75F);
            colorTransience.Add(Color.Green, 1.0F);
            colorTransience.Add(Color.FromArgb(128, 96, 0), 1.05F);
            colorTransience.Add(Color.DarkRed, 1.12F);
            colorTransience.Add(Color.Black, 1.5F);

            eCyclicFilling.SetTableColorTrans(colorTransience);
            eCyclicFilling.SynchronizeChart();
            
            colorTransience = new ColorTransience(5.0F, 20.0F, Color.Gray);
            colorTransience.Add(Color.Black, 5.0F);
            colorTransience.Add(Color.Red, 12.0F);
            colorTransience.Add(Color.FromArgb(128, 128, 0), 13.7F);
            colorTransience.Add(Color.Green, 14.7F);
            colorTransience.Add(Color.FromArgb(0, 72, 180), 15.6F);
            colorTransience.Add(Color.Blue, 18.0F);
            colorTransience.Add(Color.DeepSkyBlue, 20.0F);

            eFuelMixturesPart.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                1.0D, 20.0D, 0.1D, 100.0D, 0.5D, 12.0D, 15.0D, 500, 0.5D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 1, true);

            eFuelMixturesPart.SetConfig("fuel_mixtures_part", "rotates_count", "fillings_count", "rotates", "fillings");
            eFuelMixturesPart.SetX("RPM", "RPM", "F0");
            eFuelMixturesPart.SetY("WishFuelRatio", "FuelRatio", "F1");
            eFuelMixturesPart.SetD("CyclicAirFlow", "Filling", "F1");
            eFuelMixturesPart.SetTableEventHandler(ChartUpdateEvent);

            eFuelMixturesPart.SetTableColorTrans(colorTransience);

            eFuelMixturesFull.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                1.0D, 20.0D, 0.1D, 100.0D, 0.5D, 12.0D, 15.0D, 500, 0.5D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 1, true);

            eFuelMixturesFull.SetConfig("fuel_mixtures_full", "rotates_count", "fillings_count", "rotates", "fillings");
            eFuelMixturesFull.SetX("RPM", "RPM", "F0");
            eFuelMixturesFull.SetY("WishFuelRatio", "FuelRatio", "F1");
            eFuelMixturesFull.SetD("CyclicAirFlow", "Filling", "F1");
            eFuelMixturesFull.SetTableEventHandler(ChartUpdateEvent);

            eFuelMixturesFull.SetTableColorTrans(colorTransience);

            colorTransience = new ColorTransience(100.0F, 600, Color.Gray);
            colorTransience.Add(Color.DeepSkyBlue, 100);
            colorTransience.Add(Color.Blue, 170);
            colorTransience.Add(Color.FromArgb(0, 72, 180), 200);
            colorTransience.Add(Color.Green, 250);
            colorTransience.Add(Color.FromArgb(128, 128, 0), 280);
            colorTransience.Add(Color.Red, 350);
            colorTransience.Add(Color.Black, 600);

            eInjectionPhasePart.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                0.0D, 720.0, 5.0D, 100.0D, 10D, 100.0D, 400.0D, 500, 50D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 0);

            eInjectionPhasePart.SetConfig("injection_phase_part", "rotates_count", "fillings_count", "rotates", "fillings");
            eInjectionPhasePart.SetX("RPM", "RPM", "F0");
            eInjectionPhasePart.SetY("InjectionPhase", "Phase", "F0");
            eInjectionPhasePart.SetD("CyclicAirFlow", "Filling", "F1");
            eInjectionPhasePart.SetTableEventHandler(ChartUpdateEvent);

            eInjectionPhasePart.SetTableColorTrans(colorTransience);

            eInjectionPhaseFull.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                0.0D, 720.0, 5.0D, 100.0D, 10D, 100.0D, 400.0D, 500, 50D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 0);

            eInjectionPhaseFull.SetConfig("injection_phase_full", "rotates_count", "fillings_count", "rotates", "fillings");
            eInjectionPhaseFull.SetX("RPM", "RPM", "F0");
            eInjectionPhaseFull.SetY("InjectionPhase", "Phase", "F0");
            eInjectionPhaseFull.SetD("CyclicAirFlow", "Filling", "F1");
            eInjectionPhaseFull.SetTableEventHandler(ChartUpdateEvent);

            eInjectionPhaseFull.SetTableColorTrans(colorTransience);

            colorTransience = new ColorTransience(-15, 45, Color.Gray);
            colorTransience.Add(Color.DeepSkyBlue, -15);
            colorTransience.Add(Color.Orange, 15);
            colorTransience.Add(Color.Red, 45);

            eIgnitionPart.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                -45, 90, 0.1D, 100.0D, 0.5D, 0.0D, 45.0D, 500, 5, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 1);

            eIgnitionPart.SetConfig("ignitions_part", "rotates_count", "fillings_count", "rotates", "fillings");
            eIgnitionPart.SetX("RPM", "RPM", "F0");
            eIgnitionPart.SetY("IgnitionAngle", "Ignition", "F1");
            eIgnitionPart.SetD("CyclicAirFlow", "Filling", "F1");
            eIgnitionPart.SetTableEventHandler(ChartUpdateEvent);

            eIgnitionPart.SetTableColorTrans(colorTransience);

            eIgnitionFull.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                -45, 90, 0.1D, 100.0D, 0.5D, 0.0D, 45.0D, 500, 5, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 1);

            eIgnitionFull.SetConfig("ignitions_full", "rotates_count", "fillings_count", "rotates", "fillings");
            eIgnitionFull.SetX("RPM", "RPM", "F0");
            eIgnitionFull.SetY("IgnitionAngle", "Ignition", "F1");
            eIgnitionFull.SetD("CyclicAirFlow", "Filling", "F1");
            eIgnitionFull.SetTableEventHandler(ChartUpdateEvent);

            eIgnitionFull.SetTableColorTrans(colorTransience);

            eSaturationPulse.Initialize(cs, 0, 10000, 100, 500, 0, 5000, 1, 500, 0);
            eSaturationPulse.SetConfig("ignition_time", "voltages_count", "voltages");
            eSaturationPulse.SetX("PowerVoltage", "Voltage", "F1");
            eSaturationPulse.SetY("IgnitionPulse", "Pulse", "F0");
            eSaturationPulse.SetTableEventHandler(ChartUpdateEvent);

            eSatByRPM.Initialize(cs, 0.1D, 10D, 0.1D, 0.2D, 0.2D, 2.0F, 500, 0.2D, 1);
            eSatByRPM.SetConfig("ignition_time_rpm_mult", "rotates_count", "rotates");
            eSatByRPM.SetX("RPM", "RPM", "F0");
            eSatByRPM.SetTableEventHandler(ChartUpdateEvent);

            eInjectorLag.Initialize(cs, 0, 10, 0.01D, 0.1D, 0, 2, 1, 0.2, 2);
            eInjectorLag.SetConfig("injector_lag", "voltages_count", "voltages");
            eInjectorLag.SetX("PowerVoltage", "Voltage", "F1");
            eInjectorLag.SetY("InjectionLag", "Lag", "F2");
            eInjectorLag.SetTableEventHandler(ChartUpdateEvent);

            eInjectionPhaseLPF.Initialize(cs, 0, 1, 0.001D, 0.1D, 0D, 0.5F, 500, 0.05D, 3);
            eInjectionPhaseLPF.SetConfig("injection_phase_lpf", "rotates_count", "rotates");
            eInjectionPhaseLPF.SetX("RPM", "RPM", "F0");
            eInjectionPhaseLPF.SetTableEventHandler(ChartUpdateEvent);

            eEnrichmentByMAP.Initialize(cs, -1, 5, 0.001D, 0.1D, 0D, 1.0F, 5000, 0.1D, 3);
            eEnrichmentByMAP.SetConfig("enrichment_by_map_sens", "pressures_count", "pressures");
            eEnrichmentByMAP.SetX("ManifoldAirPressure", "MAP", "F0");
            eEnrichmentByMAP.SetY("InjectionEnrichment", "Enr.", "F3");
            eEnrichmentByMAP.SetTableEventHandler(ChartUpdateEvent);

            eEnrichmentByTPS.Initialize(cs, -1, 5, 0.001D, 0.1D, 0D, 1.0F, 5, 0.1D, 3);
            eEnrichmentByTPS.SetConfig("enrichment_by_thr_sens", "throttles_count", "throttles");
            eEnrichmentByTPS.SetX("ThrottlePosition", "TPS", "F0");
            eEnrichmentByTPS.SetY("InjectionEnrichment", "Enr.", "F3");
            eEnrichmentByTPS.SetTableEventHandler(ChartUpdateEvent);

            eEnrichmentMAPHPF.Initialize(cs, 0, 1, 0.001D, 0.1D, 0D, 0.5F, 500, 0.05D, 3);
            eEnrichmentMAPHPF.SetConfig("enrichment_by_map_hpf", "rotates_count", "rotates");
            eEnrichmentMAPHPF.SetX("RPM", "RPM", "F0");
            eEnrichmentMAPHPF.SetTableEventHandler(ChartUpdateEvent);

            eEnrichmentTPSHPF.Initialize(cs, 0, 1, 0.001D, 0.1D, 0D, 0.5F, 500, 0.05D, 3);
            eEnrichmentTPSHPF.SetConfig("enrichment_by_thr_hpf", "rotates_count", "rotates");
            eEnrichmentTPSHPF.SetX("RPM", "RPM", "F0");
            eEnrichmentTPSHPF.SetTableEventHandler(ChartUpdateEvent);
            
            eEnrichmentTempMult.Initialize(cs, 0, 5, 0.01D, 0.1D, 0D, 0.5F, 10D, 0.05D, 2);
            eEnrichmentTempMult.SetConfig("enrichment_temp_mult", "engine_temp_count", "engine_temps");
            eEnrichmentTempMult.SetX("EngineTemp", "Temperature", "F1");
            eEnrichmentTempMult.SetTableEventHandler(ChartUpdateEvent);

            ePressures.Initialize(cs, 0, 1000000, 200, 500, 0, 100000, 1, 10000, 0);
            ePressures.SetConfig("pressures", "pressures_count", string.Empty);
            ePressures.SetY("ManifoldAirPressure", "MAP", "F0");
            ePressures.SetTableEventHandler(ChartUpdateEvent);

            eRotates.Initialize(cs, 0, 10000, 50, 100, 0, 8000, 1, 500, 0);
            eRotates.SetConfig("rotates", "rotates_count", string.Empty);
            eRotates.SetY("RPM", "RPM", "F0");
            eRotates.SetTableEventHandler(ChartUpdateEvent);

            eIdleRotates.Initialize(cs, 0, 10000, 50, 100, 0, 8000, 1, 500, 0);
            eIdleRotates.SetConfig("idle_rotates", "idle_rotates_count", string.Empty);
            eIdleRotates.SetY("RPM", "RPM", "F0");
            eIdleRotates.SetTableEventHandler(ChartUpdateEvent);

            eThrottles.Initialize(cs, 0, 100, 1, 1, 0, 100, 1, 10, 1);
            eThrottles.SetConfig("throttles", "throttles_count", string.Empty);
            eThrottles.SetY("ThrottlePosition", "TPS", "F1");
            eThrottles.SetTableEventHandler(ChartUpdateEvent);

            eFillings.Initialize(cs, 0, 5000, 1, 2, 0, 350, 1, 50, 0);
            eFillings.SetConfig("fillings", "fillings_count", string.Empty);
            eFillings.SetY("CyclicAirFlow", "Filling", "F1");
            eFillings.SetTableEventHandler(ChartUpdateEvent);

            eVoltages.Initialize(cs, 0, 25, 0.1D, 1, 0, 15, 1, 2, 1);
            eVoltages.SetConfig("voltages", "voltages_count", string.Empty);
            eVoltages.SetY("PowerVoltage", "Voltage", "F1");
            eVoltages.SetTableEventHandler(ChartUpdateEvent);

            eEngTemps.Initialize(cs, -50, 150, 1, 1, 0, 100, 1, 10, 0);
            eEngTemps.SetConfig("engine_temps", "engine_temp_count", string.Empty);
            eEngTemps.SetY("EngineTemp", "Temperature", "F1");
            eEngTemps.SetTableEventHandler(ChartUpdateEvent);

            eAirTemps.Initialize(cs, -50, 150, 1, 1, 0, 100, 1, 10, 0);
            eAirTemps.SetConfig("air_temps", "air_temp_count", string.Empty);
            eAirTemps.SetY("AirTemp", "Temperature", "F1");
            eAirTemps.SetTableEventHandler(ChartUpdateEvent);

            eSpeeds.Initialize(cs, 0, 990, 5, 1, 0, 100, 1, 10, 0);
            eSpeeds.SetConfig("idle_rpm_shift_speeds", "idle_speeds_shift_count", string.Empty);
            eSpeeds.SetY("Speed", "Speed", "F1");
            eSpeeds.SetTableEventHandler(ChartUpdateEvent);


            ePressureByRPMvsTPS.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].throttles_count,
                0, 1000000, 1000, 100.0D, 1D, 0.0D, 100000.0D, 500, 10000D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_THROTTLES_MAX, 0);


            ePressureByRPMvsTPS.SetConfig("map_by_thr", "rotates_count", "throttles_count", "rotates", "throttles");
            ePressureByRPMvsTPS.SetX("RPM", "RPM", "F0");
            ePressureByRPMvsTPS.SetY("ManifoldAirPressure", "Pressure", "F0");
            ePressureByRPMvsTPS.SetD("ThrottlePosition", "TPS", "F1");
            ePressureByRPMvsTPS.SetTableEventHandler(ChartUpdateEvent);

            colorTransience = new ColorTransience(0, 1000000, Color.Gray);
            colorTransience.Add(Color.DeepSkyBlue, 0);
            colorTransience.Add(Color.Blue, 20000);
            colorTransience.Add(Color.FromArgb(0, 72, 180), 40000);
            colorTransience.Add(Color.Green, 60000);
            colorTransience.Add(Color.FromArgb(128, 128, 0), 80000);
            colorTransience.Add(Color.Red, 100000);
            colorTransience.Add(Color.DarkRed, 200000);
            colorTransience.Add(Color.FromArgb(64, 20, 0), 300000);
            colorTransience.Add(Color.Black, 1000000);

            ePressureByRPMvsTPS.SetTableColorTrans(colorTransience);

            eIdleWishRPM.Initialize(cs, 100D, 10000D, 20D, 100D, 500D, 2000D, 10D, 100D, 0);
            eIdleWishRPM.SetConfig("idle_wish_rotates", "engine_temp_count", "engine_temps");
            eIdleWishRPM.SetX("EngineTemp", "Temp.", "F1");
            eIdleWishRPM.SetY("RPM", "RPM", "F0");
            eIdleWishRPM.SetTableEventHandler(ChartUpdateEvent);

            eIdleWishMassAirFlow.Initialize(cs, 0, 500D, 0.1D, 5D, 0, 30D, 10D, 5D, 1);
            eIdleWishMassAirFlow.SetConfig("idle_wish_massair", "engine_temp_count", "engine_temps");
            eIdleWishMassAirFlow.SetX("EngineTemp", "Temp.", "F1");
            eIdleWishMassAirFlow.SetY("MassAirFlow", "Mass Air Flow", "F1");
            eIdleWishMassAirFlow.SetTableEventHandler(ChartUpdateEvent);

            eIdleWishIgnition.Initialize(cs, -15D, 60D, 0.1D, 5D, 10D, 20D, 10D, 2D, 1);
            eIdleWishIgnition.SetConfig("idle_wish_ignition", "engine_temp_count", "engine_temps");
            eIdleWishIgnition.SetX("EngineTemp", "Temp", "F1");
            eIdleWishIgnition.SetY("IgnitionAngle", "Ignition", "F1");
            eIdleWishIgnition.SetTableEventHandler(ChartUpdateEvent);

            eIdleIgnitionStatic.Initialize(cs, -15D, 60D, 0.1D, 5D, 10D, 20D, 100D, 2D, 1);
            eIdleIgnitionStatic.SetConfig("idle_wish_ignition_static", "idle_rotates_count", "idle_rotates");
            eIdleIgnitionStatic.SetX("RPM", "RPM", "F0");
            eIdleIgnitionStatic.SetY("IgnitionAngle", "Ignition", "F1");
            eIdleIgnitionStatic.SetTableEventHandler(ChartUpdateEvent);

            eIdleSpeedShift.Initialize(cs, 0, 2000, 20D, 10D, 0, 100, 10D, 20D, 0);
            eIdleSpeedShift.SetConfig("idle_rpm_shift", "idle_speeds_shift_count", "idle_rpm_shift_speeds");
            eIdleSpeedShift.SetX("Speed", "Speed", "F1");
            eIdleSpeedShift.SetY("IdleSpeedShift", "RPM Shift", "F0");
            eIdleSpeedShift.SetTableEventHandler(ChartUpdateEvent);


            eIdleValveVsRpm.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].engine_temp_count,
                0, Consts.IDLE_VALVE_POS_MAX, 1, 100.0D, 1D, 0.0D, 50D, 500, 5, Consts.TABLE_ROTATES_MAX, Consts.TABLE_TEMPERATURES_MAX, 0);


            eIdleValveVsRpm.SetConfig("idle_valve_to_rpm", "rotates_count", "engine_temp_count", "rotates", "engine_temps");
            eIdleValveVsRpm.SetX("RPM", "RPM", "F0");
            eIdleValveVsRpm.SetY("WishIdleValvePosition", "Valve", "F0");
            eIdleValveVsRpm.SetD("EngineTemp", "Temp.", "F1");
            eIdleValveVsRpm.SetTableEventHandler(ChartUpdateEvent);

            colorTransience = new ColorTransience(0, 255, Color.Gray);
            colorTransience.Add(Color.DeepSkyBlue, 0);
            colorTransience.Add(Color.Green, 50);
            colorTransience.Add(Color.Orange, 100);
            colorTransience.Add(Color.Red, 255);

            eIdleValveVsRpm.SetTableColorTrans(colorTransience);

            eWarmupMixture.Initialize(cs, 1, 20, 0.1D, 1D, 8, 14, 10D, 0.5D, 1);
            eWarmupMixture.SetConfig("warmup_mixtures", "engine_temp_count", "engine_temps");
            eWarmupMixture.SetX("EngineTemp", "Temp.", "F1");
            eWarmupMixture.SetY("WishFuelRatio", "Fuel Ratio", "F1");
            eWarmupMixture.SetTableEventHandler(ChartUpdateEvent);

            eWarmupMixKoffs.Initialize(cs, 0, 1, 0.01D, 0.1D, 0, 1, 10D, 0.1D, 2);
            eWarmupMixKoffs.SetConfig("warmup_mix_koffs", "engine_temp_count", "engine_temps");
            eWarmupMixKoffs.SetX("EngineTemp", "Temp.", "F1");
            eWarmupMixKoffs.SetTableEventHandler(ChartUpdateEvent);

            eWarmupMixCorrs.Initialize(cs, 0, 10D, 0.01D, 0.1D, 0, 5, 10D, 0.2D, 2);
            eWarmupMixCorrs.SetConfig("warmup_mix_corrs", "engine_temp_count", "engine_temps");
            eWarmupMixCorrs.SetX("EngineTemp", "Temp.", "F1");
            eWarmupMixCorrs.SetTableEventHandler(ChartUpdateEvent);

            eColdStartTimes.Initialize(cs, 0, 300D, 1D, 1D, 0, 60, 10D, 10D, 0);
            eColdStartTimes.SetConfig("cold_start_idle_times", "engine_temp_count", "engine_temps");
            eColdStartTimes.SetX("EngineTemp", "Temp.", "F1");
            eColdStartTimes.SetTableEventHandler(ChartUpdateEvent);

            eColdStartCorrs.Initialize(cs, 0, 10D, 0.01D, 0.1D, 0, 5, 10D, 0.2D, 2);
            eColdStartCorrs.SetConfig("cold_start_idle_corrs", "engine_temp_count", "engine_temps");
            eColdStartCorrs.SetX("EngineTemp", "Temp.", "F1");
            eColdStartCorrs.SetTableEventHandler(ChartUpdateEvent);

            eKnockNoiseLevel.Initialize(cs, 0, 5, 0.01D, 0.2D, 0D, 1D, 500, 0.2D, 2);
            eKnockNoiseLevel.SetConfig("knock_noise_level", "rotates_count", "rotates");
            eKnockNoiseLevel.SetX("RPM", "RPM", "F0");
            eKnockNoiseLevel.SetY("KnockSensor", "Knock Level", "F2");
            eKnockNoiseLevel.SetTableEventHandler(ChartUpdateEvent);

            eKnockThreshold.Initialize(cs, 0, 5, 0.01D, 0.2D, 0D, 1D, 500, 0.2D, 2);
            eKnockThreshold.SetConfig("knock_threshold", "rotates_count", "rotates");
            eKnockThreshold.SetX("RPM", "RPM", "F0");
            eKnockThreshold.SetY("KnockSensorFiltered", "Knock Level", "F2");
            eKnockThreshold.SetTableEventHandler(ChartUpdateEvent);

            eKnockFilterFrequency.Initialize(cs, 0, 63, 1, 1, 30D, 50D, 500, 2D, 0);
            eKnockFilterFrequency.SetConfig("knock_filter_frequency", "rotates_count", "rotates");
            eKnockFilterFrequency.SetX("RPM", "RPM", "F0");
            eKnockFilterFrequency.SetTableEventHandler(ChartUpdateEvent);

            eKnockGain.Initialize(cs, 0, 63, 1, 1, 0D, 63D, 500, 5D, 0);
            eKnockGain.SetConfig("knock_gain", "rotates_count", "rotates");
            eKnockGain.SetX("RPM", "RPM", "F0");
            eKnockGain.SetTableEventHandler(ChartUpdateEvent);


            eKnockZone.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                0.0D, 1.0D, 0.01D, 100.0D, 0.1D, 0.0D, 1.0D, 500, 0.1D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 2);

            eKnockZone.SetConfig("knock_zone", "rotates_count", "fillings_count", "rotates", "fillings");
            eKnockZone.SetX("RPM", "RPM", "F0");
            eKnockZone.SetY("KnockZone", "KnockZone", "F2");
            eKnockZone.SetD("CyclicAirFlow", "Filling", "F1");
            eKnockZone.SetTableEventHandler(ChartUpdateEvent);

            colorTransience = new ColorTransience(0.0F, 1.0F, Color.Gray);
            colorTransience.Add(Color.Green, 0.0F);
            colorTransience.Add(Color.OrangeRed, 0.3F);
            colorTransience.Add(Color.Red, 1.0F);

            eKnockZone.SetTableColorTrans(colorTransience);

            ePartFullLPFIgnition.Initialize(cs, 0, 1, 0.001D, 0.1D, 0D, 0.5F, 500, 0.05D, 3);
            ePartFullLPFIgnition.SetConfig("switch_ign_lpf", "rotates_count", "rotates");
            ePartFullLPFIgnition.SetX("RPM", "RPM", "F0");
            ePartFullLPFIgnition.SetTableEventHandler(ChartUpdateEvent);

            ePartFullLPFMixture.Initialize(cs, 0, 1, 0.001D, 0.1D, 0D, 0.5F, 500, 0.05D, 3);
            ePartFullLPFMixture.SetConfig("switch_mix_lpf", "rotates_count", "rotates");
            ePartFullLPFMixture.SetX("RPM", "RPM", "F0");
            ePartFullLPFMixture.SetTableEventHandler(ChartUpdateEvent);

            ePartFullLPFInjPhase.Initialize(cs, 0, 1, 0.001D, 0.1D, 0D, 0.5F, 500, 0.05D, 3);
            ePartFullLPFInjPhase.SetConfig("switch_phase_lpf", "rotates_count", "rotates");
            ePartFullLPFInjPhase.SetX("RPM", "RPM", "F0");
            ePartFullLPFInjPhase.SetTableEventHandler(ChartUpdateEvent);

            colorTransience = new ColorTransience(-1.0F, 1.0F, Color.Gray);
            colorTransience.Add(Color.DeepSkyBlue, -1.0F);
            colorTransience.Add(Color.Blue, -0.5F);
            colorTransience.Add(Color.FromArgb(0, 128, 255), -0.1F);
            colorTransience.Add(Color.Green, 0.0F);
            colorTransience.Add(Color.FromArgb(192, 128, 0), 0.1F);
            colorTransience.Add(Color.Red, 0.5F);
            colorTransience.Add(Color.DarkRed, 1.0F);

            eCorrsFillByMAP.Initialize(cs, Editor2DMode.CorrectionsTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].pressures_count,
                -10.0D, 10.0D, 0.005D, 100.0D, 0.1D, -0.2D, 0.2D, 500, 0.1D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_PRESSURES_MAX, 3);

            eCorrsFillByMAP.SetConfig("fill_by_map", "rotates_count", "pressures_count", "rotates", "pressures");
            eCorrsFillByMAP.SetX("RPM", "RPM", "F0");
            eCorrsFillByMAP.SetY(string.Empty, "Correction", "F3");
            eCorrsFillByMAP.SetD("ManifoldAirPressure", "MAP", "F1");
            eCorrsFillByMAP.SetTableEventHandler(ChartCorrectionEvent);
            eCorrsFillByMAP.scHorisontal.SplitterDistance = (int)Math.Round(eCorrsFillByMAP.scHorisontal.Width * 0.75);

            eCorrsFillByMAP.SetTableColorTrans(colorTransience);
            eCorrsFillByMAP.SynchronizeChart();


            eCorrsPressureByTPS.Initialize(cs, Editor2DMode.CorrectionsTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].throttles_count,
                -10.0D, 10.0D, 0.005D, 100.0D, 0.1D, -0.2D, 0.2D, 500, 0.1D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_THROTTLES_MAX, 3);

            eCorrsPressureByTPS.SetConfig("map_by_thr", "rotates_count", "throttles_count", "rotates", "throttles");
            eCorrsPressureByTPS.SetX("RPM", "RPM", "F0");
            eCorrsPressureByTPS.SetY(string.Empty, "Correction", "F3");
            eCorrsPressureByTPS.SetD("ThrottlePosition", "TPS", "F1");
            eCorrsPressureByTPS.SetTableEventHandler(ChartCorrectionEvent);
            eCorrsPressureByTPS.scHorisontal.SplitterDistance = (int)Math.Round(eCorrsPressureByTPS.scHorisontal.Width * 0.75);

            eCorrsPressureByTPS.SetTableColorTrans(colorTransience);
            eCorrsPressureByTPS.SynchronizeChart();


            eCorrsIdleValveToRPM.Initialize(cs, Editor2DMode.CorrectionsTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].engine_temp_count,
                -10.0D, 10.0D, 0.02D, 100.0D, 0.1D, -0.2D, 0.2D, 500, 0.1D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_TEMPERATURES_MAX, 2);

            eCorrsIdleValveToRPM.SetConfig("idle_valve_to_rpm", "rotates_count", "engine_temp_count", "rotates", "engine_temps");
            eCorrsIdleValveToRPM.SetX("RPM", "RPM", "F0");
            eCorrsIdleValveToRPM.SetY(string.Empty, "Correction", "F3");
            eCorrsIdleValveToRPM.SetD("EngineTemp", "Temperature", "F1");
            eCorrsIdleValveToRPM.SetTableEventHandler(ChartCorrectionEvent);
            eCorrsIdleValveToRPM.scHorisontal.SplitterDistance = (int)Math.Round(eCorrsIdleValveToRPM.scHorisontal.Width * 0.75);

            eCorrsIdleValveToRPM.SetTableColorTrans(colorTransience);
            eCorrsIdleValveToRPM.SynchronizeChart();

            colorTransience = new ColorTransience(-10.0F, 10.0F, Color.Gray);
            colorTransience.Add(Color.DeepSkyBlue, -10.0F);
            colorTransience.Add(Color.Blue, -5.0F);
            colorTransience.Add(Color.FromArgb(0, 128, 255), -2.0F);
            colorTransience.Add(Color.Green, 0.0F);
            colorTransience.Add(Color.FromArgb(192, 128, 0), 2.0F);
            colorTransience.Add(Color.Red, 3.0F);
            colorTransience.Add(Color.DarkRed, 5.0F);
            colorTransience.Add(Color.Black, 10.0F);

            eCorrsIgnition.Initialize(cs, Editor2DMode.CorrectionsTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                -45.0D, 45.0D, 0.1D, 100.0D, 1D, -5D, 5D, 500, 1D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 2);

            eCorrsIgnition.SetConfig("ignitions", "rotates_count", "fillings_count", "rotates", "fillings");
            eCorrsIgnition.SetX("RPM", "RPM", "F0");
            eCorrsIgnition.SetY(string.Empty, "Ignition Correction", "F2");
            eCorrsIgnition.SetD("CyclicAirFlow", "Filling", "F1");
            eCorrsIgnition.SetTableEventHandler(ChartCorrectionEvent);
            eCorrsIgnition.scHorisontal.SplitterDistance = (int)Math.Round(eCorrsIgnition.scHorisontal.Width * 0.7);

            eCorrsIgnition.SetTableColorTrans(colorTransience);
            eCorrsIgnition.SynchronizeChart();


            colorTransience = new ColorTransience(-0.2F, 0.3F, Color.Gray);
            colorTransience.Add(Color.DeepSkyBlue, -0.2F);
            colorTransience.Add(Color.Blue, -0.1F);
            colorTransience.Add(Color.FromArgb(0, 128, 255), -0.05F);
            colorTransience.Add(Color.Green, 0.0F);
            colorTransience.Add(Color.FromArgb(192, 128, 0), 0.05F);
            colorTransience.Add(Color.Red, 0.1F);
            colorTransience.Add(Color.DarkRed, 0.2F);
            colorTransience.Add(Color.Black, 0.3F);

            eAirTempMixCorr.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                cs.ConfigStruct.tables[cs.CurrentTable].air_temp_count,
                -0.9D, 5.0D, 0.01D, 20D, 0.1D, -0.2D, 0.2D, 20, 0.1D, Consts.TABLE_FILLING_MAX, Consts.TABLE_TEMPERATURES_MAX, 2);

            eAirTempMixCorr.SetConfig("air_temp_mix_corr", "fillings_count", "air_temp_count", "fillings", "air_temps");
            eAirTempMixCorr.SetX("CyclicAirFlow", "CyclicAirFlow", "F1");
            eAirTempMixCorr.SetY(string.Empty, "Mix.Corr.", "F2");
            eAirTempMixCorr.SetD("AirTemp", "AirTemp", "F1");
            eAirTempMixCorr.SetTableEventHandler(ChartUpdateEvent);
            eAirTempMixCorr.scHorisontal.SplitterDistance = (int)Math.Round(eAirTempMixCorr.scHorisontal.Width * 0.65);

            eAirTempMixCorr.SetTableColorTrans(colorTransience);
            eAirTempMixCorr.SynchronizeChart();


            colorTransience = new ColorTransience(-5F, 5F, Color.Gray);
            colorTransience.Add(Color.DeepSkyBlue, -5.0F);
            colorTransience.Add(Color.Blue, -3.0F);
            colorTransience.Add(Color.FromArgb(0, 128, 255), -2.0F);
            colorTransience.Add(Color.Green, 0.0F);
            colorTransience.Add(Color.FromArgb(192, 128, 0), 2.0F);
            colorTransience.Add(Color.Red, 3.0F);
            colorTransience.Add(Color.DarkRed, 4.0F);
            colorTransience.Add(Color.Black, 5.0F);

            eAirTempIgnCorr.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                cs.ConfigStruct.tables[cs.CurrentTable].air_temp_count,
                -10.0D, 10.0D, 0.1D, 20D, 1D, -5D, 5D, 20, 1D, Consts.TABLE_FILLING_MAX, Consts.TABLE_TEMPERATURES_MAX, 1);

            eAirTempIgnCorr.SetConfig("air_temp_ign_corr", "fillings_count", "air_temp_count", "fillings", "air_temps");
            eAirTempIgnCorr.SetX("CyclicAirFlow", "CyclicAirFlow", "F1");
            eAirTempIgnCorr.SetY(string.Empty, "Ign.Corr.", "F1");
            eAirTempIgnCorr.SetD("AirTemp", "AirTemp", "F1");
            eAirTempIgnCorr.SetTableEventHandler(ChartUpdateEvent);
            eAirTempIgnCorr.scHorisontal.SplitterDistance = (int)Math.Round(eAirTempIgnCorr.scHorisontal.Width * 0.65);

            eAirTempIgnCorr.SetTableColorTrans(colorTransience);
            eAirTempIgnCorr.SynchronizeChart();


            eStartIgnition.Initialize(cs, -15D, 60D, 1D, 5D, 0D, 20D, 10D, 2D, 1);
            eStartIgnition.SetConfig("start_ignition", "engine_temp_count", "engine_temps");
            eStartIgnition.SetX("EngineTemp", "Temperature", "F0");
            eStartIgnition.SetY("IgnitionAngle", "Ignition", "F0");
            eStartIgnition.SetTableEventHandler(ChartUpdateEvent);

            eStartIdleValvePos.Initialize(cs, 0D, Consts.IDLE_VALVE_POS_MAX, 1D, 20D, 20D, 80D, 10D, 5D, 1);
            eStartIdleValvePos.SetConfig("start_idle_valve_pos", "engine_temp_count", "engine_temps");
            eStartIdleValvePos.SetX("EngineTemp", "Temperature", "F0");
            eStartIdleValvePos.SetY("IdleValvePosition", "Valve", "F0");
            eStartIdleValvePos.SetTableEventHandler(ChartUpdateEvent);

            eStartAsyncInject.Initialize(cs, 0D, 5000D, 10D, 50D, 0D, 500D, 10D, 50D, 0);
            eStartAsyncInject.SetConfig("start_async_filling", "engine_temp_count", "engine_temps");
            eStartAsyncInject.SetX("EngineTemp", "Temperature", "F0");
            eStartAsyncInject.SetTableEventHandler(ChartUpdateEvent);

            eStartLargeInject.Initialize(cs, 0D, 4000D, 10D, 50D, 0D, 500D, 10D, 50D, 0);
            eStartLargeInject.SetConfig("start_large_filling", "engine_temp_count", "engine_temps");
            eStartLargeInject.SetX("EngineTemp", "Temperature", "F0");
            eStartLargeInject.SetTableEventHandler(ChartUpdateEvent);

            eStartSmallInject.Initialize(cs, 0D, 3000D, 10D, 50D, 0D, 500D, 10D, 50D, 0);
            eStartSmallInject.SetConfig("start_small_filling", "engine_temp_count", "engine_temps");
            eStartSmallInject.SetX("EngineTemp", "Temperature", "F0");
            eStartSmallInject.SetTableEventHandler(ChartUpdateEvent);

            eStartFillingTime.Initialize(cs, 0D, 60D, 0.1D, 1D, 0D, 10D, 10D, 1D, 1);
            eStartFillingTime.SetConfig("start_filling_time", "engine_temp_count", "engine_temps");
            eStartFillingTime.SetX("EngineTemp", "Temperature", "F0");
            eStartFillingTime.SetTableEventHandler(ChartUpdateEvent);

            eStartInjPhase.Initialize(cs, 0D, 360D, 1D, 5D, 0D, 200D, 10D, 20D, 0);
            eStartInjPhase.SetConfig("start_injection_phase", "engine_temp_count", "engine_temps");
            eStartInjPhase.SetX("EngineTemp", "Temperature", "F0");
            eStartInjPhase.SetTableEventHandler(ChartUpdateEvent);

            eStartTpsCorrs.Initialize(cs, 0D, 1D, 0.01D, 0.05D, 0D, 1D, 10D, 0.1D, 2);
            eStartTpsCorrs.SetConfig("start_tps_corrs", "throttles_count", "throttles");
            eStartTpsCorrs.SetX("ThrottlePosition", "TPS", "F1");
            eStartTpsCorrs.SetTableEventHandler(ChartUpdateEvent);

            eIgnitionTimeByTPS.Initialize(cs, 0D, 10D, 0.01D, 0.1D, 0D, 1D, 10D, 0.1D, 2);
            eIgnitionTimeByTPS.SetConfig("idle_ignition_time_by_tps", "throttles_count", "throttles");
            eIgnitionTimeByTPS.SetX("ThrottlePosition", "TPS", "F1");
            eIgnitionTimeByTPS.SetTableEventHandler(ChartUpdateEvent);

            eIdleEconDelay.Initialize(cs, 0D, 60D, 0.1D, 1D, 0D, 10D, 10D, 1D, 1);
            eIdleEconDelay.SetConfig("idle_econ_delay", "throttles_count", "throttles");
            eIdleEconDelay.SetX("EngineTemp", "Temperature", "F0");
            eIdleEconDelay.SetTableEventHandler(ChartUpdateEvent);

            eStartEconDelay.Initialize(cs, 0D, 60D, 0.1D, 1D, 0D, 10D, 10D, 1D, 1);
            eStartEconDelay.SetConfig("start_econ_delay", "throttles_count", "throttles");
            eStartEconDelay.SetX("EngineTemp", "Temperature", "F0");
            eStartEconDelay.SetTableEventHandler(ChartUpdateEvent); 
            
            eIdleRegThr1.Initialize(cs, 0.02D, 2D, 0.001D, 0.05D, 0D, 1.0D, 10D, 0.1D, 3);
            eIdleRegThr1.SetConfig("idle_rpm_pid_act_1", "engine_temp_count", "engine_temps");
            eIdleRegThr1.SetX("EngineTemp", "Temperature", "F0");
            eIdleRegThr1.SetTableEventHandler(ChartUpdateEvent);
            
            eIdleRegThr2.Initialize(cs, 0.02D, 2D, 0.001D, 0.05D, 0D, 1.0D, 10D, 0.1D, 3);
            eIdleRegThr2.SetConfig("idle_rpm_pid_act_2", "engine_temp_count", "engine_temps");
            eIdleRegThr2.SetX("EngineTemp", "Temperature", "F0");
            eIdleRegThr2.SetTableEventHandler(ChartUpdateEvent);
        
            eIdleValvePidP.Initialize(cs, -10D, 10D, 0.001D, 0.01D, 0D, 1.0D, 0.05D, 0.2D, 3);
            eIdleValvePidP.SetConfig("idle_valve_to_massair_pid_p", "idle_pids_rpm_koffs_count", "idle_pids_rpm_koffs");
            eIdleValvePidP.SetX("IdleWishToRpmRelation", "RPM koff", "F2");
            eIdleValvePidP.SetTableEventHandler(ChartUpdateEvent);

            eIdleValvePidI.Initialize(cs, -10D, 10D, 0.001D, 0.01D, 0D, 1.0D, 0.05D, 0.2D, 3);
            eIdleValvePidI.SetConfig("idle_valve_to_massair_pid_i", "idle_pids_rpm_koffs_count", "idle_pids_rpm_koffs");
            eIdleValvePidI.SetX("IdleWishToRpmRelation", "RPM koff", "F2");
            eIdleValvePidI.SetTableEventHandler(ChartUpdateEvent);

            eIdleValvePidD.Initialize(cs, -10D, 10D, 0.0001D, 0.01D, 0D, 0.1D, 0.05D, 0.01D, 4);
            eIdleValvePidD.SetConfig("idle_valve_to_massair_pid_d", "idle_pids_rpm_koffs_count", "idle_pids_rpm_koffs");
            eIdleValvePidD.SetX("IdleWishToRpmRelation", "RPM koff", "F2");
            eIdleValvePidD.SetTableEventHandler(ChartUpdateEvent);

            eIdleIgnPidP.Initialize(cs, -10D, 10D, 0.001D, 0.01D, 0D, 1.0D, 0.05D, 0.2D, 3);
            eIdleIgnPidP.SetConfig("idle_ign_to_rpm_pid_p", "idle_pids_rpm_koffs_count", "idle_pids_rpm_koffs");
            eIdleIgnPidP.SetX("IdleWishToRpmRelation", "RPM koff", "F2");
            eIdleIgnPidP.SetTableEventHandler(ChartUpdateEvent);

            eIdleIgnPidI.Initialize(cs, -10D, 10D, 0.001D, 0.01D, 0D, 1.0D, 0.05D, 0.2D, 3);
            eIdleIgnPidI.SetConfig("idle_ign_to_rpm_pid_i", "idle_pids_rpm_koffs_count", "idle_pids_rpm_koffs");
            eIdleIgnPidI.SetX("IdleWishToRpmRelation", "RPM koff", "F2");
            eIdleIgnPidI.SetTableEventHandler(ChartUpdateEvent);

            eIdleIgnPidD.Initialize(cs, -10D, 10D, 0.0001D, 0.01D, 0D, 0.1D, 0.05D, 0.01D, 4);
            eIdleIgnPidD.SetConfig("idle_ign_to_rpm_pid_d", "idle_pids_rpm_koffs_count", "idle_pids_rpm_koffs");
            eIdleIgnPidD.SetX("IdleWishToRpmRelation", "RPM koff", "F2");
            eIdleIgnPidD.SetTableEventHandler(ChartUpdateEvent);
            
            eTspsRelativePosition.Initialize(cs, -175D, 175D, 0.1D, 2D, -120D, -110D, 500, 1D, 1);
            eTspsRelativePosition.SetConfig("tsps_relative_pos", "rotates_count", "rotates");
            eTspsRelativePosition.SetX("RPM", "RPM", "F0");
            eTspsRelativePosition.SetTableEventHandler(ChartUpdateEvent);
            
            eTspsDesyncThr.Initialize(cs, 0.1D, 100D, 0.1D, 0.1D, 0D, 5D, 500, 0.1D, 1);
            eTspsDesyncThr.SetConfig("tsps_desync_thr", "rotates_count", "rotates");
            eTspsDesyncThr.SetX("RPM", "RPM", "F0");
            eTspsDesyncThr.SetTableEventHandler(ChartUpdateEvent);

            SynchronizeCharts();
        }

        private void SynchronizeCharts()
        {
            eCyclicFilling.SynchronizeChart();
            eFuelMixturesPart.SynchronizeChart();
            eInjectionPhasePart.SynchronizeChart();
            eFuelMixturesFull.SynchronizeChart();
            eInjectionPhaseFull.SynchronizeChart();
            eIgnitionPart.SynchronizeChart();
            eIgnitionFull.SynchronizeChart();
            ePressureByRPMvsTPS.SynchronizeChart();
            eIdleValveVsRpm.SynchronizeChart();
            eCorrsFillByMAP.SynchronizeChart();
            eCorrsIdleValveToRPM.SynchronizeChart();
            eCorrsIgnition.SynchronizeChart();
            eCorrsPressureByTPS.SynchronizeChart();
            eAirTempMixCorr.SynchronizeChart();
            eAirTempIgnCorr.SynchronizeChart();
            eKnockZone.SynchronizeChart();
            UpdateCharts();
        }
        private void UpdateCharts()
        {
            eCyclicFilling.UpdateChart();
            eFuelMixturesPart.UpdateChart();
            eInjectionPhasePart.UpdateChart();
            eFuelMixturesFull.UpdateChart();
            eInjectionPhaseFull.UpdateChart();
            eIgnitionPart.UpdateChart();
            eIgnitionFull.UpdateChart();
            ePressureByRPMvsTPS.UpdateChart();
            eSatByRPM.UpdateChart();
            eInjectorLag.UpdateChart();
            eInjectionPhaseLPF.UpdateChart();
            eSaturationPulse.UpdateChart();
            eEnrichmentByMAP.UpdateChart();
            eEnrichmentByTPS.UpdateChart();
            eEnrichmentMAPHPF.UpdateChart();
            eEnrichmentTPSHPF.UpdateChart();
            eEnrichmentTempMult.UpdateChart();

            ePressures.UpdateChart();
            eRotates.UpdateChart();
            eIdleRotates.UpdateChart();
            eThrottles.UpdateChart();
            eEngTemps.UpdateChart();
            eAirTemps.UpdateChart();
            eFillings.UpdateChart();
            eSpeeds.UpdateChart();
            eVoltages.UpdateChart();

            eIdleWishRPM.UpdateChart();
            eIdleWishIgnition.UpdateChart();
            eIdleIgnitionStatic.UpdateChart();
            eIdleWishMassAirFlow.UpdateChart();
            eIdleSpeedShift.UpdateChart();
            eIdleValveVsRpm.UpdateChart();

            eIdleRegThr1.UpdateChart();
            eIdleRegThr2.UpdateChart();
            eIdleValvePidP.UpdateChart();
            eIdleValvePidI.UpdateChart();
            eIdleValvePidD.UpdateChart();
            eIdleIgnPidP.UpdateChart();
            eIdleIgnPidI.UpdateChart();
            eIdleIgnPidD.UpdateChart();
            
            eWarmupMixture.UpdateChart();
            eWarmupMixKoffs.UpdateChart();
            eWarmupMixCorrs.UpdateChart();
            eColdStartCorrs.UpdateChart();
            eColdStartTimes.UpdateChart();
            eStartAsyncInject.UpdateChart();
            eStartLargeInject.UpdateChart();
            eStartSmallInject.UpdateChart();
            eStartFillingTime.UpdateChart();
            eIgnitionTimeByTPS.UpdateChart();
            eIdleEconDelay.UpdateChart();
            eStartEconDelay.UpdateChart();
            eStartInjPhase.UpdateChart();
            eStartTpsCorrs.UpdateChart();
            eStartIgnition.UpdateChart();
            eStartIdleValvePos.UpdateChart();

            eKnockThreshold.UpdateChart();
            eKnockNoiseLevel.UpdateChart();
            eKnockZone.UpdateChart();
            eKnockFilterFrequency.UpdateChart();
            eKnockGain.UpdateChart();

            ePartFullLPFIgnition.UpdateChart();
            ePartFullLPFMixture.UpdateChart();
            ePartFullLPFInjPhase.UpdateChart();

            eCorrsFillByMAP.UpdateChart();
            eCorrsIdleValveToRPM.UpdateChart();
            eCorrsIgnition.UpdateChart();
            eCorrsPressureByTPS.UpdateChart();
            eAirTempMixCorr.UpdateChart();
            eAirTempIgnCorr.UpdateChart();

            eTspsRelativePosition.UpdateChart();
            eTspsDesyncThr.UpdateChart();
        }

        private void CorrStart()
        {
            btnCorrStart.Enabled = false;
            btnCorrStop.Enabled = true;
            lblCorrStatus.Text = "Status: Learning";
            lblCorrStats.Text = string.Empty;

            eCorrsFillByMAP.SetCalibrationTable("progress_fill_by_map");
            eCorrsIdleValveToRPM.SetCalibrationTable("progress_idle_valve_to_rpm");
            eCorrsIgnition.SetCalibrationTable("progress_ignitions");
            eCorrsPressureByTPS.SetCalibrationTable("progress_map_by_thr");

            btnCorrAppendFillingByMAP.Enabled = false;
            btnCorrAppendIdleValve.Enabled = false;
            btnCorrAppendIgnitions.Enabled = false;
            btnCorrAppendPressureByTPS.Enabled = false;

            rbCorrInterpolationFunc.Enabled = false;
            rbCorrPointFunc.Enabled = false;
        }

        private void CorrStop()
        {
            btnCorrStart.Enabled = true;
            btnCorrStop.Enabled = false;
            lblCorrStatus.Text = "Status: Idle";

            eCorrsFillByMAP.ClearCalibrationTable();
            eCorrsIdleValveToRPM.ClearCalibrationTable();
            eCorrsIgnition.ClearCalibrationTable();
            eCorrsPressureByTPS.ClearCalibrationTable();

            btnCorrAppendFillingByMAP.Enabled = true;
            btnCorrAppendIdleValve.Enabled = true;
            btnCorrAppendIgnitions.Enabled = true;
            btnCorrAppendPressureByTPS.Enabled = true;

            rbCorrInterpolationFunc.Enabled = true;
            rbCorrPointFunc.Enabled = true;
        }

        private void ChartUpdateEvent(object sender, EventArgs e)
        {
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void ChartCorrectionEvent(object sender, EventArgs e)
        {
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateCorrections();
            }
        }

        public void UpdateStatusEvent(IEnumerable<CheckDataItem> checkDataList)
        {
            Action action = new Action(() => { this.UpdateStatus(checkDataList); });
            if (this.InvokeRequired)
                this.BeginInvoke(action);
            else action.Invoke();
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

        public void SynchronizedEvent(int errorCode, bool fast)
        {
            Action action = new Action(() => { this.SynchronizedEventInternal(errorCode, fast); });
            if (this.InvokeRequired)
                this.BeginInvoke(action);
            else action.Invoke();
        }

        private void SynchronizedEventInternal(int errorCode, bool fast)
        {
            if (syncForm != null)
            {
                if (syncForm.InvokeRequired)
                    syncForm.BeginInvoke(new Action(() => syncForm.CloseForm()));
                else syncForm.Close();
            }

            if (cs.ConfigStruct.parameters.performAdaptation > 0)
            {
                string text = string.Empty;

                int count = 4;
                byte[][] array = new byte[count][];
                double[] stats = new double[count];
                int size;
                double total = 0;

                array[0] = cs.ConfigStruct.corrections.progress_fill_by_map;
                array[1] = cs.ConfigStruct.corrections.progress_idle_valve_to_rpm;
                array[2] = cs.ConfigStruct.corrections.progress_ignitions;
                array[3] = cs.ConfigStruct.corrections.progress_map_by_thr;


                for (int i = 0; i < count; i++)
                {
                    stats[i] = 0;
                    size = array[i].Length;
                    for (int j = 0; j < size; j++)
                    {
                        stats[i] += array[i][j];
                    }
                    stats[i] /= 255.0D;
                    stats[i] /= size;
                    stats[i] *= 100.0D;
                    total += stats[i];
                }
                total /= count;


                text += $"Fill by MAP: {stats[0].ToString("F2")}%\r\n";
                text += $"Idle Valve: {stats[1].ToString("F2")}%\r\n";
                text += $"Ignitions: {stats[2].ToString("F2")}%\r\n";
                text += $"Press.by TPS: {stats[3].ToString("F2")}%\r\n";

                text += $"\r\nTotal: {total.ToString("F2")}%\r\n";


                lblCorrStats.Text = text;
            }

            if (!fast)
            {

                if (errorCode != 0)
                {
                    MessageBox.Show("Error during sync.\r\n\r\nCode: " + (errorCode == -1 ? "Timeout occired" : errorCode.ToString()), "Engine Control Unit", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                rbShiftMode0.Checked = false;
                rbShiftMode1.Checked = false;
                rbShiftMode2.Checked = false;
                rbShiftMode3.Checked = false;
                rbShiftMode4.Checked = false;

                switch (cs.ConfigStruct.parameters.shiftMode)
                {
                    case 0: rbShiftMode0.Checked = true; break;
                    case 1: rbShiftMode1.Checked = true; break;
                    case 2: rbShiftMode2.Checked = true; break;
                    case 3: rbShiftMode3.Checked = true; break;
                    case 4: rbShiftMode4.Checked = true; break;
                    default: break;
                }

                rbIndividualCoils.Checked = false;
                rbIgnitionModule.Checked = false;
                rbSingleCoil.Checked = false;

                if (cs.ConfigStruct.parameters.isSingleCoil > 0 && cs.ConfigStruct.parameters.isIndividualCoils == 0)
                {
                    rbSingleCoil.Checked = true;
                }
                else if (cs.ConfigStruct.parameters.isSingleCoil == 0 && cs.ConfigStruct.parameters.isIndividualCoils > 0)
                {
                    rbIndividualCoils.Checked = true;
                }
                else if (cs.ConfigStruct.parameters.isSingleCoil == 0 && cs.ConfigStruct.parameters.isIndividualCoils == 0)
                {
                    rbIgnitionModule.Checked = true;
                }
                else
                {
                    MessageBox.Show("Wrong Coils configuration!", "Engine Control Unit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                lblCutoffRPM.Text = cs.ConfigStruct.parameters.cutoffRPM.ToString("F0");
                tbCutoffRPM.Value = Convert.ToInt32(cs.ConfigStruct.parameters.cutoffRPM);

                lblCutoffAngle.Text = cs.ConfigStruct.parameters.cutoffAngle.ToString("F1");
                tbCutoffAngle.Value = Convert.ToInt32(cs.ConfigStruct.parameters.cutoffAngle * 10.0f);

                lblCutoffMixture.Text = cs.ConfigStruct.parameters.cutoffMixture.ToString("F1");
                tbCutoffMixture.Value = Convert.ToInt32(cs.ConfigStruct.parameters.cutoffMixture * 10.0f);

                lblOilPressCutoffRPM.Text = cs.ConfigStruct.parameters.oilPressureCutoffRPM.ToString("F0");
                tbOilPressCutoffRPM.Value = Convert.ToInt32(cs.ConfigStruct.parameters.oilPressureCutoffRPM);

                lblShiftThrThr.Text = cs.ConfigStruct.parameters.shiftThrThr.ToString("F0");
                tbShiftThrThr.Value = Convert.ToInt32(cs.ConfigStruct.parameters.shiftThrThr);

                lblShiftRpmThr.Text = cs.ConfigStruct.parameters.shiftRpmThr.ToString("F0");
                tbShiftRpmThr.Value = Convert.ToInt32(cs.ConfigStruct.parameters.shiftRpmThr);

                lblShiftRpmTill.Text = cs.ConfigStruct.parameters.shiftRpmTill.ToString("F0");
                tbShiftRpmTill.Value = Convert.ToInt32(cs.ConfigStruct.parameters.shiftRpmTill);

                lblShiftAngle.Text = cs.ConfigStruct.parameters.shiftAngle.ToString("F1");
                tbShiftAngle.Value = Convert.ToInt32(cs.ConfigStruct.parameters.shiftAngle * 10.0f);

                lblShiftMixture.Text = cs.ConfigStruct.parameters.shiftMixture.ToString("F1");
                tbShiftMixture.Value = Convert.ToInt32(cs.ConfigStruct.parameters.shiftMixture * 10.0f);

                nudSwPos1.Value = cs.ConfigStruct.parameters.switchPos1Table + 1;
                nudSwPos0.Value = cs.ConfigStruct.parameters.switchPos0Table + 1;
                nudSwPos2.Value = cs.ConfigStruct.parameters.switchPos2Table + 1;
                nudFuelForce.Value = cs.ConfigStruct.parameters.forceTable + 1;
                nudEngVol.Value = (decimal)cs.ConfigStruct.parameters.engineVolume;
                nudSpeedInputCorr.Value = (decimal)cs.ConfigStruct.parameters.speedInputCorrection;
                nudSpeedOutputCorr.Value = (decimal)cs.ConfigStruct.parameters.speedOutputCorrection;
                nudKnockIntegratorTimeConstant.Value = cs.ConfigStruct.parameters.knockIntegratorTime;
                nudParamsFanLowT.Value = (decimal)cs.ConfigStruct.parameters.fanLowTemperature;
                nudParamsFanMidT.Value = (decimal)cs.ConfigStruct.parameters.fanMidTemperature;
                nudParamsFanHighT.Value = (decimal)cs.ConfigStruct.parameters.fanHighTemperature;

                cbUseTSPS.Checked = cs.ConfigStruct.parameters.useTSPS > 0;
                cbUseKnock.Checked = cs.ConfigStruct.parameters.useKnockSensor > 0;
                cbUseLambdaAC.Checked = cs.ConfigStruct.parameters.useLambdaSensor > 0;
                cbLambdaForceEnabled.Checked = cs.ConfigStruct.parameters.isLambdaForceEnabled > 0;
                cbUseShortTermCorr.Checked = cs.ConfigStruct.parameters.useShortTermCorr > 0;
                cbUseLongTermCorr.Checked = cs.ConfigStruct.parameters.useLongTermCorr > 0;
                cbIsEconEnabled.Checked = cs.ConfigStruct.parameters.isEconEnabled > 0;
                cbPerformIdleAdaptation.Checked = cs.ConfigStruct.parameters.performIdleAdaptation > 0;
                btnCorrStop.Enabled = cs.ConfigStruct.parameters.performAdaptation > 0;
                btnCorrStart.Enabled = cs.ConfigStruct.parameters.performAdaptation == 0;
                if (cs.ConfigStruct.parameters.performAdaptation > 0)
                    CorrStart();
                else
                    CorrStop();
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
                    middleLayer?.PacketHandler.SendParametersRequest();
                }
            }
        }

        private void UpdateEcuTableValues()
        {
            tbParamsName.MaxLength = Consts.TABLE_STRING_MAX - 1;
            tbParamsName.Text = cs.ConfigStruct.tables[cs.CurrentTable].name;
            rbInjCh1.Checked = false;
            rbInjCh2.Checked = false;

            switch (cs.ConfigStruct.tables[cs.CurrentTable].inj_channel)
            {
                case 0: rbInjCh1.Checked = true; break;
                case 1: rbInjCh2.Checked = true; break;
                default: break;
            }

            cbParamsIsFuelPressureConst.Checked = cs.ConfigStruct.tables[cs.CurrentTable].is_fuel_pressure_const > 0;
            cbParamsIsFullThrottleUsed.Checked = cs.ConfigStruct.tables[cs.CurrentTable].is_full_thr_used > 0;
            cbParamsIsInjectionPhaseByEnd.Checked = cs.ConfigStruct.tables[cs.CurrentTable].is_fuel_phase_by_end > 0;
            cbParamsIsPhAsyncEnrichmentEnabled.Checked = cs.ConfigStruct.tables[cs.CurrentTable].enrichment_ph_async_enabled > 0;
            cbParamsIsPhSyncEnrichmentEnabled.Checked = cs.ConfigStruct.tables[cs.CurrentTable].enrichment_ph_sync_enabled > 0;
            cbParamsIsPpAsyncEnrichmentEnabled.Checked = cs.ConfigStruct.tables[cs.CurrentTable].enrichment_pp_async_enabled > 0;
            cbParamsIsPpSyncEnrichmentEnabled.Checked = cs.ConfigStruct.tables[cs.CurrentTable].enrichment_pp_sync_enabled > 0;
            nudParamsCntPress.Maximum = Consts.TABLE_PRESSURES_MAX;
            nudParamsCntRPMs.Maximum = Consts.TABLE_ROTATES_MAX;
            nudParamsCntIdleRPMs.Maximum = Consts.TABLE_ROTATES_MAX;
            nudParamsCntThrottles.Maximum = Consts.TABLE_THROTTLES_MAX;
            nudParamsCntVoltages.Maximum = Consts.TABLE_VOLTAGES_MAX;
            nudParamsCntFillings.Maximum = Consts.TABLE_FILLING_MAX;
            nudParamsCntEngineTemps.Maximum = Consts.TABLE_TEMPERATURES_MAX;
            nudParamsCntAirTemps.Maximum = Consts.TABLE_TEMPERATURES_MAX;
            nudParamsCntSpeeds.Maximum = Consts.TABLE_SPEEDS_MAX;

            nudParamsCntPress.Value = cs.ConfigStruct.tables[cs.CurrentTable].pressures_count;
            nudParamsCntRPMs.Value = cs.ConfigStruct.tables[cs.CurrentTable].rotates_count;
            nudParamsCntIdleRPMs.Value = cs.ConfigStruct.tables[cs.CurrentTable].idle_rotates_count;
            nudParamsCntThrottles.Value = cs.ConfigStruct.tables[cs.CurrentTable].throttles_count;
            nudParamsCntVoltages.Value = cs.ConfigStruct.tables[cs.CurrentTable].voltages_count;
            nudParamsCntFillings.Value = cs.ConfigStruct.tables[cs.CurrentTable].fillings_count;
            nudParamsCntEngineTemps.Value = cs.ConfigStruct.tables[cs.CurrentTable].engine_temp_count;
            nudParamsCntAirTemps.Value = cs.ConfigStruct.tables[cs.CurrentTable].air_temp_count;
            nudParamsCntSpeeds.Value = cs.ConfigStruct.tables[cs.CurrentTable].idle_speeds_shift_count;

            nudParamsFuelPressure.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].fuel_pressure;
            nudParamsInjPerformance.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].injector_performance;
            nudParamsFuelKgL.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].fuel_mass_per_cc;
            nudParamsFuelAFR.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].fuel_afr;
            
            nudParamsPidShortCorrP.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].short_term_corr_pid_p;
            nudParamsPidShortCorrI.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].short_term_corr_pid_i;
            nudParamsPidShortCorrD.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].short_term_corr_pid_d;

            nudParamsEnrPMapTps.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].enrichment_proportion_map_vs_thr;
            nudParamsIdleIgnDevMin.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].idle_ign_deviation_min;
            nudParamsIdleIgnDevMax.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].idle_ign_deviation_max;
            nudParamsIdleIgnFanLCorr.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].idle_ign_fan_low_corr;
            nudParamsIdleIgnFanHCorr.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].idle_ign_fan_high_corr;
            nudParamsIdleAirFanLCorr.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].idle_air_fan_low_corr;
            nudParamsIdleAirFanHCorr.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].idle_air_fan_high_corr;

            nudParamsCorrInjCy1.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_injection[0];
            nudParamsCorrInjCy2.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_injection[1];
            nudParamsCorrInjCy3.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_injection[2];
            nudParamsCorrInjCy4.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_injection[3];

            nudParamsCorrIgnCy1.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_ignition[0];
            nudParamsCorrIgnCy2.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_ignition[1];
            nudParamsCorrIgnCy3.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_ignition[2];
            nudParamsCorrIgnCy4.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_ignition[3];

            nudParamsKnockIgnCorr.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].knock_ign_corr_max;
            nudParamsKnockInjCorr.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].knock_inj_corr_max;

            if (cs.ConfigStruct.parameters.performAdaptation == 1)
            {
                rbCorrInterpolationFunc.Checked = false;
                rbCorrPointFunc.Checked = false;

                rbCorrInterpolationFunc.Checked = true;
            }
            else if (cs.ConfigStruct.parameters.performAdaptation == 2)
            {
                rbCorrInterpolationFunc.Checked = false;
                rbCorrPointFunc.Checked = false;

                rbCorrPointFunc.Checked = true;
            }

            SynchronizeCharts();
        }

        private void ClearDragCharts()
        {
            chartDragTime.Series.Clear();
            chartDragAccel.Series.Clear();
            chartDragRPM.Series.Clear();
            chartDragPressure.Series.Clear();
            chartDragMAF.Series.Clear();
            chartDragCAF.Series.Clear();
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
            chartDragRPM.Series.Clear();
            chartDragPressure.Series.Clear();
            chartDragMAF.Series.Clear();
            chartDragCAF.Series.Clear();
            lvDragTable.Items.Clear();
            for (int i = lvDragTable.Columns.Count - 1; i >= 2; i--)
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
                        series.LabelForeColor = Color.White;
                        series.MarkerColor = Color.White;
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
                        series.LabelForeColor = Color.White;
                        series.MarkerColor = Color.White;
                        series.BorderWidth = 2;
                        series.Tag = lDragRuns[i];
                        //float trans = (float)i / (float)(lDragRuns.Count - 1);
                        //series.Color = Color.FromArgb((int)(min.R * (1.0f - trans) + max.R * trans), (int)(min.G * (1.0f - trans) + max.G * trans), (int)(min.B * (1.0f - trans) + max.B * trans));

                        for (int j = 0; j < lDragRuns[i].Points.Count; j++)
                        {
                            series.Points.AddXY(lDragRuns[i].Points[j].Time, lDragRuns[i].Points[j].Acceleration);
                        }

                        series = chartDragRPM.Series.Add(lDragRuns[i].Label);
                        series.ChartType = SeriesChartType.Line;
                        series.XAxisType = AxisType.Primary;
                        series.XValueType = ChartValueType.Single;
                        series.YAxisType = AxisType.Primary;
                        series.YValueType = ChartValueType.Single;
                        series.LabelForeColor = Color.White;
                        series.MarkerColor = Color.White;
                        series.BorderWidth = 2;
                        series.Tag = lDragRuns[i];
                        //float trans = (float)i / (float)(lDragRuns.Count - 1);
                        //series.Color = Color.FromArgb((int)(min.R * (1.0f - trans) + max.R * trans), (int)(min.G * (1.0f - trans) + max.G * trans), (int)(min.B * (1.0f - trans) + max.B * trans));

                        for (int j = 0; j < lDragRuns[i].Points.Count; j++)
                        {
                            series.Points.AddXY(lDragRuns[i].Points[j].Time, lDragRuns[i].Points[j].Acceleration);
                        }

                        series = chartDragPressure.Series.Add(lDragRuns[i].Label);
                        series.ChartType = SeriesChartType.Line;
                        series.XAxisType = AxisType.Primary;
                        series.XValueType = ChartValueType.Single;
                        series.YAxisType = AxisType.Primary;
                        series.YValueType = ChartValueType.Single;
                        series.LabelForeColor = Color.White;
                        series.MarkerColor = Color.White;
                        series.BorderWidth = 2;
                        series.Tag = lDragRuns[i];
                        //float trans = (float)i / (float)(lDragRuns.Count - 1);
                        //series.Color = Color.FromArgb((int)(min.R * (1.0f - trans) + max.R * trans), (int)(min.G * (1.0f - trans) + max.G * trans), (int)(min.B * (1.0f - trans) + max.B * trans));

                        for (int j = 0; j < lDragRuns[i].Points.Count; j++)
                        {
                            series.Points.AddXY(lDragRuns[i].Points[j].Time, lDragRuns[i].Points[j].Pressure);
                        }

                        series = chartDragMAF.Series.Add(lDragRuns[i].Label);
                        series.ChartType = SeriesChartType.Line;
                        series.XAxisType = AxisType.Primary;
                        series.XValueType = ChartValueType.Single;
                        series.YAxisType = AxisType.Primary;
                        series.YValueType = ChartValueType.Single;
                        series.LabelForeColor = Color.White;
                        series.MarkerColor = Color.White;
                        series.BorderWidth = 2;
                        series.Tag = lDragRuns[i];
                        //float trans = (float)i / (float)(lDragRuns.Count - 1);
                        //series.Color = Color.FromArgb((int)(min.R * (1.0f - trans) + max.R * trans), (int)(min.G * (1.0f - trans) + max.G * trans), (int)(min.B * (1.0f - trans) + max.B * trans));

                        for (int j = 0; j < lDragRuns[i].Points.Count; j++)
                        {
                            series.Points.AddXY(lDragRuns[i].Points[j].Time, lDragRuns[i].Points[j].MassAirFlow);
                        }

                        series = chartDragCAF.Series.Add(lDragRuns[i].Label);
                        series.ChartType = SeriesChartType.Line;
                        series.XAxisType = AxisType.Primary;
                        series.XValueType = ChartValueType.Single;
                        series.YAxisType = AxisType.Primary;
                        series.YValueType = ChartValueType.Single;
                        series.LabelForeColor = Color.White;
                        series.MarkerColor = Color.White;
                        series.BorderWidth = 2;
                        series.Tag = lDragRuns[i];
                        //float trans = (float)i / (float)(lDragRuns.Count - 1);
                        //series.Color = Color.FromArgb((int)(min.R * (1.0f - trans) + max.R * trans), (int)(min.G * (1.0f - trans) + max.G * trans), (int)(min.B * (1.0f - trans) + max.B * trans));

                        for (int j = 0; j < lDragRuns[i].Points.Count; j++)
                        {
                            series.Points.AddXY(lDragRuns[i].Points[j].Time, lDragRuns[i].Points[j].CycleAirFlow);
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

                chartDragRPM.ChartAreas[0].AxisX.Minimum = 0;
                chartDragPressure.ChartAreas[0].AxisX.Minimum = 0;
                chartDragMAF.ChartAreas[0].AxisX.Minimum = 0;
                chartDragCAF.ChartAreas[0].AxisX.Minimum = 0;

                chartDragRPM.ChartAreas[0].AxisY.Minimum = 0;
                chartDragPressure.ChartAreas[0].AxisY.Minimum = 0;
                chartDragMAF.ChartAreas[0].AxisY.Minimum = 0;
                chartDragCAF.ChartAreas[0].AxisY.Minimum = 0;


                if (TableSplit >= 2)
                {
                    double[] speeds = new double[TableSplit + 1];
                    for (int i = 0; i <= TableSplit; i++)
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
                                if (lDragRuns[j].Points[p].Speed >= speeds[i] && lDragRuns[j].Points[p].Speed < speeds[i + 1])
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
            if (eDragStatus == DragStatusType.Set || eDragStatus == DragStatusType.Go)
            {
                if (dur.ErrorCode > 0)
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
                    if (dur.Completed > 0)
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
                            middleLayer?.PacketHandler.SendDragPointRequest(drCurrentRun.FromSpeed, drCurrentRun.ToSpeed, drCurrentRun.Points.Count);
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
                    middleLayer?.PacketHandler.SendDragPointRequest(drCurrentRun.FromSpeed, drCurrentRun.ToSpeed, drCurrentRun.Points.Count);
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
            if (eDragStatus == DragStatusType.Ready)
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

        private void UpdateStatus(IEnumerable<CheckDataItem> checkDataList)
        {
            ListViewItem selected = lvFailureCodes.SelectedItems.Count > 0 ? lvFailureCodes.SelectedItems[0] : null;
            lvFailureCodes.SuspendLayout();
            lvFailureCodes.Items.Clear();
            foreach (CheckDataItem item in checkDataList)
            {
                ListViewItem lvitem = lvFailureCodes.Items.Add(((int)item.ErrorCode).ToString(), ((int)item.ErrorCode).ToString(), string.Empty);
                lvitem.SubItems.Add(item.Active ? "Active" : "Inactive");
                lvitem.SubItems.Add(item.ErrorCode.ToString());
                lvitem.SubItems.Add(item.Message);
                lvitem.Tag = item;
            }
            if (selected != null && lvFailureCodes.Items.Count > 0 && lvFailureCodes.Items.ContainsKey(selected.Text))
            {
                lvFailureCodes.Items[selected.Text].Selected = true;
            }
            lvFailureCodes.ResumeLayout();

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

            UpdateCharts();

        }

        private void tmrSync_Tick(object sender, EventArgs e)
        {
            if (middleLayer != null)
            {
                if (cs.ConfigStruct.parameters.performAdaptation > 0)
                    middleLayer.SyncFast();
                middleLayer.PacketHandler.SendStatusRequest();

                if (cs.EcuParameters.IsCheckEngine > 0)
                    pbCheckEngine.Visible = !pbCheckEngine.Visible;
                else pbCheckEngine.Visible = false;
            }
        }

        private void tmr50ms_Tick(object sender, EventArgs e)
        {
            if (middleLayer != null)
            {
                tableLayoutPanel1.Enabled = !middleLayer.IsSynchronizing;
                toolStripProgressBar1.Style = middleLayer.IsSynchronizing ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks;
                if (parametersReceived || (DateTime.Now - lastReceivedarameters).TotalMilliseconds > 200)
                {
                    middleLayer.PacketHandler.SendParametersRequest();
                    parametersReceived = false;
                    lastReceivedarameters = DateTime.Now;
                }
                if ((tabControl1.Visible && tabControl1.SelectedTab == tabPage18) || eDragStatus == DragStatusType.Set || eDragStatus == DragStatusType.Go)
                {
                    middleLayer.PacketHandler.SendDragUpdateRequest();
                }
            }
        }

        private void rbCutoffMode1_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                cs.ConfigStruct.parameters.cutoffMode = 0;
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
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
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
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
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
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
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
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
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
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
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
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
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
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
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateConfig();
                }
            }
        }

        private void nudParamsFanLowT_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.fanLowTemperature = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudParamsFanMidT_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.fanMidTemperature = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudParamsFanHighT_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.fanHighTemperature = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void rbShiftMode0_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                cs.ConfigStruct.parameters.shiftMode = 0;
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateConfig();
                }
            }
        }

        private void rbShiftMode1_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                cs.ConfigStruct.parameters.shiftMode = 1;
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateConfig();
                }
            }
        }

        private void rbShiftMode2_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                cs.ConfigStruct.parameters.shiftMode = 2;
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateConfig();
                }
            }
        }

        private void rbShiftMode3_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                cs.ConfigStruct.parameters.shiftMode = 3;
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateConfig();
                }
            }
        }

        private void rbShiftMode4_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                cs.ConfigStruct.parameters.shiftMode = 4;
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateConfig();
                }
            }
        }

        private void tbCutoffRPM_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (cs.ConfigStruct.parameters.cutoffRPM > ((TrackBar)sender).Value)
                    ((TrackBar)sender).Value -= ((TrackBar)sender).Value % 50;
                else if (cs.ConfigStruct.parameters.cutoffRPM < ((TrackBar)sender).Value)
                    ((TrackBar)sender).Value += 50 - ((TrackBar)sender).Value % 50;

                lblCutoffRPM.Text = ((TrackBar)sender).Value.ToString();
                cs.ConfigStruct.parameters.cutoffRPM = ((TrackBar)sender).Value;
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateConfig();
                }
            }
            catch
            {

            }
        }

        private void tbCutoffAngle_Scroll(object sender, EventArgs e)
        {
            lblCutoffAngle.Text = ((float)((TrackBar)sender).Value / 10.0f).ToString("F1");
            cs.ConfigStruct.parameters.cutoffAngle = ((float)((TrackBar)sender).Value / 10.0f);
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void tbCutoffMixture_Scroll(object sender, EventArgs e)
        {
            lblCutoffMixture.Text = ((float)((TrackBar)sender).Value / 10.0f).ToString("F1");
            cs.ConfigStruct.parameters.cutoffMixture = ((float)((TrackBar)sender).Value / 10.0f);
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void tbShiftThrThr_Scroll(object sender, EventArgs e)
        {
            lblShiftThrThr.Text = ((TrackBar)sender).Value.ToString();
            cs.ConfigStruct.parameters.shiftThrThr = ((TrackBar)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void tbOilPressCutoffRPM_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (cs.ConfigStruct.parameters.oilPressureCutoffRPM > ((TrackBar)sender).Value)
                    ((TrackBar)sender).Value -= ((TrackBar)sender).Value % 100;
                else if (cs.ConfigStruct.parameters.oilPressureCutoffRPM < ((TrackBar)sender).Value)
                    ((TrackBar)sender).Value += 100 - ((TrackBar)sender).Value % 100;

                lblOilPressCutoffRPM.Text = ((TrackBar)sender).Value.ToString();
                cs.ConfigStruct.parameters.oilPressureCutoffRPM = ((TrackBar)sender).Value;
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateConfig();
                }
            }
            catch
            {

            }
        }

        private void tbShiftRpmThr_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (cs.ConfigStruct.parameters.shiftRpmThr > ((TrackBar)sender).Value)
                    ((TrackBar)sender).Value -= ((TrackBar)sender).Value % 50;
                else if (cs.ConfigStruct.parameters.shiftRpmThr < ((TrackBar)sender).Value)
                    ((TrackBar)sender).Value += 50 - ((TrackBar)sender).Value % 50;

                lblShiftRpmThr.Text = ((TrackBar)sender).Value.ToString();
                cs.ConfigStruct.parameters.shiftRpmThr = ((TrackBar)sender).Value;
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateConfig();
                }
            }
            catch
            {

            }
        }

        private void tbShiftRpmTill_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (cs.ConfigStruct.parameters.shiftRpmTill > ((TrackBar)sender).Value)
                    ((TrackBar)sender).Value -= ((TrackBar)sender).Value % 50;
                else if (cs.ConfigStruct.parameters.shiftRpmTill < ((TrackBar)sender).Value)
                    ((TrackBar)sender).Value += 50 - ((TrackBar)sender).Value % 50;

                lblShiftRpmTill.Text = ((TrackBar)sender).Value.ToString();
                cs.ConfigStruct.parameters.shiftRpmTill = ((TrackBar)sender).Value;
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateConfig();
                }
            }
            catch
            {

            }
        }

        private void tbShiftAngle_Scroll(object sender, EventArgs e)
        {
            lblShiftAngle.Text = ((float)((TrackBar)sender).Value / 10.0f).ToString("F1");
            cs.ConfigStruct.parameters.shiftAngle = ((float)((TrackBar)sender).Value / 10.0f);
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void tbShiftMixture_Scroll(object sender, EventArgs e)
        {
            lblShiftMixture.Text = ((float)((TrackBar)sender).Value / 10.0f).ToString("F1");
            cs.ConfigStruct.parameters.shiftMixture = ((float)((TrackBar)sender).Value / 10.0f);
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbFuelExtSw_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.isSwitchByExternal = ((CheckBox)sender).Checked ? 1 : 0;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudSwPos1_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.switchPos1Table = (int)((NumericUpDown)sender).Value - 1;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudEngVol_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.engineVolume = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudSpeedInputCorr_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.speedInputCorrection = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudSpeedOutputCorr_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.speedOutputCorrection = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudSwPos0_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.switchPos0Table = (int)((NumericUpDown)sender).Value - 1;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudSwPos2_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.switchPos2Table = (int)((NumericUpDown)sender).Value - 1;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbFuelForce_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.isForceTable = ((CheckBox)sender).Checked ? 1 : 0;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudFuelForce_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.forceTable = (int)((NumericUpDown)sender).Value - 1;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudKnockIntegratorTimeConstant_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.knockIntegratorTime = (int)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbUseTSPS_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.useTSPS = ((CheckBox)sender).Checked ? 1 : 0;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbUseKnock_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.useKnockSensor = ((CheckBox)sender).Checked ? 1 : 0;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbPerformIdleAdaptation_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.performIdleAdaptation = ((CheckBox)sender).Checked ? 1 : 0;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbUseLambda_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.useLambdaSensor = ((CheckBox)sender).Checked ? 1 : 0;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbLambdaForceEnabled_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.isLambdaForceEnabled = ((CheckBox)sender).Checked ? 1 : 0;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbUseShortTermCorr_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.useShortTermCorr = ((CheckBox)sender).Checked ? 1 : 0;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbUseLongTermCorr_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.useLongTermCorr = ((CheckBox)sender).Checked ? 1 : 0;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbIsEconEnabled_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.isEconEnabled = ((CheckBox)sender).Checked ? 1 : 0;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void btnCorrStart_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Previous calibration result will be lost!\r\nAre you sure you want to perform calibration?", "Engine Control Unit", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (dialogResult == DialogResult.Yes)
            {
                if (!middleLayer.IsSynchronizing)
                {
                    if (rbCorrInterpolationFunc.Checked && !rbCorrPointFunc.Checked)
                        cs.ConfigStruct.parameters.performAdaptation = 1;
                    else if (rbCorrPointFunc.Checked && !rbCorrInterpolationFunc.Checked)
                        cs.ConfigStruct.parameters.performAdaptation = 2;

                    if (cs.ConfigStruct.parameters.performAdaptation > 0)
                    {
                        CorrStart();
                        middleLayer.UpdateConfig();
                    }
                }
            }
        }

        private void btnCorrStop_Click(object sender, EventArgs e)
        {
             if (!middleLayer.IsSynchronizing)
            {
                cs.ConfigStruct.parameters.performAdaptation = 0;
                CorrStop();
                middleLayer.UpdateConfig();
            }
        }

        private void rbIndividualCoils_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                rbSingleCoil.Checked = false;
                rbIgnitionModule.Checked = false;
                cs.ConfigStruct.parameters.isIndividualCoils = 1;
                cs.ConfigStruct.parameters.isSingleCoil = 0;
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateConfig();
                }
            }
        }

        private void rbIgnitionModule_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                rbSingleCoil.Checked = false;
                rbIndividualCoils.Checked = false;
                cs.ConfigStruct.parameters.isIndividualCoils = 0;
                cs.ConfigStruct.parameters.isSingleCoil = 0;
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateConfig();
                }
            }
        }

        private void rbSingleCoil_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                rbIgnitionModule.Checked = false;
                rbIndividualCoils.Checked = false;
                cs.ConfigStruct.parameters.isIndividualCoils = 0;
                cs.ConfigStruct.parameters.isSingleCoil = 1;
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateConfig();
                }
            }
        }

        private void cbLive_CheckedChanged(object sender, EventArgs e)
        {
            bool check = ((CheckBox)sender).Checked;
            if (check != bLiveCheckOld)
            {
                bLiveCheckOld = check;
                middleLayer?.SyncSave(false);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (middleLayer != null)
            {
                while (!middleLayer.SyncSave(true)) ;
            }
            else
            {
                try
                {
                    Serializator<ConfigStruct>.Serialize(standaloneFileInfo.FullName, cs.ConfigStruct);
                    MessageBox.Show($"Setup save success.", "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Setup save failed.\r\n{ex.Message}", "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            if (middleLayer != null)
            {
                while (!middleLayer.SyncLoad(true)) ;
            }
            else
            {
                try
                {
                    cs.ConfigStruct = Serializator<ConfigStruct>.Deserialize(standaloneFileInfo.FullName);
                    SynchronizedEventInternal(0, false);
                    MessageBox.Show($"Setup reload success.", "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Setup reload failed.\r\n{ex.Message}", "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRedownload_Click(object sender, EventArgs e)
        {
            if (middleLayer != null)
            {
                while (!middleLayer.SyncLoad(false)) ;
            }
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
                middleLayer?.SyncSave(false);
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
            if (dlgTableImport.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    cs.ConfigStruct.tables[cs.CurrentTable] = Serializator<EcuTable>.Deserialize(dlgTableImport.FileName);
                    if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
                    {
                        middleLayer.UpdateTable(cs.CurrentTable);
                    }
                    SynchronizedEvent(0, false);
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
            if (dlgTableExport.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Serializator<EcuTable>.Serialize(dlgTableExport.FileName, cs.ConfigStruct.tables[cs.CurrentTable]);
                    MessageBox.Show($"Table export success.", "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Table export failed.\r\n{ex.Message}", "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSetupImport_Click(object sender, EventArgs e)
        {
            if (dlgSetupImport.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    cs.ConfigStruct = Serializator<ConfigStruct>.Deserialize(dlgSetupImport.FileName);
                    if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
                    {
                        middleLayer.SyncSave(false);
                    }
                    SynchronizedEvent(0, false);
                    MessageBox.Show($"Setup import success.", "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Setup import failed.\r\n{ex.Message}", "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSetupExport_Click(object sender, EventArgs e)
        {
            if (dlgSetupExport.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Serializator<ConfigStruct>.Serialize(dlgSetupExport.FileName, cs.ConfigStruct);
                    MessageBox.Show($"Setup export success.", "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Setup export failed.\r\n{ex.Message}", "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tbParamsName_TextChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].name = ((TextBox)sender).Text;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void rbInjCh1_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                cs.ConfigStruct.tables[cs.CurrentTable].inj_channel = 0;
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
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
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateTable(cs.CurrentTable);
                }
            }
        }

        private void cbParamsIsInjectionPhaseByEnd_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].is_fuel_phase_by_end = ((CheckBox)sender).Checked ? 1 : 0;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void cbParamsIsFuelPressureConst_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].is_fuel_pressure_const = ((CheckBox)sender).Checked ? 1 : 0;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void cbParamsIsFullThrottleUsed_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].is_full_thr_used = ((CheckBox)sender).Checked ? 1 : 0;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void cbParamsIsPhAsyncEnrichmentEnabled_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].enrichment_ph_async_enabled = ((CheckBox)sender).Checked ? 1 : 0;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void cbParamsIsPhSyncEnrichmentEnabled_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].enrichment_ph_sync_enabled = ((CheckBox)sender).Checked ? 1 : 0;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void cbParamsIsPpAsyncEnrichmentEnabled_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].enrichment_pp_async_enabled = ((CheckBox)sender).Checked ? 1 : 0;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void cbParamsIsPpSyncEnrichmentEnabled_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].enrichment_pp_sync_enabled = ((CheckBox)sender).Checked ? 1 : 0;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsFuelPressure_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].fuel_pressure = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsFuelKgL_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].fuel_mass_per_cc = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsFuelAFR_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].fuel_afr = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsInjPerformance_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].injector_performance = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
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
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
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
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCntIdleRPMs_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].idle_rotates_count = (int)((NumericUpDown)sender).Value;
            for (int i = 0; i < cs.ConfigStruct.tables[cs.CurrentTable].idle_rotates_count - 1; i++)
                if (cs.ConfigStruct.tables[cs.CurrentTable].idle_rotates[i + 1] <= cs.ConfigStruct.tables[cs.CurrentTable].idle_rotates[i])
                    cs.ConfigStruct.tables[cs.CurrentTable].idle_rotates[i + 1] = cs.ConfigStruct.tables[cs.CurrentTable].idle_rotates[i] + 5;
            UpdateEcuTableValues();
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
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
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCntVoltages_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].voltages_count = (int)((NumericUpDown)sender).Value;
            for (int i = 0; i < cs.ConfigStruct.tables[cs.CurrentTable].voltages_count - 1; i++)
                if (cs.ConfigStruct.tables[cs.CurrentTable].voltages[i + 1] <= cs.ConfigStruct.tables[cs.CurrentTable].voltages[i])
                    cs.ConfigStruct.tables[cs.CurrentTable].voltages[i + 1] = cs.ConfigStruct.tables[cs.CurrentTable].voltages[i] + 1;
            UpdateEcuTableValues();
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCntFillings_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].fillings_count = (int)((NumericUpDown)sender).Value;
            for (int i = 0; i < cs.ConfigStruct.tables[cs.CurrentTable].fillings_count - 1; i++)
                if (cs.ConfigStruct.tables[cs.CurrentTable].fillings[i + 1] <= cs.ConfigStruct.tables[cs.CurrentTable].fillings[i])
                    cs.ConfigStruct.tables[cs.CurrentTable].fillings[i + 1] = cs.ConfigStruct.tables[cs.CurrentTable].fillings[i] + 2;
            UpdateEcuTableValues();
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCntEngineTemps_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].engine_temp_count = (int)((NumericUpDown)sender).Value;
            for (int i = 0; i < cs.ConfigStruct.tables[cs.CurrentTable].engine_temp_count - 1; i++)
                if (cs.ConfigStruct.tables[cs.CurrentTable].engine_temps[i + 1] <= cs.ConfigStruct.tables[cs.CurrentTable].engine_temps[i])
                    cs.ConfigStruct.tables[cs.CurrentTable].engine_temps[i + 1] = cs.ConfigStruct.tables[cs.CurrentTable].engine_temps[i] + 1;
            UpdateEcuTableValues();
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCntSpeeds_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].idle_speeds_shift_count = (int)((NumericUpDown)sender).Value;
            for (int i = 0; i < cs.ConfigStruct.tables[cs.CurrentTable].idle_speeds_shift_count - 1; i++)
                if (cs.ConfigStruct.tables[cs.CurrentTable].idle_rpm_shift_speeds[i + 1] <= cs.ConfigStruct.tables[cs.CurrentTable].idle_rpm_shift_speeds[i])
                    cs.ConfigStruct.tables[cs.CurrentTable].idle_rpm_shift_speeds[i + 1] = cs.ConfigStruct.tables[cs.CurrentTable].idle_rpm_shift_speeds[i] + 2;
            UpdateEcuTableValues();
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCntAirTemps_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].air_temp_count = (int)((NumericUpDown)sender).Value;
            for (int i = 0; i < cs.ConfigStruct.tables[cs.CurrentTable].air_temp_count - 1; i++)
                if (cs.ConfigStruct.tables[cs.CurrentTable].air_temps[i + 1] <= cs.ConfigStruct.tables[cs.CurrentTable].air_temps[i])
                    cs.ConfigStruct.tables[cs.CurrentTable].air_temps[i + 1] = cs.ConfigStruct.tables[cs.CurrentTable].air_temps[i] + 1;
            UpdateEcuTableValues();
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }
        

        private void nudParamsPidShortCorrP_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].short_term_corr_pid_p = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsPidShortCorrI_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].short_term_corr_pid_i = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsPidShortCorrD_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].short_term_corr_pid_d = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCorrInjCy1_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_injection[0] = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCorrInjCy2_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_injection[1] = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCorrInjCy3_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_injection[2] = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCorrInjCy4_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_injection[3] = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCorrIgnCy1_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_ignition[0] = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCorrIgnCy2_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_ignition[1] = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCorrIgnCy3_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_ignition[2] = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCorrIgnCy4_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].cy_corr_ignition[3] = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsEnrPMapTps_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].enrichment_proportion_map_vs_thr = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsKnockIgnCorr_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].knock_ign_corr_max = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsKnockInjCorr_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].knock_inj_corr_max = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsIdleIgnDevMin_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].idle_ign_deviation_min = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsIdleIgnDevMax_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].idle_ign_deviation_max = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsIdleIgnFanLCorr_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].idle_ign_fan_low_corr = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsIdleIgnFanHCorr_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].idle_ign_fan_high_corr = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsIdleAirFanLCorr_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].idle_air_fan_low_corr = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsIdleAirFanHCorr_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].idle_air_fan_high_corr = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            bLiveCheckOld = true;
            cbLive.Checked = true;
        }

        private void btnResetFailures_Click(object sender, EventArgs e)
        {
            middleLayer?.PacketHandler.SendResetStatusRequest();
        }

        private void btnCorrAppendFillingByMAP_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to append Filling by MAP?", "Engine Control Unit", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dialogResult == DialogResult.Yes)
            {
                float[] array2d = cs.ConfigStruct.tables[cs.CurrentTable].fill_by_map;
                float[] corrs2d = cs.ConfigStruct.corrections.fill_by_map;
                int size = array2d.Length;

                for(int i = 0; i < size; i++)
                {
                    array2d[i] *= corrs2d[i] + 1.0F;
                    corrs2d[i] = 0.0F;
                }

                middleLayer?.SyncSave(false);
            }
        }
            

        private void btnCorrAppendIdleValve_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to append Idle Valve to RPM?", "Engine Control Unit", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dialogResult == DialogResult.Yes)
            {
                float[] array2d = cs.ConfigStruct.tables[cs.CurrentTable].idle_valve_to_rpm;
                float[] corrs2d = cs.ConfigStruct.corrections.idle_valve_to_rpm;
                int size = array2d.Length;

                for (int i = 0; i < size; i++)
                {
                    array2d[i] *= corrs2d[i] + 1.0F;
                    corrs2d[i] = 0.0F;
                }

                middleLayer?.SyncSave(false);
            }
        }

        private void btnCorrAppendIgnitions_Click(object sender, EventArgs e)
        {
            //TODO: keep or refactor it? Part/Full throttle feature
            //DialogResult dialogResult = MessageBox.Show("Are you sure you want to append Ignitions?", "Engine Control Unit", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            //if (dialogResult == DialogResult.Yes)
            //{
            //    float[] array2d = cs.ConfigStruct.tables[cs.CurrentTable].ignitions;
            //    float[] corrs2d = cs.ConfigStruct.corrections.ignitions;
            //    int size = array2d.Length;

            //    for (int i = 0; i < size; i++)
            //    {
            //        array2d[i] += corrs2d[i];
            //        corrs2d[i] = 0.0F;
            //    }

            //    middleLayer?.SyncSave(false);
            //}
        }

        private void btnCorrAppendPressureByTPS_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to append Pressure by TPS?", "Engine Control Unit", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dialogResult == DialogResult.Yes)
            {
                float[] array2d = cs.ConfigStruct.tables[cs.CurrentTable].map_by_thr;
                float[] corrs2d = cs.ConfigStruct.corrections.map_by_thr;
                int size = array2d.Length;

                for (int i = 0; i < size; i++)
                {
                    array2d[i] *= corrs2d[i] + 1.0F;
                    corrs2d[i] = 0.0F;
                }

                middleLayer?.SyncSave(false);
            }
        }

        private void btnIITestRun_Click(object sender, EventArgs e)
        {
            byte ignition = 0;
            byte injection = 0;

            ignition |= (byte)(cbIITIgnCy1.Checked ? 1 : 0);
            ignition |= (byte)(cbIITIgnCy2.Checked ? 2 : 0);
            ignition |= (byte)(cbIITIgnCy3.Checked ? 4 : 0);
            ignition |= (byte)(cbIITIgnCy4.Checked ? 8 : 0);

            injection |= (byte)(cbIITInjCy1.Checked ? 1 : 0);
            injection |= (byte)(cbIITInjCy2.Checked ? 2 : 0);
            injection |= (byte)(cbIITInjCy3.Checked ? 4 : 0);
            injection |= (byte)(cbIITInjCy4.Checked ? 8 : 0);

            middleLayer.PacketHandler.SendIgnitionInjectionTestRequest(
                ignition, injection,
                (int)nudIITestCount.Value, (int)nudIITestPeriod.Value * 1000,
                (int)nudIITestIgnPulse.Value, (int)nudIITestInjPulse.Value);
        }

        private void btnIITestAbort_Click(object sender, EventArgs e)
        {
            middleLayer.PacketHandler.SendIgnitionInjectionTestRequest(0, 0, 0, 0, 0, 0);
        }
    }
}
