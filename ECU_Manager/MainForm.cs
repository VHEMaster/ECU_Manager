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
using ECU_Framework.Packets;
using ECU_Framework.Structs;
using ECU_Framework.Tools;
using ECU_Framework;

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

            eCyclicFillingMAP.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].pressures_count,
                0.0D, 2.0D, 0.001D, 100.0D, 0.2D, 0.6D, 1.2D, 500, 0.1D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_PRESSURES_MAX, 4);

            eCyclicFillingMAP.SetConfig("filling_gbc_map", "rotates_count", "pressures_count", "rotates", "pressures");
            eCyclicFillingMAP.SetX("RPM", "RPM", "F0");
            eCyclicFillingMAP.SetY(string.Empty, "Fill", "F2");
            eCyclicFillingMAP.SetD("ManifoldAirPressure", "Pressure", "F0");
            eCyclicFillingMAP.SetTableEventHandler(ChartUpdateEvent);

            colorTransience = new ColorTransience(0.1F, 1.5F, Color.Gray);
            colorTransience.Add(Color.FromArgb(0, 128, 255), 0.1F);
            colorTransience.Add(Color.Blue, 0.4F);
            colorTransience.Add(Color.FromArgb(0, 92, 160), 0.7F);
            colorTransience.Add(Color.Green, 1.0F);
            colorTransience.Add(Color.FromArgb(128, 96, 0), 1.05F);
            colorTransience.Add(Color.DarkRed, 1.12F);
            colorTransience.Add(Color.Black, 1.5F);

            eCyclicFillingMAP.SetTableColorTrans(colorTransience);
            eCyclicFillingMAP.SynchronizeChart();


            eCyclicFillingTPS.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].pressures_count,
                0.0D, 2.0D, 0.001D, 100.0D, 0.2D, 0.6D, 1.2D, 500, 0.1D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_PRESSURES_MAX, 4);

            eCyclicFillingTPS.SetConfig("filling_gbc_tps", "rotates_count", "throttles_count", "rotates", "throttles");
            eCyclicFillingTPS.SetX("RPM", "RPM", "F0");
            eCyclicFillingTPS.SetY(string.Empty, "Fill", "F2");
            eCyclicFillingTPS.SetD("ThrottlePosition", "Throttle", "F1");
            eCyclicFillingTPS.SetTableEventHandler(ChartUpdateEvent);

            colorTransience = new ColorTransience(0.1F, 1.5F, Color.Gray);
            colorTransience.Add(Color.FromArgb(0, 128, 255), 0.1F);
            colorTransience.Add(Color.Blue, 0.4F);
            colorTransience.Add(Color.FromArgb(0, 92, 160), 0.7F);
            colorTransience.Add(Color.Green, 1.0F);
            colorTransience.Add(Color.FromArgb(128, 96, 0), 1.05F);
            colorTransience.Add(Color.DarkRed, 1.12F);
            colorTransience.Add(Color.Black, 1.5F);

            eCyclicFillingTPS.SetTableColorTrans(colorTransience);
            eCyclicFillingTPS.SynchronizeChart();

            colorTransience = new ColorTransience(5.0F, 20.0F, Color.Gray);
            colorTransience.Add(Color.Black, 5.0F);
            colorTransience.Add(Color.Red, 12.0F);
            colorTransience.Add(Color.FromArgb(128, 128, 0), 13.7F);
            colorTransience.Add(Color.Green, 14.7F);
            colorTransience.Add(Color.FromArgb(0, 72, 180), 15.6F);
            colorTransience.Add(Color.Blue, 18.0F);
            colorTransience.Add(Color.DeepSkyBlue, 20.0F);


            eFuelMixturesFull.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                1.0D, 20.0D, 0.1D, 100.0D, 0.5D, 12.0D, 15.0D, 500, 0.5D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 1, true);

            eFuelMixturesFull.SetConfig("fuel_mixtures", "rotates_count", "fillings_count", "rotates", "fillings");
            eFuelMixturesFull.SetX("RPM", "RPM", "F0");
            eFuelMixturesFull.SetY("WishFuelRatio", "FuelRatio", "F1");
            eFuelMixturesFull.SetD("CyclicAirFlow", "Filling", "F1");
            eFuelMixturesFull.SetTableEventHandler(ChartUpdateEvent);

            eFuelMixturesFull.SetTableColorTrans(colorTransience);
            

            colorTransience = new ColorTransience(0.0F, 100.0F, Color.Gray);
            colorTransience.Add(Color.DeepSkyBlue, 0.0F);
            colorTransience.Add(Color.Blue, 5.0F);
            colorTransience.Add(Color.FromArgb(0, 72, 180), 10.0F);
            colorTransience.Add(Color.Green, 20.0F);
            colorTransience.Add(Color.FromArgb(128, 128, 0), 60.0F);
            colorTransience.Add(Color.Red, 80.0F);
            colorTransience.Add(Color.Black, 100.0F);


            eEtcPositions2D.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].pedals_count,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                0.0D, 100.0D, 0.5D, 10.0f, 10.0D, 0.0D, 100.0D, 10.0D, 10.0D, Consts.TABLE_PEDALS_MAX, Consts.TABLE_ROTATES_MAX, 1, false);

            eEtcPositions2D.SetConfig("throttle_position", "pedals_count", "rotates_count", "pedals", "rotates");
            eEtcPositions2D.SetX("PedalPosition", "Pedal", "F1");
            eEtcPositions2D.SetY(string.Empty, "Throttle", "F1");
            eEtcPositions2D.SetD("RPM", "RPM", "F0");
            eEtcPositions2D.SetTableEventHandler(ChartUpdateEvent);

            eEtcPositions2D.SetTableColorTrans(colorTransience);


            colorTransience = new ColorTransience(100.0F, 600, Color.Gray);
            colorTransience.Add(Color.DeepSkyBlue, 100);
            colorTransience.Add(Color.Blue, 170);
            colorTransience.Add(Color.FromArgb(0, 72, 180), 200);
            colorTransience.Add(Color.Green, 250);
            colorTransience.Add(Color.FromArgb(128, 128, 0), 280);
            colorTransience.Add(Color.Red, 350);
            colorTransience.Add(Color.Black, 600);

            eInjectionPhaseFull.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                0.0D, 720.0, 5.0D, 100.0D, 10D, 100.0D, 400.0D, 500, 50D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 0);

            eInjectionPhaseFull.SetConfig("injection_phase", "rotates_count", "fillings_count", "rotates", "fillings");
            eInjectionPhaseFull.SetX("RPM", "RPM", "F0");
            eInjectionPhaseFull.SetY("InjectionPhase", "Phase", "F0");
            eInjectionPhaseFull.SetD("CyclicAirFlow", "Filling", "F1");
            eInjectionPhaseFull.SetTableEventHandler(ChartUpdateEvent);

            eInjectionPhaseFull.SetTableColorTrans(colorTransience);

            colorTransience = new ColorTransience(-15, 45, Color.Gray);
            colorTransience.Add(Color.DeepSkyBlue, -15);
            colorTransience.Add(Color.Orange, 15);
            colorTransience.Add(Color.Red, 45);
            

            eIgnitionFull.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                -45, 90, 0.1D, 100.0D, 0.5D, 0.0D, 45.0D, 500, 5, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 1);

            eIgnitionFull.SetConfig("ignitions", "rotates_count", "fillings_count", "rotates", "fillings");
            eIgnitionFull.SetX("RPM", "RPM", "F0");
            eIgnitionFull.SetY("IgnitionAdvance", "Ignition", "F1");
            eIgnitionFull.SetD("CyclicAirFlow", "Filling", "F1");
            eIgnitionFull.SetTableEventHandler(ChartUpdateEvent);

            eIgnitionFull.SetTableColorTrans(colorTransience);
            
            colorTransience = new ColorTransience(-10.0F, 10.0F, Color.Gray);
            colorTransience.Add(Color.DeepSkyBlue, -10.0F);
            colorTransience.Add(Color.Blue, -5.0F);
            colorTransience.Add(Color.FromArgb(0, 128, 255), -2.0F);
            colorTransience.Add(Color.Green, 0.0F);
            colorTransience.Add(Color.FromArgb(192, 128, 0), 2.0F);
            colorTransience.Add(Color.Red, 3.0F);
            colorTransience.Add(Color.DarkRed, 5.0F);
            colorTransience.Add(Color.Black, 10.0F);

            eIgnitionFullCy1.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                -45.0D, 45.0D, 0.1D, 100.0D, 1D, -5D, 5D, 500, 1D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 2);

            eIgnitionFullCy1.SetConfig("ignition_corr_cy1", "rotates_count", "fillings_count", "rotates", "fillings");
            eIgnitionFullCy1.SetX("RPM", "RPM", "F0");
            eIgnitionFullCy1.SetY(string.Empty, "Ignition Corr.Cy1", "F2");
            eIgnitionFullCy1.SetD("CyclicAirFlow", "Filling", "F1");
            eIgnitionFullCy1.SetTableEventHandler(ChartUpdateEvent);
            eIgnitionFullCy1.scHorisontal.SplitterDistance = (int)Math.Round(eIgnitionFullCy1.scHorisontal.Width * 0.8);

            eIgnitionFullCy1.SetTableColorTrans(colorTransience);
            eIgnitionFullCy1.SynchronizeChart();



            eIgnitionFullCy2.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                -45.0D, 45.0D, 0.1D, 100.0D, 1D, -5D, 5D, 500, 1D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 2);

            eIgnitionFullCy2.SetConfig("ignition_corr_cy2", "rotates_count", "fillings_count", "rotates", "fillings");
            eIgnitionFullCy2.SetX("RPM", "RPM", "F0");
            eIgnitionFullCy2.SetY(string.Empty, "Ignition Corr.Cy1", "F2");
            eIgnitionFullCy2.SetD("CyclicAirFlow", "Filling", "F1");
            eIgnitionFullCy2.SetTableEventHandler(ChartUpdateEvent);
            eIgnitionFullCy2.scHorisontal.SplitterDistance = (int)Math.Round(eIgnitionFullCy2.scHorisontal.Width * 0.8);

            eIgnitionFullCy2.SetTableColorTrans(colorTransience);
            eIgnitionFullCy2.SynchronizeChart();



            eIgnitionFullCy3.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                -45.0D, 45.0D, 0.1D, 100.0D, 1D, -5D, 5D, 500, 1D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 2);

            eIgnitionFullCy3.SetConfig("ignition_corr_cy3", "rotates_count", "fillings_count", "rotates", "fillings");
            eIgnitionFullCy3.SetX("RPM", "RPM", "F0");
            eIgnitionFullCy3.SetY(string.Empty, "Ignition Corr.Cy1", "F2");
            eIgnitionFullCy3.SetD("CyclicAirFlow", "Filling", "F1");
            eIgnitionFullCy3.SetTableEventHandler(ChartUpdateEvent);
            eIgnitionFullCy3.scHorisontal.SplitterDistance = (int)Math.Round(eIgnitionFullCy3.scHorisontal.Width * 0.8);

            eIgnitionFullCy3.SetTableColorTrans(colorTransience);
            eIgnitionFullCy3.SynchronizeChart();



            eIgnitionFullCy4.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                -45.0D, 45.0D, 0.1D, 100.0D, 1D, -5D, 5D, 500, 1D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 2);

            eIgnitionFullCy4.SetConfig("ignition_corr_cy4", "rotates_count", "fillings_count", "rotates", "fillings");
            eIgnitionFullCy4.SetX("RPM", "RPM", "F0");
            eIgnitionFullCy4.SetY(string.Empty, "Ignition Corr.Cy1", "F2");
            eIgnitionFullCy4.SetD("CyclicAirFlow", "Filling", "F1");
            eIgnitionFullCy4.SetTableEventHandler(ChartUpdateEvent);
            eIgnitionFullCy4.scHorisontal.SplitterDistance = (int)Math.Round(eIgnitionFullCy4.scHorisontal.Width * 0.8);

            eIgnitionFullCy4.SetTableColorTrans(colorTransience);
            eIgnitionFullCy4.SynchronizeChart();


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

            eFillingSelectKoffTPS.Initialize(cs, 0, 1, 0.01D, 0.1D, 0D, 1.0F, 500, 0.1D, 2);
            eFillingSelectKoffTPS.SetConfig("filling_select_koff_tps", "rotates_count", "rotates");
            eFillingSelectKoffTPS.SetX("RPM", "RPM", "F0");
            eFillingSelectKoffTPS.SetTableEventHandler(ChartUpdateEvent);

            eEnrichmentStartLoad.Initialize(cs, 0, 100000, 1, 1D, 0D, 10D, 1D, 10D, 0);
            eEnrichmentStartLoad.SetConfig("enrichment_rate_start_load", "enrichment_rate_start_load_count", string.Empty);
            eEnrichmentStartLoad.SetTableEventHandler(ChartUpdateEvent);

            eEnrichmentLoadDerivative.Initialize(cs, 0, 1000000, 100, 10D, 0D, 1000D, 1D, 200D, 0);
            eEnrichmentLoadDerivative.SetConfig("enrichment_rate_load_derivative", "enrichment_rate_load_derivative_count", string.Empty);
            eEnrichmentLoadDerivative.SetTableEventHandler(ChartUpdateEvent);

            eEnrichmentSyncAmount.Initialize(cs, 0, 10, 0.05D, 0.01D, 0D, 1D, 500, 0.1D, 2);
            eEnrichmentSyncAmount.SetConfig("enrichment_sync_amount", "rotates_count", "rotates");
            eEnrichmentSyncAmount.SetX("RPM", "RPM", "F0");
            eEnrichmentSyncAmount.SetTableEventHandler(ChartUpdateEvent);

            eEnrichmentAsyncAmount.Initialize(cs, 0, 10, 0.05D, 0.01D, 0D, 1D, 500, 0.1D, 2);
            eEnrichmentAsyncAmount.SetConfig("enrichment_async_amount", "rotates_count", "rotates");
            eEnrichmentAsyncAmount.SetX("RPM", "RPM", "F0");
            eEnrichmentAsyncAmount.SetTableEventHandler(ChartUpdateEvent);

            eEnrichmentTempMult.Initialize(cs, -1, 5, 0.01D, 0.1D, 0D, 0.5D, 10D, 0.05D, 2);
            eEnrichmentTempMult.SetConfig("enrichment_temp_mult", "engine_temp_count", "engine_temps");
            eEnrichmentTempMult.SetX("EngineTemp", "Temperature", "F1");
            eEnrichmentTempMult.SetTableEventHandler(ChartUpdateEvent);

            eEnrichmentInjectionPhase.Initialize(cs, 0, 720, 10, 50, 0D, 600, 500D, 50, 0);
            eEnrichmentInjectionPhase.SetConfig("enrichment_injection_phase", "rotates_count", "rotates");
            eEnrichmentInjectionPhase.SetX("RPM", "RPM", "F0");
            eEnrichmentInjectionPhase.SetTableEventHandler(ChartUpdateEvent);

            
            eEnrichmentRate.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].enrichment_rate_load_derivative_count,
                cs.ConfigStruct.tables[cs.CurrentTable].enrichment_rate_start_load_count,
                0.0D, 5.0D, 0.01D, 0.5D, 0.1D, 0.0D, 1.0D, 100.0D, 0.1D, Consts.TABLE_ENRICHMENT_PERCENTS_MAX, Consts.TABLE_ENRICHMENT_PERCENTS_MAX, 2);

            eEnrichmentRate.SetConfig("enrichment_rate", "enrichment_rate_load_derivative_count", "enrichment_rate_start_load_count", "enrichment_rate_load_derivative", "enrichment_rate_start_load");
            eEnrichmentRate.SetX("EnrichmentLoadDerivative", "Load Derivative", "F0");
            eEnrichmentRate.SetD("EnrichmentStartLoad", "Start Load", "F1");
            eEnrichmentRate.SetTableEventHandler(ChartUpdateEvent);

            colorTransience = new ColorTransience(0.5F, 1.5F, Color.Gray);
            colorTransience.Add(Color.FromArgb(0, 128, 255), 0.5F);
            colorTransience.Add(Color.Blue, 0.7F);
            colorTransience.Add(Color.FromArgb(0, 92, 160), 0.75F);
            colorTransience.Add(Color.Green, 1.0F);
            colorTransience.Add(Color.FromArgb(128, 96, 0), 1.05F);
            colorTransience.Add(Color.DarkRed, 1.12F);
            colorTransience.Add(Color.Black, 1.5F);

            eEnrichmentRate.SetTableColorTrans(colorTransience);
            eEnrichmentRate.SynchronizeChart();
            

            colorTransience = new ColorTransience(-5F, 5F, Color.Gray);
            colorTransience.Add(Color.DeepSkyBlue, -5.0F);
            colorTransience.Add(Color.Blue, -3.0F);
            colorTransience.Add(Color.FromArgb(0, 128, 255), -2.0F);
            colorTransience.Add(Color.Green, 0.0F);
            colorTransience.Add(Color.FromArgb(192, 128, 0), 2.0F);
            colorTransience.Add(Color.Red, 3.0F);
            colorTransience.Add(Color.DarkRed, 4.0F);
            colorTransience.Add(Color.Black, 5.0F);

            eEnrichmentIgnCorr.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].enrichment_rate_start_load_count,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                -25.0D, 25.0D, 0.1D, 10D, 1D, -5D, 5D, 10, 1D, Consts.TABLE_ENRICHMENT_PERCENTS_MAX, Consts.TABLE_ROTATES_MAX, 1);

            eEnrichmentIgnCorr.SetConfig("enrichment_ign_corr", "enrichment_rate_start_load_count", "rotates_count", "enrichment_rate_start_load", "rotates");
            eEnrichmentIgnCorr.SetTableEventHandler(ChartUpdateEvent);
            eEnrichmentIgnCorr.scHorisontal.SplitterDistance = (int)Math.Round(eEnrichmentIgnCorr.scHorisontal.Width * 0.65);

            eEnrichmentIgnCorr.SetTableColorTrans(colorTransience);
            eEnrichmentIgnCorr.SynchronizeChart();


            ePressures.Initialize(cs, 0, 1000000, 100, 50, 0, 100000, 1, 10000, 0);
            ePressures.SetConfig("pressures", "pressures_count", string.Empty);
            ePressures.SetY("ManifoldAirPressure", "Pressure", "F0");
            ePressures.SetTableEventHandler(ChartUpdateEvent);

            eRotates.Initialize(cs, 0, 10000, 50, 100, 0, 8000, 1, 500, 0);
            eRotates.SetConfig("rotates", "rotates_count", string.Empty);
            eRotates.SetY("RPM", "RPM", "F0");
            eRotates.SetTableEventHandler(ChartUpdateEvent);
           
            eIdleRotates.Initialize(cs, 0, 10000, 50, 100, 0, 8000, 1, 500, 0);
            eIdleRotates.SetConfig("idle_rotates", "idle_rotates_count", string.Empty);
            eIdleRotates.SetY("RPM", "RPM", "F0");
            eIdleRotates.SetTableEventHandler(ChartUpdateEvent);

            eThrottles.Initialize(cs, 0, 100, 0.1, 1, 0, 100, 1, 10, 1);
            eThrottles.SetConfig("throttles", "throttles_count", string.Empty);
            eThrottles.SetY("ThrottlePosition", "TPS", "F1");
            eThrottles.SetTableEventHandler(ChartUpdateEvent);

            ePedals.Initialize(cs, 0, 100, 0.1, 1, 0, 100, 1, 10, 1);
            ePedals.SetConfig("pedals", "pedals_count", string.Empty);
            ePedals.SetY("PedalPosition", "TPS", "F1");
            ePedals.SetTableEventHandler(ChartUpdateEvent);

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
            eIdleWishIgnition.SetY("IgnitionAdvance", "Ignition", "F1");
            eIdleWishIgnition.SetTableEventHandler(ChartUpdateEvent);

            eIdleIgnitionStatic.Initialize(cs, -15D, 60D, 0.1D, 5D, 10D, 20D, 100D, 2D, 1);
            eIdleIgnitionStatic.SetConfig("idle_wish_ignition_static", "idle_rotates_count", "idle_rotates");
            eIdleIgnitionStatic.SetX("RPM", "RPM", "F0");
            eIdleIgnitionStatic.SetY("IgnitionAdvance", "Ignition", "F1");
            eIdleIgnitionStatic.SetTableEventHandler(ChartUpdateEvent);

            eIdleSpeedShift.Initialize(cs, 0, 2000, 20D, 10D, 0, 100, 10D, 20D, 0);
            eIdleSpeedShift.SetConfig("idle_rpm_shift", "idle_speeds_shift_count", "idle_rpm_shift_speeds");
            eIdleSpeedShift.SetX("Speed", "Speed", "F1");
            eIdleSpeedShift.SetY("IdleSpeedShift", "RPM Shift", "F0");
            eIdleSpeedShift.SetTableEventHandler(ChartUpdateEvent);

            eIdleValvePos.Initialize(cs, 0, 160, 1, 10D, 0, 100, 10D, 10, 0);
            eIdleValvePos.SetConfig("idle_valve_position", "engine_temp_count", "engine_temps");
            eIdleValvePos.SetX("EngineTemp", "Temp.", "F1");
            eIdleValvePos.SetY("IdleValvePosition", "Valve Pos", "F0");
            eIdleValvePos.SetTableEventHandler(ChartUpdateEvent);

            eIdleValveEconPos.Initialize(cs, 0, 160, 1, 10D, 0, 100, 500D, 10, 0);
            eIdleValveEconPos.SetConfig("idle_valve_econ_position", "rotates_count", "rotates");
            eIdleValveEconPos.SetX("RPM", "RPM", "F0");
            eIdleValveEconPos.SetY("IdleValvePosition", "Valve Pos", "F0");
            eIdleValveEconPos.SetTableEventHandler(ChartUpdateEvent);

            eEtcEconPositions.Initialize(cs, 0, 100, 0.1, 2D, 0, 20, 500D, 2, 1);
            eEtcEconPositions.SetConfig("idle_throttle_econ_position", "rotates_count", "rotates");
            eEtcEconPositions.SetX("RPM", "RPM", "F0");
            eEtcEconPositions.SetY("ThrottlePosition", "Throttle Pos", "F1");
            eEtcEconPositions.SetTableEventHandler(ChartUpdateEvent);

            eEtcStartupMoveTime.Initialize(cs, 0, 10, 0.1, 0.5D, 0, 5, 10.0D, 0.5, 1);
            eEtcStartupMoveTime.SetConfig("throttle_startup_move_time", "engine_temp_count", "engine_temps");
            eEtcStartupMoveTime.SetX("EngineTemp", "Temp.", "F1");
            eEtcStartupMoveTime.SetY(string.Empty, "Move time", "F1");
            eEtcStartupMoveTime.SetTableEventHandler(ChartUpdateEvent);
            
            eEtcPedalIgnitionControl.Initialize(cs, 0, 100, 0.1, 1D, 0, 10, 500D, 1, 1);
            eEtcPedalIgnitionControl.SetConfig("pedal_ignition_control", "rotates_count", "rotates");
            eEtcPedalIgnitionControl.SetX("RPM", "RPM", "F0");
            eEtcPedalIgnitionControl.SetY("PedalPosition", "Pedal", "F1");
            eEtcPedalIgnitionControl.SetTableEventHandler(ChartUpdateEvent);
            
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


            eKnockCyLevelMultiplier.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].cylinders_count,
                0.0D, 5.0D, 0.01D, 100.0D, 0.1D, 0.0D, 1.0D, 500, 0.1D, Consts.TABLE_ROTATES_MAX, Consts.ECU_CYLINDERS_COUNT, 2);

            eKnockCyLevelMultiplier.SetConfig("knock_cy_level_multiplier", "rotates_count", "cylinders_count", "rotates", "cylinders");
            eKnockCyLevelMultiplier.SetX("RPM", "RPM", "F0");
            eKnockCyLevelMultiplier.SetTableEventHandler(ChartUpdateEvent);

            colorTransience = new ColorTransience(0.0F, 5.0F, Color.Gray);
            colorTransience.Add(Color.DeepSkyBlue, 0.0F);
            colorTransience.Add(Color.DeepSkyBlue, 0.4F);
            colorTransience.Add(Color.Blue, 0.6F);
            colorTransience.Add(Color.FromArgb(0, 128, 255), 0.8F);
            colorTransience.Add(Color.Green, 1.0F);
            colorTransience.Add(Color.FromArgb(192, 128, 0), 1.2F);
            colorTransience.Add(Color.Red, 1.4F);
            colorTransience.Add(Color.DarkRed, 1.6F);
            colorTransience.Add(Color.DarkRed, 5.0F);

            eKnockCyLevelMultiplier.SetTableColorTrans(colorTransience);


            colorTransience = new ColorTransience(-1.0F, 1.0F, Color.Gray);
            colorTransience.Add(Color.DeepSkyBlue, -1.0F);
            colorTransience.Add(Color.Blue, -0.5F);
            colorTransience.Add(Color.FromArgb(0, 128, 255), -0.1F);
            colorTransience.Add(Color.Green, 0.0F);
            colorTransience.Add(Color.FromArgb(192, 128, 0), 0.1F);
            colorTransience.Add(Color.Red, 0.5F);
            colorTransience.Add(Color.DarkRed, 1.0F);

            eCorrsFillingGbcMAP.Initialize(cs, Editor2DMode.CorrectionsTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].pressures_count,
                -10.0D, 10.0D, 0.005D, 100.0D, 0.1D, -0.2D, 0.2D, 500, 0.1D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_PRESSURES_MAX, 3);

            eCorrsFillingGbcMAP.SetConfig("filling_gbc_map", "rotates_count", "pressures_count", "rotates", "pressures");
            eCorrsFillingGbcMAP.SetX("RPM", "RPM", "F0");
            eCorrsFillingGbcMAP.SetY(string.Empty, "Correction", "F3");
            eCorrsFillingGbcMAP.SetD("ManifoldAirPressure", "Pressure", "F0");
            eCorrsFillingGbcMAP.SetTableEventHandler(ChartCorrectionEvent);
            eCorrsFillingGbcMAP.scHorisontal.SplitterDistance = (int)Math.Round(eCorrsFillingGbcMAP.scHorisontal.Width * 0.75);

            eCorrsFillingGbcMAP.SetTableColorTrans(colorTransience);
            eCorrsFillingGbcMAP.SynchronizeChart();

            eCorrsFillingGbcTPS.Initialize(cs, Editor2DMode.CorrectionsTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].pressures_count,
                -10.0D, 10.0D, 0.005D, 100.0D, 0.1D, -0.2D, 0.2D, 500, 0.1D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_PRESSURES_MAX, 3);

            eCorrsFillingGbcTPS.SetConfig("filling_gbc_tps", "rotates_count", "throttles_count", "rotates", "throttles");
            eCorrsFillingGbcTPS.SetX("RPM", "RPM", "F0");
            eCorrsFillingGbcTPS.SetY(string.Empty, "Correction", "F3");
            eCorrsFillingGbcTPS.SetD("ThrottlePosition", "Throttle", "F1");
            eCorrsFillingGbcTPS.SetTableEventHandler(ChartCorrectionEvent);
            eCorrsFillingGbcTPS.scHorisontal.SplitterDistance = (int)Math.Round(eCorrsFillingGbcMAP.scHorisontal.Width * 0.75);

            eCorrsFillingGbcTPS.SetTableColorTrans(colorTransience);
            eCorrsFillingGbcTPS.SynchronizeChart();
            

            eCorrsKnockCyNoiseLevelMult.Initialize(cs, Editor2DMode.CorrectionsTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].cylinders_count,
                -10.0D, 10.0D, 0.005D, 100.0D, 0.1D, -0.2D, 0.2D, 500, 0.1D, Consts.TABLE_ROTATES_MAX, Consts.ECU_CYLINDERS_COUNT, 3);

            eCorrsKnockCyNoiseLevelMult.SetConfig("knock_cy_level_multiplier", "rotates_count", "cylinders_count", "rotates", "cylinders");
            eCorrsKnockCyNoiseLevelMult.SetX("RPM", "RPM", "F0");
            eCorrsKnockCyNoiseLevelMult.SetY(string.Empty, "Correction", "F3");
            eCorrsKnockCyNoiseLevelMult.SetTableEventHandler(ChartCorrectionEvent);
            eCorrsKnockCyNoiseLevelMult.scHorisontal.SplitterDistance = (int)Math.Round(eCorrsKnockCyNoiseLevelMult.scHorisontal.Width * 0.75);

            eCorrsKnockCyNoiseLevelMult.SetTableColorTrans(colorTransience);
            eCorrsKnockCyNoiseLevelMult.SynchronizeChart();

            //TODO:
            //eCorrIdleValvePos.Initialize(cs, Editor2DMode.CorrectionsTable,
            //    cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
            //    cs.ConfigStruct.tables[cs.CurrentTable].engine_temp_count,
            //    -10.0D, 10.0D, 0.02D, 100.0D, 0.1D, -0.2D, 0.2D, 500, 0.1D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_TEMPERATURES_MAX, 2);

            //eCorrIdleValvePos.SetConfig("idle_valve_position", "rotates_count", "engine_temp_count", "rotates", "engine_temps");
            //eCorrIdleValvePos.SetX("RPM", "RPM", "F0");
            //eCorrIdleValvePos.SetY(string.Empty, "Correction", "F3");
            //eCorrIdleValvePos.SetD("EngineTemp", "Temperature", "F1");
            //eCorrIdleValvePos.SetTableEventHandler(ChartCorrectionEvent);
            //eCorrIdleValvePos.scHorisontal.SplitterDistance = (int)Math.Round(eCorrsIdleValveToRPM.scHorisontal.Width * 0.75);

            //eCorrIdleValvePos.SetTableColorTrans(colorTransience);
            //eCorrIdleValvePos.SynchronizeChart();

            colorTransience = new ColorTransience(-5.0F, 4.0F, Color.Gray);
            colorTransience.Add(Color.Black, -5.0F);
            colorTransience.Add(Color.DarkRed, -3.0F);
            colorTransience.Add(Color.Red, -1.5F);
            colorTransience.Add(Color.FromArgb(192, 128, 0), -0.2F);
            colorTransience.Add(Color.Green, 0.0F);
            colorTransience.Add(Color.FromArgb(0, 128, 255), 0.5F);
            colorTransience.Add(Color.Blue, 2.0F);
            colorTransience.Add(Color.DeepSkyBlue, 4.0F);

            eCorrsIgnition.Initialize(cs, Editor2DMode.CorrectionsTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                -45.0D, 45.0D, 0.1D, 100.0D, 1D, -5D, 5D, 500, 1D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 2);

            eCorrsIgnition.SetConfig("ignitions", "rotates_count", "fillings_count", "rotates", "fillings");
            eCorrsIgnition.SetX("RPM", "RPM", "F0");
            eCorrsIgnition.SetY(string.Empty, "Ignition Correction", "F2");
            eCorrsIgnition.SetD("CyclicAirFlow", "Filling", "F1");
            eCorrsIgnition.SetTableEventHandler(ChartCorrectionEvent);
            eCorrsIgnition.scHorisontal.SplitterDistance = (int)Math.Round(eCorrsIgnition.scHorisontal.Width * 0.85);

            eCorrsIgnition.SetTableColorTrans(colorTransience);
            eCorrsIgnition.SynchronizeChart();


            eCorrsIgnitionCy1.Initialize(cs, Editor2DMode.CorrectionsTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                -45.0D, 45.0D, 0.1D, 100.0D, 1D, -5D, 5D, 500, 1D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 2);

            eCorrsIgnitionCy1.SetConfig("ignition_corr_cy1", "rotates_count", "fillings_count", "rotates", "fillings");
            eCorrsIgnitionCy1.SetX("RPM", "RPM", "F0");
            eCorrsIgnitionCy1.SetY(string.Empty, "Ignition Corr.Cy1", "F2");
            eCorrsIgnitionCy1.SetD("CyclicAirFlow", "Filling", "F1");
            eCorrsIgnitionCy1.SetTableEventHandler(ChartCorrectionEvent);
            eCorrsIgnitionCy1.scHorisontal.SplitterDistance = (int)Math.Round(eCorrsIgnitionCy1.scHorisontal.Width * 0.85);

            eCorrsIgnitionCy1.SetTableColorTrans(colorTransience);
            eCorrsIgnitionCy1.SynchronizeChart();



            eCorrsIgnitionCy2.Initialize(cs, Editor2DMode.CorrectionsTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                -45.0D, 45.0D, 0.1D, 100.0D, 1D, -5D, 5D, 500, 1D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 2);
        
            eCorrsIgnitionCy2.SetConfig("ignition_corr_cy2", "rotates_count", "fillings_count", "rotates", "fillings");
            eCorrsIgnitionCy2.SetX("RPM", "RPM", "F0");
            eCorrsIgnitionCy2.SetY(string.Empty, "Ignition Corr.Cy2", "F2");
            eCorrsIgnitionCy2.SetD("CyclicAirFlow", "Filling", "F1");
            eCorrsIgnitionCy2.SetTableEventHandler(ChartCorrectionEvent);
            eCorrsIgnitionCy2.scHorisontal.SplitterDistance = (int)Math.Round(eCorrsIgnitionCy2.scHorisontal.Width * 0.85);

            eCorrsIgnitionCy2.SetTableColorTrans(colorTransience);
            eCorrsIgnitionCy2.SynchronizeChart();



            eCorrsIgnitionCy3.Initialize(cs, Editor2DMode.CorrectionsTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                -45.0D, 45.0D, 0.1D, 100.0D, 1D, -5D, 5D, 500, 1D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 2);

            eCorrsIgnitionCy3.SetConfig("ignition_corr_cy3", "rotates_count", "fillings_count", "rotates", "fillings");
            eCorrsIgnitionCy3.SetX("RPM", "RPM", "F0");
            eCorrsIgnitionCy3.SetY(string.Empty, "Ignition Corr.Cy3", "F2");
            eCorrsIgnitionCy3.SetD("CyclicAirFlow", "Filling", "F1");
            eCorrsIgnitionCy3.SetTableEventHandler(ChartCorrectionEvent);
            eCorrsIgnitionCy3.scHorisontal.SplitterDistance = (int)Math.Round(eCorrsIgnitionCy3.scHorisontal.Width * 0.85);

            eCorrsIgnitionCy3.SetTableColorTrans(colorTransience);
            eCorrsIgnitionCy3.SynchronizeChart();



            eCorrsIgnitionCy4.Initialize(cs, Editor2DMode.CorrectionsTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                -45.0D, 45.0D, 0.1D, 100.0D, 1D, -5D, 5D, 500, 1D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 2);

            eCorrsIgnitionCy4.SetConfig("ignition_corr_cy4", "rotates_count", "fillings_count", "rotates", "fillings");
            eCorrsIgnitionCy4.SetX("RPM", "RPM", "F0");
            eCorrsIgnitionCy4.SetY(string.Empty, "Ignition Corr.Cy4", "F2");
            eCorrsIgnitionCy4.SetD("CyclicAirFlow", "Filling", "F1");
            eCorrsIgnitionCy4.SetTableEventHandler(ChartCorrectionEvent);
            eCorrsIgnitionCy4.scHorisontal.SplitterDistance = (int)Math.Round(eCorrsIgnitionCy4.scHorisontal.Width * 0.85);

            eCorrsIgnitionCy4.SetTableColorTrans(colorTransience);
            eCorrsIgnitionCy4.SynchronizeChart();
            

            colorTransience = new ColorTransience(0.0F, 10.0F, Color.Gray);
            colorTransience.Add(Color.Green, 0.0F);
            colorTransience.Add(Color.FromArgb(192, 128, 0), 2.0F);
            colorTransience.Add(Color.Red, 3.0F);
            colorTransience.Add(Color.DarkRed, 5.0F);
            colorTransience.Add(Color.Black, 10.0F);

            eCorrsKnockDetonationCounter.Initialize(cs, Editor2DMode.CorrectionsTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                -10D, 99.0D, 0.5D, 100.0D, 1D, -5D, 5D, 500, 1D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 1);

            eCorrsKnockDetonationCounter.SetConfig("knock_detonation_counter", "rotates_count", "fillings_count", "rotates", "fillings");
            eCorrsKnockDetonationCounter.SetX("RPM", "RPM", "F0");
            eCorrsKnockDetonationCounter.SetY(string.Empty, "DetonationCounter", "F1");
            eCorrsKnockDetonationCounter.SetD("CyclicAirFlow", "Filling", "F1");
            eCorrsKnockDetonationCounter.SetTableEventHandler(ChartCorrectionEvent);
            eCorrsKnockDetonationCounter.scHorisontal.SplitterDistance = (int)Math.Round(eCorrsKnockDetonationCounter.scHorisontal.Width * 0.85);

            eCorrsKnockDetonationCounter.SetTableColorTrans(colorTransience);
            eCorrsKnockDetonationCounter.SynchronizeChart();


            colorTransience = new ColorTransience(-0.1F, 0.2F, Color.Gray);
            colorTransience.Add(Color.DeepSkyBlue, -0.1F);
            colorTransience.Add(Color.Blue, -0.05F);
            colorTransience.Add(Color.FromArgb(0, 128, 255), -0.02F);
            colorTransience.Add(Color.Green, 0.0F);
            colorTransience.Add(Color.FromArgb(192, 128, 0), 0.02F);
            colorTransience.Add(Color.Red, 0.05F);
            colorTransience.Add(Color.DarkRed, 0.1F);
            colorTransience.Add(Color.Black, 0.2F);

            

            eCorrsInjectionCy1.Initialize(cs, Editor2DMode.CorrectionsTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                -1.0D, 1.0D, 0.002D, 100.0D, 0.2D, -0.2D, 0.2D, 500, 0.05D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 3);

            eCorrsInjectionCy1.SetConfig("injection_corr_cy1", "rotates_count", "fillings_count", "rotates", "fillings");
            eCorrsInjectionCy1.SetX("RPM", "RPM", "F0");
            eCorrsInjectionCy1.SetY(string.Empty, "Injection Corr.Cy1", "F2");
            eCorrsInjectionCy1.SetD("CyclicAirFlow", "Filling", "F1");
            eCorrsInjectionCy1.SetTableEventHandler(ChartCorrectionEvent);
            eCorrsInjectionCy1.scHorisontal.SplitterDistance = (int)Math.Round(eCorrsInjectionCy1.scHorisontal.Width * 0.85);

            eCorrsInjectionCy1.SetTableColorTrans(colorTransience);
            eCorrsInjectionCy1.SynchronizeChart();



            eCorrsInjectionCy2.Initialize(cs, Editor2DMode.CorrectionsTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                -1.0D, 1.0D, 0.002D, 100.0D, 0.2D, -0.2D, 0.2D, 500, 0.05D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 3);

            eCorrsInjectionCy2.SetConfig("injection_corr_cy2", "rotates_count", "fillings_count", "rotates", "fillings");
            eCorrsInjectionCy2.SetX("RPM", "RPM", "F0");
            eCorrsInjectionCy2.SetY(string.Empty, "Injection Corr.Cy2", "F2");
            eCorrsInjectionCy2.SetD("CyclicAirFlow", "Filling", "F1");
            eCorrsInjectionCy2.SetTableEventHandler(ChartCorrectionEvent);
            eCorrsInjectionCy2.scHorisontal.SplitterDistance = (int)Math.Round(eCorrsInjectionCy2.scHorisontal.Width * 0.85);

            eCorrsInjectionCy2.SetTableColorTrans(colorTransience);
            eCorrsInjectionCy2.SynchronizeChart();



            eCorrsInjectionCy3.Initialize(cs, Editor2DMode.CorrectionsTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                -1.0D, 1.0D, 0.002D, 100.0D, 0.2D, -0.2D, 0.2D, 500, 0.05D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 3);

            eCorrsInjectionCy3.SetConfig("injection_corr_cy3", "rotates_count", "fillings_count", "rotates", "fillings");
            eCorrsInjectionCy3.SetX("RPM", "RPM", "F0");
            eCorrsInjectionCy3.SetY(string.Empty, "Injection Corr.Cy3", "F2");
            eCorrsInjectionCy3.SetD("CyclicAirFlow", "Filling", "F1");
            eCorrsInjectionCy3.SetTableEventHandler(ChartCorrectionEvent);
            eCorrsInjectionCy3.scHorisontal.SplitterDistance = (int)Math.Round(eCorrsInjectionCy3.scHorisontal.Width * 0.85);

            eCorrsInjectionCy3.SetTableColorTrans(colorTransience);
            eCorrsInjectionCy3.SynchronizeChart();



            eCorrsInjectionCy4.Initialize(cs, Editor2DMode.CorrectionsTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                -1.0D, 1.0D, 0.002D, 100.0D, 0.2D, -0.2D, 0.2D, 500, 0.05D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 3);

            eCorrsInjectionCy4.SetConfig("injection_corr_cy4", "rotates_count", "fillings_count", "rotates", "fillings");
            eCorrsInjectionCy4.SetX("RPM", "RPM", "F0");
            eCorrsInjectionCy4.SetY(string.Empty, "Injection Corr.Cy4", "F2");
            eCorrsInjectionCy4.SetD("CyclicAirFlow", "Filling", "F1");
            eCorrsInjectionCy4.SetTableEventHandler(ChartCorrectionEvent);
            eCorrsInjectionCy4.scHorisontal.SplitterDistance = (int)Math.Round(eCorrsInjectionCy4.scHorisontal.Width * 0.85);

            eCorrsInjectionCy4.SetTableColorTrans(colorTransience);
            eCorrsInjectionCy4.SynchronizeChart();

          


            eInjectionCy1.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                -1.0D, 1.0D, 0.002D, 100.0D, 0.2D, -0.2D, 0.2D, 500, 0.05D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 3);

            eInjectionCy1.SetConfig("injection_corr_cy1", "rotates_count", "fillings_count", "rotates", "fillings");
            eInjectionCy1.SetX("RPM", "RPM", "F0");
            eInjectionCy1.SetY(string.Empty, "Injection Corr.Cy1", "F2");
            eInjectionCy1.SetD("CyclicAirFlow", "Filling", "F1");
            eInjectionCy1.SetTableEventHandler(ChartCorrectionEvent);

            eInjectionCy1.SetTableColorTrans(colorTransience);
            eInjectionCy1.SynchronizeChart();



            eInjectionCy2.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                -1.0D, 1.0D, 0.002D, 100.0D, 0.2D, -0.2D, 0.2D, 500, 0.05D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 3);

            eInjectionCy2.SetConfig("injection_corr_cy2", "rotates_count", "fillings_count", "rotates", "fillings");
            eInjectionCy2.SetX("RPM", "RPM", "F0");
            eInjectionCy2.SetY(string.Empty, "Injection Corr.Cy2", "F2");
            eInjectionCy2.SetD("CyclicAirFlow", "Filling", "F1");
            eInjectionCy2.SetTableEventHandler(ChartCorrectionEvent);

            eInjectionCy2.SetTableColorTrans(colorTransience);
            eInjectionCy2.SynchronizeChart();



            eInjectionCy3.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                -1.0D, 1.0D, 0.002D, 100.0D, 0.2D, -0.2D, 0.2D, 500, 0.05D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 3);

            eInjectionCy3.SetConfig("injection_corr_cy3", "rotates_count", "fillings_count", "rotates", "fillings");
            eInjectionCy3.SetX("RPM", "RPM", "F0");
            eInjectionCy3.SetY(string.Empty, "Injection Corr.Cy3", "F2");
            eInjectionCy3.SetD("CyclicAirFlow", "Filling", "F1");
            eInjectionCy3.SetTableEventHandler(ChartCorrectionEvent);

            eInjectionCy3.SetTableColorTrans(colorTransience);
            eInjectionCy3.SynchronizeChart();



            eInjectionCy4.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].rotates_count,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                -1.0D, 1.0D, 0.002D, 100.0D, 0.2D, -0.2D, 0.2D, 500, 0.05D, Consts.TABLE_ROTATES_MAX, Consts.TABLE_FILLING_MAX, 3);

            eInjectionCy4.SetConfig("injection_corr_cy4", "rotates_count", "fillings_count", "rotates", "fillings");
            eInjectionCy4.SetX("RPM", "RPM", "F0");
            eInjectionCy4.SetY(string.Empty, "Injection Corr.Cy4", "F2");
            eInjectionCy4.SetD("CyclicAirFlow", "Filling", "F1");
            eInjectionCy4.SetTableEventHandler(ChartCorrectionEvent);

            eInjectionCy4.SetTableColorTrans(colorTransience);
            eInjectionCy4.SynchronizeChart();

            

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
            eAirTempMixCorr.SetD("CalculatedAirTemp", "CalcdAirTemp", "F1");
            eAirTempMixCorr.SetTableEventHandler(ChartUpdateEvent);
            eAirTempMixCorr.scHorisontal.SplitterDistance = (int)Math.Round(eAirTempMixCorr.scHorisontal.Width * 0.65);

            eAirTempMixCorr.SetTableColorTrans(colorTransience);
            eAirTempMixCorr.SynchronizeChart();

            eEngineTempMixCorr.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                cs.ConfigStruct.tables[cs.CurrentTable].engine_temp_count,
                -0.9D, 5.0D, 0.01D, 20D, 0.1D, -0.2D, 0.2D, 20, 0.1D, Consts.TABLE_FILLING_MAX, Consts.TABLE_TEMPERATURES_MAX, 2);

            eEngineTempMixCorr.SetConfig("engine_temp_mix_corr", "fillings_count", "engine_temp_count", "fillings", "engine_temps");
            eEngineTempMixCorr.SetX("CyclicEngineFlow", "CyclicEngineFlow", "F1");
            eEngineTempMixCorr.SetY(string.Empty, "Mix.Corr.", "F2");
            eEngineTempMixCorr.SetD("EngineTemp", "EngineTemp", "F1");
            eEngineTempMixCorr.SetTableEventHandler(ChartUpdateEvent);
            eEngineTempMixCorr.scHorisontal.SplitterDistance = (int)Math.Round(eEngineTempMixCorr.scHorisontal.Width * 0.65);

            eEngineTempMixCorr.SetTableColorTrans(colorTransience);
            eEngineTempMixCorr.SynchronizeChart();


            colorTransience = new ColorTransience(-1.0F, 1.0F, Color.Gray);
            colorTransience.Add(Color.DeepSkyBlue, -1.0F);
            colorTransience.Add(Color.Blue, -0.5F);
            colorTransience.Add(Color.FromArgb(0, 128, 255), -0.1F);
            colorTransience.Add(Color.Green, 0.0F);
            colorTransience.Add(Color.FromArgb(192, 128, 0), 0.1F);
            colorTransience.Add(Color.Red, 0.5F);
            colorTransience.Add(Color.DarkRed, 1.0F);

            eAdvancedFanControl.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].idle_speeds_shift_count,
                cs.ConfigStruct.tables[cs.CurrentTable].engine_temp_count,
                -5.0D, 5.0D, 0.01D, 20D, 0.1D, -0.2D, 0.2D, 20, 0.1D, Consts.TABLE_SPEEDS_MAX, Consts.TABLE_TEMPERATURES_MAX, 2);

            eAdvancedFanControl.SetConfig("fan_advance_control", "idle_speeds_shift_count", "engine_temp_count", "idle_rpm_shift_speeds", "engine_temps");
            eAdvancedFanControl.SetX("Speed", "Speed", "F1");
            eAdvancedFanControl.SetY(string.Empty, "FanValue", "F2");
            eAdvancedFanControl.SetD("EngineTemp", "EngineTemp", "F1");
            eAdvancedFanControl.SetTableEventHandler(ChartUpdateEvent);
            eAdvancedFanControl.scHorisontal.SplitterDistance = (int)Math.Round(eEngineTempMixCorr.scHorisontal.Width * 0.65);

            eAdvancedFanControl.SetTableColorTrans(colorTransience);
            eAdvancedFanControl.SynchronizeChart();
       
           
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
            eAirTempIgnCorr.SetD("CalculatedAirTemp", "CalcAirTemp", "F1");
            eAirTempIgnCorr.SetTableEventHandler(ChartUpdateEvent);
            eAirTempIgnCorr.scHorisontal.SplitterDistance = (int)Math.Round(eAirTempIgnCorr.scHorisontal.Width * 0.65);

            eAirTempIgnCorr.SetTableColorTrans(colorTransience);
            eAirTempIgnCorr.SynchronizeChart();

            eEngineTempIgnCorr.Initialize(cs, Editor2DMode.EcuTable,
                cs.ConfigStruct.tables[cs.CurrentTable].fillings_count,
                cs.ConfigStruct.tables[cs.CurrentTable].engine_temp_count,
                -10.0D, 10.0D, 0.1D, 20D, 1D, -5D, 5D, 20, 1D, Consts.TABLE_FILLING_MAX, Consts.TABLE_TEMPERATURES_MAX, 1);

            eEngineTempIgnCorr.SetConfig("engine_temp_ign_corr", "fillings_count", "engine_temp_count", "fillings", "engine_temps");
            eEngineTempIgnCorr.SetX("CyclicEngineFlow", "CyclicEngineFlow", "F1");
            eEngineTempIgnCorr.SetY(string.Empty, "Ign.Corr.", "F1");
            eEngineTempIgnCorr.SetD("EngineTemp", "EngineTemp", "F1");
            eEngineTempIgnCorr.SetTableEventHandler(ChartUpdateEvent);
            eEngineTempIgnCorr.scHorisontal.SplitterDistance = (int)Math.Round(eEngineTempIgnCorr.scHorisontal.Width * 0.65);

            eEngineTempIgnCorr.SetTableColorTrans(colorTransience);
            eEngineTempIgnCorr.SynchronizeChart();


            eStartIgnition.Initialize(cs, -15D, 60D, 1D, 5D, 0D, 20D, 10D, 2D, 1);
            eStartIgnition.SetConfig("start_ignition", "engine_temp_count", "engine_temps");
            eStartIgnition.SetX("EngineTemp", "Temperature", "F0");
            eStartIgnition.SetY("IgnitionAdvance", "Ignition", "F0");
            eStartIgnition.SetTableEventHandler(ChartUpdateEvent);

            eStartIdleValvePos.Initialize(cs, 0D, Consts.IDLE_VALVE_POS_MAX, 1D, 20D, 20D, 80D, 10D, 5D, 1);
            eStartIdleValvePos.SetConfig("start_idle_valve_pos", "engine_temp_count", "engine_temps");
            eStartIdleValvePos.SetX("EngineTemp", "Temperature", "F1");
            eStartIdleValvePos.SetY("IdleValvePosition", "Valve", "F0");
            eStartIdleValvePos.SetTableEventHandler(ChartUpdateEvent);

            eEtcStartupPositions.Initialize(cs, 0D, 50D, 0.2D, 2D, 0D, 20D, 10D, 2D, 1);
            eEtcStartupPositions.SetConfig("start_throttle_position", "engine_temp_count", "engine_temps");
            eEtcStartupPositions.SetX("EngineTemp", "Temperature", "F1");
            eEtcStartupPositions.SetY("ThrottlePosition", "Throttle", "F1");
            eEtcStartupPositions.SetTableEventHandler(ChartUpdateEvent);

            eEtcStoppedPositions.Initialize(cs, 0D, 100D, 0.3D, 10D, 0D, 100D, 10D, 10D, 1);
            eEtcStoppedPositions.SetConfig("stop_throttle_position", "pedals_count", "pedals");
            eEtcStoppedPositions.SetX("PedalPosition", "Pedal", "F1");
            eEtcStoppedPositions.SetY(string.Empty, "Throttle", "F1");
            eEtcStoppedPositions.SetTableEventHandler(ChartUpdateEvent);

            eEtcPositions1D.Initialize(cs, 0D, 100D, 0.3D, 10D, 0D, 100D, 10D, 10D, 1);
            eEtcPositions1D.SetConfig("throttle_position_1d", "pedals_count", "pedals");
            eEtcPositions1D.SetX("PedalPosition", "Pedal", "F1");
            eEtcPositions1D.SetY(string.Empty, "Throttle", "F1");
            eEtcPositions1D.SetTableEventHandler(ChartUpdateEvent);

            eEtcIdlePositions.Initialize(cs, 0D, 50D, 0.2D, 1D, 0D, 10D, 10D, 1D, 1);
            eEtcIdlePositions.SetConfig("idle_throttle_position", "engine_temp_count", "engine_temps");
            eEtcIdlePositions.SetX("EngineTemp", "Temperature", "F1");
            eEtcIdlePositions.SetY("ThrottlePosition", "Throttle", "F1");
            eEtcIdlePositions.SetTableEventHandler(ChartUpdateEvent);

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

            eIdleValveAirPidP.Initialize(cs, -10D, 10D, 0.001D, 0.01D, 0D, 1.0D, 0.05D, 0.2D, 3);
            eIdleValveAirPidP.SetConfig("idle_valve_to_massair_pid_p", "idle_pids_rpm_koffs_count", "idle_pids_rpm_koffs");
            eIdleValveAirPidP.SetX("IdleWishToRpmRelation", "RPM koff", "F2");
            eIdleValveAirPidP.SetTableEventHandler(ChartUpdateEvent);

            eIdleValveAirPidI.Initialize(cs, -10D, 10D, 0.001D, 0.01D, 0D, 1.0D, 0.05D, 0.2D, 3);
            eIdleValveAirPidI.SetConfig("idle_valve_to_massair_pid_i", "idle_pids_rpm_koffs_count", "idle_pids_rpm_koffs");
            eIdleValveAirPidI.SetX("IdleWishToRpmRelation", "RPM koff", "F2");
            eIdleValveAirPidI.SetTableEventHandler(ChartUpdateEvent);

            eIdleValveAirPidD.Initialize(cs, -10D, 10D, 0.0001D, 0.01D, 0D, 0.1D, 0.05D, 0.01D, 4);
            eIdleValveAirPidD.SetConfig("idle_valve_to_massair_pid_d", "idle_pids_rpm_koffs_count", "idle_pids_rpm_koffs");
            eIdleValveAirPidD.SetX("IdleWishToRpmRelation", "RPM koff", "F2");
            eIdleValveAirPidD.SetTableEventHandler(ChartUpdateEvent);

            eIdleValveRpmPidP.Initialize(cs, -10D, 10D, 0.001D, 0.01D, 0D, 1.0D, 0.05D, 0.2D, 3);
            eIdleValveRpmPidP.SetConfig("idle_valve_to_rpm_pid_p", "idle_pids_rpm_koffs_count", "idle_pids_rpm_koffs");
            eIdleValveRpmPidP.SetX("IdleWishToRpmRelation", "RPM koff", "F2");
            eIdleValveRpmPidP.SetTableEventHandler(ChartUpdateEvent);

            eIdleValveRpmPidI.Initialize(cs, -10D, 10D, 0.001D, 0.01D, 0D, 1.0D, 0.05D, 0.2D, 3);
            eIdleValveRpmPidI.SetConfig("idle_valve_to_rpm_pid_i", "idle_pids_rpm_koffs_count", "idle_pids_rpm_koffs");
            eIdleValveRpmPidI.SetX("IdleWishToRpmRelation", "RPM koff", "F2");
            eIdleValveRpmPidI.SetTableEventHandler(ChartUpdateEvent);

            eIdleValveRpmPidD.Initialize(cs, -10D, 10D, 0.0001D, 0.01D, 0D, 0.1D, 0.05D, 0.01D, 4);
            eIdleValveRpmPidD.SetConfig("idle_valve_to_rpm_pid_d", "idle_pids_rpm_koffs_count", "idle_pids_rpm_koffs");
            eIdleValveRpmPidD.SetX("IdleWishToRpmRelation", "RPM koff", "F2");
            eIdleValveRpmPidD.SetTableEventHandler(ChartUpdateEvent);

            eIdleThrottleAirPidP.Initialize(cs, -10D, 10D, 0.001D, 0.01D, 0D, 1.0D, 0.05D, 0.2D, 3);
            eIdleThrottleAirPidP.SetConfig("idle_throttle_to_massair_pid_p", "idle_pids_rpm_koffs_count", "idle_pids_rpm_koffs");
            eIdleThrottleAirPidP.SetX("IdleWishToRpmRelation", "RPM koff", "F2");
            eIdleThrottleAirPidP.SetTableEventHandler(ChartUpdateEvent);

            eIdleThrottleAirPidI.Initialize(cs, -10D, 10D, 0.001D, 0.01D, 0D, 1.0D, 0.05D, 0.2D, 3);
            eIdleThrottleAirPidI.SetConfig("idle_throttle_to_massair_pid_i", "idle_pids_rpm_koffs_count", "idle_pids_rpm_koffs");
            eIdleThrottleAirPidI.SetX("IdleWishToRpmRelation", "RPM koff", "F2");
            eIdleThrottleAirPidI.SetTableEventHandler(ChartUpdateEvent);

            eIdleThrottleAirPidD.Initialize(cs, -10D, 10D, 0.0001D, 0.01D, 0D, 0.1D, 0.05D, 0.01D, 4);
            eIdleThrottleAirPidD.SetConfig("idle_throttle_to_massair_pid_d", "idle_pids_rpm_koffs_count", "idle_pids_rpm_koffs");
            eIdleThrottleAirPidD.SetX("IdleWishToRpmRelation", "RPM koff", "F2");
            eIdleThrottleAirPidD.SetTableEventHandler(ChartUpdateEvent);

            eIdleThrottleRpmPidP.Initialize(cs, -10D, 10D, 0.001D, 0.01D, 0D, 1.0D, 0.05D, 0.2D, 3);
            eIdleThrottleRpmPidP.SetConfig("idle_throttle_to_rpm_pid_p", "idle_pids_rpm_koffs_count", "idle_pids_rpm_koffs");
            eIdleThrottleRpmPidP.SetX("IdleWishToRpmRelation", "RPM koff", "F2");
            eIdleThrottleRpmPidP.SetTableEventHandler(ChartUpdateEvent);

            eIdleThrottleRpmPidI.Initialize(cs, -10D, 10D, 0.001D, 0.01D, 0D, 1.0D, 0.05D, 0.2D, 3);
            eIdleThrottleRpmPidI.SetConfig("idle_throttle_to_rpm_pid_i", "idle_pids_rpm_koffs_count", "idle_pids_rpm_koffs");
            eIdleThrottleRpmPidI.SetX("IdleWishToRpmRelation", "RPM koff", "F2");
            eIdleThrottleRpmPidI.SetTableEventHandler(ChartUpdateEvent);

            eIdleThrottleRpmPidD.Initialize(cs, -10D, 10D, 0.0001D, 0.01D, 0D, 0.1D, 0.05D, 0.01D, 4);
            eIdleThrottleRpmPidD.SetConfig("idle_throttle_to_rpm_pid_d", "idle_pids_rpm_koffs_count", "idle_pids_rpm_koffs");
            eIdleThrottleRpmPidD.SetX("IdleWishToRpmRelation", "RPM koff", "F2");
            eIdleThrottleRpmPidD.SetTableEventHandler(ChartUpdateEvent);

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

            int index, subindex1, subindex2, subindex3, subindex4;

            index = treeView.Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl1, tabPage1), Text = "General status" });
            index = treeView.Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl1, tabPage12), Text = "Tools" });
            index = treeView.Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl1, tabPage2), Text = "Basic parameters" });
            index = treeView.Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl1, tabPage3), Text = "Table setup" });
            subindex1 = treeView.Nodes[index].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl111, tabPage8), Text = "Parameters" });
            subindex1 = treeView.Nodes[index].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl111, tabPage4), Text = "Basic setup" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl3, tabPage11), Text = "Pressures" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl3, tabPage7), Text = "Rotates" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl3, tabPage92), Text = "Idle rotates" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl3, tabPage13), Text = "Throttles" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl3, tabPage52), Text = "Pedals" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl3, tabPage14), Text = "Voltages" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl3, tabPage15), Text = "Fillings" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl3, tabPage42), Text = "Speeds" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl3, tabPage16), Text = "Engine temperatures" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl3, tabPage55), Text = "Air temperatures" });
            subindex1 = treeView.Nodes[index].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl111, tabPage9), Text = "Setup" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl4, tabPage91), Text = "Filling" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl13, tabPage10), Text = "By MAP" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl13, tabPage37), Text = "By TPS" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl13, tabPage90), Text = "Selection" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl4, tabPage17), Text = "Enrichment" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl5, tabPage99), Text = "Basic" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl5, tabPage34), Text = "Start Load" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl5, tabPage23), Text = "Load Derivative" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl5, tabPage28), Text = "Rate" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl5, tabPage29), Text = "Sync Amount" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl5, tabPage100), Text = "Async Amount" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl5, tabPage101), Text = "Ignition Correction" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl5, tabPage102), Text = "Inj.Phase Correction" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl5, tabPage85), Text = "Temperature multiplier" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl4, tabPage22), Text = "Ignition" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl6, tpIgnFull), Text = "Advances" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl6, tabPage115), Text = "Cylinders" });
            subindex4 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes[subindex3].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl17, tabPage116), Text = "Cylinder 1" });
            subindex4 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes[subindex3].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl17, tabPage117), Text = "Cylinder 2" });
            subindex4 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes[subindex3].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl17, tabPage118), Text = "Cylinder 3" });
            subindex4 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes[subindex3].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl17, tabPage119), Text = "Cylinder 4" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl6, tabPage31), Text = "Saturation pulse" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl6, tabPage32), Text = "Saturation by RPM" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl6, tabPage59), Text = "Correction by Air Temperature" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl6, tabPage105), Text = "Correction by Engine Temperature" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl4, tabPage24), Text = "Injection" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl7, tabPage33), Text = "Mixture" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl7, tabPage36), Text = "Phase" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl7, tabPage120), Text = "Cylinders" });
            subindex4 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes[subindex3].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl19, tabPage125), Text = "Cylinder 1" });
            subindex4 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes[subindex3].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl19, tabPage126), Text = "Cylinder 2" });
            subindex4 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes[subindex3].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl19, tabPage127), Text = "Cylinder 3" });
            subindex4 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes[subindex3].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl19, tabPage128), Text = "Cylinder 4" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl7, tabPage74), Text = "Injection phase LPF" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl7, tabPage35), Text = "Injector lag" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl7, tabPage56), Text = "Correction by Air Temperature" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl7, tabPage106), Text = "Correction by Engine Temperature" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl4, tabPage25), Text = "Idle" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl9, tabPage38), Text = "Wish RPM" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl9, tabPage41), Text = "Valve position" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl9, tabPage112), Text = "Valve Econ position" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl9, tabPage39), Text = "Mass air flow" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl9, tabPage40), Text = "Ignition" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl9, tabPage80), Text = "Static ignition" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl9, tabPage43), Text = "Shift by Speed" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl9, tabPage71), Text = "Regulation Low Threshold" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl9, tabPage79), Text = "Regulation High Threshold" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl9, tabPage63), Text = "Valve Air PID" });
            subindex4 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes[subindex3].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl11, tabPage69), Text = "Proportional" });
            subindex4 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes[subindex3].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl11, tabPage65), Text = "Integral" });
            subindex4 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes[subindex3].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl11, tabPage66), Text = "Differentional" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl9, tabPage107), Text = "Valve RPM PID" });
            subindex4 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes[subindex3].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl14, tabPage108), Text = "Proportional" });
            subindex4 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes[subindex3].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl14, tabPage109), Text = "Integral" });
            subindex4 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes[subindex3].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl14, tabPage110), Text = "Differentional" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl9, tabPage64), Text = "Ignition PID" });
            subindex4 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes[subindex3].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl1245, tabPage67), Text = "Proportional" });
            subindex4 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes[subindex3].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl1245, tabPage68), Text = "Integral" });
            subindex4 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes[subindex3].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl1245, tabPage70), Text = "Differentional" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl9, tabPage94), Text = "HPF by TPS" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl9, tabPage95), Text = "Economizer delay" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl9, tabPage111), Text = "Fan Control" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl4, tabPage26), Text = "Startup" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl234, tabPage57), Text = "Ignition" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl234, tabPage77), Text = "Injection phase" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl234, tabPage78), Text = "Mixture correction by TPS" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl234, tabPage58), Text = "Idle valve position" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl234, tabPage44), Text = "Async filling" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl234, tabPage76), Text = "Large filling" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl234, tabPage75), Text = "Small filling" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl234, tabPage86), Text = "Filling change time" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl234, tabPage72), Text = "Cold mixture correction" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl234, tabPage73), Text = "Cold mixture change time" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl234, tabPage98), Text = "Economizer delay" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl4, tabPage26), Text = "Warmup" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl234, tabPage45), Text = "Mixture" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl234, tabPage46), Text = "Mixture koff" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl234, tabPage54), Text = "Mixture correction" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl4, tabPage27), Text = "Knock" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl10, tabPage47), Text = "Noise level" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl10, tabPage48), Text = "Threshold" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl10, tabPage60), Text = "Zone" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl10, tabPage62), Text = "Gain" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl10, tabPage61), Text = "Filter frequency" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl10, tabPage30), Text = "Cy Noise Level Mult." });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl4, tabPage93), Text = "TSPS" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl16, tabPage96), Text = "Relative position" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl16, tabPage97), Text = "Desync threshold" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl4, tabPage129), Text = "ETC" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl20, tabPage130), Text = "Positions 2D" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl20, tabPage144), Text = "Positions 1D" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl20, tabPage131), Text = "Stopped Positions" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl20, tabPage132), Text = "Startup Positions" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl20, tabPage133), Text = "Idle Positions" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl20, tabPage135), Text = "Throttle Air PID" });
            subindex4 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes[subindex3].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl21, tabPage137), Text = "Proportional" });
            subindex4 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes[subindex3].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl21, tabPage138), Text = "Integral" });
            subindex4 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes[subindex3].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl21, tabPage139), Text = "Differentional" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl20, tabPage136), Text = "Throttle RPM PID" });
            subindex4 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes[subindex3].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl22, tabPage140), Text = "Proportional" });
            subindex4 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes[subindex3].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl22, tabPage141), Text = "Integral" });
            subindex4 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes[subindex3].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl22, tabPage142), Text = "Differentional" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl20, tabPage134), Text = "Econ Positions" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl20, tabPage143), Text = "Ignition Control" });
            subindex3 = treeView.Nodes[index].Nodes[subindex1].Nodes[subindex2].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl20, tabPage145), Text = "Startup Move Time" });
            index = treeView.Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl1, tabPage49), Text = "Corrections" });
            subindex1 = treeView.Nodes[index].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl12, tabPage51), Text = "Filling By MAP" });
            subindex1 = treeView.Nodes[index].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl12, tabPage89), Text = "Filling By TPS" });
            subindex1 = treeView.Nodes[index].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl12, tabPage50), Text = "Ignitions" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl15, tabPage53), Text = "Common" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl15, tabPage103), Text = "Cylinder 1" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl15, tabPage104), Text = "Cylinder 2" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl15, tabPage113), Text = "Cylinder 3" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl15, tabPage114), Text = "Cylinder 4" });
            subindex1 = treeView.Nodes[index].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl12, tabPage21), Text = "Injections" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl18, tabPage121), Text = "Cylinder 1" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl18, tabPage122), Text = "Cylinder 2" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl18, tabPage123), Text = "Cylinder 3" });
            subindex2 = treeView.Nodes[index].Nodes[subindex1].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl18, tabPage124), Text = "Cylinder 4" });
            subindex1 = treeView.Nodes[index].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl12, tabPage87), Text = "Cylinder Noise" });
            subindex1 = treeView.Nodes[index].Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl12, tabPage88), Text = "Knock Counter" });
            index = treeView.Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl1, tabPage18), Text = "Drag measure" });
            index = treeView.Nodes.Add(new TreeNode { Tag = new TreeNodeListInfo(tabControl1, tabPage5), Text = "Failure codes" });
        }

        private class TreeNodeListInfo
        {
            public TabControl TabControl;
            public TabPage TabPage;

            public TreeNodeListInfo(TabControl tabControl, TabPage tabPage)
            {
                TabControl = tabControl;
                TabPage = tabPage;
                tabControl.Appearance = TabAppearance.FlatButtons;
                tabControl.ItemSize = new Size(0, 1);
                tabControl.SizeMode = TabSizeMode.Fixed;
            }
        }

        private void SynchronizeCharts()
        {
            eCyclicFillingMAP.SynchronizeChart();
            eCyclicFillingTPS.SynchronizeChart();
            eFuelMixturesFull.SynchronizeChart();
            eInjectionPhaseFull.SynchronizeChart();
            eIgnitionFull.SynchronizeChart();
            eInjectionCy1.SynchronizeChart();
            eInjectionCy2.SynchronizeChart();
            eInjectionCy3.SynchronizeChart();
            eInjectionCy4.SynchronizeChart();
            eIgnitionFullCy1.SynchronizeChart();
            eIgnitionFullCy2.SynchronizeChart();
            eIgnitionFullCy3.SynchronizeChart();
            eIgnitionFullCy4.SynchronizeChart();
            eCorrsFillingGbcMAP.SynchronizeChart();
            eCorrsFillingGbcTPS.SynchronizeChart();
            eCorrsIgnition.SynchronizeChart();
            eCorrsIgnitionCy1.SynchronizeChart();
            eCorrsIgnitionCy2.SynchronizeChart();
            eCorrsIgnitionCy3.SynchronizeChart();
            eCorrsIgnitionCy4.SynchronizeChart();
            eCorrsInjectionCy1.SynchronizeChart();
            eCorrsInjectionCy2.SynchronizeChart();
            eCorrsInjectionCy3.SynchronizeChart();
            eCorrsInjectionCy4.SynchronizeChart();
            eCorrsKnockCyNoiseLevelMult.SynchronizeChart();
            eCorrsKnockDetonationCounter.SynchronizeChart();
            eAirTempMixCorr.SynchronizeChart();
            eAirTempIgnCorr.SynchronizeChart();
            eEngineTempMixCorr.SynchronizeChart();
            eEngineTempIgnCorr.SynchronizeChart();
            eAdvancedFanControl.SynchronizeChart();
            eKnockZone.SynchronizeChart();
            eKnockCyLevelMultiplier.SynchronizeChart();
            eEnrichmentRate.SynchronizeChart();
            eEnrichmentIgnCorr.SynchronizeChart();

            eEtcPositions2D.SynchronizeChart();
            UpdateCharts();
        }
        private void UpdateCharts()
        {
            eCyclicFillingMAP.UpdateChart();
            eCyclicFillingTPS.UpdateChart();
            eFuelMixturesFull.UpdateChart();
            eInjectionPhaseFull.UpdateChart();
            eIgnitionFull.UpdateChart();
            eSatByRPM.UpdateChart();
            eInjectorLag.UpdateChart();
            eInjectionPhaseLPF.UpdateChart();
            eFillingSelectKoffTPS.UpdateChart();
            eSaturationPulse.UpdateChart();
            eEnrichmentStartLoad.UpdateChart();
            eEnrichmentLoadDerivative.UpdateChart();
            eEnrichmentRate.UpdateChart();
            eEnrichmentSyncAmount.UpdateChart();
            eEnrichmentAsyncAmount.UpdateChart();
            eEnrichmentIgnCorr.UpdateChart();
            eEnrichmentTempMult.UpdateChart();
            eEnrichmentInjectionPhase.UpdateChart();

            eIgnitionFullCy1.UpdateChart();
            eIgnitionFullCy2.UpdateChart();
            eIgnitionFullCy3.UpdateChart();
            eIgnitionFullCy4.UpdateChart();
            eInjectionCy1.UpdateChart();
            eInjectionCy2.UpdateChart();
            eInjectionCy3.UpdateChart();
            eInjectionCy4.UpdateChart();

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
            eIdleValvePos.UpdateChart();
            eIdleValveEconPos.UpdateChart();

            eIdleRegThr1.UpdateChart();
            eIdleRegThr2.UpdateChart();
            eIdleValveAirPidP.UpdateChart();
            eIdleValveAirPidI.UpdateChart();
            eIdleValveAirPidD.UpdateChart();
            eIdleValveRpmPidP.UpdateChart();
            eIdleValveRpmPidI.UpdateChart();
            eIdleValveRpmPidD.UpdateChart();
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
            eKnockCyLevelMultiplier.UpdateChart();

            eCorrsFillingGbcMAP.UpdateChart();
            eCorrsFillingGbcTPS.UpdateChart();
            eCorrsIgnition.UpdateChart();
            eCorrsIgnitionCy1.UpdateChart();
            eCorrsIgnitionCy2.UpdateChart();
            eCorrsIgnitionCy3.UpdateChart();
            eCorrsIgnitionCy4.UpdateChart();
            eCorrsInjectionCy1.UpdateChart();
            eCorrsInjectionCy2.UpdateChart();
            eCorrsInjectionCy3.UpdateChart();
            eCorrsInjectionCy4.UpdateChart();
            eCorrsKnockDetonationCounter.UpdateChart();
            eCorrsKnockCyNoiseLevelMult.UpdateChart();
            eAirTempMixCorr.UpdateChart();
            eAirTempIgnCorr.UpdateChart();
            eEngineTempMixCorr.UpdateChart();
            eEngineTempIgnCorr.UpdateChart();
            eAdvancedFanControl.UpdateChart();

            eTspsRelativePosition.UpdateChart();
            eTspsDesyncThr.UpdateChart();

            eEtcPositions2D.UpdateChart();
            eEtcStoppedPositions.UpdateChart();
            eEtcPositions1D.UpdateChart();
            eEtcEconPositions.UpdateChart();
            eEtcStartupMoveTime.UpdateChart();
            eEtcPedalIgnitionControl.UpdateChart();
            eEtcIdlePositions.UpdateChart();
            eEtcStartupPositions.UpdateChart();
            eIdleThrottleAirPidP.UpdateChart();
            eIdleThrottleAirPidI.UpdateChart();
            eIdleThrottleAirPidD.UpdateChart();
            eIdleThrottleRpmPidP.UpdateChart();
            eIdleThrottleRpmPidI.UpdateChart();
            eIdleThrottleRpmPidD.UpdateChart();
        }

        private void CorrStart()
        {
            btnCorrStart.Enabled = false;
            btnCorrStop.Enabled = true;
            lblCorrStatus.Text = "Status: Learning";
            lblCorrStats.Text = string.Empty;

            eCorrsFillingGbcMAP.SetProgressTable("progress_filling_gbc_map");
            eCorrsFillingGbcTPS.SetProgressTable("progress_filling_gbc_tps");
            //TODO:
            //eCorrIdleValvePos.SetCalibrationTable("progress_idle_valve_position");
            eCorrsIgnition.SetProgressTable("progress_ignitions");
            eCorrsIgnitionCy1.SetProgressTable("progress_ignitions");
            eCorrsIgnitionCy2.SetProgressTable("progress_ignitions");
            eCorrsIgnitionCy3.SetProgressTable("progress_ignitions");
            eCorrsIgnitionCy4.SetProgressTable("progress_ignitions");
            eCorrsKnockDetonationCounter.SetProgressTable("progress_ignitions");
            eCorrsKnockCyNoiseLevelMult.SetProgressTable("progress_knock_cy_level_multiplier");

            btnCorrAppendFillingByMAP.Enabled = false;
            btnCorrAppendFillingByTPS.Enabled = false;
            btnCorrAppendIdleValve.Enabled = false;
            btnCorrAppendIgnitions.Enabled = false;
            btnCorrAppendInjections.Enabled = false;
            btnCorrAppendKnockNoise.Enabled = false;

            rbCorrInterpolationFunc.Enabled = false;
            rbCorrPointFunc.Enabled = false;

            UpdateCharts();
        }

        private void CorrClear()
        {
            eCorrsFillingGbcMAP.ClearTable();
            eCorrsFillingGbcTPS.ClearTable();
            eCorrsIgnition.ClearTable();
            eCorrsIgnitionCy1.ClearTable();
            eCorrsIgnitionCy2.ClearTable();
            eCorrsIgnitionCy3.ClearTable();
            eCorrsIgnitionCy4.ClearTable();
            eCorrsKnockDetonationCounter.ClearTable();
            eCorrsKnockCyNoiseLevelMult.ClearTable();
        }

        private void CorrStop()
        {
            btnCorrStart.Enabled = true;
            btnCorrStop.Enabled = false;
            lblCorrStatus.Text = "Status: Idle";

            eCorrsFillingGbcMAP.ClearPregressTable();
            eCorrsFillingGbcTPS.ClearPregressTable();
            //TODO:
            //eCorrIdleValvePos.ClearCalibrationTable();
            eCorrsIgnition.ClearPregressTable();
            eCorrsIgnitionCy1.ClearPregressTable();
            eCorrsIgnitionCy2.ClearPregressTable();
            eCorrsIgnitionCy3.ClearPregressTable();
            eCorrsIgnitionCy4.ClearPregressTable();
            eCorrsInjectionCy1.ClearPregressTable();
            eCorrsInjectionCy2.ClearPregressTable();
            eCorrsInjectionCy3.ClearPregressTable();
            eCorrsInjectionCy4.ClearPregressTable();
            eCorrsKnockCyNoiseLevelMult.ClearPregressTable();
            eCorrsKnockDetonationCounter.ClearPregressTable();

            btnCorrAppendFillingByMAP.Enabled = true;
            btnCorrAppendFillingByTPS.Enabled = true;
            btnCorrAppendIdleValve.Enabled = true;
            btnCorrAppendIgnitions.Enabled = true;
            btnCorrAppendInjections.Enabled = true;
            btnCorrAppendKnockNoise.Enabled = true;

            rbCorrInterpolationFunc.Enabled = true;
            rbCorrPointFunc.Enabled = true;

            UpdateCharts();
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

                int count = 5;
                byte[][] array = new byte[count][];
                double[] stats = new double[count];
                int size;
                double total = 0;

                array[0] = cs.ConfigStruct.corrections.progress_filling_gbc_map;
                array[1] = cs.ConfigStruct.corrections.progress_filling_gbc_tps;
                array[2] = cs.ConfigStruct.corrections.progress_idle_valve_position;
                array[3] = cs.ConfigStruct.corrections.progress_ignitions;
                array[4] = cs.ConfigStruct.corrections.progress_knock_cy_level_multiplier;


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
                text += $"Fill by TPS: {stats[1].ToString("F2")}%\r\n";
                text += $"Idle Valve: {stats[2].ToString("F2")}%\r\n";
                text += $"Ignitions: {stats[3].ToString("F2")}%\r\n";
                text += $"Knock Noise: {stats[4].ToString("F2")}%\r\n";

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

                rbPhasedDisabled.Checked = false;
                rbPhasedWithSensor.Checked = false;
                rbPhasedWithoutSensor.Checked = false;

                if (cs.ConfigStruct.parameters.phasedMode == 0)
                {
                    rbPhasedDisabled.Checked = true;
                }
                else if (cs.ConfigStruct.parameters.phasedMode == 1)
                {
                    rbPhasedWithSensor.Checked = true;
                }
                else if (cs.ConfigStruct.parameters.phasedMode == 2)
                {
                    rbPhasedWithoutSensor.Checked = true;
                }
                else
                {
                    MessageBox.Show("Wrong Phased configuration!", "Engine Control Unit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                lblCutoffRPM.Text = cs.ConfigStruct.parameters.cutoffRPM.ToString("F0");
                tbCutoffRPM.Value = Convert.ToInt32(cs.ConfigStruct.parameters.cutoffRPM);

                lblCutoffAngle.Text = cs.ConfigStruct.parameters.cutoffAdvance.ToString("F1");
                tbCutoffAngle.Value = Convert.ToInt32(cs.ConfigStruct.parameters.cutoffAdvance * 10.0f);

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

                lblShiftAngle.Text = cs.ConfigStruct.parameters.shiftAdvance.ToString("F1");
                tbShiftAngle.Value = Convert.ToInt32(cs.ConfigStruct.parameters.shiftAdvance * 10.0f);

                lblShiftMixture.Text = cs.ConfigStruct.parameters.shiftMixture.ToString("F1");
                tbShiftMixture.Value = Convert.ToInt32(cs.ConfigStruct.parameters.shiftMixture * 10.0f);

                nudSwPos1.Value = cs.ConfigStruct.parameters.switchPos1Table + 1;
                nudSwPos0.Value = cs.ConfigStruct.parameters.switchPos0Table + 1;
                nudSwPos2.Value = cs.ConfigStruct.parameters.switchPos2Table + 1;
                nudFuelForce.Value = cs.ConfigStruct.parameters.forceTable + 1;
                nudEngVol.Value = (decimal)cs.ConfigStruct.parameters.engineVolume;
                nudSpeedInputCorr.Value = (decimal)cs.ConfigStruct.parameters.speedInputCorrection;
                nudSpeedOutputCorr.Value = (decimal)cs.ConfigStruct.parameters.speedOutputCorrection;
                nudParamsAirCalcKoffMin.Value = (decimal)cs.ConfigStruct.parameters.air_temp_corr_koff_min;
                nudParamsAirCalcKoffMax.Value = (decimal)cs.ConfigStruct.parameters.air_temp_corr_koff_max;
                nudLearnCyclesDelayMult.Value = (decimal)cs.ConfigStruct.parameters.learn_cycles_delay_mult;
                nudKnockIntegratorTimeConstant.Value = cs.ConfigStruct.parameters.knockIntegratorTime;
                nudParamsFanLowT.Value = (decimal)cs.ConfigStruct.parameters.fanLowTemperature;
                nudParamsFanMidT.Value = (decimal)cs.ConfigStruct.parameters.fanMidTemperature;
                nudParamsFanHighT.Value = (decimal)cs.ConfigStruct.parameters.fanHighTemperature;

                nudSensMapGain.Value = (decimal)cs.ConfigStruct.parameters.map_pressure_gain;
                nudSensMapOffset.Value = (decimal)cs.ConfigStruct.parameters.map_pressure_offset;
                nudSensTpsLow.Value = (decimal)cs.ConfigStruct.parameters.tps_voltage_low;
                nudSensTpsHigh.Value = (decimal)cs.ConfigStruct.parameters.tps_voltage_high;

                cbUseKnock.Checked = cs.ConfigStruct.parameters.useKnockSensor > 0;
                cbUseLambda.Checked = cs.ConfigStruct.parameters.useLambdaSensor > 0;
                cbUseIdleValve.Checked = cs.ConfigStruct.parameters.useIdleValve > 0;
                cbUseETC.Checked = cs.ConfigStruct.parameters.useETC > 0;
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
            rbEtcPos1D.Checked = false;
            rbEtcPos2D.Checked = false;

            switch (cs.ConfigStruct.tables[cs.CurrentTable].inj_channel)
            {
                case 0: rbInjCh1.Checked = true; break;
                case 1: rbInjCh2.Checked = true; break;
                default: break;
            }

            switch (cs.ConfigStruct.tables[cs.CurrentTable].throttle_position_use_1d)
            {
                case 0: rbEtcPos2D.Checked = true; break;
                case 1: rbEtcPos1D.Checked = true; break;
                default: break;
            }

            switch (cs.ConfigStruct.tables[cs.CurrentTable].enrichment_load_type)
            {
                case 0: rbEnrichTypeTPS.Checked = true; break;
                case 1: rbEnrichTypeMAP.Checked = true; break;
                default: break;
            }

            cbParamsIsInjectionPhaseByEnd.Checked = cs.ConfigStruct.tables[cs.CurrentTable].is_fuel_phase_by_end > 0;
            cbParamsIsInjectionPhaseByEnd.Checked = cs.ConfigStruct.tables[cs.CurrentTable].throttle_position_use_1d > 0;
            cbParamsIsPhAsyncEnrichmentEnabled.Checked = cs.ConfigStruct.tables[cs.CurrentTable].enrichment_ph_async_enabled > 0;
            cbParamsIsPhSyncEnrichmentEnabled.Checked = cs.ConfigStruct.tables[cs.CurrentTable].enrichment_ph_sync_enabled > 0;
            cbParamsIsPhPostEnrichmentEnabled.Checked = cs.ConfigStruct.tables[cs.CurrentTable].enrichment_ph_post_injection_enabled > 0;
            cbParamsIsPpAsyncEnrichmentEnabled.Checked = cs.ConfigStruct.tables[cs.CurrentTable].enrichment_pp_async_enabled > 0;
            cbParamsIsPpSyncEnrichmentEnabled.Checked = cs.ConfigStruct.tables[cs.CurrentTable].enrichment_pp_sync_enabled > 0;
            cbParamsIsPpPostEnrichmentEnabled.Checked = cs.ConfigStruct.tables[cs.CurrentTable].enrichment_pp_post_injection_enabled > 0;
            nudParamsCntPressures.Maximum = Consts.TABLE_PRESSURES_MAX;
            nudParamsCntRPMs.Maximum = Consts.TABLE_ROTATES_MAX;
            nudParamsCntIdleRPMs.Maximum = Consts.TABLE_ROTATES_MAX;
            nudParamsCntThrottles.Maximum = Consts.TABLE_THROTTLES_MAX;
            nudParamsCntPedals.Maximum = Consts.TABLE_PEDALS_MAX;
            nudParamsCntVoltages.Maximum = Consts.TABLE_VOLTAGES_MAX;
            nudParamsCntFillings.Maximum = Consts.TABLE_FILLING_MAX;
            nudParamsCntEngineTemps.Maximum = Consts.TABLE_TEMPERATURES_MAX;
            nudParamsCntAirTemps.Maximum = Consts.TABLE_TEMPERATURES_MAX;
            nudParamsCntSpeeds.Maximum = Consts.TABLE_SPEEDS_MAX;
            nudParamsIdleValveMin.Maximum = Consts.IDLE_VALVE_POS_MAX;
            nudParamsIdleValveMax.Maximum = Consts.IDLE_VALVE_POS_MAX;
            nudParamsCntEnrichmentStartLoad.Maximum = Consts.TABLE_ENRICHMENT_PERCENTS_MAX;
            nudParamsCntEnrichmentLoadDerivative.Maximum = Consts.TABLE_ENRICHMENT_PERCENTS_MAX;

            nudParamsCntPressures.Value = cs.ConfigStruct.tables[cs.CurrentTable].pressures_count;
            nudParamsCntRPMs.Value = cs.ConfigStruct.tables[cs.CurrentTable].rotates_count;
            nudParamsCntIdleRPMs.Value = cs.ConfigStruct.tables[cs.CurrentTable].idle_rotates_count;
            nudParamsCntThrottles.Value = cs.ConfigStruct.tables[cs.CurrentTable].throttles_count;
            nudParamsCntPedals.Value = cs.ConfigStruct.tables[cs.CurrentTable].pedals_count;
            nudParamsCntVoltages.Value = cs.ConfigStruct.tables[cs.CurrentTable].voltages_count;
            nudParamsCntFillings.Value = cs.ConfigStruct.tables[cs.CurrentTable].fillings_count;
            nudParamsCntEngineTemps.Value = cs.ConfigStruct.tables[cs.CurrentTable].engine_temp_count;
            nudParamsCntAirTemps.Value = cs.ConfigStruct.tables[cs.CurrentTable].air_temp_count;
            nudParamsCntSpeeds.Value = cs.ConfigStruct.tables[cs.CurrentTable].idle_speeds_shift_count;
            nudParamsCntEnrichmentStartLoad.Value = cs.ConfigStruct.tables[cs.CurrentTable].enrichment_rate_start_load_count;
            nudParamsCntEnrichmentLoadDerivative.Value = cs.ConfigStruct.tables[cs.CurrentTable].enrichment_rate_load_derivative_count;

            nudParamsIdleValveMin.Value = cs.ConfigStruct.tables[cs.CurrentTable].idle_valve_pos_min;
            nudParamsIdleValveMax.Value = cs.ConfigStruct.tables[cs.CurrentTable].idle_valve_pos_max;
            nudParamsIdleThrottleMin.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].idle_throttle_pos_min;
            nudParamsIdleThrottleMax.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].idle_throttle_pos_max;

            nudParamsEnrichmentLoadDeadBand.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].enrichment_load_dead_band;
            nudParamsEnrichmentAccelDeadBand.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].enrichment_accel_dead_band;
            nudParamsEnrichmentDetectDuration.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].enrichment_detect_duration;
            nudParamsEnrichmentIgnitionDecayTime.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].enrichment_ign_corr_decay_time;
            nudParamsEnrichmentInjectionPhaseDecayTime.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].enrichment_injection_phase_decay_time;
            nudParamsEnrichmentAsyncPulsesDivider.Value = cs.ConfigStruct.tables[cs.CurrentTable].enrichment_async_pulses_divider;
            nudParamsEnrichmentPostInjectionFinalPhase.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].enrichment_end_injection_final_phase;
            nudParamsEnrichmentPostInjectionFinalAmount.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].enrichment_end_injection_final_amount; 
            
            nudParamsInjPerformance.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].injector_performance;
            nudParamsFuelKgL.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].fuel_mass_per_cc;
            nudParamsFuelAFR.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].fuel_afr;

            nudParamsPidShortCorrP.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].short_term_corr_pid_p;
            nudParamsPidShortCorrI.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].short_term_corr_pid_i;
            nudParamsPidShortCorrD.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].short_term_corr_pid_d;

            nudParamsFanLowV.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].fan_advance_control_low;
            nudParamsFanMidV.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].fan_advance_control_mid;
            nudParamsFanHighV.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].fan_advance_control_high;

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
            nudParamsStartLargeCount.Value = (decimal)cs.ConfigStruct.tables[cs.CurrentTable].start_large_count;

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
                            series.Points.AddXY(lDragRuns[i].Points[j].Time, lDragRuns[i].Points[j].RPM);
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
                                item.SubItems.Add($"{((timemax - timemin) / 1000000.0).ToString("F2")}s");
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
                mGenIgn.NeedleVal = parameters.IgnitionAdvance;
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

        private void nudParamsFanLowV_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].fan_advance_control_low = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsFanMidV_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].fan_advance_control_mid = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsFanHighV_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].fan_advance_control_high = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
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
            cs.ConfigStruct.parameters.cutoffAdvance = ((float)((TrackBar)sender).Value / 10.0f);
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
            cs.ConfigStruct.parameters.shiftAdvance = ((float)((TrackBar)sender).Value / 10.0f);
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

        private void nudParamsAirCalcKoffMin_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.air_temp_corr_koff_min = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudParamsAirCalcKoffMax_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.air_temp_corr_koff_max = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudLearnCyclesDelayMult_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.learn_cycles_delay_mult = (float)((NumericUpDown)sender).Value;
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

        private void cbUseIdleValve_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.useIdleValve = ((CheckBox)sender).Checked ? 1 : 0;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void cbUseETC_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.useETC = ((CheckBox)sender).Checked ? 1 : 0;
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
                if (middleLayer == null || !middleLayer.IsSynchronizing)
                {
                    if (rbCorrInterpolationFunc.Checked && !rbCorrPointFunc.Checked)
                        cs.ConfigStruct.parameters.performAdaptation = 1;
                    else if (rbCorrPointFunc.Checked && !rbCorrInterpolationFunc.Checked)
                        cs.ConfigStruct.parameters.performAdaptation = 2;

                    if (cs.ConfigStruct.parameters.performAdaptation > 0)
                    {
                        CorrStart();
                        CorrClear();
                        middleLayer?.UpdateConfig();
                    }
                }
            }
        }

        private void btnCorrStop_Click(object sender, EventArgs e)
        {
            if (middleLayer == null || !middleLayer.IsSynchronizing)
            {
                cs.ConfigStruct.parameters.performAdaptation = 0;
                CorrStop();
                middleLayer?.UpdateConfig();
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

        private void rbPhasedDisabled_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                rbPhasedWithSensor.Checked = false;
                rbPhasedWithoutSensor.Checked = false;
                cs.ConfigStruct.parameters.phasedMode = 0;
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateConfig();
                }
            }
        }

        private void rbPhasedWithSensor_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                rbPhasedDisabled.Checked = false;
                rbPhasedWithoutSensor.Checked = false;
                cs.ConfigStruct.parameters.phasedMode = 1;
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateConfig();
                }
            }
        }

        private void rbPhasedWithoutSensor_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                rbPhasedWithSensor.Checked = false;
                rbPhasedDisabled.Checked = false;
                cs.ConfigStruct.parameters.phasedMode = 2;
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

        private void rbEtcPos1D_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                cs.ConfigStruct.tables[cs.CurrentTable].throttle_position_use_1d = 1;
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateTable(cs.CurrentTable);
                }
            }
        }

        private void rbEtcPos2D_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                cs.ConfigStruct.tables[cs.CurrentTable].throttle_position_use_1d = 0;
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
                    cs.ConfigStruct.tables[cs.CurrentTable].pressures[i + 1] = cs.ConfigStruct.tables[cs.CurrentTable].pressures[i] + 0.01f;
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

        private void nudParamsCntPedals_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].pedals_count = (int)((NumericUpDown)sender).Value;
            for (int i = 0; i < cs.ConfigStruct.tables[cs.CurrentTable].pedals_count - 1; i++)
                if (cs.ConfigStruct.tables[cs.CurrentTable].pedals[i + 1] <= cs.ConfigStruct.tables[cs.CurrentTable].pedals[i])
                    cs.ConfigStruct.tables[cs.CurrentTable].pedals[i + 1] = cs.ConfigStruct.tables[cs.CurrentTable].pedals[i] + 2;
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

        private void nudParamsStartLargeCount_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].start_large_count = (int)((NumericUpDown)sender).Value;
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
                float[] array2d = cs.ConfigStruct.tables[cs.CurrentTable].filling_gbc_map;
                float[] corrs2d = cs.ConfigStruct.corrections.filling_gbc_map;
                int size = array2d.Length;

                for (int i = 0; i < size; i++)
                {
                    array2d[i] *= corrs2d[i] + 1.0F;
                    corrs2d[i] = 0.0F;
                }

                middleLayer?.SyncSave(false);
                UpdateCharts();
            }
        }

        private void btnCorrAppendFillingByTPS_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to append Filling by TPS?", "Engine Control Unit", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dialogResult == DialogResult.Yes)
            {
                float[] array2d = cs.ConfigStruct.tables[cs.CurrentTable].filling_gbc_tps;
                float[] corrs2d = cs.ConfigStruct.corrections.filling_gbc_tps;
                int size = array2d.Length;

                for (int i = 0; i < size; i++)
                {
                    array2d[i] *= corrs2d[i] + 1.0F;
                    corrs2d[i] = 0.0F;
                }

                middleLayer?.SyncSave(false);
                UpdateCharts();
            }
        }

        private void btnCorrAppendIdleValve_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to append Idle Valve to RPM?", "Engine Control Unit", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dialogResult == DialogResult.Yes)
            {
                float[] array2d = cs.ConfigStruct.tables[cs.CurrentTable].idle_valve_position;
                float[] corrs2d = cs.ConfigStruct.corrections.idle_valve_position;
                int size = array2d.Length;

                for (int i = 0; i < size; i++)
                {
                    array2d[i] *= corrs2d[i] + 1.0F;
                    corrs2d[i] = 0.0F;
                }

                middleLayer?.SyncSave(false);
                UpdateCharts();
            }
        }

        private void btnCorrAppendIgnitions_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to append Ignitions?", "Engine Control Unit", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dialogResult == DialogResult.Yes)
            {
                float[] array2d = cs.ConfigStruct.tables[cs.CurrentTable].ignitions;
                float[] corrs2d = cs.ConfigStruct.corrections.ignitions;
                float[][] array3d = new float[Consts.ECU_CYLINDERS_COUNT][];
                float[][] corrs3d = new float[Consts.ECU_CYLINDERS_COUNT][];
                int size = array2d.Length;

                for (int i = 0; i < size; i++)
                {
                    array2d[i] += corrs2d[i];
                    corrs2d[i] = 0.0F;
                }

                array3d[0] = cs.ConfigStruct.tables[cs.CurrentTable].ignition_corr_cy1;
                array3d[1] = cs.ConfigStruct.tables[cs.CurrentTable].ignition_corr_cy2;
                array3d[2] = cs.ConfigStruct.tables[cs.CurrentTable].ignition_corr_cy3;
                array3d[3] = cs.ConfigStruct.tables[cs.CurrentTable].ignition_corr_cy4;

                corrs3d[0] = cs.ConfigStruct.corrections.ignition_corr_cy1;
                corrs3d[1] = cs.ConfigStruct.corrections.ignition_corr_cy2;
                corrs3d[2] = cs.ConfigStruct.corrections.ignition_corr_cy3;
                corrs3d[3] = cs.ConfigStruct.corrections.ignition_corr_cy4;

                for (int c = 0; c < Consts.ECU_CYLINDERS_COUNT; c++)
                {
                    array2d = array3d[c];
                    corrs2d = corrs3d[c];
                    size = array2d.Length;

                    for (int i = 0; i < size; i++)
                    {
                        array2d[i] += corrs2d[i];
                        corrs2d[i] = 0.0F;
                    }
                }

                middleLayer?.SyncSave(false);
                UpdateCharts();
            }
        }

        private void btnCorrAppendInjections_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to append Ignitions?", "Engine Control Unit", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dialogResult == DialogResult.Yes)
            {
                float[] array2d;
                float[] corrs2d;
                float[][] array3d = new float[Consts.ECU_CYLINDERS_COUNT][];
                float[][] corrs3d = new float[Consts.ECU_CYLINDERS_COUNT][];
                int size;


                array3d[0] = cs.ConfigStruct.tables[cs.CurrentTable].injection_corr_cy1;
                array3d[1] = cs.ConfigStruct.tables[cs.CurrentTable].injection_corr_cy2;
                array3d[2] = cs.ConfigStruct.tables[cs.CurrentTable].injection_corr_cy3;
                array3d[3] = cs.ConfigStruct.tables[cs.CurrentTable].injection_corr_cy4;

                corrs3d[0] = cs.ConfigStruct.corrections.injection_corr_cy1;
                corrs3d[1] = cs.ConfigStruct.corrections.injection_corr_cy2;
                corrs3d[2] = cs.ConfigStruct.corrections.injection_corr_cy3;
                corrs3d[3] = cs.ConfigStruct.corrections.injection_corr_cy4;

                for (int c = 0; c < Consts.ECU_CYLINDERS_COUNT; c++)
                {
                    array2d = array3d[c];
                    corrs2d = corrs3d[c];
                    size = array2d.Length;

                    for (int i = 0; i < size; i++)
                    {
                        array2d[i] += corrs2d[i];
                        corrs2d[i] = 0.0F;
                    }
                }

                middleLayer?.SyncSave(false);
                UpdateCharts();
            }
        }
        private void btnCorrAppendKnockNoise_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to append Knock Noise Level?", "Engine Control Unit", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dialogResult == DialogResult.Yes)
            {
                float[] array2d = cs.ConfigStruct.tables[cs.CurrentTable].knock_cy_level_multiplier;
                float[] corrs2d = cs.ConfigStruct.corrections.knock_cy_level_multiplier;
                int size = array2d.Length;

                for (int i = 0; i < size; i++)
                {
                    array2d[i] *= corrs2d[i] + 1.0F;
                    corrs2d[i] = 0.0F;
                }

                middleLayer?.SyncSave(false);
                UpdateCharts();
            }
        }

        private void btnIgnAppendCylinders_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to append Ignition Cylinders?", "Engine Control Unit", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dialogResult == DialogResult.Yes)
            {
                bool[] cy_append = new bool[] { cbIgnAppendCy1.Checked, cbIgnAppendCy2.Checked, cbIgnAppendCy3.Checked, cbIgnAppendCy4.Checked };
                float[][] cy_tables = new float[][] { cs.ConfigStruct.tables[cs.CurrentTable].ignition_corr_cy1, cs.ConfigStruct.tables[cs.CurrentTable].ignition_corr_cy2, cs.ConfigStruct.tables[cs.CurrentTable].ignition_corr_cy3, cs.ConfigStruct.tables[cs.CurrentTable].ignition_corr_cy4 };

                float[] array2d = cs.ConfigStruct.tables[cs.CurrentTable].ignitions;
                float[] mid2d = new float[array2d.Length];

                for (int i = 0; i < mid2d.Length; i++)
                {
                    mid2d[i] = float.MaxValue;
                    for (int c = 0; c < cy_tables.Length; c++)
                    {
                        if (cy_append[c])
                        {
                            mid2d[i] = mid2d[i] < cy_tables[c][i] ? mid2d[i] : cy_tables[c][i];
                            cy_tables[c][i] = 0;
                        }
                    }
                    array2d[i] += mid2d[i];
                }

                middleLayer?.SyncSave(false);
                UpdateCharts();
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

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNodeListInfo tnli = e.Node.Tag as TreeNodeListInfo;
            if(tnli != null)
            {
                try
                {
                    tnli.TabControl.SelectedTab = tnli.TabPage;
                    if (e.Node.Parent != null)
                    {
                        treeView_AfterSelect(sender, new TreeViewEventArgs(e.Node.Parent));
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine("\r\n");
                }
            }
        }


        private void rbEnrichTypeTPS_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                cs.ConfigStruct.tables[cs.CurrentTable].enrichment_load_type = 0;
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateTable(cs.CurrentTable);
                }
            }
        }

        private void rbEnrichTypeMAP_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                cs.ConfigStruct.tables[cs.CurrentTable].enrichment_load_type = 1;
                if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
                {
                    middleLayer.UpdateTable(cs.CurrentTable);
                }
            }
        }

        private void nudParamsCntEnrichmentStartLoad_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].enrichment_rate_start_load_count = (int)((NumericUpDown)sender).Value;
            for (int i = 0; i < cs.ConfigStruct.tables[cs.CurrentTable].enrichment_rate_start_load_count - 1; i++)
                if (cs.ConfigStruct.tables[cs.CurrentTable].enrichment_rate_start_load[i + 1] <= cs.ConfigStruct.tables[cs.CurrentTable].enrichment_rate_start_load[i])
                    cs.ConfigStruct.tables[cs.CurrentTable].enrichment_rate_start_load[i + 1] = cs.ConfigStruct.tables[cs.CurrentTable].enrichment_rate_start_load[i] + 1;
            UpdateEcuTableValues();
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsCntEnrichmentLoadDerivative_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].enrichment_rate_load_derivative_count = (int)((NumericUpDown)sender).Value;
            for (int i = 0; i < cs.ConfigStruct.tables[cs.CurrentTable].enrichment_rate_load_derivative_count - 1; i++)
                if (cs.ConfigStruct.tables[cs.CurrentTable].enrichment_rate_load_derivative[i + 1] <= cs.ConfigStruct.tables[cs.CurrentTable].enrichment_rate_load_derivative[i])
                    cs.ConfigStruct.tables[cs.CurrentTable].enrichment_rate_load_derivative[i + 1] = cs.ConfigStruct.tables[cs.CurrentTable].enrichment_rate_load_derivative[i] + 10;
            UpdateEcuTableValues();
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsEnrichmentLoadDeadBand_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].enrichment_load_dead_band = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsEnrichmentAccelDeadBand_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].enrichment_accel_dead_band = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsEnrichmentDetectDuration_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].enrichment_detect_duration = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsEnrichmentIgnitionDecayTime_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].enrichment_ign_corr_decay_time = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsEnrichmentInjectionPhaseDecayTime_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].enrichment_injection_phase_decay_time = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsEnrichmentAsyncPulsesDivider_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].enrichment_async_pulses_divider = (int)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsEnrichmentPostInjectionFinalPhase_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].enrichment_end_injection_final_phase = (int)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void cbParamsIsPhPostEnrichmentEnabled_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].enrichment_ph_post_injection_enabled = ((CheckBox)sender).Checked ? 1 : 0;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void cbParamsIsPpPostEnrichmentEnabled_CheckedChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].enrichment_pp_post_injection_enabled = ((CheckBox)sender).Checked ? 1 : 0;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsEnrichmentPostInjectionFinalAmount_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].enrichment_end_injection_final_amount = (int)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudSensMapGain_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.map_pressure_gain = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudSensMapOffset_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.map_pressure_offset = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudSensTpsMin_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.tps_voltage_low = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudSensTpsMax_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.parameters.tps_voltage_high = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateConfig();
            }
        }

        private void nudParamsIdleValveMin_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].idle_valve_pos_min = (int)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsIdleValveMax_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].idle_valve_pos_max = (int)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsIdleThrottleMin_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].idle_throttle_pos_min = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }

        private void nudParamsIdleThrottleMax_ValueChanged(object sender, EventArgs e)
        {
            cs.ConfigStruct.tables[cs.CurrentTable].idle_throttle_pos_max = (float)((NumericUpDown)sender).Value;
            if (middleLayer != null && !middleLayer.IsSynchronizing && cbLive.Checked)
            {
                middleLayer.UpdateTable(cs.CurrentTable);
            }
        }
    }
}

