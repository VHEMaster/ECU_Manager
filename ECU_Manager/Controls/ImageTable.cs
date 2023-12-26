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
        public string[] ColumnTitles { get; set; }
        public string[] RowTitles { get; set; }

        public bool Initialized { get; set; } = false;
        public ColorTransience ColorTransience { get; set; }
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
                    UpdateTableEvent?.Invoke(sender, new ImageTableEventArg() { Index = index });

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

                this.TextBox = new NumericUpDownOneWheel();
                this.TextBox.Margin = new Padding(0);
                this.TextBox.Minimum = (decimal)this.ValueMin;
                this.TextBox.Maximum = (decimal)this.ValueMax;
                this.TextBox.DecimalPlaces = this.DecPlaces;
                this.TextBox.Increment = (decimal)this.Increment;
                this.TextBox.Tag = index;
                this.TextBox.Value = (decimal)value;
                this.TextBox.ValueChanged += textBox_ValueChanged;
                this.TextBox.Visible = true;
                this.TextBox.ForeColor = this.ForeColor;
                this.TextBox.BackColor = this.ColorTransience.Get(value);
                this.TextBox.Location = new Point((int)this.CellRectangles[index].X, (int)this.CellRectangles[index].Y);
                this.TextBox.Size = new Size((int)this.TextSizes[index].Height + 50, (int)this.CellRectangles[index].Height);
                this.Controls.Add(this.TextBox);

                this.TextBox.BringToFront();
            }
        }

        private void textBox_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown textBox = sender as NumericUpDown;
            int index;
            float value;

            if(textBox != null)
            {
                index = (int)textBox.Tag;
                if(index < this.ArraySizeX * this.ArraySizeY)
                {
                    value = (float)textBox.Value;
                    if (this.Array[index] != value)
                    {
                        this.TextBox.BackColor = this.ColorTransience.Get(value);
                        this.Array[index] = value;
                        this.RedrawTable();
                        UpdateTableEvent?.Invoke(sender, new ImageTableEventArg() { Index = index });
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
            if (this.Initialized != true)
                return;

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

            //g.DrawRectangle(forePen, 0, 0, this.Width - 1, this.Height - 1);

            for (int x = 0; x < this.SizeX; x++)
            {
                captionX = g.MeasureString(this.ColumnTitles[x], this.Font);
                if (captionX.Width >= captionXmax.Width && captionX.Height >= captionXmax.Height)
                    captionXmax = captionX;
            }

            for (int y = 0; y < this.SizeY; y++)
            {
                captionY = g.MeasureString(this.RowTitles[y], this.Font);
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
                g.DrawString(this.ColumnTitles[x], this.Font, foreBrush, pointX, pointY, strFormat);
            }

            for (int y = 0; y < this.SizeY; y++)
            {
                pointX = 0;
                pointY = captionXmax.Height + sizeY * y + sizeY2;
                g.DrawString(this.RowTitles[y], this.Font, foreBrush, pointX, pointY, strFormatLeft);
            }

            for (int y = -1; y < this.SizeY; y++)
            {
                for (int x = -1; x < this.SizeX; x++)
                {
                    sizeX = (float)(bitmap.Width - captionYmax.Width) / (float)this.SizeX;
                    sizeY = (float)(bitmap.Height - captionXmax.Height) / (float)this.SizeY;
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
                        backBrush.Color = this.ColorTransience.Get(value);
                        RectangleF rectangleF = new RectangleF(pointX + 2, pointY + 2, sizeX - 2, sizeY - 2);
                        this.CellRectangles[index] = rectangleF;
                        this.TextSizes[index] = g.MeasureString(text, this.Font);
                        g.FillRectangle(backBrush, rectangleF);
                        g.DrawString(text, this.Font, foreBrush, rectangleF, strFormatLeft);
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
