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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.nudValue = new System.Windows.Forms.NumericUpDown();
            this.label22 = new System.Windows.Forms.Label();
            this.btnPressApply = new System.Windows.Forms.Button();
            this.nudItem = new System.Windows.Forms.NumericUpDown();
            this.label21 = new System.Windows.Forms.Label();
            this.chart1DChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tableLayoutPanel5.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1DChart)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.chart1DChart, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(958, 531);
            this.tableLayoutPanel5.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.nudValue);
            this.panel2.Controls.Add(this.label22);
            this.panel2.Controls.Add(this.btnPressApply);
            this.panel2.Controls.Add(this.nudItem);
            this.panel2.Controls.Add(this.label21);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 493);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(958, 38);
            this.panel2.TabIndex = 1;
            // 
            // nudValue
            // 
            this.nudValue.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudValue.Location = new System.Drawing.Point(363, 3);
            this.nudValue.Maximum = new decimal(new int[] {
            115000,
            0,
            0,
            0});
            this.nudValue.Name = "nudValue";
            this.nudValue.Size = new System.Drawing.Size(138, 29);
            this.nudValue.TabIndex = 4;
            this.nudValue.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(293, 5);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(64, 24);
            this.label22.TabIndex = 3;
            this.label22.Text = "Value:";
            // 
            // btnPressApply
            // 
            this.btnPressApply.Location = new System.Drawing.Point(507, 2);
            this.btnPressApply.Name = "btnPressApply";
            this.btnPressApply.Size = new System.Drawing.Size(99, 31);
            this.btnPressApply.TabIndex = 2;
            this.btnPressApply.Text = "Apply";
            this.btnPressApply.UseVisualStyleBackColor = true;
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
            chartArea1.AxisX.Interval = 1D;
            chartArea1.AxisX.IsMarginVisible = false;
            chartArea1.AxisX.Minimum = 1D;
            chartArea1.AxisY.Maximum = 120000D;
            chartArea1.AxisY.Minimum = 0D;
            chartArea1.Name = "ChartArea1";
            this.chart1DChart.ChartAreas.Add(chartArea1);
            this.chart1DChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1DChart.Location = new System.Drawing.Point(3, 3);
            this.chart1DChart.Name = "chart1DChart";
            series1.BorderWidth = 3;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = System.Drawing.Color.Brown;
            series1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            series1.IsValueShownAsLabel = true;
            series1.LabelBorderWidth = 0;
            series1.MarkerColor = System.Drawing.Color.Black;
            series1.MarkerSize = 8;
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series1.Name = "Series1";
            series1.SmartLabelStyle.AllowOutsidePlotArea = System.Windows.Forms.DataVisualization.Charting.LabelOutsidePlotAreaStyle.No;
            series1.SmartLabelStyle.MaxMovingDistance = 100D;
            series1.SmartLabelStyle.MinMovingDistance = 10D;
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series1.YValuesPerPoint = 2;
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Single;
            this.chart1DChart.Series.Add(series1);
            this.chart1DChart.Size = new System.Drawing.Size(952, 487);
            this.chart1DChart.TabIndex = 2;
            this.chart1DChart.Text = " ";
            // 
            // Editor1D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 531);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel5);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Editor1D";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Editor1D";
            this.tableLayoutPanel5.ResumeLayout(false);
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
        private System.Windows.Forms.NumericUpDown nudValue;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button btnPressApply;
        private System.Windows.Forms.NumericUpDown nudItem;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1DChart;
    }
}