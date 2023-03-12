namespace ECU_Manager.Controls
{
    partial class Editor2D
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.scVertical = new System.Windows.Forms.SplitContainer();
            this.scHorisontal = new System.Windows.Forms.SplitContainer();
            this.tlp2DTable = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpGraph = new System.Windows.Forms.TabPage();
            this.graph3D = new ECU_Manager.Controls.Graph3D();
            this.tpInterpolation = new System.Windows.Forms.TabPage();
            this.cbInterpolateUseProgress = new System.Windows.Forms.CheckBox();
            this.btnInterpolate = new System.Windows.Forms.Button();
            this.nudInterpolationRadius = new ECU_Manager.Controls.NumericUpDownOneWheel();
            this.nudInterpolationAmount = new ECU_Manager.Controls.NumericUpDownOneWheel();
            this.label3 = new System.Windows.Forms.Label();
            this.nudInterpolationKoff = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tpTools = new System.Windows.Forms.TabPage();
            this.btnImportFromCCode = new System.Windows.Forms.Button();
            this.btnExport2DChart = new System.Windows.Forms.Button();
            this.btnImport2DChart = new System.Windows.Forms.Button();
            this.btnCopyToC = new System.Windows.Forms.Button();
            this.chart2DChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblParams = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.dlgExport2DChart = new System.Windows.Forms.SaveFileDialog();
            this.dlgImport2DChart = new System.Windows.Forms.OpenFileDialog();
            this.btnCopyToText = new System.Windows.Forms.Button();
            this.btnImportFromText = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.scVertical)).BeginInit();
            this.scVertical.Panel1.SuspendLayout();
            this.scVertical.Panel2.SuspendLayout();
            this.scVertical.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scHorisontal)).BeginInit();
            this.scHorisontal.Panel1.SuspendLayout();
            this.scHorisontal.Panel2.SuspendLayout();
            this.scHorisontal.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpGraph.SuspendLayout();
            this.tpInterpolation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterpolationRadius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterpolationAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterpolationKoff)).BeginInit();
            this.tpTools.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart2DChart)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // scVertical
            // 
            this.scVertical.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanel1.SetColumnSpan(this.scVertical, 2);
            this.scVertical.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scVertical.Location = new System.Drawing.Point(0, 30);
            this.scVertical.Margin = new System.Windows.Forms.Padding(0);
            this.scVertical.Name = "scVertical";
            this.scVertical.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scVertical.Panel1
            // 
            this.scVertical.Panel1.Controls.Add(this.scHorisontal);
            this.scVertical.Panel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.scVertical.Panel1MinSize = 128;
            // 
            // scVertical.Panel2
            // 
            this.scVertical.Panel2.Controls.Add(this.chart2DChart);
            this.scVertical.Panel2.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.scVertical.Panel2MinSize = 128;
            this.scVertical.Size = new System.Drawing.Size(1050, 592);
            this.scVertical.SplitterDistance = 346;
            this.scVertical.TabIndex = 2;
            // 
            // scHorisontal
            // 
            this.scHorisontal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scHorisontal.Location = new System.Drawing.Point(0, 0);
            this.scHorisontal.Margin = new System.Windows.Forms.Padding(0);
            this.scHorisontal.Name = "scHorisontal";
            // 
            // scHorisontal.Panel1
            // 
            this.scHorisontal.Panel1.Controls.Add(this.tlp2DTable);
            // 
            // scHorisontal.Panel2
            // 
            this.scHorisontal.Panel2.Controls.Add(this.tabControl1);
            this.scHorisontal.Size = new System.Drawing.Size(1046, 339);
            this.scHorisontal.SplitterDistance = 669;
            this.scHorisontal.TabIndex = 1;
            // 
            // tlp2DTable
            // 
            this.tlp2DTable.AutoScroll = true;
            this.tlp2DTable.ColumnCount = 2;
            this.tlp2DTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp2DTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp2DTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp2DTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tlp2DTable.Location = new System.Drawing.Point(0, 0);
            this.tlp2DTable.Margin = new System.Windows.Forms.Padding(0);
            this.tlp2DTable.Name = "tlp2DTable";
            this.tlp2DTable.RowCount = 2;
            this.tlp2DTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp2DTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp2DTable.Size = new System.Drawing.Size(669, 339);
            this.tlp2DTable.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpGraph);
            this.tabControl1.Controls.Add(this.tpInterpolation);
            this.tabControl1.Controls.Add(this.tpTools);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(0, 0);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(373, 339);
            this.tabControl1.TabIndex = 1;
            // 
            // tpGraph
            // 
            this.tpGraph.Controls.Add(this.graph3D);
            this.tpGraph.Location = new System.Drawing.Point(4, 33);
            this.tpGraph.Margin = new System.Windows.Forms.Padding(0);
            this.tpGraph.Name = "tpGraph";
            this.tpGraph.Size = new System.Drawing.Size(365, 302);
            this.tpGraph.TabIndex = 0;
            this.tpGraph.Text = "3D Graph";
            this.tpGraph.UseVisualStyleBackColor = true;
            // 
            // graph3D
            // 
            this.graph3D.AxisX_Color = System.Drawing.Color.DarkBlue;
            this.graph3D.AxisX_Legend = null;
            this.graph3D.AxisY_Color = System.Drawing.Color.DarkGreen;
            this.graph3D.AxisY_Legend = null;
            this.graph3D.AxisZ_Color = System.Drawing.Color.DarkRed;
            this.graph3D.AxisZ_Legend = null;
            this.graph3D.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.graph3D.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graph3D.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.graph3D.Location = new System.Drawing.Point(0, 0);
            this.graph3D.Margin = new System.Windows.Forms.Padding(0);
            this.graph3D.Name = "graph3D";
            this.graph3D.PolygonLineColor = System.Drawing.Color.Black;
            this.graph3D.Raster = ECU_Manager.Controls.Graph3D.eRaster.Off;
            this.graph3D.Size = new System.Drawing.Size(365, 302);
            this.graph3D.TabIndex = 0;
            this.graph3D.TopLegendColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(150)))));
            // 
            // tpInterpolation
            // 
            this.tpInterpolation.BackColor = System.Drawing.Color.Transparent;
            this.tpInterpolation.Controls.Add(this.cbInterpolateUseProgress);
            this.tpInterpolation.Controls.Add(this.btnInterpolate);
            this.tpInterpolation.Controls.Add(this.nudInterpolationRadius);
            this.tpInterpolation.Controls.Add(this.nudInterpolationAmount);
            this.tpInterpolation.Controls.Add(this.label3);
            this.tpInterpolation.Controls.Add(this.nudInterpolationKoff);
            this.tpInterpolation.Controls.Add(this.label2);
            this.tpInterpolation.Controls.Add(this.label1);
            this.tpInterpolation.Location = new System.Drawing.Point(4, 22);
            this.tpInterpolation.Name = "tpInterpolation";
            this.tpInterpolation.Padding = new System.Windows.Forms.Padding(3);
            this.tpInterpolation.Size = new System.Drawing.Size(365, 313);
            this.tpInterpolation.TabIndex = 1;
            this.tpInterpolation.Text = "Interpolation";
            // 
            // cbInterpolateUseProgress
            // 
            this.cbInterpolateUseProgress.AutoSize = true;
            this.cbInterpolateUseProgress.Checked = true;
            this.cbInterpolateUseProgress.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbInterpolateUseProgress.Enabled = false;
            this.cbInterpolateUseProgress.Location = new System.Drawing.Point(10, 152);
            this.cbInterpolateUseProgress.Name = "cbInterpolateUseProgress";
            this.cbInterpolateUseProgress.Size = new System.Drawing.Size(142, 28);
            this.cbInterpolateUseProgress.TabIndex = 9;
            this.cbInterpolateUseProgress.Text = "Use Progress";
            this.cbInterpolateUseProgress.UseVisualStyleBackColor = true;
            this.cbInterpolateUseProgress.Visible = false;
            // 
            // btnInterpolate
            // 
            this.btnInterpolate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInterpolate.Location = new System.Drawing.Point(52, 113);
            this.btnInterpolate.Name = "btnInterpolate";
            this.btnInterpolate.Size = new System.Drawing.Size(147, 33);
            this.btnInterpolate.TabIndex = 8;
            this.btnInterpolate.Text = "Interpolate";
            this.btnInterpolate.UseVisualStyleBackColor = true;
            this.btnInterpolate.Click += new System.EventHandler(this.btnInterpolate_Click);
            // 
            // nudInterpolationRadius
            // 
            this.nudInterpolationRadius.Location = new System.Drawing.Point(114, 6);
            this.nudInterpolationRadius.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.nudInterpolationRadius.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudInterpolationRadius.Name = "nudInterpolationRadius";
            this.nudInterpolationRadius.Size = new System.Drawing.Size(120, 29);
            this.nudInterpolationRadius.TabIndex = 7;
            this.nudInterpolationRadius.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // nudInterpolationAmount
            // 
            this.nudInterpolationAmount.Location = new System.Drawing.Point(114, 78);
            this.nudInterpolationAmount.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudInterpolationAmount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudInterpolationAmount.Name = "nudInterpolationAmount";
            this.nudInterpolationAmount.Size = new System.Drawing.Size(120, 29);
            this.nudInterpolationAmount.TabIndex = 6;
            this.nudInterpolationAmount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 24);
            this.label3.TabIndex = 4;
            this.label3.Text = "Amount:";
            // 
            // nudInterpolationKoff
            // 
            this.nudInterpolationKoff.DecimalPlaces = 2;
            this.nudInterpolationKoff.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudInterpolationKoff.Location = new System.Drawing.Point(114, 41);
            this.nudInterpolationKoff.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudInterpolationKoff.Name = "nudInterpolationKoff";
            this.nudInterpolationKoff.Size = new System.Drawing.Size(120, 29);
            this.nudInterpolationKoff.TabIndex = 3;
            this.nudInterpolationKoff.Value = new decimal(new int[] {
            9,
            0,
            0,
            65536});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "Koff:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Radius:";
            // 
            // tpTools
            // 
            this.tpTools.BackColor = System.Drawing.Color.Transparent;
            this.tpTools.Controls.Add(this.btnImportFromText);
            this.tpTools.Controls.Add(this.btnCopyToText);
            this.tpTools.Controls.Add(this.btnImportFromCCode);
            this.tpTools.Controls.Add(this.btnExport2DChart);
            this.tpTools.Controls.Add(this.btnImport2DChart);
            this.tpTools.Controls.Add(this.btnCopyToC);
            this.tpTools.Location = new System.Drawing.Point(4, 33);
            this.tpTools.Name = "tpTools";
            this.tpTools.Padding = new System.Windows.Forms.Padding(3);
            this.tpTools.Size = new System.Drawing.Size(365, 302);
            this.tpTools.TabIndex = 2;
            this.tpTools.Text = "Tools";
            // 
            // btnImportFromCCode
            // 
            this.btnImportFromCCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportFromCCode.Location = new System.Drawing.Point(6, 84);
            this.btnImportFromCCode.Name = "btnImportFromCCode";
            this.btnImportFromCCode.Size = new System.Drawing.Size(201, 33);
            this.btnImportFromCCode.TabIndex = 12;
            this.btnImportFromCCode.Text = "Import from C code";
            this.btnImportFromCCode.UseVisualStyleBackColor = true;
            this.btnImportFromCCode.Click += new System.EventHandler(this.btnImportFromCCode_Click);
            // 
            // btnExport2DChart
            // 
            this.btnExport2DChart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport2DChart.Location = new System.Drawing.Point(109, 6);
            this.btnExport2DChart.Name = "btnExport2DChart";
            this.btnExport2DChart.Size = new System.Drawing.Size(98, 33);
            this.btnExport2DChart.TabIndex = 11;
            this.btnExport2DChart.Text = "Export";
            this.btnExport2DChart.UseVisualStyleBackColor = true;
            this.btnExport2DChart.Click += new System.EventHandler(this.btnExport2DChart_Click);
            // 
            // btnImport2DChart
            // 
            this.btnImport2DChart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImport2DChart.Location = new System.Drawing.Point(6, 6);
            this.btnImport2DChart.Name = "btnImport2DChart";
            this.btnImport2DChart.Size = new System.Drawing.Size(98, 33);
            this.btnImport2DChart.TabIndex = 10;
            this.btnImport2DChart.Text = "Import";
            this.btnImport2DChart.UseVisualStyleBackColor = true;
            this.btnImport2DChart.Click += new System.EventHandler(this.btnImport2DChart_Click);
            // 
            // btnCopyToC
            // 
            this.btnCopyToC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopyToC.Location = new System.Drawing.Point(6, 45);
            this.btnCopyToC.Name = "btnCopyToC";
            this.btnCopyToC.Size = new System.Drawing.Size(201, 33);
            this.btnCopyToC.TabIndex = 9;
            this.btnCopyToC.Text = "Copy to C code";
            this.btnCopyToC.UseVisualStyleBackColor = true;
            this.btnCopyToC.Click += new System.EventHandler(this.btnCopyToC_Click);
            // 
            // chart2DChart
            // 
            chartArea2.AxisX.Interval = 1D;
            chartArea2.AxisY.Interval = 10D;
            chartArea2.AxisY.Maximum = 60D;
            chartArea2.AxisY.Minimum = 0D;
            chartArea2.Name = "ChartArea1";
            this.chart2DChart.ChartAreas.Add(chartArea2);
            this.chart2DChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chart2DChart.Legends.Add(legend2);
            this.chart2DChart.Location = new System.Drawing.Point(0, 3);
            this.chart2DChart.Name = "chart2DChart";
            series2.BorderWidth = 3;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = System.Drawing.Color.Brown;
            series2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            series2.IsValueShownAsLabel = true;
            series2.LabelBorderWidth = 0;
            series2.Legend = "Legend1";
            series2.MarkerColor = System.Drawing.Color.Black;
            series2.MarkerSize = 8;
            series2.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series2.Name = "Series1";
            series2.SmartLabelStyle.AllowOutsidePlotArea = System.Windows.Forms.DataVisualization.Charting.LabelOutsidePlotAreaStyle.No;
            series2.SmartLabelStyle.MaxMovingDistance = 100D;
            series2.SmartLabelStyle.MinMovingDistance = 10D;
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series2.YValuesPerPoint = 2;
            series2.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Single;
            this.chart2DChart.Series.Add(series2);
            this.chart2DChart.Size = new System.Drawing.Size(1046, 235);
            this.chart2DChart.TabIndex = 4;
            this.chart2DChart.Text = " ";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lblParams, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.scVertical, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblTitle, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1050, 622);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // lblParams
            // 
            this.lblParams.AutoSize = true;
            this.lblParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblParams.Location = new System.Drawing.Point(528, 0);
            this.lblParams.Name = "lblParams";
            this.lblParams.Size = new System.Drawing.Size(519, 30);
            this.lblParams.TabIndex = 6;
            this.lblParams.Text = "Parameters";
            this.lblParams.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Location = new System.Drawing.Point(3, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(519, 30);
            this.lblTitle.TabIndex = 5;
            this.lblTitle.Text = "Chart 1D";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dlgExport2DChart
            // 
            this.dlgExport2DChart.DefaultExt = "xml";
            this.dlgExport2DChart.Filter = "AutoECU 2D Chart|*.ecu2dchart";
            this.dlgExport2DChart.RestoreDirectory = true;
            // 
            // dlgImport2DChart
            // 
            this.dlgImport2DChart.DefaultExt = "xml";
            this.dlgImport2DChart.Filter = "AutoECU 2D Chart|*.ecu2dchart";
            this.dlgImport2DChart.RestoreDirectory = true;
            // 
            // btnCopyToText
            // 
            this.btnCopyToText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopyToText.Location = new System.Drawing.Point(6, 139);
            this.btnCopyToText.Name = "btnCopyToText";
            this.btnCopyToText.Size = new System.Drawing.Size(201, 33);
            this.btnCopyToText.TabIndex = 13;
            this.btnCopyToText.Text = "Copy to Text";
            this.btnCopyToText.UseVisualStyleBackColor = true;
            this.btnCopyToText.Click += new System.EventHandler(this.btnCopyToText_Click);
            // 
            // btnImportFromText
            // 
            this.btnImportFromText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportFromText.Location = new System.Drawing.Point(6, 178);
            this.btnImportFromText.Name = "btnImportFromText";
            this.btnImportFromText.Size = new System.Drawing.Size(201, 33);
            this.btnImportFromText.TabIndex = 14;
            this.btnImportFromText.Text = "Import from Text";
            this.btnImportFromText.UseVisualStyleBackColor = true;
            this.btnImportFromText.Click += new System.EventHandler(this.btnImportFromText_Click);
            // 
            // Editor2D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Editor2D";
            this.Size = new System.Drawing.Size(1050, 622);
            this.Load += new System.EventHandler(this.Editor2D_Load);
            this.scVertical.Panel1.ResumeLayout(false);
            this.scVertical.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scVertical)).EndInit();
            this.scVertical.ResumeLayout(false);
            this.scHorisontal.Panel1.ResumeLayout(false);
            this.scHorisontal.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scHorisontal)).EndInit();
            this.scHorisontal.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tpGraph.ResumeLayout(false);
            this.tpInterpolation.ResumeLayout(false);
            this.tpInterpolation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterpolationRadius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterpolationAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInterpolationKoff)).EndInit();
            this.tpTools.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart2DChart)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tlp2DTable;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblParams;
        private System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.DataVisualization.Charting.Chart chart2DChart;
        private Graph3D graph3D;
        internal System.Windows.Forms.SplitContainer scVertical;
        internal System.Windows.Forms.SplitContainer scHorisontal;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpGraph;
        private System.Windows.Forms.TabPage tpInterpolation;
        private System.Windows.Forms.Button btnInterpolate;
        private NumericUpDownOneWheel nudInterpolationRadius;
        private NumericUpDownOneWheel nudInterpolationAmount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudInterpolationKoff;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbInterpolateUseProgress;
        private System.Windows.Forms.TabPage tpTools;
        private System.Windows.Forms.Button btnCopyToC;
        private System.Windows.Forms.Button btnExport2DChart;
        private System.Windows.Forms.Button btnImport2DChart;
        private System.Windows.Forms.SaveFileDialog dlgExport2DChart;
        private System.Windows.Forms.OpenFileDialog dlgImport2DChart;
        private System.Windows.Forms.Button btnImportFromCCode;
        private System.Windows.Forms.Button btnImportFromText;
        private System.Windows.Forms.Button btnCopyToText;
    }
}