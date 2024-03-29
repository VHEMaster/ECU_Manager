﻿namespace ECU_Manager
{
    partial class DiagForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panelChart = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblTimeScale = new System.Windows.Forms.Label();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.tlpCharts = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblTimePos = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.cbLiveView = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panelSetup = new System.Windows.Forms.Panel();
            this.btnExportLog = new System.Windows.Forms.Button();
            this.gpForceTemplate = new System.Windows.Forms.GroupBox();
            this.tbForceTemplate = new System.Windows.Forms.TrackBar();
            this.cbForceTemplate = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnUsedMoveUp = new System.Windows.Forms.Button();
            this.btnUsedMoveDown = new System.Windows.Forms.Button();
            this.btnParamAdd = new System.Windows.Forms.Button();
            this.btnParamRemove = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lbParamsUsed = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbParamsAvailable = new System.Windows.Forms.ListBox();
            this.timer200ms = new System.Windows.Forms.Timer(this.components);
            this.sfdExportLog = new System.Windows.Forms.SaveFileDialog();
            this.btnSaveParams = new System.Windows.Forms.Button();
            this.btnImportParams = new System.Windows.Forms.Button();
            this.sfdSaveParams = new System.Windows.Forms.SaveFileDialog();
            this.ofdImportParams = new System.Windows.Forms.OpenFileDialog();
            this.nudForceTemplate = new ECU_Manager.Controls.NumericUpDownOneWheel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panelChart.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tlpCharts.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panelSetup.SuspendLayout();
            this.gpForceTemplate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbForceTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudForceTemplate)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Panel2MinSize = 330;
            this.splitContainer1.Size = new System.Drawing.Size(362, 563);
            this.splitContainer1.SplitterDistance = 27;
            this.splitContainer1.TabIndex = 1;
            this.splitContainer1.Visible = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Right;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ItemSize = new System.Drawing.Size(50, 29);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(0, 0);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(362, 563);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.panelChart);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(325, 555);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Charts";
            // 
            // panelChart
            // 
            this.panelChart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(16)))), ((int)(((byte)(0)))));
            this.panelChart.Controls.Add(this.tableLayoutPanel1);
            this.panelChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChart.ForeColor = System.Drawing.Color.White;
            this.panelChart.Location = new System.Drawing.Point(0, 0);
            this.panelChart.Margin = new System.Windows.Forms.Padding(0);
            this.panelChart.Name = "panelChart";
            this.panelChart.Size = new System.Drawing.Size(325, 555);
            this.panelChart.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lblTimeScale, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.hScrollBar1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tlpCharts, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblTimePos, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cbLiveView, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(325, 555);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // lblTimeScale
            // 
            this.lblTimeScale.AutoSize = true;
            this.lblTimeScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTimeScale.Location = new System.Drawing.Point(162, 500);
            this.lblTimeScale.Margin = new System.Windows.Forms.Padding(0);
            this.lblTimeScale.Name = "lblTimeScale";
            this.lblTimeScale.Size = new System.Drawing.Size(163, 30);
            this.lblTimeScale.TabIndex = 3;
            this.lblTimeScale.Text = "+10s";
            this.lblTimeScale.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hScrollBar1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.hScrollBar1, 2);
            this.hScrollBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hScrollBar1.Location = new System.Drawing.Point(0, 530);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(325, 25);
            this.hScrollBar1.TabIndex = 0;
            this.hScrollBar1.Value = 90;
            // 
            // tlpCharts
            // 
            this.tlpCharts.ColumnCount = 1;
            this.tableLayoutPanel1.SetColumnSpan(this.tlpCharts, 2);
            this.tlpCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpCharts.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tlpCharts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCharts.Location = new System.Drawing.Point(0, 0);
            this.tlpCharts.Margin = new System.Windows.Forms.Padding(0);
            this.tlpCharts.Name = "tlpCharts";
            this.tlpCharts.RowCount = 2;
            this.tlpCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCharts.Size = new System.Drawing.Size(325, 475);
            this.tlpCharts.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel2.Controls.Add(this.label3, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label4, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.chart1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(319, 231);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(122, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(194, 50);
            this.label3.TabIndex = 0;
            this.label3.Text = "Parameter #124543";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(122, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(194, 181);
            this.label4.TabIndex = 1;
            this.label4.Text = "SOMEVAL";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // chart1
            // 
            this.chart1.AntiAliasing = System.Windows.Forms.DataVisualization.Charting.AntiAliasingStyles.None;
            this.chart1.BackColor = System.Drawing.Color.Transparent;
            this.chart1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.HorizontalCenter;
            this.chart1.BackSecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            chartArea1.AxisX.Crossing = -1.7976931348623157E+308D;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea1.AxisX.LineColor = System.Drawing.Color.DimGray;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(72)))), ((int)(((byte)(0)))));
            chartArea1.AxisX.TitleForeColor = System.Drawing.Color.Empty;
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisY.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea1.AxisY.LineColor = System.Drawing.Color.DimGray;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(72)))), ((int)(((byte)(0)))));
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.BorderColor = System.Drawing.Color.Empty;
            chartArea1.Name = "ChartArea1";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 100F;
            chartArea1.Position.Width = 99F;
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1.IsSoftShadows = false;
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Margin = new System.Windows.Forms.Padding(0);
            this.chart1.Name = "chart1";
            this.tableLayoutPanel2.SetRowSpan(this.chart1, 2);
            series1.BorderWidth = 3;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = System.Drawing.Color.Gold;
            series1.LabelForeColor = System.Drawing.Color.White;
            series1.MarkerSize = 7;
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Square;
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(119, 231);
            this.chart1.TabIndex = 2;
            this.chart1.Text = "chart1";
            this.chart1.TextAntiAliasingQuality = System.Windows.Forms.DataVisualization.Charting.TextAntiAliasingQuality.SystemDefault;
            // 
            // lblTimePos
            // 
            this.lblTimePos.AutoSize = true;
            this.lblTimePos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTimePos.Location = new System.Drawing.Point(0, 500);
            this.lblTimePos.Margin = new System.Windows.Forms.Padding(0);
            this.lblTimePos.Name = "lblTimePos";
            this.lblTimePos.Size = new System.Drawing.Size(162, 30);
            this.lblTimePos.TabIndex = 2;
            this.lblTimePos.Text = "0s";
            this.lblTimePos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.Controls.Add(this.btnZoomOut, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnZoomIn, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(162, 475);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(163, 25);
            this.tableLayoutPanel3.TabIndex = 5;
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.btnZoomOut.Location = new System.Drawing.Point(133, 0);
            this.btnZoomOut.Margin = new System.Windows.Forms.Padding(0);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(30, 25);
            this.btnZoomOut.TabIndex = 7;
            this.btnZoomOut.Text = "-";
            this.btnZoomOut.UseVisualStyleBackColor = true;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.btnZoomIn.Location = new System.Drawing.Point(103, 0);
            this.btnZoomIn.Margin = new System.Windows.Forms.Padding(0);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(30, 25);
            this.btnZoomIn.TabIndex = 6;
            this.btnZoomIn.Text = "+";
            this.btnZoomIn.UseVisualStyleBackColor = true;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // cbLiveView
            // 
            this.cbLiveView.AutoSize = true;
            this.cbLiveView.Checked = true;
            this.cbLiveView.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLiveView.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.cbLiveView.Location = new System.Drawing.Point(5, 475);
            this.cbLiveView.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.cbLiveView.Name = "cbLiveView";
            this.cbLiveView.Size = new System.Drawing.Size(103, 24);
            this.cbLiveView.TabIndex = 4;
            this.cbLiveView.Text = "Live View";
            this.cbLiveView.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panelSetup);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(325, 555);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Setup";
            // 
            // panelSetup
            // 
            this.panelSetup.AutoScroll = true;
            this.panelSetup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(32)))), ((int)(((byte)(0)))));
            this.panelSetup.Controls.Add(this.tableLayoutPanel4);
            this.panelSetup.Controls.Add(this.gpForceTemplate);
            this.panelSetup.Controls.Add(this.label5);
            this.panelSetup.Controls.Add(this.btnUsedMoveUp);
            this.panelSetup.Controls.Add(this.btnUsedMoveDown);
            this.panelSetup.Controls.Add(this.btnParamAdd);
            this.panelSetup.Controls.Add(this.btnParamRemove);
            this.panelSetup.Controls.Add(this.label2);
            this.panelSetup.Controls.Add(this.lbParamsUsed);
            this.panelSetup.Controls.Add(this.label1);
            this.panelSetup.Controls.Add(this.lbParamsAvailable);
            this.panelSetup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSetup.ForeColor = System.Drawing.Color.White;
            this.panelSetup.Location = new System.Drawing.Point(0, 0);
            this.panelSetup.Margin = new System.Windows.Forms.Padding(0);
            this.panelSetup.Name = "panelSetup";
            this.panelSetup.Size = new System.Drawing.Size(325, 555);
            this.panelSetup.TabIndex = 1;
            // 
            // btnExportLog
            // 
            this.btnExportLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExportLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportLog.Location = new System.Drawing.Point(1, 37);
            this.btnExportLog.Margin = new System.Windows.Forms.Padding(1);
            this.btnExportLog.Name = "btnExportLog";
            this.btnExportLog.Size = new System.Drawing.Size(150, 34);
            this.btnExportLog.TabIndex = 12;
            this.btnExportLog.Text = "Export Log";
            this.btnExportLog.UseVisualStyleBackColor = true;
            this.btnExportLog.Click += new System.EventHandler(this.btnExportLog_Click);
            // 
            // gpForceTemplate
            // 
            this.gpForceTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpForceTemplate.Controls.Add(this.tbForceTemplate);
            this.gpForceTemplate.Controls.Add(this.nudForceTemplate);
            this.gpForceTemplate.Controls.Add(this.cbForceTemplate);
            this.gpForceTemplate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gpForceTemplate.ForeColor = System.Drawing.Color.White;
            this.gpForceTemplate.Location = new System.Drawing.Point(8, 422);
            this.gpForceTemplate.Margin = new System.Windows.Forms.Padding(0);
            this.gpForceTemplate.Name = "gpForceTemplate";
            this.gpForceTemplate.Padding = new System.Windows.Forms.Padding(0);
            this.gpForceTemplate.Size = new System.Drawing.Size(297, 91);
            this.gpForceTemplate.TabIndex = 11;
            this.gpForceTemplate.TabStop = false;
            this.gpForceTemplate.Text = "WishParameterName";
            this.gpForceTemplate.Enter += new System.EventHandler(this.gpForceTemplate_Enter);
            // 
            // tbForceTemplate
            // 
            this.tbForceTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbForceTemplate.AutoSize = false;
            this.tbForceTemplate.LargeChange = 200;
            this.tbForceTemplate.Location = new System.Drawing.Point(3, 50);
            this.tbForceTemplate.Maximum = 300;
            this.tbForceTemplate.Name = "tbForceTemplate";
            this.tbForceTemplate.Size = new System.Drawing.Size(291, 38);
            this.tbForceTemplate.SmallChange = 100;
            this.tbForceTemplate.TabIndex = 2;
            // 
            // cbForceTemplate
            // 
            this.cbForceTemplate.Checked = true;
            this.cbForceTemplate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbForceTemplate.Location = new System.Drawing.Point(4, 19);
            this.cbForceTemplate.Name = "cbForceTemplate";
            this.cbForceTemplate.Size = new System.Drawing.Size(18, 27);
            this.cbForceTemplate.TabIndex = 0;
            this.cbForceTemplate.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 398);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(160, 24);
            this.label5.TabIndex = 9;
            this.label5.Text = "Force Parameters";
            // 
            // btnUsedMoveUp
            // 
            this.btnUsedMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUsedMoveUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUsedMoveUp.Location = new System.Drawing.Point(283, 204);
            this.btnUsedMoveUp.Name = "btnUsedMoveUp";
            this.btnUsedMoveUp.Size = new System.Drawing.Size(30, 58);
            this.btnUsedMoveUp.TabIndex = 7;
            this.btnUsedMoveUp.Text = "▲";
            this.btnUsedMoveUp.UseVisualStyleBackColor = true;
            this.btnUsedMoveUp.Click += new System.EventHandler(this.btnUsedMoveUp_Click);
            // 
            // btnUsedMoveDown
            // 
            this.btnUsedMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUsedMoveDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUsedMoveDown.Location = new System.Drawing.Point(283, 268);
            this.btnUsedMoveDown.Name = "btnUsedMoveDown";
            this.btnUsedMoveDown.Size = new System.Drawing.Size(30, 52);
            this.btnUsedMoveDown.TabIndex = 6;
            this.btnUsedMoveDown.Text = "▼";
            this.btnUsedMoveDown.UseVisualStyleBackColor = true;
            this.btnUsedMoveDown.Click += new System.EventHandler(this.btnUsedMoveDown_Click);
            // 
            // btnParamAdd
            // 
            this.btnParamAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnParamAdd.Location = new System.Drawing.Point(141, 163);
            this.btnParamAdd.Name = "btnParamAdd";
            this.btnParamAdd.Size = new System.Drawing.Size(30, 35);
            this.btnParamAdd.TabIndex = 5;
            this.btnParamAdd.Text = "▼";
            this.btnParamAdd.UseVisualStyleBackColor = true;
            this.btnParamAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnParamRemove
            // 
            this.btnParamRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnParamRemove.Location = new System.Drawing.Point(92, 163);
            this.btnParamRemove.Name = "btnParamRemove";
            this.btnParamRemove.Size = new System.Drawing.Size(30, 35);
            this.btnParamRemove.TabIndex = 4;
            this.btnParamRemove.Text = "▲";
            this.btnParamRemove.UseVisualStyleBackColor = true;
            this.btnParamRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 177);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "Used";
            // 
            // lbParamsUsed
            // 
            this.lbParamsUsed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbParamsUsed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lbParamsUsed.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbParamsUsed.ForeColor = System.Drawing.Color.White;
            this.lbParamsUsed.FormattingEnabled = true;
            this.lbParamsUsed.ItemHeight = 16;
            this.lbParamsUsed.Location = new System.Drawing.Point(8, 204);
            this.lbParamsUsed.Name = "lbParamsUsed";
            this.lbParamsUsed.ScrollAlwaysVisible = true;
            this.lbParamsUsed.Size = new System.Drawing.Size(269, 116);
            this.lbParamsUsed.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(186, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Parameters Available";
            // 
            // lbParamsAvailable
            // 
            this.lbParamsAvailable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbParamsAvailable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lbParamsAvailable.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbParamsAvailable.ForeColor = System.Drawing.Color.White;
            this.lbParamsAvailable.FormattingEnabled = true;
            this.lbParamsAvailable.ItemHeight = 16;
            this.lbParamsAvailable.Location = new System.Drawing.Point(8, 41);
            this.lbParamsAvailable.Name = "lbParamsAvailable";
            this.lbParamsAvailable.ScrollAlwaysVisible = true;
            this.lbParamsAvailable.Size = new System.Drawing.Size(297, 116);
            this.lbParamsAvailable.TabIndex = 0;
            // 
            // timer200ms
            // 
            this.timer200ms.Enabled = true;
            this.timer200ms.Interval = 200;
            this.timer200ms.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // sfdExportLog
            // 
            this.sfdExportLog.DefaultExt = "*.eculog";
            this.sfdExportLog.FileName = "*.eculog";
            this.sfdExportLog.Filter = "AutoECU log|*.eculog";
            // 
            // btnSaveParams
            // 
            this.btnSaveParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveParams.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveParams.Location = new System.Drawing.Point(1, 1);
            this.btnSaveParams.Margin = new System.Windows.Forms.Padding(1);
            this.btnSaveParams.Name = "btnSaveParams";
            this.btnSaveParams.Size = new System.Drawing.Size(150, 34);
            this.btnSaveParams.TabIndex = 13;
            this.btnSaveParams.Text = "Save Params";
            this.btnSaveParams.UseVisualStyleBackColor = true;
            this.btnSaveParams.Click += new System.EventHandler(this.btnSaveParams_Click);
            // 
            // btnImportParams
            // 
            this.btnImportParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnImportParams.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportParams.Location = new System.Drawing.Point(153, 1);
            this.btnImportParams.Margin = new System.Windows.Forms.Padding(1);
            this.btnImportParams.Name = "btnImportParams";
            this.btnImportParams.Size = new System.Drawing.Size(151, 34);
            this.btnImportParams.TabIndex = 14;
            this.btnImportParams.Text = "Import Params";
            this.btnImportParams.UseVisualStyleBackColor = true;
            this.btnImportParams.Click += new System.EventHandler(this.btnImportParams_Click);
            // 
            // sfdSaveParams
            // 
            this.sfdSaveParams.DefaultExt = "*.eculogparams";
            this.sfdSaveParams.FileName = "*.eculogparams";
            this.sfdSaveParams.Filter = "AutoECU Params|*.eculogparams";
            // 
            // ofdImportParams
            // 
            this.ofdImportParams.DefaultExt = "*.eculogparams";
            this.ofdImportParams.FileName = "*.eculogparams";
            this.ofdImportParams.Filter = "AutoECU Params|*.eculogparams";
            this.ofdImportParams.ShowReadOnly = true;
            // 
            // nudForceTemplate
            // 
            this.nudForceTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nudForceTemplate.Location = new System.Drawing.Point(28, 21);
            this.nudForceTemplate.Name = "nudForceTemplate";
            this.nudForceTemplate.Size = new System.Drawing.Size(262, 23);
            this.nudForceTemplate.TabIndex = 1;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.btnSaveParams, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnExportLog, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.btnImportParams, 1, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(8, 323);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(305, 72);
            this.tableLayoutPanel4.TabIndex = 15;
            // 
            // DiagForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(362, 563);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MinimumSize = new System.Drawing.Size(378, 480);
            this.Name = "DiagForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Engine Control Unit - Parameters";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DiagForm_FormClosed);
            this.SizeChanged += new System.EventHandler(this.DiagForm_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panelChart.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tlpCharts.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panelSetup.ResumeLayout(false);
            this.panelSetup.PerformLayout();
            this.gpForceTemplate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbForceTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudForceTemplate)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panelChart;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panelSetup;
        private System.Windows.Forms.Button btnParamAdd;
        private System.Windows.Forms.Button btnParamRemove;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lbParamsUsed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbParamsAvailable;
        private System.Windows.Forms.Button btnUsedMoveUp;
        private System.Windows.Forms.Button btnUsedMoveDown;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblTimeScale;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.TableLayoutPanel tlpCharts;
        private System.Windows.Forms.Label lblTimePos;
        private System.Windows.Forms.CheckBox cbLiveView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnZoomOut;
        private System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Timer timer200ms;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox gpForceTemplate;
        private System.Windows.Forms.TrackBar tbForceTemplate;
        private System.Windows.Forms.CheckBox cbForceTemplate;
        private Controls.NumericUpDownOneWheel nudForceTemplate;
        private System.Windows.Forms.Button btnExportLog;
        private System.Windows.Forms.SaveFileDialog sfdExportLog;
        private System.Windows.Forms.Button btnImportParams;
        private System.Windows.Forms.Button btnSaveParams;
        private System.Windows.Forms.SaveFileDialog sfdSaveParams;
        private System.Windows.Forms.OpenFileDialog ofdImportParams;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
    }
}