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

        public ImportCCode(ArrayType arrayType, float[] initial, int sizex, int sizey = 1)
        {
            InitializeComponent();

            this.arrayType = arrayType;
            this.sizex = sizex;
            this.sizey = sizey;

            if (initial == null)
                this.result = new float[this.sizex * this.sizey];
            else this.result = (float[])initial.Clone();


            string text = string.Empty;
            string line = string.Empty;
            string decplaces = string.Empty;
            int index;

            if (this.arrayType == ArrayType.Array1D)
            {
               decplaces = "." + Enumerable.Repeat("0", 4).Aggregate((sum, next) => sum + next);

                text = "\t";
                for (int x = 0; x < sizex; x++)
                {
                    text += string.Format("{0:0" + decplaces + "}f, ", initial[x]);
                    if ((x + 1) % 8 == 0 && (x + 1) < sizex && x > 0)
                        text += "\r\n\t";
                }
                text += "\r\n";

                this.textBox1.Text = text;

            }
            else if(this.arrayType == ArrayType.Array2D)
            {
                decplaces = "." + Enumerable.Repeat("0", 4).Aggregate((sum, next) => sum + next);

                for (int y = 0; y < sizey; y++)
                {
                    line = string.Empty;
                    text += "\t{ ";
                    for (int x = 0; x < sizex; x++)
                    {
                        index = y * sizex + x;
                        line += string.Format("{0:0" + decplaces + "}f, ", initial[index]);
                    }
                    text += line;
                    text += "},\r\n";
                }

                this.textBox1.Text = text;
            }
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
