using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECU_Manager.Dialogs
{
    public enum ArrayType
    {
        Array1D,
        Array2D
    }

    public partial class ImportCCode : Form
    {
        private ArrayType arrayType;
        private int sizex, sizey;
        private float[] result;

        public ImportCCode(ArrayType arrayType, int sizex, int sizey = 1)
        {
            InitializeComponent();

            this.arrayType = arrayType;
            this.sizex = sizex;
            this.sizey = sizey;

            this.result = new float[this.sizex * this.sizey];
        }

        public float[] GetResult()
        {
            return this.result;
        }
       

        private void btnAccept_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
