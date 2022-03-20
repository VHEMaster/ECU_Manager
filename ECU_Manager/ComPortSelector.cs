using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

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
                button1.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string portname = comboBox1.SelectedItem.ToString();
            if (SerialPort.GetPortNames().Contains(portname))
            {
                this.Hide();
                MainForm mainForm = new MainForm(portname);
                mainForm.ShowDialog();
                this.Close();
            }
            else MessageBox.Show($"Port {portname} not available.", "ECU", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
