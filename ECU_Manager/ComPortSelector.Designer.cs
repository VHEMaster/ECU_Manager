namespace ECU_Manager
{
    partial class ComPortSelector
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComPortSelector));
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnEnter = new System.Windows.Forms.Button();
            this.btnLogReader = new System.Windows.Forms.Button();
            this.btnStandalone = new System.Windows.Forms.Button();
            this.ofdLogReader = new System.Windows.Forms.OpenFileDialog();
            this.ofdStandalone = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.Name = "comboBox1";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Name = "label2";
            // 
            // btnEnter
            // 
            this.btnEnter.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.btnEnter, "btnEnter");
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.UseVisualStyleBackColor = true;
            this.btnEnter.Click += new System.EventHandler(this.btnEnter_Click);
            // 
            // btnLogReader
            // 
            resources.ApplyResources(this.btnLogReader, "btnLogReader");
            this.btnLogReader.Name = "btnLogReader";
            this.btnLogReader.UseVisualStyleBackColor = true;
            this.btnLogReader.Click += new System.EventHandler(this.btnLogReader_Click);
            // 
            // btnStandalone
            // 
            resources.ApplyResources(this.btnStandalone, "btnStandalone");
            this.btnStandalone.Name = "btnStandalone";
            this.btnStandalone.UseVisualStyleBackColor = true;
            this.btnStandalone.Click += new System.EventHandler(this.btnStandalone_Click);
            // 
            // ofdLogReader
            // 
            this.ofdLogReader.FileName = "*.eculog";
            resources.ApplyResources(this.ofdLogReader, "ofdLogReader");
            // 
            // ofdStandalone
            // 
            resources.ApplyResources(this.ofdStandalone, "ofdStandalone");
            this.ofdStandalone.ReadOnlyChecked = true;
            // 
            // ComPortSelector
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSkyBlue;
            this.Controls.Add(this.btnStandalone);
            this.Controls.Add(this.btnLogReader);
            this.Controls.Add(this.btnEnter);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ComPortSelector";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnEnter;
        private System.Windows.Forms.Button btnLogReader;
        private System.Windows.Forms.Button btnStandalone;
        private System.Windows.Forms.OpenFileDialog ofdLogReader;
        private System.Windows.Forms.OpenFileDialog ofdStandalone;
    }
}

