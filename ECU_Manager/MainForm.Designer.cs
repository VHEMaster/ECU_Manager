namespace ECU_Manager
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tmr50ms = new System.Windows.Forms.Timer(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label56 = new System.Windows.Forms.Label();
            this.mGenFuelUsage = new ECU_Manager.Controls.Meter();
            this.label5 = new System.Windows.Forms.Label();
            this.mGenTemp = new ECU_Manager.Controls.Meter();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.mGenPress = new ECU_Manager.Controls.Meter();
            this.mGenRPM = new ECU_Manager.Controls.Meter();
            this.mGenIgn = new ECU_Manager.Controls.Meter();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label60 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.nudForceIgnitionAngle = new System.Windows.Forms.NumericUpDown();
            this.cbForceIgnition = new System.Windows.Forms.CheckBox();
            this.nudEngVol = new System.Windows.Forms.NumericUpDown();
            this.cbForceIdle = new System.Windows.Forms.CheckBox();
            this.cbHallLearn = new System.Windows.Forms.CheckBox();
            this.cbHallIgnition = new System.Windows.Forms.CheckBox();
            this.cbAutostartEnabled = new System.Windows.Forms.CheckBox();
            this.cbTempEnabled = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.nudFuelForce = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.nudSwPos2 = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.nudSwPos0 = new System.Windows.Forms.NumericUpDown();
            this.cbFuelForce = new System.Windows.Forms.CheckBox();
            this.cbFuelExtSw = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.nudSwPos1 = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblEconRPM = new System.Windows.Forms.Label();
            this.tbEconRPM = new System.Windows.Forms.TrackBar();
            this.label11 = new System.Windows.Forms.Label();
            this.cbEconIgnitionBreak = new System.Windows.Forms.CheckBox();
            this.cbEconStrobe = new System.Windows.Forms.CheckBox();
            this.cbEconEnabled = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblCutoffAngle = new System.Windows.Forms.Label();
            this.tbCutoffAngle = new System.Windows.Forms.TrackBar();
            this.label9 = new System.Windows.Forms.Label();
            this.lblCutoffRPM = new System.Windows.Forms.Label();
            this.tbCutoffRPM = new System.Windows.Forms.TrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.rbCutoffMode7 = new System.Windows.Forms.RadioButton();
            this.rbCutoffMode6 = new System.Windows.Forms.RadioButton();
            this.rbCutoffMode5 = new System.Windows.Forms.RadioButton();
            this.rbCutoffMode8 = new System.Windows.Forms.RadioButton();
            this.rbCutoffMode4 = new System.Windows.Forms.RadioButton();
            this.rbCutoffMode3 = new System.Windows.Forms.RadioButton();
            this.rbCutoffMode2 = new System.Windows.Forms.RadioButton();
            this.rbCutoffMode1 = new System.Windows.Forms.RadioButton();
            this.cbCutoffEnabled = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabControl111 = new System.Windows.Forms.TabControl();
            this.tabPage12 = new System.Windows.Forms.TabPage();
            this.btnTableImport = new System.Windows.Forms.Button();
            this.btnTableExport = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnToolsCopy = new System.Windows.Forms.Button();
            this.nudToolsCopyTo = new System.Windows.Forms.NumericUpDown();
            this.label43 = new System.Windows.Forms.Label();
            this.nudToolsCopyFrom = new System.Windows.Forms.NumericUpDown();
            this.label42 = new System.Windows.Forms.Label();
            this.nudToolsCurTable = new System.Windows.Forms.NumericUpDown();
            this.label23 = new System.Windows.Forms.Label();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.label55 = new System.Windows.Forms.Label();
            this.nudParamsFuelVolume = new System.Windows.Forms.NumericUpDown();
            this.label54 = new System.Windows.Forms.Label();
            this.nudParamsFuelRate = new System.Windows.Forms.NumericUpDown();
            this.label51 = new System.Windows.Forms.Label();
            this.nudParamsCntTemps = new System.Windows.Forms.NumericUpDown();
            this.label52 = new System.Windows.Forms.Label();
            this.nudParamsCntIdles = new System.Windows.Forms.NumericUpDown();
            this.label50 = new System.Windows.Forms.Label();
            this.nudParamsValveTimeout = new System.Windows.Forms.NumericUpDown();
            this.label49 = new System.Windows.Forms.Label();
            this.nudParamsCntRPMs = new System.Windows.Forms.NumericUpDown();
            this.label48 = new System.Windows.Forms.Label();
            this.nudParamsCntPress = new System.Windows.Forms.NumericUpDown();
            this.label47 = new System.Windows.Forms.Label();
            this.nudParamsInitial = new System.Windows.Forms.NumericUpDown();
            this.label46 = new System.Windows.Forms.Label();
            this.nudParamsOctane = new System.Windows.Forms.NumericUpDown();
            this.rbParamsValve2 = new System.Windows.Forms.RadioButton();
            this.rbParamsValve1 = new System.Windows.Forms.RadioButton();
            this.label45 = new System.Windows.Forms.Label();
            this.rbParamsValve0 = new System.Windows.Forms.RadioButton();
            this.label44 = new System.Windows.Forms.Label();
            this.tbParamsName = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage18 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.label63 = new System.Windows.Forms.Label();
            this.tbDragName = new System.Windows.Forms.TextBox();
            this.label65 = new System.Windows.Forms.Label();
            this.nudDragTableSplit = new System.Windows.Forms.NumericUpDown();
            this.lblDragSpeed = new System.Windows.Forms.Label();
            this.label64 = new System.Windows.Forms.Label();
            this.lblDragTime = new System.Windows.Forms.Label();
            this.label62 = new System.Windows.Forms.Label();
            this.btnDragClear = new System.Windows.Forms.Button();
            this.btnDragStop = new System.Windows.Forms.Button();
            this.btnDragStart = new System.Windows.Forms.Button();
            this.lblDragStatus = new System.Windows.Forms.Label();
            this.label59 = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.nudDragSpeedTo = new System.Windows.Forms.NumericUpDown();
            this.label58 = new System.Windows.Forms.Label();
            this.nudDragSpeedFrom = new System.Windows.Forms.NumericUpDown();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage19 = new System.Windows.Forms.TabPage();
            this.chartDragTime = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabPage21 = new System.Windows.Forms.TabPage();
            this.chartDragAccel = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabPage20 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.lvDragTable = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.cbLive = new System.Windows.Forms.CheckBox();
            this.dlgExport = new System.Windows.Forms.SaveFileDialog();
            this.dlgImport = new System.Windows.Forms.OpenFileDialog();
            this.tmr1sec = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudForceIgnitionAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEngVol)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFuelForce)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSwPos2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSwPos0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSwPos1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbEconRPM)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbCutoffAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbCutoffRPM)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tabControl111.SuspendLayout();
            this.tabPage12.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudToolsCopyTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudToolsCopyFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudToolsCurTable)).BeginInit();
            this.tabPage8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudParamsFuelVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudParamsFuelRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudParamsCntTemps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudParamsCntIdles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudParamsValveTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudParamsCntRPMs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudParamsCntPress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudParamsInitial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudParamsOctane)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.tabPage18.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDragTableSplit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDragSpeedTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDragSpeedFrom)).BeginInit();
            this.tabControl2.SuspendLayout();
            this.tabPage19.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartDragTime)).BeginInit();
            this.tabPage21.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartDragAccel)).BeginInit();
            this.tabPage20.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmr50ms
            // 
            this.tmr50ms.Enabled = true;
            this.tmr50ms.Interval = 50;
            this.tmr50ms.Tick += new System.EventHandler(this.tmr50ms_Tick);
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage18);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(2, 36);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(18, 7);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1107, 653);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.Controls.Add(this.tableLayoutPanel3);
            this.tabPage1.Location = new System.Drawing.Point(4, 44);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1099, 605);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General Status";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.13343F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 44.86657F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1093, 599);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.00049F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0005F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0005F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.9985F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.Controls.Add(this.label56, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.mGenFuelUsage, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.label5, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.mGenTemp, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.label4, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.mGenPress, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.mGenRPM, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.mGenIgn, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1087, 324);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label56.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label56.Location = new System.Drawing.Point(871, 274);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(213, 50);
            this.label56.TabIndex = 9;
            this.label56.Text = "Fuel Usage (l/h)";
            this.label56.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mGenFuelUsage
            // 
            this.mGenFuelUsage.CustNumSize = 20F;
            this.mGenFuelUsage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mGenFuelUsage.DynNumSize = 31F;
            this.mGenFuelUsage.FaceColor = System.Drawing.Color.Black;
            this.mGenFuelUsage.Location = new System.Drawing.Point(871, 3);
            this.mGenFuelUsage.MaxDeg = 320F;
            this.mGenFuelUsage.MinDeg = 40F;
            this.mGenFuelUsage.Name = "mGenFuelUsage";
            this.mGenFuelUsage.NeedleColor = System.Drawing.Color.Yellow;
            this.mGenFuelUsage.NeedleVal = 0F;
            this.mGenFuelUsage.NumColor = System.Drawing.Color.White;
            this.mGenFuelUsage.Size = new System.Drawing.Size(213, 268);
            this.mGenFuelUsage.TabIndex = 8;
            this.mGenFuelUsage.TickColor = System.Drawing.Color.White;
            this.mGenFuelUsage.TickIncrement = 2F;
            this.mGenFuelUsage.TickWarningColor = System.Drawing.Color.Orange;
            this.mGenFuelUsage.UseCustNumSize = true;
            this.mGenFuelUsage.ValueRange = 50F;
            this.mGenFuelUsage.WarnTickStart = 25F;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(654, 274);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(211, 50);
            this.label5.TabIndex = 7;
            this.label5.Text = "Temperature (°C)";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mGenTemp
            // 
            this.mGenTemp.CustNumSize = 20F;
            this.mGenTemp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mGenTemp.DynNumSize = 31F;
            this.mGenTemp.FaceColor = System.Drawing.Color.Black;
            this.mGenTemp.Location = new System.Drawing.Point(654, 3);
            this.mGenTemp.MaxDeg = 320F;
            this.mGenTemp.MinDeg = 40F;
            this.mGenTemp.Name = "mGenTemp";
            this.mGenTemp.NeedleColor = System.Drawing.Color.Yellow;
            this.mGenTemp.NeedleVal = 0F;
            this.mGenTemp.NumColor = System.Drawing.Color.White;
            this.mGenTemp.Size = new System.Drawing.Size(211, 268);
            this.mGenTemp.TabIndex = 6;
            this.mGenTemp.TickColor = System.Drawing.Color.White;
            this.mGenTemp.TickIncrement = 5F;
            this.mGenTemp.TickWarningColor = System.Drawing.Color.Orange;
            this.mGenTemp.UseCustNumSize = true;
            this.mGenTemp.ValueRange = 150F;
            this.mGenTemp.WarnTickStart = 100F;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(437, 274);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(211, 50);
            this.label4.TabIndex = 5;
            this.label4.Text = "Pressure (Pa)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(3, 274);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(211, 50);
            this.label3.TabIndex = 4;
            this.label3.Text = "RPM";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mGenPress
            // 
            this.mGenPress.CustNumSize = 20F;
            this.mGenPress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mGenPress.DynNumSize = 31F;
            this.mGenPress.FaceColor = System.Drawing.Color.Black;
            this.mGenPress.Location = new System.Drawing.Point(437, 3);
            this.mGenPress.MaxDeg = 320F;
            this.mGenPress.MinDeg = 40F;
            this.mGenPress.Name = "mGenPress";
            this.mGenPress.NeedleColor = System.Drawing.Color.Yellow;
            this.mGenPress.NeedleVal = 0F;
            this.mGenPress.NumColor = System.Drawing.Color.White;
            this.mGenPress.Size = new System.Drawing.Size(211, 268);
            this.mGenPress.TabIndex = 2;
            this.mGenPress.TickColor = System.Drawing.Color.White;
            this.mGenPress.TickIncrement = 10000F;
            this.mGenPress.TickWarningColor = System.Drawing.Color.Orange;
            this.mGenPress.UseCustNumSize = true;
            this.mGenPress.ValueRange = 110000F;
            this.mGenPress.WarnTickStart = 95000F;
            // 
            // mGenRPM
            // 
            this.mGenRPM.CustNumSize = 20F;
            this.mGenRPM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mGenRPM.DynNumSize = 31F;
            this.mGenRPM.FaceColor = System.Drawing.Color.Black;
            this.mGenRPM.Location = new System.Drawing.Point(3, 3);
            this.mGenRPM.MaxDeg = 320F;
            this.mGenRPM.MinDeg = 40F;
            this.mGenRPM.Name = "mGenRPM";
            this.mGenRPM.NeedleColor = System.Drawing.Color.Yellow;
            this.mGenRPM.NeedleVal = 0F;
            this.mGenRPM.NumColor = System.Drawing.Color.White;
            this.mGenRPM.Size = new System.Drawing.Size(211, 268);
            this.mGenRPM.TabIndex = 1;
            this.mGenRPM.TickColor = System.Drawing.Color.White;
            this.mGenRPM.TickIncrement = 1000F;
            this.mGenRPM.TickWarningColor = System.Drawing.Color.Red;
            this.mGenRPM.UseCustNumSize = true;
            this.mGenRPM.ValueRange = 8000F;
            this.mGenRPM.WarnTickStart = 6000F;
            // 
            // mGenIgn
            // 
            this.mGenIgn.CustNumSize = 20F;
            this.mGenIgn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mGenIgn.DynNumSize = 31F;
            this.mGenIgn.FaceColor = System.Drawing.Color.Black;
            this.mGenIgn.Location = new System.Drawing.Point(220, 3);
            this.mGenIgn.MaxDeg = 320F;
            this.mGenIgn.MinDeg = 40F;
            this.mGenIgn.Name = "mGenIgn";
            this.mGenIgn.NeedleColor = System.Drawing.Color.Yellow;
            this.mGenIgn.NeedleVal = 0F;
            this.mGenIgn.NumColor = System.Drawing.Color.White;
            this.mGenIgn.Size = new System.Drawing.Size(211, 268);
            this.mGenIgn.TabIndex = 0;
            this.mGenIgn.TickColor = System.Drawing.Color.White;
            this.mGenIgn.TickIncrement = 5F;
            this.mGenIgn.TickWarningColor = System.Drawing.Color.White;
            this.mGenIgn.UseCustNumSize = true;
            this.mGenIgn.ValueRange = 60F;
            this.mGenIgn.WarnTickStart = 45F;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(220, 274);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(211, 50);
            this.label2.TabIndex = 3;
            this.label2.Text = "Ignition Angle";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label20);
            this.panel1.Controls.Add(this.label19);
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.label17);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 333);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1087, 263);
            this.panel1.TabIndex = 1;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(6, 34);
            this.label20.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(119, 24);
            this.label20.TabIndex = 7;
            this.label20.Text = "Table Name:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 58);
            this.label19.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(105, 24);
            this.label19.TabIndex = 6;
            this.label19.Text = "Fuel Valve:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 82);
            this.label18.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(114, 24);
            this.label18.TabIndex = 5;
            this.label18.Text = "Batt Voltage:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 10);
            this.label17.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(137, 24);
            this.label17.TabIndex = 4;
            this.label17.Text = "Table Number:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(143, 82);
            this.label16.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(63, 24);
            this.label16.TabIndex = 3;
            this.label16.Text = "Invalid";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(143, 58);
            this.label10.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 24);
            this.label10.TabIndex = 2;
            this.label10.Text = "Invalid";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(143, 34);
            this.label8.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 24);
            this.label8.TabIndex = 1;
            this.label8.Text = "Invalid";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(143, 10);
            this.label7.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 24);
            this.label7.TabIndex = 0;
            this.label7.Text = "Invalid";
            // 
            // tabPage2
            // 
            this.tabPage2.AutoScroll = true;
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 44);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1099, 605);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Parameters";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label60);
            this.groupBox4.Controls.Add(this.label53);
            this.groupBox4.Controls.Add(this.nudForceIgnitionAngle);
            this.groupBox4.Controls.Add(this.cbForceIgnition);
            this.groupBox4.Controls.Add(this.nudEngVol);
            this.groupBox4.Controls.Add(this.cbForceIdle);
            this.groupBox4.Controls.Add(this.cbHallLearn);
            this.groupBox4.Controls.Add(this.cbHallIgnition);
            this.groupBox4.Controls.Add(this.cbAutostartEnabled);
            this.groupBox4.Controls.Add(this.cbTempEnabled);
            this.groupBox4.Location = new System.Drawing.Point(444, 11);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(225, 294);
            this.groupBox4.TabIndex = 17;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Miscellaneous";
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Location = new System.Drawing.Point(6, 256);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(93, 24);
            this.label60.TabIndex = 12;
            this.label60.Text = "Ign.angle:";
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(3, 198);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(114, 24);
            this.label53.TabIndex = 8;
            this.label53.Text = "Engine Vol.:";
            // 
            // nudForceIgnitionAngle
            // 
            this.nudForceIgnitionAngle.Location = new System.Drawing.Point(102, 254);
            this.nudForceIgnitionAngle.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nudForceIgnitionAngle.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.nudForceIgnitionAngle.Name = "nudForceIgnitionAngle";
            this.nudForceIgnitionAngle.Size = new System.Drawing.Size(99, 29);
            this.nudForceIgnitionAngle.TabIndex = 11;
            this.nudForceIgnitionAngle.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudForceIgnitionAngle.ValueChanged += new System.EventHandler(this.nudForceIgnitionAngle_ValueChanged);
            // 
            // cbForceIgnition
            // 
            this.cbForceIgnition.AutoSize = true;
            this.cbForceIgnition.Location = new System.Drawing.Point(6, 226);
            this.cbForceIgnition.Name = "cbForceIgnition";
            this.cbForceIgnition.Size = new System.Drawing.Size(199, 28);
            this.cbForceIgnition.TabIndex = 10;
            this.cbForceIgnition.Text = "Force Ignition Angle";
            this.cbForceIgnition.UseVisualStyleBackColor = true;
            this.cbForceIgnition.CheckedChanged += new System.EventHandler(this.cbForceIgnition_CheckedChanged);
            // 
            // nudEngVol
            // 
            this.nudEngVol.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudEngVol.Location = new System.Drawing.Point(123, 196);
            this.nudEngVol.Maximum = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.nudEngVol.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudEngVol.Name = "nudEngVol";
            this.nudEngVol.Size = new System.Drawing.Size(96, 29);
            this.nudEngVol.TabIndex = 7;
            this.nudEngVol.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudEngVol.ValueChanged += new System.EventHandler(this.nudEngVol_ValueChanged);
            // 
            // cbForceIdle
            // 
            this.cbForceIdle.AutoSize = true;
            this.cbForceIdle.Location = new System.Drawing.Point(4, 164);
            this.cbForceIdle.Name = "cbForceIdle";
            this.cbForceIdle.Size = new System.Drawing.Size(114, 28);
            this.cbForceIdle.TabIndex = 6;
            this.cbForceIdle.Text = "Force Idle";
            this.cbForceIdle.UseVisualStyleBackColor = true;
            this.cbForceIdle.CheckedChanged += new System.EventHandler(this.cbForceIdle_CheckedChanged);
            // 
            // cbHallLearn
            // 
            this.cbHallLearn.AutoSize = true;
            this.cbHallLearn.Location = new System.Drawing.Point(4, 130);
            this.cbHallLearn.Name = "cbHallLearn";
            this.cbHallLearn.Size = new System.Drawing.Size(194, 28);
            this.cbHallLearn.TabIndex = 5;
            this.cbHallLearn.Text = "Hall Learning Mode";
            this.cbHallLearn.UseVisualStyleBackColor = true;
            this.cbHallLearn.CheckedChanged += new System.EventHandler(this.cbHallLearn_CheckedChanged);
            // 
            // cbHallIgnition
            // 
            this.cbHallIgnition.AutoSize = true;
            this.cbHallIgnition.Location = new System.Drawing.Point(6, 96);
            this.cbHallIgnition.Name = "cbHallIgnition";
            this.cbHallIgnition.Size = new System.Drawing.Size(216, 28);
            this.cbHallIgnition.TabIndex = 4;
            this.cbHallIgnition.Text = "Ignition by Hall Sensor";
            this.cbHallIgnition.UseVisualStyleBackColor = true;
            this.cbHallIgnition.CheckedChanged += new System.EventHandler(this.cbHallIgnition_CheckedChanged);
            // 
            // cbAutostartEnabled
            // 
            this.cbAutostartEnabled.AutoSize = true;
            this.cbAutostartEnabled.Enabled = false;
            this.cbAutostartEnabled.Location = new System.Drawing.Point(6, 62);
            this.cbAutostartEnabled.Name = "cbAutostartEnabled";
            this.cbAutostartEnabled.Size = new System.Drawing.Size(171, 28);
            this.cbAutostartEnabled.TabIndex = 3;
            this.cbAutostartEnabled.Text = "Autostart Feature";
            this.cbAutostartEnabled.UseVisualStyleBackColor = true;
            this.cbAutostartEnabled.CheckedChanged += new System.EventHandler(this.cbAutostartEnabled_CheckedChanged);
            // 
            // cbTempEnabled
            // 
            this.cbTempEnabled.AutoSize = true;
            this.cbTempEnabled.Location = new System.Drawing.Point(6, 28);
            this.cbTempEnabled.Name = "cbTempEnabled";
            this.cbTempEnabled.Size = new System.Drawing.Size(214, 28);
            this.cbTempEnabled.TabIndex = 2;
            this.cbTempEnabled.Text = "Temperature Enabled";
            this.cbTempEnabled.UseVisualStyleBackColor = true;
            this.cbTempEnabled.CheckedChanged += new System.EventHandler(this.cbTempEnabled_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.nudFuelForce);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.nudSwPos2);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.nudSwPos0);
            this.groupBox3.Controls.Add(this.cbFuelForce);
            this.groupBox3.Controls.Add(this.cbFuelExtSw);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.nudSwPos1);
            this.groupBox3.Location = new System.Drawing.Point(225, 219);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(213, 225);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Fuel Switch";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 192);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(90, 24);
            this.label15.TabIndex = 9;
            this.label15.Text = "Force №:";
            // 
            // nudFuelForce
            // 
            this.nudFuelForce.Location = new System.Drawing.Point(102, 190);
            this.nudFuelForce.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudFuelForce.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFuelForce.Name = "nudFuelForce";
            this.nudFuelForce.Size = new System.Drawing.Size(99, 29);
            this.nudFuelForce.TabIndex = 8;
            this.nudFuelForce.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFuelForce.ValueChanged += new System.EventHandler(this.nudFuelForce_ValueChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 129);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(62, 24);
            this.label14.TabIndex = 7;
            this.label14.Text = "Pos 2:";
            // 
            // nudSwPos2
            // 
            this.nudSwPos2.Location = new System.Drawing.Point(102, 127);
            this.nudSwPos2.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudSwPos2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSwPos2.Name = "nudSwPos2";
            this.nudSwPos2.Size = new System.Drawing.Size(99, 29);
            this.nudSwPos2.TabIndex = 6;
            this.nudSwPos2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSwPos2.ValueChanged += new System.EventHandler(this.nudSwPos2_ValueChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 94);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(62, 24);
            this.label13.TabIndex = 5;
            this.label13.Text = "Pos 0:";
            // 
            // nudSwPos0
            // 
            this.nudSwPos0.Location = new System.Drawing.Point(102, 92);
            this.nudSwPos0.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudSwPos0.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSwPos0.Name = "nudSwPos0";
            this.nudSwPos0.Size = new System.Drawing.Size(99, 29);
            this.nudSwPos0.TabIndex = 4;
            this.nudSwPos0.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSwPos0.ValueChanged += new System.EventHandler(this.nudSwPos0_ValueChanged);
            // 
            // cbFuelForce
            // 
            this.cbFuelForce.AutoSize = true;
            this.cbFuelForce.Location = new System.Drawing.Point(6, 162);
            this.cbFuelForce.Name = "cbFuelForce";
            this.cbFuelForce.Size = new System.Drawing.Size(111, 28);
            this.cbFuelForce.TabIndex = 3;
            this.cbFuelForce.Text = "Force Set";
            this.cbFuelForce.UseVisualStyleBackColor = true;
            this.cbFuelForce.CheckedChanged += new System.EventHandler(this.cbFuelForce_CheckedChanged);
            // 
            // cbFuelExtSw
            // 
            this.cbFuelExtSw.AutoSize = true;
            this.cbFuelExtSw.Location = new System.Drawing.Point(6, 28);
            this.cbFuelExtSw.Name = "cbFuelExtSw";
            this.cbFuelExtSw.Size = new System.Drawing.Size(158, 28);
            this.cbFuelExtSw.TabIndex = 2;
            this.cbFuelExtSw.Text = "External Switch";
            this.cbFuelExtSw.UseVisualStyleBackColor = true;
            this.cbFuelExtSw.CheckedChanged += new System.EventHandler(this.cbFuelExtSw_CheckedChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 59);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(62, 24);
            this.label12.TabIndex = 1;
            this.label12.Text = "Pos 1:";
            // 
            // nudSwPos1
            // 
            this.nudSwPos1.Location = new System.Drawing.Point(102, 57);
            this.nudSwPos1.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudSwPos1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSwPos1.Name = "nudSwPos1";
            this.nudSwPos1.Size = new System.Drawing.Size(99, 29);
            this.nudSwPos1.TabIndex = 0;
            this.nudSwPos1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSwPos1.ValueChanged += new System.EventHandler(this.nudSwPos1_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblEconRPM);
            this.groupBox2.Controls.Add(this.tbEconRPM);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.cbEconIgnitionBreak);
            this.groupBox2.Controls.Add(this.cbEconStrobe);
            this.groupBox2.Controls.Add(this.cbEconEnabled);
            this.groupBox2.Location = new System.Drawing.Point(225, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(213, 207);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Economizer";
            // 
            // lblEconRPM
            // 
            this.lblEconRPM.AutoSize = true;
            this.lblEconRPM.Location = new System.Drawing.Point(65, 130);
            this.lblEconRPM.Margin = new System.Windows.Forms.Padding(0);
            this.lblEconRPM.Name = "lblEconRPM";
            this.lblEconRPM.Size = new System.Drawing.Size(20, 24);
            this.lblEconRPM.TabIndex = 15;
            this.lblEconRPM.Text = "0";
            // 
            // tbEconRPM
            // 
            this.tbEconRPM.BackColor = System.Drawing.Color.White;
            this.tbEconRPM.LargeChange = 500;
            this.tbEconRPM.Location = new System.Drawing.Point(3, 157);
            this.tbEconRPM.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.tbEconRPM.Maximum = 6000;
            this.tbEconRPM.Minimum = 1500;
            this.tbEconRPM.Name = "tbEconRPM";
            this.tbEconRPM.Size = new System.Drawing.Size(206, 45);
            this.tbEconRPM.SmallChange = 100;
            this.tbEconRPM.TabIndex = 14;
            this.tbEconRPM.TickFrequency = 500;
            this.tbEconRPM.Value = 2000;
            this.tbEconRPM.Scroll += new System.EventHandler(this.tbEconRPM_Scroll);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 130);
            this.label11.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(56, 24);
            this.label11.TabIndex = 13;
            this.label11.Text = "RPM:";
            // 
            // cbEconIgnitionBreak
            // 
            this.cbEconIgnitionBreak.AutoSize = true;
            this.cbEconIgnitionBreak.Location = new System.Drawing.Point(6, 96);
            this.cbEconIgnitionBreak.Name = "cbEconIgnitionBreak";
            this.cbEconIgnitionBreak.Size = new System.Drawing.Size(195, 28);
            this.cbEconIgnitionBreak.TabIndex = 2;
            this.cbEconIgnitionBreak.Text = "Ignition break mode";
            this.cbEconIgnitionBreak.UseVisualStyleBackColor = true;
            this.cbEconIgnitionBreak.CheckedChanged += new System.EventHandler(this.cbEconIgnitionBreak_CheckedChanged);
            // 
            // cbEconStrobe
            // 
            this.cbEconStrobe.AutoSize = true;
            this.cbEconStrobe.Location = new System.Drawing.Point(6, 62);
            this.cbEconStrobe.Name = "cbEconStrobe";
            this.cbEconStrobe.Size = new System.Drawing.Size(143, 28);
            this.cbEconStrobe.TabIndex = 1;
            this.cbEconStrobe.Text = "Out as Strobe";
            this.cbEconStrobe.UseVisualStyleBackColor = true;
            this.cbEconStrobe.CheckedChanged += new System.EventHandler(this.cbEconStrobe_CheckedChanged);
            // 
            // cbEconEnabled
            // 
            this.cbEconEnabled.AutoSize = true;
            this.cbEconEnabled.Location = new System.Drawing.Point(6, 28);
            this.cbEconEnabled.Name = "cbEconEnabled";
            this.cbEconEnabled.Size = new System.Drawing.Size(100, 28);
            this.cbEconEnabled.TabIndex = 0;
            this.cbEconEnabled.Text = "Enabled";
            this.cbEconEnabled.UseVisualStyleBackColor = true;
            this.cbEconEnabled.CheckedChanged += new System.EventHandler(this.cbEconEnabled_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblCutoffAngle);
            this.groupBox1.Controls.Add(this.tbCutoffAngle);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.lblCutoffRPM);
            this.groupBox1.Controls.Add(this.tbCutoffRPM);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.rbCutoffMode7);
            this.groupBox1.Controls.Add(this.rbCutoffMode6);
            this.groupBox1.Controls.Add(this.rbCutoffMode5);
            this.groupBox1.Controls.Add(this.rbCutoffMode8);
            this.groupBox1.Controls.Add(this.rbCutoffMode4);
            this.groupBox1.Controls.Add(this.rbCutoffMode3);
            this.groupBox1.Controls.Add(this.rbCutoffMode2);
            this.groupBox1.Controls.Add(this.rbCutoffMode1);
            this.groupBox1.Controls.Add(this.cbCutoffEnabled);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(213, 438);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cutoff Setup";
            // 
            // lblCutoffAngle
            // 
            this.lblCutoffAngle.AutoSize = true;
            this.lblCutoffAngle.Location = new System.Drawing.Point(71, 356);
            this.lblCutoffAngle.Margin = new System.Windows.Forms.Padding(0);
            this.lblCutoffAngle.Name = "lblCutoffAngle";
            this.lblCutoffAngle.Size = new System.Drawing.Size(20, 24);
            this.lblCutoffAngle.TabIndex = 15;
            this.lblCutoffAngle.Text = "0";
            // 
            // tbCutoffAngle
            // 
            this.tbCutoffAngle.BackColor = System.Drawing.Color.White;
            this.tbCutoffAngle.LargeChange = 10;
            this.tbCutoffAngle.Location = new System.Drawing.Point(3, 383);
            this.tbCutoffAngle.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.tbCutoffAngle.Maximum = 60;
            this.tbCutoffAngle.Name = "tbCutoffAngle";
            this.tbCutoffAngle.Size = new System.Drawing.Size(206, 45);
            this.tbCutoffAngle.TabIndex = 14;
            this.tbCutoffAngle.TickFrequency = 5;
            this.tbCutoffAngle.Value = 10;
            this.tbCutoffAngle.Scroll += new System.EventHandler(this.tbCutoffAngle_Scroll);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 356);
            this.label9.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 24);
            this.label9.TabIndex = 13;
            this.label9.Text = "Angle:";
            // 
            // lblCutoffRPM
            // 
            this.lblCutoffRPM.AutoSize = true;
            this.lblCutoffRPM.Location = new System.Drawing.Point(62, 294);
            this.lblCutoffRPM.Margin = new System.Windows.Forms.Padding(0);
            this.lblCutoffRPM.Name = "lblCutoffRPM";
            this.lblCutoffRPM.Size = new System.Drawing.Size(20, 24);
            this.lblCutoffRPM.TabIndex = 12;
            this.lblCutoffRPM.Text = "0";
            // 
            // tbCutoffRPM
            // 
            this.tbCutoffRPM.BackColor = System.Drawing.Color.White;
            this.tbCutoffRPM.LargeChange = 500;
            this.tbCutoffRPM.Location = new System.Drawing.Point(3, 321);
            this.tbCutoffRPM.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.tbCutoffRPM.Maximum = 8000;
            this.tbCutoffRPM.Minimum = 2000;
            this.tbCutoffRPM.Name = "tbCutoffRPM";
            this.tbCutoffRPM.Size = new System.Drawing.Size(206, 45);
            this.tbCutoffRPM.SmallChange = 100;
            this.tbCutoffRPM.TabIndex = 11;
            this.tbCutoffRPM.TickFrequency = 500;
            this.tbCutoffRPM.Value = 5500;
            this.tbCutoffRPM.Scroll += new System.EventHandler(this.tbCutoffRPM_Scroll);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 294);
            this.label6.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 24);
            this.label6.TabIndex = 10;
            this.label6.Text = "RPM:";
            // 
            // rbCutoffMode7
            // 
            this.rbCutoffMode7.AutoSize = true;
            this.rbCutoffMode7.Location = new System.Drawing.Point(6, 230);
            this.rbCutoffMode7.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.rbCutoffMode7.Name = "rbCutoffMode7";
            this.rbCutoffMode7.Size = new System.Drawing.Size(134, 28);
            this.rbCutoffMode7.TabIndex = 8;
            this.rbCutoffMode7.TabStop = true;
            this.rbCutoffMode7.Text = "Mode 7 (1/3)";
            this.rbCutoffMode7.UseVisualStyleBackColor = true;
            this.rbCutoffMode7.CheckedChanged += new System.EventHandler(this.rbCutoffMode7_CheckedChanged);
            // 
            // rbCutoffMode6
            // 
            this.rbCutoffMode6.AutoSize = true;
            this.rbCutoffMode6.Location = new System.Drawing.Point(6, 202);
            this.rbCutoffMode6.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.rbCutoffMode6.Name = "rbCutoffMode6";
            this.rbCutoffMode6.Size = new System.Drawing.Size(134, 28);
            this.rbCutoffMode6.TabIndex = 7;
            this.rbCutoffMode6.TabStop = true;
            this.rbCutoffMode6.Text = "Mode 6 (1/5)";
            this.rbCutoffMode6.UseVisualStyleBackColor = true;
            this.rbCutoffMode6.CheckedChanged += new System.EventHandler(this.rbCutoffMode6_CheckedChanged);
            // 
            // rbCutoffMode5
            // 
            this.rbCutoffMode5.AutoSize = true;
            this.rbCutoffMode5.Location = new System.Drawing.Point(6, 174);
            this.rbCutoffMode5.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.rbCutoffMode5.Name = "rbCutoffMode5";
            this.rbCutoffMode5.Size = new System.Drawing.Size(135, 28);
            this.rbCutoffMode5.TabIndex = 6;
            this.rbCutoffMode5.TabStop = true;
            this.rbCutoffMode5.Text = "Mode 5 (4-8)";
            this.rbCutoffMode5.UseVisualStyleBackColor = true;
            this.rbCutoffMode5.CheckedChanged += new System.EventHandler(this.rbCutoffMode5_CheckedChanged);
            // 
            // rbCutoffMode8
            // 
            this.rbCutoffMode8.AutoSize = true;
            this.rbCutoffMode8.Location = new System.Drawing.Point(6, 258);
            this.rbCutoffMode8.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.rbCutoffMode8.Name = "rbCutoffMode8";
            this.rbCutoffMode8.Size = new System.Drawing.Size(134, 28);
            this.rbCutoffMode8.TabIndex = 5;
            this.rbCutoffMode8.TabStop = true;
            this.rbCutoffMode8.Text = "Mode 8 (2/3)";
            this.rbCutoffMode8.UseVisualStyleBackColor = true;
            this.rbCutoffMode8.CheckedChanged += new System.EventHandler(this.rbCutoffMode8_CheckedChanged);
            // 
            // rbCutoffMode4
            // 
            this.rbCutoffMode4.AutoSize = true;
            this.rbCutoffMode4.Location = new System.Drawing.Point(6, 146);
            this.rbCutoffMode4.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.rbCutoffMode4.Name = "rbCutoffMode4";
            this.rbCutoffMode4.Size = new System.Drawing.Size(145, 28);
            this.rbCutoffMode4.TabIndex = 4;
            this.rbCutoffMode4.TabStop = true;
            this.rbCutoffMode4.Text = "Mode 4 (4-16)";
            this.rbCutoffMode4.UseVisualStyleBackColor = true;
            this.rbCutoffMode4.CheckedChanged += new System.EventHandler(this.rbCutoffMode4_CheckedChanged);
            // 
            // rbCutoffMode3
            // 
            this.rbCutoffMode3.AutoSize = true;
            this.rbCutoffMode3.Location = new System.Drawing.Point(6, 118);
            this.rbCutoffMode3.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.rbCutoffMode3.Name = "rbCutoffMode3";
            this.rbCutoffMode3.Size = new System.Drawing.Size(145, 28);
            this.rbCutoffMode3.TabIndex = 3;
            this.rbCutoffMode3.TabStop = true;
            this.rbCutoffMode3.Text = "Mode 3 (4-24)";
            this.rbCutoffMode3.UseVisualStyleBackColor = true;
            this.rbCutoffMode3.CheckedChanged += new System.EventHandler(this.rbCutoffMode3_CheckedChanged);
            // 
            // rbCutoffMode2
            // 
            this.rbCutoffMode2.AutoSize = true;
            this.rbCutoffMode2.Location = new System.Drawing.Point(6, 90);
            this.rbCutoffMode2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.rbCutoffMode2.Name = "rbCutoffMode2";
            this.rbCutoffMode2.Size = new System.Drawing.Size(145, 28);
            this.rbCutoffMode2.TabIndex = 2;
            this.rbCutoffMode2.TabStop = true;
            this.rbCutoffMode2.Text = "Mode 2 (4-36)";
            this.rbCutoffMode2.UseVisualStyleBackColor = true;
            this.rbCutoffMode2.CheckedChanged += new System.EventHandler(this.rbCutoffMode2_CheckedChanged);
            // 
            // rbCutoffMode1
            // 
            this.rbCutoffMode1.AutoSize = true;
            this.rbCutoffMode1.Location = new System.Drawing.Point(6, 62);
            this.rbCutoffMode1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.rbCutoffMode1.Name = "rbCutoffMode1";
            this.rbCutoffMode1.Size = new System.Drawing.Size(203, 28);
            this.rbCutoffMode1.TabIndex = 1;
            this.rbCutoffMode1.TabStop = true;
            this.rbCutoffMode1.Text = "Mode 1 (Hard Break)";
            this.rbCutoffMode1.UseVisualStyleBackColor = true;
            this.rbCutoffMode1.CheckedChanged += new System.EventHandler(this.rbCutoffMode1_CheckedChanged);
            // 
            // cbCutoffEnabled
            // 
            this.cbCutoffEnabled.AutoSize = true;
            this.cbCutoffEnabled.Location = new System.Drawing.Point(6, 28);
            this.cbCutoffEnabled.Name = "cbCutoffEnabled";
            this.cbCutoffEnabled.Size = new System.Drawing.Size(100, 28);
            this.cbCutoffEnabled.TabIndex = 0;
            this.cbCutoffEnabled.Text = "Enabled";
            this.cbCutoffEnabled.UseVisualStyleBackColor = true;
            this.cbCutoffEnabled.CheckedChanged += new System.EventHandler(this.cbCutoffEnabled_CheckedChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.AutoScroll = true;
            this.tabPage3.BackColor = System.Drawing.Color.White;
            this.tabPage3.Controls.Add(this.tabControl111);
            this.tabPage3.Location = new System.Drawing.Point(4, 44);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1099, 605);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Ignition Setup";
            // 
            // tabControl111
            // 
            this.tabControl111.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabControl111.Controls.Add(this.tabPage12);
            this.tabControl111.Controls.Add(this.tabPage8);
            this.tabControl111.Controls.Add(this.tabPage4);
            this.tabControl111.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl111.Location = new System.Drawing.Point(3, 3);
            this.tabControl111.Margin = new System.Windows.Forms.Padding(1);
            this.tabControl111.Name = "tabControl111";
            this.tabControl111.Padding = new System.Drawing.Point(12, 5);
            this.tabControl111.SelectedIndex = 0;
            this.tabControl111.Size = new System.Drawing.Size(1093, 599);
            this.tabControl111.TabIndex = 1;
            // 
            // tabPage12
            // 
            this.tabPage12.BackColor = System.Drawing.Color.White;
            this.tabPage12.Controls.Add(this.btnTableImport);
            this.tabPage12.Controls.Add(this.btnTableExport);
            this.tabPage12.Controls.Add(this.groupBox5);
            this.tabPage12.Controls.Add(this.nudToolsCurTable);
            this.tabPage12.Controls.Add(this.label23);
            this.tabPage12.Location = new System.Drawing.Point(4, 40);
            this.tabPage12.Name = "tabPage12";
            this.tabPage12.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage12.Size = new System.Drawing.Size(1085, 555);
            this.tabPage12.TabIndex = 5;
            this.tabPage12.Text = "Tools";
            // 
            // btnTableImport
            // 
            this.btnTableImport.Location = new System.Drawing.Point(268, 10);
            this.btnTableImport.Name = "btnTableImport";
            this.btnTableImport.Size = new System.Drawing.Size(100, 31);
            this.btnTableImport.TabIndex = 5;
            this.btnTableImport.Text = "Import";
            this.btnTableImport.UseVisualStyleBackColor = true;
            this.btnTableImport.Click += new System.EventHandler(this.btnTableImport_Click);
            // 
            // btnTableExport
            // 
            this.btnTableExport.Location = new System.Drawing.Point(374, 10);
            this.btnTableExport.Name = "btnTableExport";
            this.btnTableExport.Size = new System.Drawing.Size(100, 31);
            this.btnTableExport.TabIndex = 4;
            this.btnTableExport.Text = "Export";
            this.btnTableExport.UseVisualStyleBackColor = true;
            this.btnTableExport.Click += new System.EventHandler(this.btnTableExport_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnToolsCopy);
            this.groupBox5.Controls.Add(this.nudToolsCopyTo);
            this.groupBox5.Controls.Add(this.label43);
            this.groupBox5.Controls.Add(this.nudToolsCopyFrom);
            this.groupBox5.Controls.Add(this.label42);
            this.groupBox5.Location = new System.Drawing.Point(10, 72);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(274, 142);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Table Copy";
            // 
            // btnToolsCopy
            // 
            this.btnToolsCopy.Location = new System.Drawing.Point(10, 96);
            this.btnToolsCopy.Name = "btnToolsCopy";
            this.btnToolsCopy.Size = new System.Drawing.Size(242, 32);
            this.btnToolsCopy.TabIndex = 7;
            this.btnToolsCopy.Text = "Copy";
            this.btnToolsCopy.UseVisualStyleBackColor = true;
            this.btnToolsCopy.Click += new System.EventHandler(this.btnToolsCopy_Click);
            // 
            // nudToolsCopyTo
            // 
            this.nudToolsCopyTo.Location = new System.Drawing.Point(72, 61);
            this.nudToolsCopyTo.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudToolsCopyTo.Name = "nudToolsCopyTo";
            this.nudToolsCopyTo.Size = new System.Drawing.Size(180, 29);
            this.nudToolsCopyTo.TabIndex = 6;
            this.nudToolsCopyTo.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudToolsCopyTo.ValueChanged += new System.EventHandler(this.nudToolsCopyTo_ValueChanged);
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(6, 63);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(38, 24);
            this.label43.TabIndex = 5;
            this.label43.Text = "To:";
            // 
            // nudToolsCopyFrom
            // 
            this.nudToolsCopyFrom.Location = new System.Drawing.Point(72, 26);
            this.nudToolsCopyFrom.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudToolsCopyFrom.Name = "nudToolsCopyFrom";
            this.nudToolsCopyFrom.Size = new System.Drawing.Size(180, 29);
            this.nudToolsCopyFrom.TabIndex = 4;
            this.nudToolsCopyFrom.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudToolsCopyFrom.ValueChanged += new System.EventHandler(this.nudToolsCopyFrom_ValueChanged);
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(6, 28);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(60, 24);
            this.label42.TabIndex = 3;
            this.label42.Text = "From:";
            // 
            // nudToolsCurTable
            // 
            this.nudToolsCurTable.Location = new System.Drawing.Point(142, 11);
            this.nudToolsCurTable.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudToolsCurTable.Name = "nudToolsCurTable";
            this.nudToolsCurTable.Size = new System.Drawing.Size(120, 29);
            this.nudToolsCurTable.TabIndex = 2;
            this.nudToolsCurTable.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudToolsCurTable.ValueChanged += new System.EventHandler(this.nudToolsCurTable_ValueChanged);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(6, 13);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(130, 24);
            this.label23.TabIndex = 1;
            this.label23.Text = "Current Table:";
            // 
            // tabPage8
            // 
            this.tabPage8.BackColor = System.Drawing.Color.White;
            this.tabPage8.Controls.Add(this.label55);
            this.tabPage8.Controls.Add(this.nudParamsFuelVolume);
            this.tabPage8.Controls.Add(this.label54);
            this.tabPage8.Controls.Add(this.nudParamsFuelRate);
            this.tabPage8.Controls.Add(this.label51);
            this.tabPage8.Controls.Add(this.nudParamsCntTemps);
            this.tabPage8.Controls.Add(this.label52);
            this.tabPage8.Controls.Add(this.nudParamsCntIdles);
            this.tabPage8.Controls.Add(this.label50);
            this.tabPage8.Controls.Add(this.nudParamsValveTimeout);
            this.tabPage8.Controls.Add(this.label49);
            this.tabPage8.Controls.Add(this.nudParamsCntRPMs);
            this.tabPage8.Controls.Add(this.label48);
            this.tabPage8.Controls.Add(this.nudParamsCntPress);
            this.tabPage8.Controls.Add(this.label47);
            this.tabPage8.Controls.Add(this.nudParamsInitial);
            this.tabPage8.Controls.Add(this.label46);
            this.tabPage8.Controls.Add(this.nudParamsOctane);
            this.tabPage8.Controls.Add(this.rbParamsValve2);
            this.tabPage8.Controls.Add(this.rbParamsValve1);
            this.tabPage8.Controls.Add(this.label45);
            this.tabPage8.Controls.Add(this.rbParamsValve0);
            this.tabPage8.Controls.Add(this.label44);
            this.tabPage8.Controls.Add(this.tbParamsName);
            this.tabPage8.Location = new System.Drawing.Point(4, 40);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(1085, 555);
            this.tabPage8.TabIndex = 4;
            this.tabPage8.Text = "Parameters";
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(6, 395);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(87, 24);
            this.label55.TabIndex = 23;
            this.label55.Text = "Fuel kg/l:";
            // 
            // nudParamsFuelVolume
            // 
            this.nudParamsFuelVolume.DecimalPlaces = 1;
            this.nudParamsFuelVolume.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudParamsFuelVolume.Location = new System.Drawing.Point(171, 393);
            this.nudParamsFuelVolume.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudParamsFuelVolume.Name = "nudParamsFuelVolume";
            this.nudParamsFuelVolume.Size = new System.Drawing.Size(160, 29);
            this.nudParamsFuelVolume.TabIndex = 22;
            this.nudParamsFuelVolume.ValueChanged += new System.EventHandler(this.nudParamsFuelVolume_ValueChanged);
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Location = new System.Drawing.Point(6, 360);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(133, 24);
            this.label54.TabIndex = 21;
            this.label54.Text = "Stoichiometric:";
            // 
            // nudParamsFuelRate
            // 
            this.nudParamsFuelRate.DecimalPlaces = 1;
            this.nudParamsFuelRate.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudParamsFuelRate.Location = new System.Drawing.Point(171, 358);
            this.nudParamsFuelRate.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.nudParamsFuelRate.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudParamsFuelRate.Name = "nudParamsFuelRate";
            this.nudParamsFuelRate.Size = new System.Drawing.Size(160, 29);
            this.nudParamsFuelRate.TabIndex = 20;
            this.nudParamsFuelRate.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudParamsFuelRate.ValueChanged += new System.EventHandler(this.nudParamsFuelRate_ValueChanged);
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(6, 309);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(134, 24);
            this.label51.TabIndex = 19;
            this.label51.Text = "Temps. Count:";
            // 
            // nudParamsCntTemps
            // 
            this.nudParamsCntTemps.Location = new System.Drawing.Point(171, 307);
            this.nudParamsCntTemps.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.nudParamsCntTemps.Name = "nudParamsCntTemps";
            this.nudParamsCntTemps.Size = new System.Drawing.Size(160, 29);
            this.nudParamsCntTemps.TabIndex = 18;
            this.nudParamsCntTemps.ValueChanged += new System.EventHandler(this.nudParamsCntTemps_ValueChanged);
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(6, 274);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(109, 24);
            this.label52.TabIndex = 17;
            this.label52.Text = "Idles Count:";
            // 
            // nudParamsCntIdles
            // 
            this.nudParamsCntIdles.Location = new System.Drawing.Point(171, 272);
            this.nudParamsCntIdles.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.nudParamsCntIdles.Name = "nudParamsCntIdles";
            this.nudParamsCntIdles.Size = new System.Drawing.Size(160, 29);
            this.nudParamsCntIdles.TabIndex = 16;
            this.nudParamsCntIdles.ValueChanged += new System.EventHandler(this.nudParamsCntIdles_ValueChanged);
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(6, 83);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(136, 24);
            this.label50.TabIndex = 15;
            this.label50.Text = "Valve Timeout:";
            // 
            // nudParamsValveTimeout
            // 
            this.nudParamsValveTimeout.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudParamsValveTimeout.Location = new System.Drawing.Point(171, 81);
            this.nudParamsValveTimeout.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudParamsValveTimeout.Name = "nudParamsValveTimeout";
            this.nudParamsValveTimeout.Size = new System.Drawing.Size(160, 29);
            this.nudParamsValveTimeout.TabIndex = 14;
            this.nudParamsValveTimeout.ValueChanged += new System.EventHandler(this.nudParamsValveTimeout_ValueChanged);
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(6, 239);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(132, 24);
            this.label49.TabIndex = 13;
            this.label49.Text = "Rotates Count:";
            // 
            // nudParamsCntRPMs
            // 
            this.nudParamsCntRPMs.Location = new System.Drawing.Point(171, 237);
            this.nudParamsCntRPMs.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.nudParamsCntRPMs.Name = "nudParamsCntRPMs";
            this.nudParamsCntRPMs.Size = new System.Drawing.Size(160, 29);
            this.nudParamsCntRPMs.TabIndex = 12;
            this.nudParamsCntRPMs.ValueChanged += new System.EventHandler(this.nudParamsCntRPMs_ValueChanged);
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(6, 204);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(154, 24);
            this.label48.TabIndex = 11;
            this.label48.Text = "Pressures Count:";
            // 
            // nudParamsCntPress
            // 
            this.nudParamsCntPress.Location = new System.Drawing.Point(171, 202);
            this.nudParamsCntPress.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.nudParamsCntPress.Name = "nudParamsCntPress";
            this.nudParamsCntPress.Size = new System.Drawing.Size(160, 29);
            this.nudParamsCntPress.TabIndex = 10;
            this.nudParamsCntPress.ValueChanged += new System.EventHandler(this.nudParamsCntPress_ValueChanged);
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(6, 153);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(121, 24);
            this.label47.TabIndex = 9;
            this.label47.Text = "Initial Ignition:";
            // 
            // nudParamsInitial
            // 
            this.nudParamsInitial.DecimalPlaces = 1;
            this.nudParamsInitial.Location = new System.Drawing.Point(171, 151);
            this.nudParamsInitial.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nudParamsInitial.Minimum = new decimal(new int[] {
            15,
            0,
            0,
            -2147483648});
            this.nudParamsInitial.Name = "nudParamsInitial";
            this.nudParamsInitial.Size = new System.Drawing.Size(160, 29);
            this.nudParamsInitial.TabIndex = 8;
            this.nudParamsInitial.ValueChanged += new System.EventHandler(this.nudParamsInitial_ValueChanged);
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(6, 118);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(159, 24);
            this.label46.TabIndex = 7;
            this.label46.Text = "Octane Corrector:";
            // 
            // nudParamsOctane
            // 
            this.nudParamsOctane.DecimalPlaces = 1;
            this.nudParamsOctane.Location = new System.Drawing.Point(171, 116);
            this.nudParamsOctane.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.nudParamsOctane.Minimum = new decimal(new int[] {
            15,
            0,
            0,
            -2147483648});
            this.nudParamsOctane.Name = "nudParamsOctane";
            this.nudParamsOctane.Size = new System.Drawing.Size(160, 29);
            this.nudParamsOctane.TabIndex = 6;
            this.nudParamsOctane.ValueChanged += new System.EventHandler(this.nudParamsOctane_ValueChanged);
            // 
            // rbParamsValve2
            // 
            this.rbParamsValve2.AutoSize = true;
            this.rbParamsValve2.Location = new System.Drawing.Point(231, 47);
            this.rbParamsValve2.Name = "rbParamsValve2";
            this.rbParamsValve2.Size = new System.Drawing.Size(100, 28);
            this.rbParamsValve2.TabIndex = 5;
            this.rbParamsValve2.TabStop = true;
            this.rbParamsValve2.Text = "Propane";
            this.rbParamsValve2.UseVisualStyleBackColor = true;
            this.rbParamsValve2.CheckedChanged += new System.EventHandler(this.rbParamsValve2_CheckedChanged);
            // 
            // rbParamsValve1
            // 
            this.rbParamsValve1.AutoSize = true;
            this.rbParamsValve1.Location = new System.Drawing.Point(149, 47);
            this.rbParamsValve1.Name = "rbParamsValve1";
            this.rbParamsValve1.Size = new System.Drawing.Size(76, 28);
            this.rbParamsValve1.TabIndex = 4;
            this.rbParamsValve1.TabStop = true;
            this.rbParamsValve1.Text = "Petrol";
            this.rbParamsValve1.UseVisualStyleBackColor = true;
            this.rbParamsValve1.CheckedChanged += new System.EventHandler(this.rbParamsValve1_CheckedChanged);
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(6, 49);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(62, 24);
            this.label45.TabIndex = 3;
            this.label45.Text = "Valve:";
            // 
            // rbParamsValve0
            // 
            this.rbParamsValve0.AutoSize = true;
            this.rbParamsValve0.Location = new System.Drawing.Point(74, 47);
            this.rbParamsValve0.Name = "rbParamsValve0";
            this.rbParamsValve0.Size = new System.Drawing.Size(75, 28);
            this.rbParamsValve0.TabIndex = 2;
            this.rbParamsValve0.TabStop = true;
            this.rbParamsValve0.Text = "None";
            this.rbParamsValve0.UseVisualStyleBackColor = true;
            this.rbParamsValve0.CheckedChanged += new System.EventHandler(this.rbParamsValve0_CheckedChanged);
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(6, 13);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(125, 24);
            this.label44.TabIndex = 1;
            this.label44.Text = "Config Name:";
            // 
            // tbParamsName
            // 
            this.tbParamsName.Location = new System.Drawing.Point(156, 10);
            this.tbParamsName.MaxLength = 10;
            this.tbParamsName.Name = "tbParamsName";
            this.tbParamsName.Size = new System.Drawing.Size(175, 29);
            this.tbParamsName.TabIndex = 0;
            this.tbParamsName.TextChanged += new System.EventHandler(this.tbParamsName_TextChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.White;
            this.tabPage4.Controls.Add(this.tabControl3);
            this.tabPage4.Location = new System.Drawing.Point(4, 40);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1085, 555);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Main Setup";
            // 
            // tabControl3
            // 
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(0, 0);
            this.tabControl3.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.Padding = new System.Drawing.Point(12, 4);
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(1085, 555);
            this.tabControl3.TabIndex = 1;
            // 
            // tabPage18
            // 
            this.tabPage18.BackColor = System.Drawing.Color.White;
            this.tabPage18.Controls.Add(this.tableLayoutPanel13);
            this.tabPage18.Location = new System.Drawing.Point(4, 44);
            this.tabPage18.Name = "tabPage18";
            this.tabPage18.Size = new System.Drawing.Size(1099, 605);
            this.tabPage18.TabIndex = 3;
            this.tabPage18.Text = "Drag Measure";
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.ColumnCount = 2;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel13.Controls.Add(this.panel10, 0, 0);
            this.tableLayoutPanel13.Controls.Add(this.tabControl2, 1, 0);
            this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel13.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel13.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 1;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 605F));
            this.tableLayoutPanel13.Size = new System.Drawing.Size(1099, 605);
            this.tableLayoutPanel13.TabIndex = 6;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.label63);
            this.panel10.Controls.Add(this.tbDragName);
            this.panel10.Controls.Add(this.label65);
            this.panel10.Controls.Add(this.nudDragTableSplit);
            this.panel10.Controls.Add(this.lblDragSpeed);
            this.panel10.Controls.Add(this.label64);
            this.panel10.Controls.Add(this.lblDragTime);
            this.panel10.Controls.Add(this.label62);
            this.panel10.Controls.Add(this.btnDragClear);
            this.panel10.Controls.Add(this.btnDragStop);
            this.panel10.Controls.Add(this.btnDragStart);
            this.panel10.Controls.Add(this.lblDragStatus);
            this.panel10.Controls.Add(this.label59);
            this.panel10.Controls.Add(this.label57);
            this.panel10.Controls.Add(this.nudDragSpeedTo);
            this.panel10.Controls.Add(this.label58);
            this.panel10.Controls.Add(this.nudDragSpeedFrom);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(0, 0);
            this.panel10.Margin = new System.Windows.Forms.Padding(0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(200, 605);
            this.panel10.TabIndex = 6;
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.Location = new System.Drawing.Point(3, 343);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(66, 24);
            this.label63.TabIndex = 24;
            this.label63.Text = "Name:";
            // 
            // tbDragName
            // 
            this.tbDragName.Location = new System.Drawing.Point(3, 370);
            this.tbDragName.Name = "tbDragName";
            this.tbDragName.Size = new System.Drawing.Size(191, 29);
            this.tbDragName.TabIndex = 23;
            this.tbDragName.Text = "Drag Run";
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.Location = new System.Drawing.Point(3, 302);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(103, 24);
            this.label65.TabIndex = 20;
            this.label65.Text = "Table Split:";
            // 
            // nudDragTableSplit
            // 
            this.nudDragTableSplit.Location = new System.Drawing.Point(120, 300);
            this.nudDragTableSplit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDragTableSplit.Name = "nudDragTableSplit";
            this.nudDragTableSplit.Size = new System.Drawing.Size(75, 29);
            this.nudDragTableSplit.TabIndex = 19;
            this.nudDragTableSplit.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudDragTableSplit.ValueChanged += new System.EventHandler(this.nudDragTableSplit_ValueChanged);
            // 
            // lblDragSpeed
            // 
            this.lblDragSpeed.AutoSize = true;
            this.lblDragSpeed.Location = new System.Drawing.Point(77, 138);
            this.lblDragSpeed.Name = "lblDragSpeed";
            this.lblDragSpeed.Size = new System.Drawing.Size(66, 24);
            this.lblDragSpeed.TabIndex = 18;
            this.lblDragSpeed.Text = "0 km/h";
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.Location = new System.Drawing.Point(6, 138);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(71, 24);
            this.label64.TabIndex = 17;
            this.label64.Text = "Speed:";
            // 
            // lblDragTime
            // 
            this.lblDragTime.AutoSize = true;
            this.lblDragTime.Location = new System.Drawing.Point(77, 114);
            this.lblDragTime.Name = "lblDragTime";
            this.lblDragTime.Size = new System.Drawing.Size(54, 24);
            this.lblDragTime.TabIndex = 16;
            this.lblDragTime.Text = "0.00s";
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Location = new System.Drawing.Point(6, 114);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(58, 24);
            this.label62.TabIndex = 15;
            this.label62.Text = "Time:";
            // 
            // btnDragClear
            // 
            this.btnDragClear.Location = new System.Drawing.Point(6, 247);
            this.btnDragClear.Name = "btnDragClear";
            this.btnDragClear.Size = new System.Drawing.Size(191, 35);
            this.btnDragClear.TabIndex = 14;
            this.btnDragClear.Text = "Clear";
            this.btnDragClear.UseVisualStyleBackColor = true;
            this.btnDragClear.Click += new System.EventHandler(this.btnDragClear_Click);
            // 
            // btnDragStop
            // 
            this.btnDragStop.Enabled = false;
            this.btnDragStop.Location = new System.Drawing.Point(6, 206);
            this.btnDragStop.Name = "btnDragStop";
            this.btnDragStop.Size = new System.Drawing.Size(191, 35);
            this.btnDragStop.TabIndex = 13;
            this.btnDragStop.Text = "Stop";
            this.btnDragStop.UseVisualStyleBackColor = true;
            this.btnDragStop.Click += new System.EventHandler(this.btnDragStop_Click);
            // 
            // btnDragStart
            // 
            this.btnDragStart.Location = new System.Drawing.Point(6, 165);
            this.btnDragStart.Name = "btnDragStart";
            this.btnDragStart.Size = new System.Drawing.Size(191, 35);
            this.btnDragStart.TabIndex = 12;
            this.btnDragStart.Text = "Start";
            this.btnDragStart.UseVisualStyleBackColor = true;
            this.btnDragStart.Click += new System.EventHandler(this.btnDragStart_Click);
            // 
            // lblDragStatus
            // 
            this.lblDragStatus.AutoSize = true;
            this.lblDragStatus.Location = new System.Drawing.Point(77, 90);
            this.lblDragStatus.Name = "lblDragStatus";
            this.lblDragStatus.Size = new System.Drawing.Size(64, 24);
            this.lblDragStatus.TabIndex = 11;
            this.lblDragStatus.Text = "Ready";
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Location = new System.Drawing.Point(6, 90);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(65, 24);
            this.label59.TabIndex = 10;
            this.label59.Text = "Status:";
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(5, 53);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(38, 24);
            this.label57.TabIndex = 9;
            this.label57.Text = "To:";
            // 
            // nudDragSpeedTo
            // 
            this.nudDragSpeedTo.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudDragSpeedTo.Location = new System.Drawing.Point(83, 51);
            this.nudDragSpeedTo.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudDragSpeedTo.Name = "nudDragSpeedTo";
            this.nudDragSpeedTo.Size = new System.Drawing.Size(114, 29);
            this.nudDragSpeedTo.TabIndex = 8;
            this.nudDragSpeedTo.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Location = new System.Drawing.Point(5, 18);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(60, 24);
            this.label58.TabIndex = 7;
            this.label58.Text = "From:";
            // 
            // nudDragSpeedFrom
            // 
            this.nudDragSpeedFrom.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudDragSpeedFrom.Location = new System.Drawing.Point(83, 16);
            this.nudDragSpeedFrom.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudDragSpeedFrom.Name = "nudDragSpeedFrom";
            this.nudDragSpeedFrom.Size = new System.Drawing.Size(114, 29);
            this.nudDragSpeedFrom.TabIndex = 6;
            // 
            // tabControl2
            // 
            this.tabControl2.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabControl2.Controls.Add(this.tabPage19);
            this.tabControl2.Controls.Add(this.tabPage21);
            this.tabControl2.Controls.Add(this.tabPage20);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(200, 0);
            this.tabControl2.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(899, 605);
            this.tabControl2.TabIndex = 7;
            // 
            // tabPage19
            // 
            this.tabPage19.BackColor = System.Drawing.Color.White;
            this.tabPage19.Controls.Add(this.chartDragTime);
            this.tabPage19.Location = new System.Drawing.Point(4, 36);
            this.tabPage19.Name = "tabPage19";
            this.tabPage19.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage19.Size = new System.Drawing.Size(891, 565);
            this.tabPage19.TabIndex = 0;
            this.tabPage19.Text = "Speed vs. Time";
            // 
            // chartDragTime
            // 
            chartArea5.Name = "ChartArea1";
            chartArea5.Position.Auto = false;
            chartArea5.Position.Height = 100F;
            chartArea5.Position.Width = 88F;
            this.chartDragTime.ChartAreas.Add(chartArea5);
            this.chartDragTime.Dock = System.Windows.Forms.DockStyle.Fill;
            legend5.Name = "Legend1";
            this.chartDragTime.Legends.Add(legend5);
            this.chartDragTime.Location = new System.Drawing.Point(3, 3);
            this.chartDragTime.Name = "chartDragTime";
            series5.BorderWidth = 3;
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Color = System.Drawing.Color.Brown;
            series5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            series5.IsValueShownAsLabel = true;
            series5.LabelBorderWidth = 0;
            series5.Legend = "Legend1";
            series5.MarkerColor = System.Drawing.Color.Black;
            series5.MarkerSize = 8;
            series5.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series5.Name = "Series1";
            series5.SmartLabelStyle.AllowOutsidePlotArea = System.Windows.Forms.DataVisualization.Charting.LabelOutsidePlotAreaStyle.No;
            series5.SmartLabelStyle.MaxMovingDistance = 100D;
            series5.SmartLabelStyle.MinMovingDistance = 10D;
            series5.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series5.YValuesPerPoint = 2;
            series5.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Single;
            this.chartDragTime.Series.Add(series5);
            this.chartDragTime.Size = new System.Drawing.Size(885, 559);
            this.chartDragTime.TabIndex = 7;
            this.chartDragTime.Text = " ";
            // 
            // tabPage21
            // 
            this.tabPage21.BackColor = System.Drawing.Color.White;
            this.tabPage21.Controls.Add(this.chartDragAccel);
            this.tabPage21.Location = new System.Drawing.Point(4, 36);
            this.tabPage21.Name = "tabPage21";
            this.tabPage21.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage21.Size = new System.Drawing.Size(891, 565);
            this.tabPage21.TabIndex = 2;
            this.tabPage21.Text = "Accel. vs. Time";
            // 
            // chartDragAccel
            // 
            chartArea6.Name = "ChartArea1";
            chartArea6.Position.Auto = false;
            chartArea6.Position.Height = 100F;
            chartArea6.Position.Width = 88F;
            this.chartDragAccel.ChartAreas.Add(chartArea6);
            this.chartDragAccel.Dock = System.Windows.Forms.DockStyle.Fill;
            legend6.Name = "Legend1";
            this.chartDragAccel.Legends.Add(legend6);
            this.chartDragAccel.Location = new System.Drawing.Point(3, 3);
            this.chartDragAccel.Name = "chartDragAccel";
            series6.BorderWidth = 3;
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series6.Color = System.Drawing.Color.Brown;
            series6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            series6.IsValueShownAsLabel = true;
            series6.LabelBorderWidth = 0;
            series6.Legend = "Legend1";
            series6.MarkerColor = System.Drawing.Color.Black;
            series6.MarkerSize = 8;
            series6.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series6.Name = "Series1";
            series6.SmartLabelStyle.AllowOutsidePlotArea = System.Windows.Forms.DataVisualization.Charting.LabelOutsidePlotAreaStyle.No;
            series6.SmartLabelStyle.MaxMovingDistance = 100D;
            series6.SmartLabelStyle.MinMovingDistance = 10D;
            series6.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series6.YValuesPerPoint = 2;
            series6.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Single;
            this.chartDragAccel.Series.Add(series6);
            this.chartDragAccel.Size = new System.Drawing.Size(885, 559);
            this.chartDragAccel.TabIndex = 6;
            this.chartDragAccel.Text = " ";
            // 
            // tabPage20
            // 
            this.tabPage20.BackColor = System.Drawing.Color.White;
            this.tabPage20.Controls.Add(this.tableLayoutPanel14);
            this.tabPage20.Location = new System.Drawing.Point(4, 36);
            this.tabPage20.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage20.Name = "tabPage20";
            this.tabPage20.Size = new System.Drawing.Size(891, 565);
            this.tabPage20.TabIndex = 1;
            this.tabPage20.Text = "Table View";
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.ColumnCount = 1;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel14.Controls.Add(this.lvDragTable, 0, 0);
            this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel14.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel14.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RowCount = 1;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(891, 565);
            this.tableLayoutPanel14.TabIndex = 1;
            // 
            // lvDragTable
            // 
            this.lvDragTable.AutoArrange = false;
            this.lvDragTable.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvDragTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvDragTable.FullRowSelect = true;
            this.lvDragTable.GridLines = true;
            this.lvDragTable.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvDragTable.HideSelection = false;
            this.lvDragTable.LabelWrap = false;
            this.lvDragTable.Location = new System.Drawing.Point(0, 0);
            this.lvDragTable.Margin = new System.Windows.Forms.Padding(0);
            this.lvDragTable.MultiSelect = false;
            this.lvDragTable.Name = "lvDragTable";
            this.lvDragTable.Size = new System.Drawing.Size(891, 565);
            this.lvDragTable.TabIndex = 0;
            this.lvDragTable.UseCompatibleStateImageBehavior = false;
            this.lvDragTable.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "№";
            this.columnHeader1.Width = 50;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Time";
            this.columnHeader2.Width = 80;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1111, 711);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 691);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1111, 20);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripProgressBar1.MarqueeAnimationSpeed = 20;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(200, 14);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(178, 15);
            this.toolStripStatusLabel1.Text = "Welcome to Engine Control Unit";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 5;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel4.Controls.Add(this.button3, 4, 0);
            this.tableLayoutPanel4.Controls.Add(this.button2, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.button1, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.cbLive, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1111, 34);
            this.tableLayoutPanel4.TabIndex = 3;
            // 
            // button3
            // 
            this.button3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button3.Location = new System.Drawing.Point(961, 0);
            this.button3.Margin = new System.Windows.Forms.Padding(0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(150, 34);
            this.button3.TabIndex = 5;
            this.button3.Text = "Redownload";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.Location = new System.Drawing.Point(661, 0);
            this.button2.Margin = new System.Windows.Forms.Padding(0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(150, 34);
            this.button2.TabIndex = 3;
            this.button2.Text = "Save Flash";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(10, 3, 0, 0);
            this.label1.Size = new System.Drawing.Size(495, 34);
            this.label1.TabIndex = 1;
            this.label1.Text = "ENGINE CONTROL UNIT";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Location = new System.Drawing.Point(811, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(150, 34);
            this.button1.TabIndex = 2;
            this.button1.Text = "Restore Flash";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbLive
            // 
            this.cbLive.AutoSize = true;
            this.cbLive.Checked = true;
            this.cbLive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbLive.Location = new System.Drawing.Point(511, 3);
            this.cbLive.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.cbLive.Name = "cbLive";
            this.cbLive.Size = new System.Drawing.Size(147, 28);
            this.cbLive.TabIndex = 4;
            this.cbLive.Text = "Live Upload";
            this.cbLive.UseVisualStyleBackColor = true;
            this.cbLive.CheckedChanged += new System.EventHandler(this.cbLive_CheckedChanged);
            // 
            // dlgExport
            // 
            this.dlgExport.DefaultExt = "xml";
            this.dlgExport.Filter = "XML Ignition Config|*.xml";
            this.dlgExport.RestoreDirectory = true;
            // 
            // dlgImport
            // 
            this.dlgImport.DefaultExt = "xml";
            this.dlgImport.Filter = "XML Ignition Config|*.xml";
            this.dlgImport.RestoreDirectory = true;
            // 
            // tmr1sec
            // 
            this.tmr1sec.Enabled = true;
            this.tmr1sec.Interval = 1000;
            this.tmr1sec.Tick += new System.EventHandler(this.tmr1sec_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1111, 711);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MinimumSize = new System.Drawing.Size(854, 480);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Engine Control Unit - Manager";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudForceIgnitionAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEngVol)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFuelForce)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSwPos2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSwPos0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSwPos1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbEconRPM)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbCutoffAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbCutoffRPM)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabControl111.ResumeLayout(false);
            this.tabPage12.ResumeLayout(false);
            this.tabPage12.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudToolsCopyTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudToolsCopyFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudToolsCurTable)).EndInit();
            this.tabPage8.ResumeLayout(false);
            this.tabPage8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudParamsFuelVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudParamsFuelRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudParamsCntTemps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudParamsCntIdles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudParamsValveTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudParamsCntRPMs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudParamsCntPress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudParamsInitial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudParamsOctane)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage18.ResumeLayout(false);
            this.tableLayoutPanel13.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDragTableSplit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDragSpeedTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDragSpeedFrom)).EndInit();
            this.tabControl2.ResumeLayout(false);
            this.tabPage19.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartDragTime)).EndInit();
            this.tabPage21.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartDragAccel)).EndInit();
            this.tabPage20.ResumeLayout(false);
            this.tableLayoutPanel14.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmr50ms;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Controls.Meter mGenIgn;
        private System.Windows.Forms.Label label5;
        private Controls.Meter mGenTemp;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private Controls.Meter mGenPress;
        private Controls.Meter mGenRPM;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton rbCutoffMode7;
        private System.Windows.Forms.RadioButton rbCutoffMode6;
        private System.Windows.Forms.RadioButton rbCutoffMode5;
        private System.Windows.Forms.RadioButton rbCutoffMode8;
        private System.Windows.Forms.RadioButton rbCutoffMode4;
        private System.Windows.Forms.RadioButton rbCutoffMode3;
        private System.Windows.Forms.RadioButton rbCutoffMode2;
        private System.Windows.Forms.RadioButton rbCutoffMode1;
        private System.Windows.Forms.CheckBox cbCutoffEnabled;
        private System.Windows.Forms.Label lblCutoffAngle;
        private System.Windows.Forms.TrackBar tbCutoffAngle;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblCutoffRPM;
        private System.Windows.Forms.TrackBar tbCutoffRPM;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox cbHallLearn;
        private System.Windows.Forms.CheckBox cbHallIgnition;
        private System.Windows.Forms.CheckBox cbAutostartEnabled;
        private System.Windows.Forms.CheckBox cbTempEnabled;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown nudFuelForce;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown nudSwPos2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown nudSwPos0;
        private System.Windows.Forms.CheckBox cbFuelForce;
        private System.Windows.Forms.CheckBox cbFuelExtSw;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown nudSwPos1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblEconRPM;
        private System.Windows.Forms.TrackBar tbEconRPM;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox cbEconIgnitionBreak;
        private System.Windows.Forms.CheckBox cbEconStrobe;
        private System.Windows.Forms.CheckBox cbEconEnabled;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.CheckBox cbLive;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TabControl tabControl111;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage12;
        private System.Windows.Forms.NumericUpDown nudToolsCurTable;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.NumericUpDown nudToolsCopyFrom;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.NumericUpDown nudToolsCopyTo;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Button btnToolsCopy;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.TextBox tbParamsName;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.NumericUpDown nudParamsCntTemps;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.NumericUpDown nudParamsCntIdles;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.NumericUpDown nudParamsValveTimeout;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.NumericUpDown nudParamsCntRPMs;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.NumericUpDown nudParamsCntPress;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.NumericUpDown nudParamsInitial;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.NumericUpDown nudParamsOctane;
        private System.Windows.Forms.RadioButton rbParamsValve2;
        private System.Windows.Forms.RadioButton rbParamsValve1;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.RadioButton rbParamsValve0;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox cbForceIdle;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.NumericUpDown nudEngVol;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.NumericUpDown nudParamsFuelVolume;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.NumericUpDown nudParamsFuelRate;
        private System.Windows.Forms.Label label56;
        private Controls.Meter mGenFuelUsage;
        private System.Windows.Forms.TabPage tabPage18;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.NumericUpDown nudDragSpeedTo;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.NumericUpDown nudDragSpeedFrom;
        private System.Windows.Forms.Button btnDragClear;
        private System.Windows.Forms.Button btnDragStop;
        private System.Windows.Forms.Button btnDragStart;
        private System.Windows.Forms.Label lblDragStatus;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage19;
        private System.Windows.Forms.TabPage tabPage20;
        private System.Windows.Forms.ListView lvDragTable;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label lblDragSpeed;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.Label lblDragTime;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.NumericUpDown nudDragTableSplit;
        private System.Windows.Forms.TabPage tabPage21;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartDragAccel;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartDragTime;
        private System.Windows.Forms.Button btnTableImport;
        private System.Windows.Forms.Button btnTableExport;
        private System.Windows.Forms.SaveFileDialog dlgExport;
        private System.Windows.Forms.OpenFileDialog dlgImport;
        private System.Windows.Forms.Timer tmr1sec;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.NumericUpDown nudForceIgnitionAngle;
        private System.Windows.Forms.CheckBox cbForceIgnition;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.TextBox tbDragName;
    }
}