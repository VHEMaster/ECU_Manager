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
using ECU_Manager.Controls;
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

        public MainForm(MiddleLayer middleLayer)
        {
            ColorTransience colorTransience;
            InitializeComponent();
            middleLayer.RegisterEventHandler(this);

            this.middleLayer = middleLayer;
            this.cs = middleLayer.ComponentStructure;


            middleLayer.SyncLoad(false);

            syncForm = new SyncForm();
            syncForm.ShowDialog();

            pbCheckEngine.Image = MakeImageColored(pbCheckEngine.Image, Color.FromArgb(255, 255, 0));

            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            eCyclicFilling.Initialize(middleLayer.ComponentStructure, Editor2DMode.EcuTable,
                middleLayer.ComponentStructure.ConfigStruct.tables[middleLayer.ComponentStructure.CurrentTable].rotates_count,
                middleLayer.ComponentStructure.ConfigStruct.tables[middleLayer.ComponentStructure.CurrentTable].pressures_count,
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


            eFuelMixtures.Initialize(middleLayer.ComponentStructure, Editor2DMode.EcuTable,
                middleLayer.ComponentStructure.ConfigStruct.tables[middleLayer.ComponentStructure.CurrentTable].rotates_count,
                middleLayer.ComponentStructure.ConfigStruct.tables[middleLayer.ComponentStructure.CurrentTable].fillings_count,
                1.0D, 20.0D, 0.1D, 100.0D, 0.5D, 12.0D, 15.0D, 500, 0.5D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 1, true);
            
            eFuelMixtures.SetConfig("fuel_mixtures", "rotates_count", "fillings_count", "rotates", "fillings");
            eFuelMixtures.SetX("RPM", "RPM", "F0");
            eFuelMixtures.SetY("WishFuelRatio", "FuelRatio", "F1");
            eFuelMixtures.SetD("CyclicAirFlow", "Filling", "F1");
            eFuelMixtures.SetTableEventHandler(ChartUpdateEvent);

            colorTransience = new ColorTransience(5.0F, 20.0F, Color.Gray);
            colorTransience.Add(Color.Black, 5.0F);
            colorTransience.Add(Color.Red, 12.0F);
            colorTransience.Add(Color.FromArgb(128, 128, 0), 13.7F);
            colorTransience.Add(Color.Green, 14.7F);
            colorTransience.Add(Color.FromArgb(0, 72, 180), 15.6F);
            colorTransience.Add(Color.Blue, 18.0F);
            colorTransience.Add(Color.DeepSkyBlue, 20.0F);

            eFuelMixtures.SetTableColorTrans(colorTransience);


            eInjectionPhase.Initialize(middleLayer.ComponentStructure, Editor2DMode.EcuTable,
                middleLayer.ComponentStructure.ConfigStruct.tables[middleLayer.ComponentStructure.CurrentTable].rotates_count,
                middleLayer.ComponentStructure.ConfigStruct.tables[middleLayer.ComponentStructure.CurrentTable].fillings_count,
                0.0D, 720.0, 5.0D, 100.0D, 10D, 100.0D, 400.0D, 500, 50D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 0);

            eInjectionPhase.SetConfig("injection_phase", "rotates_count", "fillings_count", "rotates", "fillings");
            eInjectionPhase.SetX("RPM", "RPM", "F0");
            eInjectionPhase.SetY("InjectionPhase", "Phase", "F0");
            eInjectionPhase.SetD("CyclicAirFlow", "Filling", "F1");
            eInjectionPhase.SetTableEventHandler(ChartUpdateEvent);

            colorTransience = new ColorTransience(100.0F, 600, Color.Gray);
            colorTransience.Add(Color.DeepSkyBlue, 100);
            colorTransience.Add(Color.Blue, 170);
            colorTransience.Add(Color.FromArgb(0, 72, 180), 200);
            colorTransience.Add(Color.Green, 250);
            colorTransience.Add(Color.FromArgb(128, 128, 0), 280);
            colorTransience.Add(Color.Red, 350);
            colorTransience.Add(Color.Black, 600);

            eInjectionPhase.SetTableColorTrans(colorTransience);


            eIgnition.Initialize(middleLayer.ComponentStructure, Editor2DMode.EcuTable,
                middleLayer.ComponentStructure.ConfigStruct.tables[middleLayer.ComponentStructure.CurrentTable].rotates_count,
                middleLayer.ComponentStructure.ConfigStruct.tables[middleLayer.ComponentStructure.CurrentTable].fillings_count,
                -45, 90, 0.1D, 100.0D, 0.5D, 0.0D, 45.0D, 500, 5, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 1);


            eIgnition.SetConfig("ignitions", "rotates_count", "fillings_count", "rotates", "fillings");
            eIgnition.SetX("RPM", "RPM", "F0");
            eIgnition.SetY("IgnitionAngle", "Ignition", "F1");
            eIgnition.SetD("CyclicAirFlow", "Filling", "F1");
            eIgnition.SetTableEventHandler(ChartUpdateEvent);

            colorTransience = new ColorTransience(-15, 45, Color.Gray);
            colorTransience.Add(Color.DeepSkyBlue, -15);
            colorTransience.Add(Color.Orange, 15);
            colorTransience.Add(Color.Red, 45);

            eIgnition.SetTableColorTrans(colorTransience);

            eSaturationPulse.Initialize(middleLayer.ComponentStructure, 0, 10000, 100, 500, 0, 5000, 1, 500, 0);
            eSaturationPulse.SetConfig("ignition_time", "voltages_count", "voltages");
            eSaturationPulse.SetX("PowerVoltage", "Voltage", "F1");
            eSaturationPulse.SetY("IgnitionPulse", "Pulse", "F0");
            eSaturationPulse.SetTableEventHandler(ChartUpdateEvent);

            eSatByRPM.Initialize(middleLayer.ComponentStructure, 0.1D, 10D, 0.1D, 0.2D, 0.2D, 2.0F, 500, 0.2D, 1);
            eSatByRPM.SetConfig("ignition_time_rpm_mult", "rotates_count", "rotates");
            eSatByRPM.SetX("RPM", "RPM", "F0");
            eSatByRPM.SetTableEventHandler(ChartUpdateEvent);

            eInjectorLag.Initialize(middleLayer.ComponentStructure, 0, 10, 0.01D, 0.1D, 0, 2, 1, 0.2, 2);
            eInjectorLag.SetConfig("injector_lag", "voltages_count", "voltages");
            eInjectorLag.SetX("PowerVoltage", "Voltage", "F1");
            eInjectorLag.SetY("InjectionLag", "Lag", "F2");
            eInjectorLag.SetTableEventHandler(ChartUpdateEvent);

            eEnrichmentByMAP.Initialize(middleLayer.ComponentStructure, 0, 5, 0.001D, 0.1D, 0D, 1.0F, 5000, 0.1D, 3);
            eEnrichmentByMAP.SetConfig("enrichment_by_map_sens", "pressures_count", "pressures");
            eEnrichmentByMAP.SetX("ManifoldAirPressure", "MAP", "F0");
            eEnrichmentByMAP.SetY("InjectionEnrichment", "Enr.", "F3");
            eEnrichmentByMAP.SetTableEventHandler(ChartUpdateEvent);

            eEnrichmentByTPS.Initialize(middleLayer.ComponentStructure, 0, 5, 0.001D, 0.1D, 0D, 1.0F, 5, 0.1D, 3);
            eEnrichmentByTPS.SetConfig("enrichment_by_thr_sens", "throttles_count", "throttles");
            eEnrichmentByTPS.SetX("ThrottlePosition", "TPS", "F0");
            eEnrichmentByTPS.SetY("InjectionEnrichment", "Enr.", "F3");
            eEnrichmentByTPS.SetTableEventHandler(ChartUpdateEvent);

            eEnrichmentMAPHPF.Initialize(middleLayer.ComponentStructure, 0, 1, 0.001D, 0.1D, 0D, 0.5F, 500, 0.05D, 3);
            eEnrichmentMAPHPF.SetConfig("enrichment_by_map_hpf", "rotates_count", "rotates");
            eEnrichmentMAPHPF.SetX("RPM", "RPM", "F0");
            eEnrichmentMAPHPF.SetTableEventHandler(ChartUpdateEvent);

            eEnrichmentTPSHPF.Initialize(middleLayer.ComponentStructure, 0, 1, 0.001D, 0.1D, 0D, 0.5F, 500, 0.05D, 3);
            eEnrichmentTPSHPF.SetConfig("enrichment_by_thr_hpf", "rotates_count", "rotates");
            eEnrichmentTPSHPF.SetX("RPM", "RPM", "F0");
            eEnrichmentTPSHPF.SetTableEventHandler(ChartUpdateEvent);

            ePressures.Initialize(middleLayer.ComponentStructure, 0, 1000000, 200, 500, 0, 100000, 1, 10000, 0);
            ePressures.SetConfig("pressures", "pressures_count", string.Empty);
            ePressures.SetY("ManifoldAirPressure", "MAP", "F0");
            ePressures.SetTableEventHandler(ChartUpdateEvent);

            eRotates.Initialize(middleLayer.ComponentStructure, 0, 10000, 50, 100, 0, 8000, 1, 500, 0);
            eRotates.SetConfig("rotates", "rotates_count", string.Empty);
            eRotates.SetY("RPM", "RPM", "F0");
            eRotates.SetTableEventHandler(ChartUpdateEvent);

            eThrottles.Initialize(middleLayer.ComponentStructure, 0, 100, 1, 5, 0, 100, 1, 10, 1);
            eThrottles.SetConfig("throttles", "throttles_count", string.Empty);
            eThrottles.SetY("ThrottlePosition", "TPS", "F1");
            eThrottles.SetTableEventHandler(ChartUpdateEvent);

            eFillings.Initialize(middleLayer.ComponentStructure, 0, 5000, 1, 2, 0, 350, 1, 50, 0);
            eFillings.SetConfig("fillings", "fillings_count", string.Empty);
            eFillings.SetY("CyclicAirFlow", "Filling", "F1");
            eFillings.SetTableEventHandler(ChartUpdateEvent);

            eVoltages.Initialize(middleLayer.ComponentStructure, 0, 25, 0.1D, 1, 0, 15, 1, 2, 1);
            eVoltages.SetConfig("voltages", "voltages_count", string.Empty);
            eVoltages.SetY("PowerVoltage", "Voltage", "F1");
            eVoltages.SetTableEventHandler(ChartUpdateEvent);

            eEngTemps.Initialize(middleLayer.ComponentStructure, -50, 150, 1, 1, 0, 100, 1, 10, 0);
            eEngTemps.SetConfig("engine_temps", "engine_temp_count", string.Empty);
            eEngTemps.SetY("EngineTemp", "Temperature", "F1");
            eEngTemps.SetTableEventHandler(ChartUpdateEvent);

            eSpeeds.Initialize(middleLayer.ComponentStructure, 0, 990, 5, 1, 0, 100, 1, 10, 0);
            eSpeeds.SetConfig("idle_rpm_shift_speeds", "idle_speeds_shift_count", string.Empty);
            eSpeeds.SetY("Speed", "Speed", "F1");
            eSpeeds.SetTableEventHandler(ChartUpdateEvent);

            
            ePressureByRPMvsTPS.Initialize(middleLayer.ComponentStructure, Editor2DMode.EcuTable,
                middleLayer.ComponentStructure.ConfigStruct.tables[middleLayer.ComponentStructure.CurrentTable].rotates_count,
                middleLayer.ComponentStructure.ConfigStruct.tables[middleLayer.ComponentStructure.CurrentTable].throttles_count,
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

            eIdleWishRPM.Initialize(middleLayer.ComponentStructure, 100D, 10000D, 20D, 100D, 500D, 2000D, 10D, 100D, 0);
            eIdleWishRPM.SetConfig("idle_wish_rotates", "engine_temp_count", "engine_temps");
            eIdleWishRPM.SetX("EngineTemp", "Temp.", "F1");
            eIdleWishRPM.SetY("RPM", "RPM", "F0");
            eIdleWishRPM.SetTableEventHandler(ChartUpdateEvent);

            eIdleWishMassAirFlow.Initialize(middleLayer.ComponentStructure, 0, 500D, 0.1D, 5D, 0, 30D, 10D, 5D, 1);
            eIdleWishMassAirFlow.SetConfig("idle_wish_massair", "engine_temp_count", "engine_temps");
            eIdleWishMassAirFlow.SetX("EngineTemp", "Temp.", "F1");
            eIdleWishMassAirFlow.SetY("MassAirFlow", "Mass Air Flow", "F1");
            eIdleWishMassAirFlow.SetTableEventHandler(ChartUpdateEvent);
            
            eIdleWishIgnition.Initialize(middleLayer.ComponentStructure, -15D, 60D, 0.1D, 5D, 10D, 20D, 500D, 2D, 1);
            eIdleWishIgnition.SetConfig("idle_wish_ignition", "rotates_count", "rotates");
            eIdleWishIgnition.SetX("RPM", "RPM", "F0");
            eIdleWishIgnition.SetY("IgnitionAngle", "Ignition", "F1");
            eIdleWishIgnition.SetTableEventHandler(ChartUpdateEvent);
            
            eIdleSpeedShift.Initialize(middleLayer.ComponentStructure, 0, 2000, 20D, 10D, 0, 100, 10D, 20D, 0);
            eIdleSpeedShift.SetConfig("idle_rpm_shift", "idle_speeds_shift_count", "idle_rpm_shift_speeds");
            eIdleSpeedShift.SetX("Speed", "Speed", "F1");
            eIdleSpeedShift.SetY("IdleSpeedShift", "RPM Shift", "F0");
            eIdleSpeedShift.SetTableEventHandler(ChartUpdateEvent);


            eIdleValveVsRpm.Initialize(middleLayer.ComponentStructure, Editor2DMode.EcuTable,
                middleLayer.ComponentStructure.ConfigStruct.tables[middleLayer.ComponentStructure.CurrentTable].rotates_count,
                middleLayer.ComponentStructure.ConfigStruct.tables[middleLayer.ComponentStructure.CurrentTable].engine_temp_count,
                0, 255, 1, 100.0D, 1D, 0.0D, 50D, 500, 5, Consts.TABLE_ROTATES_MAX, Consts.TABLE_TEMPERATURES_MAX, 0);


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

            eStartupMixture.Initialize(middleLayer.ComponentStructure, 1, 20, 0.1D, 1D, 8, 14, 10D, 0.5D, 1);
            eStartupMixture.SetConfig("start_mixtures", "engine_temp_count", "engine_temps");
            eStartupMixture.SetX("EngineTemp", "Temp.", "F1");
            eStartupMixture.SetY("WishFuelRatio", "Fuel Ratio", "F1");
            eStartupMixture.SetTableEventHandler(ChartUpdateEvent);

            eWarmupMixture.Initialize(middleLayer.ComponentStructure, 1, 20, 0.1D, 1D, 8, 14, 10D, 0.5D, 1);
            eWarmupMixture.SetConfig("warmup_mixtures", "engine_temp_count", "engine_temps");
            eWarmupMixture.SetX("EngineTemp", "Temp.", "F1");
            eWarmupMixture.SetY("WishFuelRatio", "Fuel Ratio", "F1");
            eWarmupMixture.SetTableEventHandler(ChartUpdateEvent);

            eWarmupMixKoffs.Initialize(middleLayer.ComponentStructure, 0, 1, 0.01D, 0.1D, 0, 1, 10D, 0.1D, 2);
            eWarmupMixKoffs.SetConfig("warmup_mix_koffs", "engine_temp_count", "engine_temps");
            eWarmupMixKoffs.SetX("EngineTemp", "Temp.", "F1");
            eWarmupMixKoffs.SetTableEventHandler(ChartUpdateEvent);

            eKnockNoiseLevel.Initialize(middleLayer.ComponentStructure, 0, 5, 0.01D, 0.2D, 0D, 1D, 500, 0.2D, 2);
            eKnockNoiseLevel.SetConfig("knock_noise_level", "rotates_count", "rotates");
            eKnockNoiseLevel.SetX("RPM", "RPM", "F0");
            eKnockNoiseLevel.SetY("KnockSensor", "Knock Level", "F2");
            eKnockNoiseLevel.SetTableEventHandler(ChartUpdateEvent);

            eKnockThreshold.Initialize(middleLayer.ComponentStructure, 0, 5, 0.01D, 0.2D, 0D, 1D, 500, 0.2D, 2);
            eKnockThreshold.SetConfig("knock_threshold", "rotates_count", "rotates");
            eKnockThreshold.SetX("RPM", "RPM", "F0");
            eKnockThreshold.SetY("KnockSensorFiltered", "Knock Level", "F2");
            eKnockThreshold.SetTableEventHandler(ChartUpdateEvent);



            colorTransience = new ColorTransience(-1.0F, 1.0F, Color.Gray);
            colorTransience.Add(Color.DeepSkyBlue, -1.0F);
            colorTransience.Add(Color.Blue, -0.5F);
            colorTransience.Add(Color.FromArgb(0, 128, 255), -0.1F);
            colorTransience.Add(Color.Green, 0.0F);
            colorTransience.Add(Color.FromArgb(192, 128, 0), 0.1F);
            colorTransience.Add(Color.Red, 0.5F);
            colorTransience.Add(Color.DarkRed, 1.0F);

            eCorrsFillByMAP.Initialize(middleLayer.ComponentStructure, Editor2DMode.CorrectionsTable,
                middleLayer.ComponentStructure.ConfigStruct.tables[middleLayer.ComponentStructure.CurrentTable].rotates_count,
                middleLayer.ComponentStructure.ConfigStruct.tables[middleLayer.ComponentStructure.CurrentTable].pressures_count,
                -10.0D, 10.0D, 0.005D, 100.0D, 0.1D, -0.2D, 0.2D, 500, 0.1D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_PRESSURES_MAX, 3);

            eCorrsFillByMAP.SetConfig("fill_by_map", "rotates_count", "pressures_count", "rotates", "pressures");
            eCorrsFillByMAP.SetX("RPM", "RPM", "F0");
            eCorrsFillByMAP.SetY(string.Empty, "Correction", "F3");
            eCorrsFillByMAP.SetD("ManifoldAirPressure", "MAP", "F1");
            eCorrsFillByMAP.SetTableEventHandler(ChartCorrectionEvent);
            eCorrsFillByMAP.scHorisontal.SplitterDistance = (int)Math.Round(eCorrsFillByMAP.scHorisontal.Width * 0.75);

            eCorrsFillByMAP.SetTableColorTrans(colorTransience);
            eCorrsFillByMAP.SynchronizeChart();


            eCorrsPressureByTPS.Initialize(middleLayer.ComponentStructure, Editor2DMode.CorrectionsTable,
                middleLayer.ComponentStructure.ConfigStruct.tables[middleLayer.ComponentStructure.CurrentTable].rotates_count,
                middleLayer.ComponentStructure.ConfigStruct.tables[middleLayer.ComponentStructure.CurrentTable].throttles_count,
                -10.0D, 10.0D, 0.005D, 100.0D, 0.1D, -0.2D, 0.2D, 500, 0.1D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_THROTTLES_MAX, 3);

            eCorrsPressureByTPS.SetConfig("map_by_thr", "rotates_count", "throttles_count", "rotates", "throttles");
            eCorrsPressureByTPS.SetX("RPM", "RPM", "F0");
            eCorrsPressureByTPS.SetY(string.Empty, "Correction", "F3");
            eCorrsPressureByTPS.SetD("ThrottlePosition", "TPS", "F1");
            eCorrsPressureByTPS.SetTableEventHandler(ChartCorrectionEvent);
            eCorrsPressureByTPS.scHorisontal.SplitterDistance = (int)Math.Round(eCorrsPressureByTPS.scHorisontal.Width * 0.75);

            eCorrsPressureByTPS.SetTableColorTrans(colorTransience);
            eCorrsPressureByTPS.SynchronizeChart();


            eCorrsIdleValveToRPM.Initialize(middleLayer.ComponentStructure, Editor2DMode.CorrectionsTable,
                middleLayer.ComponentStructure.ConfigStruct.tables[middleLayer.ComponentStructure.CurrentTable].rotates_count,
                middleLayer.ComponentStructure.ConfigStruct.tables[middleLayer.ComponentStructure.CurrentTable].engine_temp_count,
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

            eCorrsIgnition.Initialize(middleLayer.ComponentStructure, Editor2DMode.CorrectionsTable,
                middleLayer.ComponentStructure.ConfigStruct.tables[middleLayer.ComponentStructure.CurrentTable].rotates_count,
                middleLayer.ComponentStructure.ConfigStruct.tables[middleLayer.ComponentStructure.CurrentTable].fillings_count,
                -45.0D, 45.0D, 0.1D, 100.0D, 1D, -5D, 5D, 500, 1D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 2);

            eCorrsIgnition.SetConfig("ignitions", "rotates_count", "fillings_count", "rotates", "fillings");
            eCorrsIgnition.SetX("RPM", "RPM", "F0");
            eCorrsIgnition.SetY(string.Empty, "Ignition Correction", "F2");
            eCorrsIgnition.SetD("CyclicAirFlow", "Filling", "F1");
            eCorrsIgnition.SetTableEventHandler(ChartCorrectionEvent);
            eCorrsIgnition.scHorisontal.SplitterDistance = (int)Math.Round(eCorrsIgnition.scHorisontal.Width * 0.7);

            eCorrsIgnition.SetTableColorTrans(colorTransience);
            eCorrsIgnition.SynchronizeChart();

            SynchronizeCharts();
        }

        private void SynchronizeCharts()
        {
            eCyclicFilling.SynchronizeChart();
            eFuelMixtures.SynchronizeChart();
            eInjectionPhase.SynchronizeChart();
            eIgnition.SynchronizeChart();
            ePressureByRPMvsTPS.SynchronizeChart();
            eIdleValveVsRpm.SynchronizeChart();
            eCorrsFillByMAP.SynchronizeChart();
            eCorrsIdleValveToRPM.SynchronizeChart();
            eCorrsIgnition.SynchronizeChart();
            eCorrsPressureByTPS.SynchronizeChart();
            UpdateCharts();
        }
        private void UpdateCharts()
        {
            eCyclicFilling.UpdateChart();
            eFuelMixtures.UpdateChart();
            eInjectionPhase.UpdateChart();
            eIgnition.UpdateChart();
            ePressureByRPMvsTPS.UpdateChart();
            eSatByRPM.UpdateChart();
            eInjectorLag.UpdateChart();
            eSaturationPulse.UpdateChart();
            eEnrichmentByMAP.UpdateChart();
            eEnrichmentByTPS.UpdateChart();
            eEnrichmentMAPHPF.UpdateChart();
            eEnrichmentTPSHPF.UpdateChart();

            ePressures.UpdateChart();
            eRotates.UpdateChart();
            eThrottles.UpdateChart();
            eEngTemps.UpdateChart();
            eFillings.UpdateChart();
            eSpeeds.UpdateChart();
            eVoltages.UpdateChart();

            eIdleWishRPM.UpdateChart();
            eIdleWishIgnition.UpdateChart();
            eIdleWishMassAirFlow.UpdateChart();
            eIdleSpeedShift.UpdateChart();
            eIdleValveVsRpm.UpdateChart();

            eStartupMixture.UpdateChart();
            eWarmupMixture.UpdateChart();
            eWarmupMixKoffs.UpdateChart();

            eKnockThreshold.UpdateChart();
            eKnockNoiseLevel.UpdateChart();

            eCorrsFillByMAP.UpdateChart();
            eCorrsIdleValveToRPM.UpdateChart();
            eCorrsIgnition.UpdateChart();
            eCorrsPressureByTPS.UpdateChart();
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
        }

        private void ChartUpdateEvent(object sender, EventArgs e)
        {
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void ChartCorrectionEvent(object sender, EventArgs e)
        {
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
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
            if (syncForm.InvokeRequired)
                syncForm.BeginInvoke(new Action(() => syncForm.CloseForm()));
            else syncForm.Close();

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
                    middleLayer.PacketHandler.SendParametersRequest();  
                }

                SynchronizeCharts();
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
            nudParamsCntSpeeds.Maximum = Consts.TABLE_SPEEDS_MAX;

            nudParamsCntPress.Value = cs.ConfigStruct.tables[cs.CurrentTable].pressures_count;
            nudParamsCntRPMs.Value = cs.ConfigStruct.tables[cs.CurrentTable].rotates_count;
            nudParamsCntThrottles.Value = cs.ConfigStruct.tables[cs.CurrentTable].throttles_count;
            nudParamsCntVoltages.Value = cs.ConfigStruct.tables[cs.CurrentTable].voltages_count;
            nudParamsCntFillings.Value = cs.ConfigStruct.tables[cs.CurrentTable].fillings_count;
            nudParamsCntEngineTemps.Value = cs.ConfigStruct.tables[cs.CurrentTable].engine_temp_count;
            nudParamsCntSpeeds.Value = cs.ConfigStruct.tables[cs.CurrentTable].idle_speeds_shift_count;

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
            middleLayer.SyncFast();
            middleLayer.PacketHandler.SendStatusRequest();
            if (middleLayer.ComponentStructure.EcuParameters.IsCheckEngine > 0)
                pbCheckEngine.Visible = !pbCheckEngine.Visible;
            else pbCheckEngine.Visible = false;
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

        private void btnCorrStart_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Previous calibration result will be lost!\r\nAre you sure you want to perform calibration?", "Engine Control Unit", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            if (dialogResult == DialogResult.Yes)
            {
                if (!middleLayer.IsSynchronizing)
                {
                    cs.ConfigStruct.parameters.performAdaptation = 1;
                    CorrStart();
                    middleLayer.UpdateConfig();
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
            bool check = ((CheckBox)sender).Checked;
            if (check != bLiveCheckOld)
            {
                bLiveCheckOld = check;
                middleLayer.SyncSave(false);
            }
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
                if (cs.ConfigStruct.tables[cs.CurrentTable].voltages[i + 1] <= cs.ConfigStruct.tables[cs.CurrentTable].voltages[i])
                    cs.ConfigStruct.tables[cs.CurrentTable].voltages[i + 1] = cs.ConfigStruct.tables[cs.CurrentTable].voltages[i] + 1;
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
                    cs.ConfigStruct.tables[cs.CurrentTable].fillings[i + 1] = cs.ConfigStruct.tables[cs.CurrentTable].fillings[i] + 5;
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
                    cs.ConfigStruct.tables[cs.CurrentTable].engine_temps[i + 1] = cs.ConfigStruct.tables[cs.CurrentTable].engine_temps[i] + 5;
            UpdateEcuTableValues();
            if (!middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCntSpeeds_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].idle_speeds_shift_count = (int)((NumericUpDown)sender).Value;
            for (int i = 0; i < cs.ConfigStruct.tables[cs.CurrentTable].idle_speeds_shift_count - 1; i++)
                if (cs.ConfigStruct.tables[cs.CurrentTable].idle_rpm_shift_speeds[i + 1] <= cs.ConfigStruct.tables[cs.CurrentTable].idle_rpm_shift_speeds[i])
                    cs.ConfigStruct.tables[cs.CurrentTable].idle_rpm_shift_speeds[i + 1] = cs.ConfigStruct.tables[cs.CurrentTable].idle_rpm_shift_speeds[i] + 5;
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

        private void MainForm_Load(object sender, EventArgs e)
        {
            bLiveCheckOld = true;
            cbLive.Checked = true;
        }

        private void btnResetFailures_Click(object sender, EventArgs e)
        {
            middleLayer.PacketHandler.SendResetStatusRequest();
        }
    }
}
