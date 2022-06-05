using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace ECU_Manager
{
    public partial class ComPortSelector : Form
    {
        public ComPortSelector()
        {
            InitializeComponent();
            string[] ports = SerialPort.GetPortNames();
            comboBox1.Items.Clear();
            foreach (string port in ports)
                comboBox1.Items.Add(port);
            if (comboBox1.Items.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(Properties.Settings.Default.ComPortName))
                {
                    comboBox1.SelectedIndex = 0;
                }
                else
                {
                    if (!comboBox1.Items.Contains(Properties.Settings.Default.ComPortName))
                    {
                        comboBox1.SelectedIndex = 0;
                    }
                    else comboBox1.SelectedItem = Properties.Settings.Default.ComPortName;
                }
            }
            else
            {
                comboBox1.SelectedIndex = comboBox1.Items.Add("No any ports");
                comboBox1.Enabled = false;
                btnEnter.Enabled = false;
            }
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            string portname = comboBox1.SelectedItem.ToString();
            if (SerialPort.GetPortNames().Contains(portname))
            {
                Properties.Settings.Default.ComPortName = portname;
                Properties.Settings.Default.Save();
                this.Hide();
                MiddleLayer middleLayer = new MiddleLayer(portname);
                MainForm mainForm = new MainForm(middleLayer);
                mainForm.Show();

                Thread threadDiag = new Thread(() =>
                {
                    DiagForm diagForm = new DiagForm(middleLayer);
                    diagForm.Show();
                    diagForm.Location = new Point(mainForm.Location.X + mainForm.Size.Width, mainForm.Location.Y);
                    diagForm.Size = new Size(diagForm.Size.Width, mainForm.Size.Height);
                    Application.Run(diagForm);
                });
                threadDiag.IsBackground = false;
                threadDiag.SetApartmentState(ApartmentState.STA);
                threadDiag.Start();
            }
            else MessageBox.Show($"Port {portname} not available.", "ECU", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnLogReader_Click(object sender, EventArgs e)
        {
            DialogResult result = ofdLogReader.ShowDialog();
            if(result == DialogResult.OK)
            {
                this.Hide();
                FileInfo fileInfo = new FileInfo(ofdLogReader.FileName);
                DiagForm diagForm = new DiagForm(fileInfo);
                diagForm.Show();
            }
        }

        private void btnStandalone_Click(object sender, EventArgs e)
        {

        }
    }
}
