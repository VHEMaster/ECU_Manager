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

    public partial class ImportCCodeForm : Form
    {
        private ArrayType arrayType;
        private int sizex, sizey;
        private float[] result;

        public ImportCCodeForm(ArrayType arrayType, float[] initial, int sizex, int sizey = 1)
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
            try
            {
                string text = textBox1.Text;
                text = text.Replace(" ", "");
                text = text.Replace("\t", "");
                text = text.Replace("\r", "");
                text = text.Replace("\n", "");
                text = text.Replace("f", "");
                text = text.Replace("D", "");
                text = text.Replace("d", "");
                text = text.Replace("u", "");
                text = text.Replace("l", "");

                text = text.Replace("},{", "|");

                string[] lines_n = text.Split('|');
                List<string> lines = new List<string>();
                float[][] values;

                for (int i = 0; i < lines_n.Length; i++)
                {
                    string tmp = lines_n[i];
                    tmp = tmp.Replace("{", "");
                    tmp = tmp.Replace("}", "");
                    if (tmp.Length > 0)
                    {
                        lines.Add(tmp);
                    }
                }

                values = new float[lines.Count][];

                for (int i = 0; i < lines.Count; i++)
                {
                    string[] vars = lines[i].Split(',');
                    List<string> tmp1 = new List<string>();
                    for (int j = 0; j < vars.Length; j++)
                    {
                        if (vars[j].Length > 0)
                        {
                            tmp1.Add(vars[j]);
                        }
                    }
                    values[i] = new float[tmp1.Count];
                    for (int j = 0; j < tmp1.Count; j++)
                    {
                        values[i][j] = Convert.ToSingle(tmp1[j]);
                    }
                }

                for (int i = 0; i < this.sizey; i++)
                {
                    for (int j = 0; j < this.sizex; j++)
                    {
                        int index = i * this.sizex + j;
                        if (i < values.Length)
                        {
                            if (j < values[i].Length)
                            {
                                this.result[index] = values[i][j];
                            }
                        }
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch
            {
                MessageBox.Show("Failed to parse C code!", "ECU Manager", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
