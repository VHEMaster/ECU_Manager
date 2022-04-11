namespace ECU_Manager
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea10 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series10 = new System.Windows.Forms.DataVisualization.Charting.Series();
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
            this.btnUsedMoveUp = new System.Windows.Forms.Button();
            this.btnUsedMoveDown = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lbUsed = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbAvailable = new System.Windows.Forms.ListBox();
            this.timer200ms = new System.Windows.Forms.Timer(this.components);
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
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Panel2MinSize = 300;
            this.splitContainer1.Size = new System.Drawing.Size(334, 563);
            this.splitContainer1.SplitterDistance = 25;
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
            this.tabControl1.Size = new System.Drawing.Size(334, 563);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.panelChart);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(297, 555);
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
            this.panelChart.Size = new System.Drawing.Size(297, 555);
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(297, 555);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // lblTimeScale
            // 
            this.lblTimeScale.AutoSize = true;
            this.lblTimeScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTimeScale.Location = new System.Drawing.Point(148, 500);
            this.lblTimeScale.Margin = new System.Windows.Forms.Padding(0);
            this.lblTimeScale.Name = "lblTimeScale";
            this.lblTimeScale.Size = new System.Drawing.Size(149, 30);
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
            this.hScrollBar1.Size = new System.Drawing.Size(297, 25);
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
            this.tlpCharts.Size = new System.Drawing.Size(297, 475);
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
            this.tableLayoutPanel2.Size = new System.Drawing.Size(291, 231);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(94, 0);
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
            this.label4.Location = new System.Drawing.Point(94, 50);
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
            chartArea10.AxisX.Crossing = -1.7976931348623157E+308D;
            chartArea10.AxisX.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            chartArea10.AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea10.AxisX.LineColor = System.Drawing.Color.DimGray;
            chartArea10.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(72)))), ((int)(((byte)(0)))));
            chartArea10.AxisX.TitleForeColor = System.Drawing.Color.Empty;
            chartArea10.AxisY.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            chartArea10.AxisY.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea10.AxisY.LineColor = System.Drawing.Color.DimGray;
            chartArea10.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(72)))), ((int)(((byte)(0)))));
            chartArea10.BackColor = System.Drawing.Color.Transparent;
            chartArea10.BorderColor = System.Drawing.Color.Empty;
            chartArea10.Name = "ChartArea1";
            chartArea10.Position.Auto = false;
            chartArea10.Position.Height = 100F;
            chartArea10.Position.Width = 99F;
            this.chart1.ChartAreas.Add(chartArea10);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1.IsSoftShadows = false;
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Margin = new System.Windows.Forms.Padding(0);
            this.chart1.Name = "chart1";
            this.tableLayoutPanel2.SetRowSpan(this.chart1, 2);
            series10.BorderWidth = 3;
            series10.ChartArea = "ChartArea1";
            series10.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series10.Color = System.Drawing.Color.Gold;
            series10.LabelForeColor = System.Drawing.Color.White;
            series10.MarkerSize = 7;
            series10.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Square;
            series10.Name = "Series1";
            this.chart1.Series.Add(series10);
            this.chart1.Size = new System.Drawing.Size(91, 231);
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
            this.lblTimePos.Size = new System.Drawing.Size(148, 30);
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
            this.tableLayoutPanel3.Location = new System.Drawing.Point(148, 475);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(149, 25);
            this.tableLayoutPanel3.TabIndex = 5;
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.btnZoomOut.Location = new System.Drawing.Point(119, 0);
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
            this.btnZoomIn.Location = new System.Drawing.Point(89, 0);
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
            this.tabPage2.Size = new System.Drawing.Size(297, 555);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Setup";
            // 
            // panelSetup
            // 
            this.panelSetup.AutoScroll = true;
            this.panelSetup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(32)))), ((int)(((byte)(0)))));
            this.panelSetup.Controls.Add(this.btnUsedMoveUp);
            this.panelSetup.Controls.Add(this.btnUsedMoveDown);
            this.panelSetup.Controls.Add(this.btnAdd);
            this.panelSetup.Controls.Add(this.btnRemove);
            this.panelSetup.Controls.Add(this.label2);
            this.panelSetup.Controls.Add(this.lbUsed);
            this.panelSetup.Controls.Add(this.label1);
            this.panelSetup.Controls.Add(this.lbAvailable);
            this.panelSetup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSetup.ForeColor = System.Drawing.Color.White;
            this.panelSetup.Location = new System.Drawing.Point(0, 0);
            this.panelSetup.Margin = new System.Windows.Forms.Padding(0);
            this.panelSetup.Name = "panelSetup";
            this.panelSetup.Size = new System.Drawing.Size(297, 555);
            this.panelSetup.TabIndex = 1;
            // 
            // btnUsedMoveUp
            // 
            this.btnUsedMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUsedMoveUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUsedMoveUp.Location = new System.Drawing.Point(243, 204);
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
            this.btnUsedMoveDown.Location = new System.Drawing.Point(243, 268);
            this.btnUsedMoveDown.Name = "btnUsedMoveDown";
            this.btnUsedMoveDown.Size = new System.Drawing.Size(30, 52);
            this.btnUsedMoveDown.TabIndex = 6;
            this.btnUsedMoveDown.Text = "▼";
            this.btnUsedMoveDown.UseVisualStyleBackColor = true;
            this.btnUsedMoveDown.Click += new System.EventHandler(this.btnUsedMoveDown_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Location = new System.Drawing.Point(141, 163);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(30, 35);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "▼";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemove.Location = new System.Drawing.Point(92, 163);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(30, 35);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.Text = "▲";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
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
            // lbUsed
            // 
            this.lbUsed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbUsed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lbUsed.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbUsed.ForeColor = System.Drawing.Color.White;
            this.lbUsed.FormattingEnabled = true;
            this.lbUsed.ItemHeight = 16;
            this.lbUsed.Location = new System.Drawing.Point(8, 204);
            this.lbUsed.Name = "lbUsed";
            this.lbUsed.ScrollAlwaysVisible = true;
            this.lbUsed.Size = new System.Drawing.Size(229, 116);
            this.lbUsed.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Available";
            // 
            // lbAvailable
            // 
            this.lbAvailable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbAvailable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lbAvailable.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbAvailable.ForeColor = System.Drawing.Color.White;
            this.lbAvailable.FormattingEnabled = true;
            this.lbAvailable.ItemHeight = 16;
            this.lbAvailable.Location = new System.Drawing.Point(8, 41);
            this.lbAvailable.Name = "lbAvailable";
            this.lbAvailable.ScrollAlwaysVisible = true;
            this.lbAvailable.Size = new System.Drawing.Size(265, 116);
            this.lbAvailable.TabIndex = 0;
            // 
            // timer200ms
            // 
            this.timer200ms.Enabled = true;
            this.timer200ms.Interval = 200;
            this.timer200ms.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // DiagForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(334, 563);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MinimumSize = new System.Drawing.Size(350, 480);
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panelChart;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panelSetup;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lbUsed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbAvailable;
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
    }
}