namespace ECU_Manager.Controls
{
    partial class Editor1D
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
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.lblParams = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCopyToC = new System.Windows.Forms.Button();
            this.lblItemValue = new System.Windows.Forms.Label();
            this.nudValue = new ECU_Manager.Controls.NumericUpDownOneWheel();
            this.label22 = new System.Windows.Forms.Label();
            this.btnPressApply = new System.Windows.Forms.Button();
            this.nudItem = new ECU_Manager.Controls.NumericUpDownOneWheel();
            this.label21 = new System.Windows.Forms.Label();
            this.chart1DChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnImportFromCCode = new System.Windows.Forms.Button();
            this.tableLayoutPanel5.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1DChart)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.lblParams, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.panel2, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.chart1DChart, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.lblTitle, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 3;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(1116, 531);
            this.tableLayoutPanel5.TabIndex = 2;
            // 
            // lblParams
            // 
            this.lblParams.AutoSize = true;
            this.lblParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblParams.Location = new System.Drawing.Point(561, 0);
            this.lblParams.Name = "lblParams";
            this.lblParams.Size = new System.Drawing.Size(552, 30);
            this.lblParams.TabIndex = 4;
            this.lblParams.Text = "Parameters";
            this.lblParams.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.tableLayoutPanel5.SetColumnSpan(this.panel2, 2);
            this.panel2.Controls.Add(this.btnImportFromCCode);
            this.panel2.Controls.Add(this.btnCopyToC);
            this.panel2.Controls.Add(this.lblItemValue);
            this.panel2.Controls.Add(this.nudValue);
            this.panel2.Controls.Add(this.label22);
            this.panel2.Controls.Add(this.btnPressApply);
            this.panel2.Controls.Add(this.nudItem);
            this.panel2.Controls.Add(this.label21);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 493);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1116, 38);
            this.panel2.TabIndex = 1;
            // 
            // btnCopyToC
            // 
            this.btnCopyToC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopyToC.Location = new System.Drawing.Point(741, 3);
            this.btnCopyToC.Name = "btnCopyToC";
            this.btnCopyToC.Size = new System.Drawing.Size(170, 31);
            this.btnCopyToC.TabIndex = 6;
            this.btnCopyToC.Text = "Copy to C code";
            this.btnCopyToC.UseVisualStyleBackColor = true;
            this.btnCopyToC.Click += new System.EventHandler(this.btnCopyToC_Click);
            // 
            // lblItemValue
            // 
            this.lblItemValue.AutoSize = true;
            this.lblItemValue.Location = new System.Drawing.Point(246, 5);
            this.lblItemValue.Name = "lblItemValue";
            this.lblItemValue.Size = new System.Drawing.Size(94, 24);
            this.lblItemValue.TabIndex = 5;
            this.lblItemValue.Text = "ItemValue";
            // 
            // nudValue
            // 
            this.nudValue.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudValue.Location = new System.Drawing.Point(492, 3);
            this.nudValue.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nudValue.Minimum = new decimal(new int[] {
            100000000,
            0,
            0,
            -2147483648});
            this.nudValue.Name = "nudValue";
            this.nudValue.Size = new System.Drawing.Size(138, 29);
            this.nudValue.TabIndex = 4;
            this.nudValue.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudValue.ValueChanged += new System.EventHandler(this.nudValue_ValueChanged);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(422, 5);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(64, 24);
            this.label22.TabIndex = 3;
            this.label22.Text = "Value:";
            // 
            // btnPressApply
            // 
            this.btnPressApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPressApply.Location = new System.Drawing.Point(636, 2);
            this.btnPressApply.Name = "btnPressApply";
            this.btnPressApply.Size = new System.Drawing.Size(99, 31);
            this.btnPressApply.TabIndex = 2;
            this.btnPressApply.Text = "Apply";
            this.btnPressApply.UseVisualStyleBackColor = true;
            this.btnPressApply.Click += new System.EventHandler(this.btnPressApply_Click);
            // 
            // nudItem
            // 
            this.nudItem.Location = new System.Drawing.Point(120, 3);
            this.nudItem.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudItem.Name = "nudItem";
            this.nudItem.Size = new System.Drawing.Size(120, 29);
            this.nudItem.TabIndex = 1;
            this.nudItem.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudItem.ValueChanged += new System.EventHandler(this.nudItem_ValueChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(64, 5);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(50, 24);
            this.label21.TabIndex = 0;
            this.label21.Text = "Item:";
            // 
            // chart1DChart
            // 
            chartArea2.AxisX.Interval = 1D;
            chartArea2.AxisX.IsMarginVisible = false;
            chartArea2.AxisX.Minimum = 1D;
            chartArea2.AxisY.Interval = 20000D;
            chartArea2.AxisY.Maximum = 120000D;
            chartArea2.AxisY.Minimum = 0D;
            chartArea2.Name = "ChartArea1";
            this.chart1DChart.ChartAreas.Add(chartArea2);
            this.tableLayoutPanel5.SetColumnSpan(this.chart1DChart, 2);
            this.chart1DChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1DChart.Location = new System.Drawing.Point(3, 33);
            this.chart1DChart.Name = "chart1DChart";
            series2.BorderWidth = 3;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = System.Drawing.Color.Brown;
            series2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            series2.IsValueShownAsLabel = true;
            series2.LabelBorderWidth = 0;
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
            this.chart1DChart.Series.Add(series2);
            this.chart1DChart.Size = new System.Drawing.Size(1110, 457);
            this.chart1DChart.TabIndex = 2;
            this.chart1DChart.Text = " ";
            this.chart1DChart.Click += new System.EventHandler(this.chart1DChart_Click);
            this.chart1DChart.Paint += new System.Windows.Forms.PaintEventHandler(this.chart1DChart_Paint);
            this.chart1DChart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chart1DChart_MouseDown);
            this.chart1DChart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart1DChart_MouseMove);
            this.chart1DChart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chart1DChart_MouseUp);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Location = new System.Drawing.Point(3, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(552, 30);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Chart 1D";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnImportFromCCode
            // 
            this.btnImportFromCCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportFromCCode.Location = new System.Drawing.Point(917, 3);
            this.btnImportFromCCode.Name = "btnImportFromCCode";
            this.btnImportFromCCode.Size = new System.Drawing.Size(184, 31);
            this.btnImportFromCCode.TabIndex = 13;
            this.btnImportFromCCode.Text = "Import from C code";
            this.btnImportFromCCode.UseVisualStyleBackColor = true;
            this.btnImportFromCCode.Click += new System.EventHandler(this.btnImportFromCCode_Click);
            // 
            // Editor1D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel5);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Editor1D";
            this.Size = new System.Drawing.Size(1116, 531);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1DChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button btnPressApply;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1DChart;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblParams;
        private System.Windows.Forms.Label lblItemValue;
        private NumericUpDownOneWheel nudValue;
        private NumericUpDownOneWheel nudItem;
        private System.Windows.Forms.Button btnCopyToC;
        private System.Windows.Forms.Button btnImportFromCCode;
    }
}