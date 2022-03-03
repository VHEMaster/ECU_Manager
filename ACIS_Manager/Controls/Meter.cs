using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ACIS_Manager.Controls
{
    /// <summary>
    /// Summary description for Meter.
    /// </summary>
    /// 
    // [ToolboxBitmap(typeof(Meter), "Images.meter.bmp")]
    public class Meter : Control
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;
        public delegate void UpdateNeedle();
        public event UpdateNeedle needleValChanged;
        #region Fields
        private Color faceColor;        //color of meter face
        private Color tickWarningColor; //color of the warning ticks
        private Color tickColor;        //color of the ticks
        private Color needleColor;      //color of the needle
        private Color numColor;         //color of the numbers
        private float textVal;        //value of where the needle points
        private float needleVal;        //value of where the needle points
        private float dynNumSize;       //size  of numbers displayed
        private float custNumSize;      //custom size of numbers
        private float valueRange;       //total number of values possible
                                        //					from 0 to x
        private bool useCustNumSize;        //turns on/off custom size of num

        private float degRange, degPercent;


        //note: 0 degrees is straight down
        private float minDeg;           //the degree of the lowest 
                                        //tickMark
        private float maxDeg;           //the degree of the highest 
                                        //tickMark
        private float tickIncrement;    //number of values between 
                                        //each tick
        private float warnTickStart;    //the first degree to 
                                        //include the warning area 
        #endregion


        public Meter()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            faceColor = Color.Black;
            needleColor = Color.Yellow;
            tickColor = Color.White;
            numColor = Color.White;
            tickWarningColor = tickColor;
            minDeg = 50;
            maxDeg = 310;
            tickIncrement = 10;
            warnTickStart = 270;
            needleVal = 0;
            textVal = 0;
            custNumSize = 15;
            dynNumSize = 1;
            useCustNumSize = false;
            degRange = maxDeg - minDeg;
            valueRange = degRange;
            degPercent = 1 / (valueRange / degRange);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.needleValChanged += new UpdateNeedle(WhenNeedleValChanged);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }
        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion


        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics displayGraphics = pe.Graphics;
            Image img = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            Graphics g = Graphics.FromImage(img);

            RectangleF textRect = new RectangleF(0, 0, 6.5f, 1.5f);


            //outer Ellipse
            //new path with ellipse
            GraphicsPath meterPath = new GraphicsPath();
            meterPath.AddEllipse(ClientRectangle);
            this.Region = new Region(meterPath);

            //fill path
            using (Brush meterBrush = new SolidBrush(this.faceColor))
            {
                g.FillRegion(meterBrush, this.Region);
            }

            //Rotation of tick marks
            int drawTo;
            int halfWidth = this.ClientRectangle.Width / 2;
            int halfHeight = this.ClientRectangle.Height / 2;

            //make sure tick marks are drawn long enough
            if (this.Width > this.Height)
                drawTo = this.Width;
            else
                drawTo = this.Height;

            for (float i = minDeg; i < maxDeg; i += tickIncrement * degPercent)
            {
                Matrix matrix = new Matrix();
                matrix.RotateAt(i, new PointF(halfWidth, halfHeight));
                g.Transform = matrix;

                if (i >= ((warnTickStart / valueRange) * degRange) + minDeg)
                    g.DrawLine(new Pen(tickWarningColor, 2), halfWidth, halfHeight, halfWidth, drawTo);
                else
                    g.DrawLine(new Pen(tickColor, 2), halfWidth, halfHeight, halfWidth, drawTo);
            }
            //makes sure the last tick is marked
            Matrix lastTickMtx = new Matrix();
            lastTickMtx.RotateAt(maxDeg, new PointF(halfWidth, halfHeight));
            g.Transform = lastTickMtx;
            g.DrawLine(new Pen(tickWarningColor, 2), halfWidth, halfHeight, halfWidth, drawTo);




            //inner ellipse   
            int upperX = (int)(this.ClientRectangle.Width * .05);
            int upperY = (int)(this.ClientRectangle.Height * .05);
            int rectWidth = (int)(this.ClientRectangle.Width * .90);
            int rectHeight = (int)(this.ClientRectangle.Height * .90);
            Rectangle innerRect = new Rectangle(upperX, upperY
                , rectWidth, rectHeight);
            using (Brush faceBrush = new SolidBrush(this.faceColor))
            {
                //get g oriented correctly
                Matrix inEllipseMtx = new Matrix();
                inEllipseMtx.RotateAt(0, new PointF(halfWidth, halfHeight));
                g.Transform = inEllipseMtx;
                //fill ellipse
                g.FillEllipse(faceBrush, innerRect);
            }


            //clean up
            this.PaintNumber(ref img);
            this.PaintNeedle(ref img);
            base.OnPaint(pe);
            g.Dispose();
            displayGraphics.DrawImage(img, ClientRectangle);
            img.Dispose();
        }


        //Paint the needle
        protected void PaintNeedle(ref Image i)
        {
            Graphics needleGraph = Graphics.FromImage(i);

            //Clip outer needle area
            int upperX = (int)(this.ClientRectangle.Width * .10);
            int upperY = (int)(this.ClientRectangle.Height * .10);
            int rectWidth = (int)(this.ClientRectangle.Width * .80);
            int rectHeight = (int)(this.ClientRectangle.Height * .80);
            Rectangle needleRect = new Rectangle(upperX, upperY
                , rectWidth, rectHeight);

            using (GraphicsPath needlePath = new GraphicsPath())
            {
                needlePath.AddEllipse(needleRect);
                using (Region needleRegion = new Region(needlePath))
                {
                    needleGraph.DrawPath(Pens.Transparent, needlePath);
                    needleGraph.Clip = needleRegion;
                }
            }

            //draw needle
            int drawTo;
            int halfWidth = this.ClientRectangle.Width / 2;
            int halfHeight = this.ClientRectangle.Height / 2;
            {
                if (this.ClientRectangle.Width > this.ClientRectangle.Height)
                    drawTo = Width;
                else
                    drawTo = Height;
            }
            //draw
            Matrix matrix = new Matrix();
            matrix.RotateAt((needleVal * degPercent) + minDeg, new PointF(halfWidth, halfHeight));
            needleGraph.Transform = matrix;
            needleGraph.DrawLine(new Pen(needleColor, 2), halfWidth, halfHeight, halfWidth, drawTo);

            //clean up
            needleGraph.Dispose();

        }

        //Paint the number
        protected void PaintNumber(ref Image i)
        {

            //capture a percentage of the width of ClientRectangle
            //to use with the dynamicNumSize
            if (this.ClientRectangle.Width < this.ClientRectangle.Height)
                this.dynNumSize = this.ClientRectangle.Width * 15 / 100;
            else
                this.dynNumSize = this.ClientRectangle.Height * 15 / 100;


            Graphics numGraph = Graphics.FromImage(i);
            Brush numBrush = new SolidBrush(this.numColor);

            //center numbers in Rectangle
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            //make Font
            float useSize;
            if (useCustNumSize == true)
            {
                useSize = this.custNumSize;
            }
            else
            {
                useSize = this.dynNumSize;
            }

            Font numFont = new Font("Serpentine-Medium", useSize, GraphicsUnit.Pixel);

            //draw Number Value 
            string strVal = textVal >= 150.0f ? textVal.ToString("F0") : textVal.ToString("F1");
            numGraph.DrawString(strVal, numFont, numBrush,
                new Rectangle(this.ClientRectangle.Width * 30 / 100,
                this.ClientRectangle.Height * 65 / 100,
                this.ClientRectangle.Width * 40 / 100,
                this.ClientRectangle.Height * 20 / 100),
                sf);

            //clean up
            numGraph.Dispose();
            numFont.Dispose();
            numBrush.Dispose();
        }

        //Event Handler for the needleValChanged event
        protected void WhenNeedleValChanged()
        {
            this.Invalidate();
            this.Update();
        }

        #region Properties
        //value of where the needle points to
        [Browsable(false)]
        public float NeedleVal
        {
            get
            { return needleVal; }
            set
            {
                textVal = value;
                if (value >= valueRange)
                    needleVal = valueRange;
                else if (value <= 0)
                    needleVal = 0;
                else
                    needleVal = value;
                needleValChanged();
            }
        }

        //sets color of meter face
        [Category("Appearance"),
        Description("sets color of meter face")]
        public Color FaceColor
        {
            get
            { return faceColor; }
            set
            {
                faceColor = value;
            }
        }

        //color of the ticks
        [Category("Appearance"),
        Description("color of the ticks")]
        public Color TickColor
        {
            get
            { return tickColor; }
            set
            {
                tickColor = value;
            }
        }

        //color of the warning ticks
        [Category("Appearance"),
        Description("color of the warning ticks")]
        public Color TickWarningColor
        {
            get
            { return tickWarningColor; }
            set
            {
                tickWarningColor = value;
            }
        }

        //color of the needle
        [Category("Appearance"),
        Description("color of the needle")]
        public Color NeedleColor
        {
            get
            { return needleColor; }
            set
            {
                needleColor = value;
            }
        }

        //color of the numbers
        [Category("Appearance"),
        Description("color of the numbers")]
        public Color NumColor
        {
            get
            { return numColor; }
            set
            {
                numColor = value;
            }
        }

        //the degree of the lowest tickMark
        [Category("Tick Mark Settings"),
        Description("the degree of the lowest tickMark")]
        public float MinDeg
        {
            get
            { return minDeg; }
            set
            {
                minDeg = value;
                degRange = maxDeg - minDeg;
            }
        }

        //the degree of the highest tickMark
        [Category("Tick Mark Settings"),
        Description("the degree of the highest tickMark")]
        public float MaxDeg
        {
            get
            { return maxDeg; }
            set
            {
                maxDeg = value;
                degRange = maxDeg - minDeg;
            }
        }

        //the value where the warning ticks begin
        [Category("Tick Mark Settings"),
        Description("the value where the warning ticks begin")]
        public float WarnTickStart
        {
            get
            { return warnTickStart; }
            set
            {
                warnTickStart = value;
            }
        }

        //total number of values possible from 0 to x
        [Category("Tick Mark Settings"),
        Description("total number of values possible from 0 to x")]
        public float ValueRange
        {
            get
            { return valueRange; }
            set
            {
                valueRange = value;
                degPercent = 1 / (valueRange / degRange);
            }
        }

        //number of values between each tick
        [Category("Tick Mark Settings"),
        Description("number of values between each tick")]
        public float TickIncrement
        {
            get
            { return tickIncrement; }
            set
            {
                tickIncrement = value;
            }
        }


        //whether to use a custom number size or not
        [Category("Appearance"),
        Description("Use a fixed custom number size, " +
            "or use a dynamic number size that stays proportional " +
            "to the size of the meter face?")]
        public bool UseCustNumSize
        {
            get
            { return useCustNumSize; }
            set
            {
                useCustNumSize = value;
            }
        }

        //custom size  of numbers displayed in pixels
        [Category("Appearance"),
        Description("fixed custom number size (pixels)," +
            "used only when the UseCustNumSize property is set to true")]
        public float CustNumSize
        {
            get
            { return custNumSize; }
            set
            {
                custNumSize = value;
            }
        }

        //dynamic size of  numbers displayed in pixels
        [Browsable(false)]
        public float DynNumSize
        {
            get
            { return dynNumSize; }
            set
            {
                dynNumSize = value;
            }
        }

        #region base class property overides

        //Remove the BackColor property from the properties window
        [Browsable(false)]
        public override Color BackColor
        { get { return base.BackColor; } }

        //Remove the ForeColor property from the properties window
        [Browsable(false)]
        public override Color ForeColor
        { get { return base.ForeColor; } }

        //Remove the BackgroundImage property from the properties window
        [Browsable(false)]
        public override Image BackgroundImage
        { get { return base.BackgroundImage; } }

        //Remove the Cursor property from the properties window
        [Browsable(false)]
        public override Cursor Cursor
        { get { return base.Cursor; } }

        //Remove the RightToLeft property from the properties window
        [Browsable(false)]
        public override RightToLeft RightToLeft
        { get { return base.RightToLeft; } }

        //Remove the Font property from the properties window
        [Browsable(false)]
        public override Font Font
        { get { return base.Font; } }

        //Remove the Text property from the properties window
        [Browsable(false)]
        public override String Text
        { get { return base.Text; } }

        #endregion


        #endregion
    }

}
