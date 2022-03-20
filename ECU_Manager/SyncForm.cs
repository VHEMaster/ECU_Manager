using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECU_Manager
{
    public partial class SyncForm : Form
    {
        private bool CanClose = false;
        public void CloseForm()
        {
            CanClose = true;
            Close();
        }

        public SyncForm()
        {
            InitializeComponent();
        }

        private void SyncForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !CanClose;
        }
    }
}
