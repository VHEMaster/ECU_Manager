using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ECU_Framework.Structs;
using ECU_Manager.Classes;
using ECU_Framework.Tools;

namespace ECU_Manager.Controls
{
    public partial class ImageTable : UserControl
    {
        public int SizeX { get; set; } = 4;
        public int SizeY { get; set; } = 4;
        public int ArraySizeX { get; set; } = 4;
        public int ArraySizeY { get; set; } = 4;
        public float ValueMin { get; set; } = 0.0f;
        public float ValueMax { get; set; } = 1.0f;
        public float Increment { get; set; } = 0.1f;
        public int DecPlaces { get; set; } = 1;

        public float[] Array { get; set; }
        public byte[] ArrayCalib { get; set; }
        public string[] ColumnTitles { get; set; }
        public string[] RowTitles { get; set; }

        public Interpolation ValueInterpolationX { get; set; }
        public Interpolation ValueInterpolationY { get; set; }

        public bool Initialized { get; set; } = false;
        public ColorTransience ColorTransience { get; set; }
        public ColorTransience CalibrationColorTransience { get; set; }
        public bool UseCalibrationColorTransience { get; set; }
        public RectangleF[] CellRectangles { get; private set; }
        public SizeF[] TextSizes { get; private set; }
        
        public delegate void ImageTableEventHandler(object sender, ImageTableEventArg e);
        public ImageTableEventHandler UpdateTableEvent { get; set; } = null;

        private NumericUpDown TextBox = null;

        public ImageTable()
        {
            InitializeComponent();

            this.pictureBox.MouseWheel += pictureBox_MouseWheel;
            this.pictureBox.MouseClick += pictureBox_MouseClick;

        }

        private void pictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            float value = 0;
            int index = -1;
            
            for (int i = 0; i < this.CellRectangles.Length; i++)
            {
                if (this.CellRectangles[i] != RectangleF.Empty)
                {
                    if (e.Location.X >= this.CellRectangles[i].X && e.Location.X <= this.CellRectangles[i].X + this.CellRectangles[i].Width)
                    {
                        if (e.Location.Y >= this.CellRectangles[i].Y && e.Location.Y <= this.CellRectangles[i].Y + this.CellRectangles[i].Height)
                        {
                            index = i;
                            break;
                        }
                    }
                }
            }

            if (index >= 0)
            {
                value = this.Array[index];

                if (e.Delta > 0)
                {
                    if (value + this.Increment > this.ValueMax)
                    {
                        value = this.ValueMax;
                    }
                    else
                    {
                        value += this.Increment;
                    }
                }
                else if (e.Delta < 0)
                {
                    if (value - this.Increment < this.ValueMin)
                    {
                        value = this.ValueMin;
                    }
                    else
                    {
                        value -= this.Increment;
                    }
                }
                if (this.Array[index] != value)
                {
                    this.Array[index] = value;
                    this.RedrawTable();
                    UpdateTableEvent?.Invoke(sender, new ImageTableEventArg() { Index = index, Value = value });

                    if(this.TextBox != null)
                    {
                        if((int)this.TextBox.Tag == index)
                        {
                            this.TextBox.Value = (decimal)value;
                        }
                    }
                }
            }
        }
        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            ColorTransience colorTransience = this.UseCalibrationColorTransience ? this.CalibrationColorTransience : this.ColorTransience;
            
            int index = -1;
            float value = 0;

            this.RemoveTextBox();

            for (int i = 0; i < this.CellRectangles.Length; i++)
            {
                if (this.CellRectangles[i] != RectangleF.Empty)
                {
                    if (e.Location.X >= this.CellRectangles[i].X && e.Location.X <= this.CellRectangles[i].X + this.CellRectangles[i].Width)
                    {
                        if (e.Location.Y >= this.CellRectangles[i].Y && e.Location.Y <= this.CellRectangles[i].Y + this.CellRectangles[i].Height)
                        {
                            index = i;
                            break;
                        }
                    }
                }
            }

            if(index >= 0)
            {
                value = this.Array[index];
                if (value < this.ValueMin)
                {
                    value = this.ValueMin;
                }
                else if (value > this.ValueMax)
                {
                    value = this.ValueMax;
                }

                float colorvalue = value;
                if (this.UseCalibrationColorTransience)
                    colorvalue = this.ArrayCalib[index] / 255.0f;

                this.TextBox = new NumericUpDownOneWheel();
                this.TextBox.Margin = new Padding(0);
                this.TextBox.Minimum = (decimal)this.ValueMin;
                this.TextBox.Maximum = (decimal)this.ValueMax;
                this.TextBox.DecimalPlaces = this.DecPlaces;
                this.TextBox.Increment = (decimal)this.Increment;
                this.TextBox.Tag = index;
                this.TextBox.Value = (decimal)value;
                this.TextBox.ValueChanged += textBox_ValueChanged;
                this.TextBox.KeyPress += textBox_KeyPress;
                this.TextBox.Visible = true;
                this.TextBox.ForeColor = this.ForeColor;
                this.TextBox.BackColor = colorTransience.Get(colorvalue);
                this.TextBox.Location = new Point((int)this.CellRectangles[index].X, (int)this.CellRectangles[index].Y);
                this.TextBox.Size = new Size((int)this.TextSizes[index].Height + 50, (int)this.CellRectangles[index].Height);
                this.Controls.Add(this.TextBox);

                this.TextBox.BringToFront();
                this.TextBox.Focus();
            }
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r' || e.KeyChar == '\n' || e.KeyChar =='\u001b')
            {
                this.RemoveTextBox();
            }
        }

        private void textBox_ValueChanged(object sender, EventArgs e)
        {
            ColorTransience colorTransience = this.UseCalibrationColorTransience ? this.CalibrationColorTransience : this.ColorTransience;
            NumericUpDown textBox = sender as NumericUpDown;
            int index;
            float value;
            float colorvalue;

            if (textBox != null)
            {
                index = (int)textBox.Tag;
                if(index < this.ArraySizeX * this.ArraySizeY)
                {
                    value = (float)textBox.Value;
                    if (this.UseCalibrationColorTransience)
                        colorvalue = this.ArrayCalib[index] / 255.0f;
                    else colorvalue = value;
                    if (this.Array[index] != value)
                    {
                        this.TextBox.BackColor = colorTransience.Get(colorvalue);
                        this.Array[index] = value;
                        this.RedrawTable();
                        UpdateTableEvent?.Invoke(sender, new ImageTableEventArg() { Index = index, Value = value });
                    }
                    
                }
            }
        }

        private void RemoveTextBox()
        {
            if(this.TextBox != null)
            {
                if(this.Controls.Contains(this.TextBox))
                {
                    this.Controls.Remove(this.TextBox);
                }
                this.TextBox.Dispose();
                this.TextBox = null;
            }
        }

        private void ImageTable_SizeChanged(object sender, EventArgs e)
        {
            this.RemoveTextBox();
            this.RedrawTable();
        }

        private void ImageTable_Load(object sender, EventArgs e)
        {
            this.RedrawTable();
        }

        private void ImageTable_VisibleChanged(object sender, EventArgs e)
        {
            this.RemoveTextBox();
            this.RedrawTable();
        }

        public void RedrawTable()
        {
            if (this.Initialized != true || this.Visible != true)
                return;

            ColorTransience colorTransience = this.UseCalibrationColorTransience ? this.CalibrationColorTransience : this.ColorTransience;
            Image oldimage = pictureBox.Image;
            Bitmap bitmap = new Bitmap(this.pictureBox.Width, this.pictureBox.Height);
            float pointX, pointY, sizeX, sizeY, sizeX2, sizeY2;
            SizeF captionX, captionY, captionXmax, captionYmax;
            captionXmax = new SizeF(0, 0);
            captionYmax = new SizeF(0, 0);

            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(this.BackColor);
            Pen forePen = new Pen(this.ForeColor);
            SolidBrush backBrush = new SolidBrush(this.BackColor);
            SolidBrush foreBrush = new SolidBrush(this.ForeColor);
            StringFormat strFormat = new StringFormat();
            StringFormat strFormatLeft;
            strFormat.Alignment = StringAlignment.Center;
            strFormat.LineAlignment = StringAlignment.Center;
            strFormat.FormatFlags |= StringFormatFlags.NoWrap;

            strFormatLeft = new StringFormat(strFormat);
            strFormatLeft.Alignment = StringAlignment.Near;

            if (this.CellRectangles == null)
            {
                this.CellRectangles = new RectangleF[this.ArraySizeX * this.ArraySizeY];
            }
            else if (this.CellRectangles.Length != this.ArraySizeX * this.ArraySizeY)
            {
                this.CellRectangles = new RectangleF[this.ArraySizeX * this.ArraySizeY];
            }

            if (this.TextSizes == null)
            {
                this.TextSizes = new SizeF[this.ArraySizeX * this.ArraySizeY];
            }
            else if (this.TextSizes.Length != this.ArraySizeX * this.ArraySizeY)
            {
                this.TextSizes = new SizeF[this.ArraySizeX * this.ArraySizeY];
            }

            float fontSizeX = bitmap.Width / (this.SizeX + 1) / 4.0F;
            float fontSizeY = bitmap.Height / (this.SizeY + 1) / 2.0F;

            Font font = new Font(this.Font.FontFamily, fontSizeX < fontSizeY ? fontSizeX : fontSizeY);
            //g.DrawRectangle(forePen, 0, 0, this.Width - 1, this.Height - 1);

            for (int x = 0; x < this.SizeX; x++)
            {
                captionX = g.MeasureString(this.ColumnTitles[x], font);
                if (captionX.Width >= captionXmax.Width && captionX.Height >= captionXmax.Height)
                    captionXmax = captionX;
            }

            for (int y = 0; y < this.SizeY; y++)
            {
                captionY = g.MeasureString(this.RowTitles[y], font);
                if (captionY.Width >= captionYmax.Width && captionY.Height >= captionYmax.Height)
                    captionYmax = captionY;
            }

            sizeX = (float)(bitmap.Width - captionYmax.Width) / (float)this.SizeX;
            sizeY = (float)(bitmap.Height - captionXmax.Height) / (float)this.SizeY;
            sizeX2 = sizeX * 0.5F;
            sizeY2 = sizeY * 0.5F;

            for (int x = 0; x < this.SizeX; x++)
            {
                pointX = captionYmax.Width + sizeX * x + sizeX2;
                pointY = captionXmax.Height * 0.5F;
                g.DrawString(this.ColumnTitles[x], font, foreBrush, pointX, pointY, strFormat);
            }

            for (int y = 0; y < this.SizeY; y++)
            {
                pointX = 0;
                pointY = captionXmax.Height + sizeY * y + sizeY2;
                g.DrawString(this.RowTitles[y], font, foreBrush, pointX, pointY, strFormatLeft);
            }


            double[] mult = new double[4];
            if (this.ValueInterpolationX != null && this.ValueInterpolationY != null)
            {

                for (int y = 0; y < 2; y++)
                {
                    for (int x = 0; x < 2; x++)
                    {
                        double value1, value2;
                        if (x == 0 && y == 0)
                        {
                            value1 = (1.0 - this.ValueInterpolationX.mult);
                            value2 = (1.0f - this.ValueInterpolationY.mult);
                        }
                        else if (x == 0 && y != 0)
                        {
                            value1 = (1.0 - this.ValueInterpolationX.mult);
                            value2 =  this.ValueInterpolationY.mult;
                        }
                        else if (x != 0 && y == 0)
                        {
                            value1 = this.ValueInterpolationX.mult;
                            value2 = (1.0f - this.ValueInterpolationY.mult);
                        }
                        else
                        {
                            value1 = this.ValueInterpolationX.mult;
                            value2 = this.ValueInterpolationY.mult;
                        }
                        if (value1 < 0)
                            value1 = 0;
                        else if (value1 > this.SizeX - 1)
                            value1 = this.SizeX - 1;
                        if (value2 < 0)
                            value2 = 0;
                        else if (value2 > this.SizeY - 1)
                            value2 = this.SizeY - 1;
                        mult[y * 2 + x] = value1 * value2;

                    }
                }

            }


            for (int y = -1; y < this.SizeY; y++)
            {
                for (int x = -1; x < this.SizeX; x++)
                {
                    sizeX2 = sizeX * 0.5F;
                    sizeY2 = sizeY * 0.5F;
                    pointX = sizeX * x;
                    pointY = sizeY * y;
                    pointX += captionYmax.Width;
                    pointY += captionXmax.Height;


                    if (x >= 0 && y >= 0)
                    {
                        int index = y * this.ArraySizeX + x;
                        float value = this.Array[index];
                        string text = value.ToString($"F{this.DecPlaces}");
                        float colorvalue = value;
                        if (this.UseCalibrationColorTransience)
                            colorvalue = this.ArrayCalib[index] / 255.0f;
                        backBrush.Color = colorTransience.Get(colorvalue);

                        if (font.Style != FontStyle.Regular)
                            font = new Font(font, FontStyle.Regular);

                        if (this.ValueInterpolationX != null && this.ValueInterpolationY != null)
                        {
                            for (int y1 = 0; y1 < 2; y1++)
                            {
                                for (int x1 = 0; x1 < 2; x1++)
                                {
                                    if (index == this.ValueInterpolationY.indexes[y1] * this.ArraySizeX + this.ValueInterpolationX.indexes[x1])
                                    {
                                        Color color = Color.DarkGray;
                                        int cr, cg, cb;

                                        if (mult[y1 * 2 + x1] > 1.0)
                                            mult[y1 * 2 + x1] = 1.0;
                                        else if (mult[y1 * 2 + x1] < 0.0)
                                            mult[y1 * 2 + x1] = 0.0;

                                        cr = (int)((color.R - backBrush.Color.R) * mult[y1 * 2 + x1] + backBrush.Color.R);
                                        cg = (int)((color.G - backBrush.Color.G) * mult[y1 * 2 + x1] + backBrush.Color.G);
                                        cb = (int)((color.B - backBrush.Color.B) * mult[y1 * 2 + x1] + backBrush.Color.B);

                                        backBrush.Color = Color.FromArgb(cr, cg, cb);

                                        if (mult[y1 * 2 + x1] == mult.Max() || mult[y1 * 2 + x1] > 0.35)
                                        {
                                            if (font.Style != FontStyle.Bold)
                                                font = new Font(font, FontStyle.Bold);
                                        }
                                    }
                                }
                            }
                        }

                        RectangleF rectangleF = new RectangleF(pointX + 2, pointY + 2, sizeX - 2, sizeY - 2);
                        this.CellRectangles[index] = rectangleF;
                        this.TextSizes[index] = g.MeasureString(text, this.Font);
                        g.FillRectangle(backBrush, rectangleF);
                        g.DrawString(text, font, foreBrush, rectangleF, strFormatLeft);

                        if(this.TextBox != null)
                        {
                            if(index == (int)this.TextBox.Tag)
                            {
                                this.TextBox.BackColor = backBrush.Color;
                            }
                        }
                    }
                }
            }

            strFormat.Dispose();
            strFormatLeft.Dispose();
            foreBrush.Dispose();
            backBrush.Dispose();
            forePen.Dispose();
            g.Dispose();

            pictureBox.Image = bitmap;

            if (oldimage != null)
                oldimage.Dispose();
        }
    }
}
