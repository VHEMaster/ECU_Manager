
/*****************************************************************************

This class has been written by Elmü (elmue@gmx.de)
It is based on code from Michał Bryłka but has been completely rewritten from the ground.
His code had several issues, bugs, misdesignes and a bad performance.

Check if you have the latest version on:
https://www.codeproject.com/Articles/5293980/Graph3D-A-Windows-Forms-Render-Control-in-Csharp

*****************************************************************************/

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using System.Diagnostics;

namespace ECU_Manager.Controls
{
    /// <summary>
    /// ATTENTION: This class is not thread safe.
    /// Call all functions only from the GUI thread or use Control.Invoke()
    /// </summary>
    public class Graph3D : UserControl
    {
        #region enums

        public enum eRaster
        {
            Off,      // draw no coordinate system
            MainAxis, // draw solid axis X,Y,Z
            Raster,   // draw axis and dotted raster lines
            Labels,   // draw axis and dotted raster lines and labels in quadrant 3
        }

        /// <summary>
        /// If a function has an asymetric range for X and Y as example "Callback" a separate normalization 
        /// would always lead to a square X,Y pane which would be a distortion for the relation between X and Y values.
        /// MaintainXY  guarantees that the relation between X and Y values is maintained.
        /// MaintainXYZ additionally guarantees that the relation between X, Y and Z values is maintained.
        /// </summary>
        public enum eNormalize
        {
            Separate,    // Normalize X,Y,Z separately (use this for discrete values)
            MaintainXY,  // Normalize X,Y   without changing their relation (use this for functions)
            MaintainXYZ, // Normalize X,Y,Z without changing their relation (use this for functions)
        }

        public enum eCoord
        {
            X = 0,
            Y = 1,
            Z = 2,
        }

        enum eMouseAction
        {
            None = 0,
            Move,
            Rho,
            Theta,
            Phi,
        }

        #endregion

        #region cMouse 

        class cMouse
        {
            public eMouseAction me_Action;     // left mouse button action
            public Point        mk_LastPos;    // last mouse location
            public Point        mk_Offset;     // Offset for painting in OnPaint()
            public TrackBar     mi_TrackRho;   // Rho trackbar (optional)
            public TrackBar     mi_TrackTheta; // Theta trackbar (optional)
            public TrackBar     mi_TrackPhi;   // Phi trackbar (optional)
            public double       md_Rho   = VALUES_RHO  [2]; // 2 = Default value
            public double       md_Theta = VALUES_THETA[2]; // 2 = Default value
            public double       md_Phi   = VALUES_PHI  [2]; // 2 = Default value

            public void AssignTrackbar(eMouseAction e_Trackbar, TrackBar i_Trackbar, EventHandler i_OnScroll)
            {
                if (i_Trackbar == null)
                    return;

                double[] d_Values = null;
                switch (e_Trackbar)
                {
                    case eMouseAction.Rho:
                        d_Values      = VALUES_RHO;
                        mi_TrackRho   = i_Trackbar;
                        break;
                    case eMouseAction.Theta:
                        d_Values      = VALUES_THETA;
                        mi_TrackTheta = i_Trackbar;
                        break;
                    case eMouseAction.Phi:
                        d_Values      = VALUES_PHI;
                        mi_TrackPhi   = i_Trackbar;
                        break;
                }

                i_Trackbar.Minimum = (int)d_Values[0]; // 0 = Minimum
                i_Trackbar.Maximum = (int)d_Values[1]; // 1 = Maximum
                i_Trackbar.Value   = (int)d_Values[2]; // 2 = Default value
                i_Trackbar.Scroll += i_OnScroll;
            }

            /// <summary>
            /// User has moved the TrackBar
            /// </summary>
            public void OnTrackBarScroll()
            {
                if (mi_TrackRho   != null) md_Rho   = mi_TrackRho  .Value;
                if (mi_TrackTheta != null) md_Theta = mi_TrackTheta.Value;
                if (mi_TrackPhi   != null) md_Phi   = mi_TrackPhi  .Value;
            }

            public bool OnMouseWheel(int s32_Delta)
            {
                if (me_Action != eMouseAction.None)
                    return false;

                me_Action = eMouseAction.Rho;
                OnMouseMove(0, s32_Delta / 10);
                me_Action = eMouseAction.None;
                return true;
            }

            /// <summary>
            /// User has dragged the mouse over the 3D control
            /// </summary>
            public void OnMouseMove(int s32_DiffX, int s32_DiffY)
            {
                switch (me_Action)
                {
                    case eMouseAction.Rho:
                        md_Rho += s32_DiffY * VALUES_RHO[3];  // 3 = Factor
                        SetRho(md_Rho);
                        break;

                    case eMouseAction.Theta:
                        md_Theta -= s32_DiffY * VALUES_THETA[3];  // 3 = Factor
                        SetTheta(md_Theta);
                        break;

                    case eMouseAction.Phi:
                        md_Phi -= s32_DiffX * VALUES_PHI[3]; // 3 = Factor
                        SetPhi(md_Phi);
                        break;
                }
            }

            public void SetRho(double d_Rho)
            {
                md_Rho = d_Rho;
                md_Rho = Math.Max(md_Rho, VALUES_RHO[0]); // 0 = Minimum
                md_Rho = Math.Min(md_Rho, VALUES_RHO[1]); // 1 = Maximum
                if (mi_TrackRho != null)
                    mi_TrackRho.Value = (int)md_Rho;
            }
            public void SetTheta(double d_Theta)
            {
                md_Theta = d_Theta;
                md_Theta = Math.Max(md_Theta, VALUES_THETA[0]); // 0 = Minimum
                md_Theta = Math.Min(md_Theta, VALUES_THETA[1]); // 1 = Maximum
                if (mi_TrackTheta != null)
                    mi_TrackTheta.Value = (int)md_Theta;
            }
            public void SetPhi(double d_Phi)
            {
                md_Phi = d_Phi;
                if (md_Phi > 360.0) md_Phi -= 360.0; // continuous rotation
                if (md_Phi <   0.0) md_Phi += 360.0; // continuous rotation
                if (mi_TrackPhi != null)
                    mi_TrackPhi.Value = (int)md_Phi;
            }
        }

        #endregion

        #region cPoint3D

        public class cPoint3D
        {
            public double md_X;
            public double md_Y;
            public double md_Z;

            public cPoint3D()
            {
            }

            public cPoint3D(double X, double Y, double Z)
            {
                md_X = X;
                md_Y = Y;
                md_Z = Z;
            }

            public cPoint3D Clone()
            {
                return new cPoint3D(md_X, md_Y, md_Z);
            }

            public bool Equals(cPoint3D i_Point)
            {
                return md_X == i_Point.md_X && md_Y == i_Point.md_Y && md_Z == i_Point.md_Z;
            }

            public double GetValue(eCoord e_Coord)
            {
                switch (e_Coord)
                {
                    case eCoord.X: return md_X;
                    case eCoord.Y: return md_Y;
                    case eCoord.Z: return md_Z;
                    default:       return 0;
                }
            }
            public void SetValue(eCoord e_Coord, double d_Value)
            {
                switch (e_Coord)
                {
                    case eCoord.X: md_X = d_Value; break;
                    case eCoord.Y: md_Y = d_Value; break;
                    case eCoord.Z: md_Z = d_Value; break;
                }
            }

            // For debugging in Visual Studio
            public override string ToString()
            {
                return String.Format("{0:0.00}, {1:0.00}, {2:0.00}", md_X, md_Y, md_Z);
            }
        }

        #endregion

        #region cPoint2D

        public class cPoint2D
        {
            public double md_X;
            public double md_Y;

            public PointF Coord
            {
                get { return new PointF((float)md_X, (float)md_Y); }
            }

            // For debugging in Visual Studio
            public override string ToString()
            {
                return String.Format("{0:0.00}, {1:0.00}", md_X, md_Y);
            }

            public bool IsValid
            {
                get
                {
                    // The screen will always be smaller than 9999 pixels
                    return (!Double.IsNaN(md_X) && Math.Abs(md_X) < 9999.9 &&
                            !Double.IsNaN(md_Y) && Math.Abs(md_Y) < 9999.9);
                }
            }
        }

        #endregion

        #region cMinMax3D

        public class cMinMax3D
        {
            public double md_MinX = double.PositiveInfinity;
            public double md_MaxX = double.NegativeInfinity;
            public double md_MinY = double.PositiveInfinity;
            public double md_MaxY = double.NegativeInfinity;
            public double md_MinZ = double.PositiveInfinity;
            public double md_MaxZ = double.NegativeInfinity;

            // Center point is subtracted from 3D values before converting to 2D
            public cPoint3D mi_Center3D = new cPoint3D();

            // Constructor
            public cMinMax3D(double d_MinX, double d_MaxX, double d_MinY, double d_MaxY, double d_MinZ, double d_MaxZ)
            {
                this.md_MinX = d_MinX;
                this.md_MaxX = d_MaxX;
                this.md_MinY = d_MinY;
                this.md_MaxY = d_MaxY;
                this.md_MinZ = d_MinZ;
                this.md_MaxZ = d_MaxZ;
            }

            // Constructor
            public cMinMax3D(cPoint3D[,] i_Points3D)
            {
                for (int X = 0; X < i_Points3D.GetLength(0); X++)
                {
                    for (int Y = 0; Y < i_Points3D.GetLength(1); Y++)
                    {
                        cPoint3D i_Point3D = i_Points3D[X, Y];

                        md_MinX = Math.Min(md_MinX, i_Point3D.md_X);
                        md_MaxX = Math.Max(md_MaxX, i_Point3D.md_X);
                        md_MinY = Math.Min(md_MinY, i_Point3D.md_Y);
                        md_MaxY = Math.Max(md_MaxY, i_Point3D.md_Y);
                        md_MinZ = Math.Min(md_MinZ, i_Point3D.md_Z);
                        md_MaxZ = Math.Max(md_MaxZ, i_Point3D.md_Z);
                    }
                }
            }

            // Constructor
            public cMinMax3D(cScatter[] i_ScatterArr)
            {
                foreach (cScatter i_Scatter in i_ScatterArr)
                {
                    cPoint3D i_Point3D = i_Scatter.mi_Point3D;

                    md_MinX = Math.Min(md_MinX, i_Point3D.md_X);
                    md_MaxX = Math.Max(md_MaxX, i_Point3D.md_X);
                    md_MinY = Math.Min(md_MinY, i_Point3D.md_Y);
                    md_MaxY = Math.Max(md_MaxY, i_Point3D.md_Y);
                    md_MinZ = Math.Min(md_MinZ, i_Point3D.md_Z);
                    md_MaxZ = Math.Max(md_MaxZ, i_Point3D.md_Z);
                }
            }
        }

        #endregion

        #region cLine

        private class cLine
        {
            public eCoord     me_Line;    // main coordinate
            public eCoord     me_Offset;  // secondary coordinate
            public double     md_Label;   // Label for axis
            public Pen        mi_Pen;
            public double     md_Sort;
            public double     md_Angle;
            public cPoint3D[] mi_Points3D = new cPoint3D[2] { new cPoint3D(), new cPoint3D() }; // start and end point of line
            public cPoint2D[] mi_Points2D = new cPoint2D[2] { new cPoint2D(), new cPoint2D() };

            public bool CoordEquals(cLine i_Line)
            {
                return mi_Points3D[0].Equals(i_Line.mi_Points3D[0]) &&
                       mi_Points3D[1].Equals(i_Line.mi_Points3D[1]);
            }

            // Graphics.DrawLine() with huge length may take several seconds and block the GUI thread --> hanging.
            public bool IsValid
            {
                get { return mi_Points2D[0].IsValid && mi_Points2D[1].IsValid; }
            }

            /// <summary>
            /// Calculate the angle of the 3 main axis on the screen in a range from 0 to 360.
            /// </summary>
            public void CalcAngle2D()
            {
                double d_DX = mi_Points2D[1].md_X - mi_Points2D[0].md_X;
                double d_DY = mi_Points2D[1].md_Y - mi_Points2D[0].md_Y;
                md_Angle = Math.Atan2(d_DY, d_DX) * 180.0 / Math.PI;
                if (md_Angle < 0.0) md_Angle += 360.0;
            }
        }

        #endregion

        #region cPoygon

        private class cPolygon
        {
            public PointF[] mk_Points;  // 4 polygon points
            public double   md_FactorZ; // used to determine scheme color
            private bool    mb_Valid;

            public cPolygon(params cPoint2D[] i_Points2D)
            {
                mb_Valid  = true;
                mk_Points = new PointF[i_Points2D.Length];

                for (int i=0; i<i_Points2D.Length; i++)
                {
                    if (i_Points2D[i].IsValid) mk_Points[i] = i_Points2D[i].Coord;
                    else                       mb_Valid = false;
                }
            }

            // Do not draw ploygons which are invalid
            public bool IsValid
            {
                get { return mb_Valid; }
            }
        }

        #endregion

        #region cScatter

        public class cScatter
        {
            public  cPoint3D mi_Point3D;
            public  PointF   mk_Point;   // upper left corner
            public  Brush    mi_Brush;
            public  Pen      mi_Pen;
            public  double   md_FactorZ; // used to determine scheme color if mi_Brush == null
            public  cScatter mi_Previous;
            public  bool     mb_Combine; // draw line from previous to this
            private bool     mb_Valid;

            /// <summary>
            /// i_Brush == null --> use color from ColorScheme
            /// i_Brush != null --> use i_Brush
            /// </summary>
            public cScatter(double X, double Y, double Z, Brush i_Brush)
            {
                mi_Point3D = new cPoint3D(X, Y, Z);
                mi_Brush   = i_Brush;
            }

            public void SetPoint2D(cPoint2D i_Point2D)
            {
                mk_Point = i_Point2D.Coord;
                mb_Valid = i_Point2D.IsValid;

                // Move from center to upper left corner of circle
                mk_Point.X -= SCATTER_SIZE;
                mk_Point.Y -= SCATTER_SIZE;
            }

            public bool IsValid
            {
                get { return mb_Valid; }
            }
        }

        #endregion

        #region cDrawObj

        private class cDrawObj
        {
            public cPolygon mi_Polygon;
            public cScatter mi_Scatter;
            public cLine    mi_Line;
            public double   md_Sort;    // sorting is important. Always draw from back to front.
            private bool    mb_Valid;

            public cDrawObj(cPolygon i_Polygon, double d_Sort)
            {
                mi_Polygon = i_Polygon;
                mb_Valid   = i_Polygon.IsValid;
                md_Sort    = d_Sort;
            }
            public cDrawObj(cScatter i_Scatter, double d_Sort)
            {
                mi_Scatter = i_Scatter;
                mb_Valid   = i_Scatter.IsValid;
                md_Sort    = d_Sort;
            }
            public cDrawObj(cLine i_Line, double d_Sort)
            {
                mi_Line    = i_Line;
                mb_Valid   = i_Line.IsValid;
                md_Sort    = d_Sort;
            }

            public bool IsValid
            {
                get { return mb_Valid; }
            }
        }

        #endregion

        #region cQuadrant

        private class cQuadrant
        {
            public double md_SortXY;   // Sort order of raster in area XY  (red)
            public double md_SortXZ;   // Sort order of X axis and raster in area XZ (blue)
            public double md_SortYZ;   // Sort order of Y axis and raster in area YZ (green)
            public int    ms32_Quadrant;
            public bool   mb_BottomView;

            public cQuadrant(double d_Phi, cLine i_AxisX, cLine i_AxisY, cLine i_AxisZ)
            {
                // Split rotation into 4 sections (0...3) which increment every 90° starting at 45°
                int s32_Section45 = (int)d_Phi + 45;
                if (s32_Section45 > 360) s32_Section45 -= 360;
                s32_Section45 = Math.Min(3, s32_Section45 / 90);

                // Theta elevation lets the camera watch the graph from the top or bottom
                switch (s32_Section45)
                {
                    case 0: mb_BottomView = i_AxisX.md_Angle < 180.0; break;
                    case 1: mb_BottomView = i_AxisY.md_Angle < 180.0; break;
                    case 2: mb_BottomView = i_AxisX.md_Angle > 180.0; break;
                    case 3: mb_BottomView = i_AxisY.md_Angle > 180.0; break;
                }

                // The quadrant changes when the 2D transformed Z axis is in line with the X or Y axis
                if (mb_BottomView)
                {
                    switch (s32_Section45)
                    {
                        case 0: ms32_Quadrant = i_AxisX.md_Angle + 180.0 < i_AxisZ.md_Angle ? 1 : 0; break;
                        case 1: ms32_Quadrant = i_AxisY.md_Angle + 180.0 < i_AxisZ.md_Angle ? 2 : 1; break;
                        case 2: ms32_Quadrant = i_AxisX.md_Angle         < i_AxisZ.md_Angle ? 3 : 2; break;
                        case 3: ms32_Quadrant = i_AxisY.md_Angle         < i_AxisZ.md_Angle ? 0 : 3; break;
                    }
                }
                else // Top View
                {
                    switch (s32_Section45)
                    {
                        case 0: ms32_Quadrant = i_AxisX.md_Angle         > i_AxisZ.md_Angle ? 1 : 0; break;
                        case 1: ms32_Quadrant = i_AxisY.md_Angle         > i_AxisZ.md_Angle ? 2 : 1; break;
                        case 2: ms32_Quadrant = i_AxisX.md_Angle + 180.0 > i_AxisZ.md_Angle ? 3 : 2; break;
                        case 3: ms32_Quadrant = i_AxisY.md_Angle + 180.0 > i_AxisZ.md_Angle ? 0 : 3; break;
                    }
                }

                md_SortXY = (mb_BottomView) ? 99999.9 : -99999.9;
                md_SortXZ = (ms32_Quadrant == 1 || ms32_Quadrant == 2) ? 99999.9 : -99999.9;
                md_SortYZ = (ms32_Quadrant == 0 || ms32_Quadrant == 1) ? 99999.9 : -99999.9;

                i_AxisX.md_Sort = md_SortXZ;
                i_AxisY.md_Sort = md_SortYZ;
                i_AxisZ.md_Sort = 0.0;

                // Debug.WriteLine(String.Format("Section: {0}  Quadrant: {1}", s32_Section45, ms32_Quadrant));
            }
        }

        #endregion

        #region cTransform

        private class cTransform
        {
            private double md_Dist; // Screen Distance
            private double md_sf;
            private double md_st;
            private double md_cf;
            private double md_ct;
            private double md_Rho;
            // ----------------
            private double md_FactX;
            private double md_OffsX;
            private double md_FactY;
            private double md_OffsY;
            // ----------------
            public double  md_NormalizeX;
            public double  md_NormalizeY;
            public double  md_NormalizeZ;

            public void SetCoeficients(cMouse i_Mouse)
            {
                md_Rho         =  i_Mouse.md_Rho;                           // Distance of viewer (zoom)
                double d_Theta =  i_Mouse.md_Theta       * Math.PI / 180.0; // Height   of viewer (elevation)
                double d_Phi   = (i_Mouse.md_Phi -180.0) * Math.PI / 180.0; // Rotation around center (-pi ... +pi)

                md_sf   = Math.Sin(d_Phi);
                md_cf   = Math.Cos(d_Phi);
                md_st   = Math.Sin(d_Theta);
                md_ct   = Math.Cos(d_Theta);
                md_Dist = 0.5; // Camera distance. Smaller values result in ugly stretched egdes when rotating.
            }

            public void SetSize(Size k_Size) // Control.ClientSize
            {
                double d_Width  = k_Size.Width  * 0.0254 / 96.0; // 0.0254 m = 1 inch. Screen has 96 DPI
                double d_Height = k_Size.Height * 0.0254 / 96.0;

                // linear transformation coeficients
                md_FactX =  k_Size.Width  / d_Width;
                md_FactY = -k_Size.Height / d_Height;
                
                md_OffsX =  md_FactX * d_Width  / 2.0;
                md_OffsY = -md_FactY * d_Height / 2.0;
            }

            // Performs projection. Calculates screen coordinates for 3D point.
            // returns Point in 2D space of the screen.
            public cPoint2D Project(cPoint3D i_Point3D, cPoint3D i_Center3D)
            {
                double X = (i_Point3D.md_X - i_Center3D.md_X) * md_NormalizeX;
                double Y = (i_Point3D.md_Y - i_Center3D.md_Y) * md_NormalizeY;
                double Z = (i_Point3D.md_Z - i_Center3D.md_Z) * md_NormalizeZ;

                // 3D coordinates with center point in the middle of the screen
                // X positive to the right, X negative to the left
                // Y positive to the top,   Y negative to the bottom
                double xn = -md_sf *         X + md_cf         * Y;
                double yn = -md_cf * md_ct * X - md_sf * md_ct * Y + md_st * Z;
                double zn = -md_cf * md_st * X - md_sf * md_st * Y - md_ct * Z + md_Rho;

                if (zn <= 0) zn = 0.01;

                // Thales' theorem
                cPoint2D i_Point2D = new cPoint2D();
                i_Point2D.md_X = md_FactX * xn * md_Dist / zn + md_OffsX;
                i_Point2D.md_Y = md_FactY * yn * md_Dist / zn + md_OffsY;
                return i_Point2D;
            }

            // Required for correct painting order of polygons (always from back to front)
            public double ProjectXY(double X, double Y)
            {
                return X * md_cf + Y * md_sf;
            }
        }

        #endregion

        // Limits and default values for mouse actions and trackbars.
        // ATTENTION: It is strongly recommended not to change the MIN, MAX values.
        // The mouse factor defines how much mouse movement you need for a change.
        // A movement of mouse by approx 1000 pixels on the screen results in getting from Min to Max or vice versa.
        //
        //                                                      MIN     MAX   DEFAULT  MOUSE FACTOR
        static readonly double[] VALUES_RHO   = new double[] {  300,   3600,  1800,    2    };
        static readonly double[] VALUES_THETA = new double[] {   10,    170,    70,    0.25 }; // degree
        static readonly double[] VALUES_PHI   = new double[] {    0,    360,   230,    0.4  }; // degree  (continuous rotation)

        // The axis are 10% longer than the highest X,Y,Z value
        const double AXIS_EXCESS = 1.0;

        // For any strange reason the graph is not centerd vertically
        const int VERT_OFFSET = -30;

        // The radius of the scatter circles
        const int SCATTER_SIZE = 3;

        // Calculate 3-dimensional Z value from X,Y values
        public delegate double delRendererFunction(double X, double Y);

        eRaster        me_Raster         = eRaster.Off;
        Pen[]          mi_AxisPens       = new Pen[3];
        Pen[]          mi_RasterPens     = new Pen[3];
        cTransform     mi_Transform      = new cTransform();
        List<cDrawObj> mi_DrawObjects    = new List<cDrawObj>(); // cPolygon or cLine or cScatter
        cMouse         mi_Mouse          = new cMouse();
        Point          mk_Offset2D       = new Point();
        String[]       ms_AxisLegends    = new String[3]; 
        SolidBrush[]   mi_AxisBrushes    = new SolidBrush[3];
        Pen            mi_PolyLinePen;
        Pen            mi_BorderPen;
        SolidBrush     mi_TopLegendBrush;
        SolidBrush[]   mi_SchemeBrushes;
        Pen[]          mi_SchemePens;
        cPoint3D[,]    mi_PolyArr;
        cScatter[]     mi_ScatterArr;
        cMinMax3D      mi_MinMax;
        cQuadrant      mi_Quadrant;
        int            ms32_Points;

        /// <summary>
        /// Off:      draw no coordinate system
        /// MainAxis: draw solid axis X,Y,Z
        /// Raster:   draw axis and dotted raster lines
        /// Labels:   draw axis and dotted raster lines and labels in quadrant 3
        /// </summary>
        public eRaster Raster
        {
            set
            {
                Debug.Assert(!InvokeRequired); // Do not call from other threads or use Invoke()
                if (me_Raster != value)
                {
                    me_Raster = value;
                    mi_DrawObjects.Clear(); // recalculate
                    Invalidate();           // repaint
                }
            }
            get
            {
                return me_Raster;
            }
        }

        /// <summary>
        /// setting PolygonLineColor = Color.Black draws black lines between the polygons
        /// setting PolygonLineColor = Color.Empty turns off lines between the polygons
        /// </summary>
        public Color PolygonLineColor
        {
            set
            {
                Debug.Assert(!InvokeRequired); // Do not call from other threads or use Invoke()
                if (value.A == 0) mi_PolyLinePen = null; // transparent color
                else              mi_PolyLinePen = new Pen(value);
                Invalidate(); // repaint only
            }
            get
            {
                if (mi_PolyLinePen != null) return mi_PolyLinePen.Color;
                else                        return Color.Empty;
            }
        }

        /// <summary>
        /// setting BorderColor = Color.Gray draws a one pixel gray border around the control
        /// setting BorderColor = Color.Empty turns off the border
        /// </summary>
        public Color BorderColor
        {
            set
            {
                Debug.Assert(!InvokeRequired); // Do not call from other threads or use Invoke()
                if (value.A == 0) mi_BorderPen = null; // transparent color
                else              mi_BorderPen = new Pen(value);
                Invalidate(); // repaint only
            }
            get
            {
                if (mi_BorderPen != null) return mi_BorderPen.Color;
                else                      return Color.Empty;
            }
        }

        /// <summary>
        /// Show a legend with Rotation, Elevation and Distance at the top left
        /// setting LegendColor = Color.Gray draws legend text in gray
        /// setting LegendColor = Color.Empty turns off the top legend
        /// </summary>
        public Color TopLegendColor
        {
            set
            {
                Debug.Assert(!InvokeRequired); // Do not call from other threads or use Invoke()
                mi_TopLegendBrush = new SolidBrush(value);
                Invalidate(); // repaint only
            }
            get
            {
                if (mi_TopLegendBrush != null) return mi_TopLegendBrush.Color;
                else                           return Color.Empty;
            }
        }

        /// <summary>
        /// Show a legend for each axis at the bottom left
        /// Set string = null to turn axis legend off
        /// </summary>
        public String AxisX_Legend
        {
            set { ms_AxisLegends[(int)eCoord.X] = value; Invalidate();}
            get { return ms_AxisLegends[(int)eCoord.X]; }
        }
        public String AxisY_Legend
        {
            set { ms_AxisLegends[(int)eCoord.Y] = value; Invalidate();}
            get { return ms_AxisLegends[(int)eCoord.Y]; }
        }
        public String AxisZ_Legend
        {
            set { ms_AxisLegends[(int)eCoord.Z] = value; Invalidate();}
            get { return ms_AxisLegends[(int)eCoord.Z]; }
        }

        /// <summary>
        /// Set the colors of axis, raster lines and lables
        /// </summary>
        public Color AxisX_Color
        {
            set { SetAxisColor(eCoord.X, value); Invalidate(); }
            get { return mi_AxisPens[(int)eCoord.X].Color; }
        }
        public Color AxisY_Color
        {
            set { SetAxisColor(eCoord.Y, value); Invalidate(); }
            get { return mi_AxisPens[(int)eCoord.Y].Color; }
        }
        public Color AxisZ_Color
        {
            set { SetAxisColor(eCoord.Z, value); Invalidate(); }
            get { return mi_AxisPens[(int)eCoord.Z].Color; }
        }

        /// <summary>
        /// Here you can set any amount of colors which will be used to paint to the polygons and scatter points.
        /// f_LineWidth defines the width of the scatter lines.
        /// </summary>
        public void SetColorScheme(Color[] c_Colors, float f_LineWidth)
        {
            Debug.Assert(!InvokeRequired); // Do not call from other threads or use Invoke()
            mi_SchemeBrushes = new SolidBrush[c_Colors.Length];
            mi_SchemePens    = new Pen       [c_Colors.Length];
            for (int i = 0; i < mi_SchemeBrushes.Length; i++)
            {
                mi_SchemeBrushes[i] = new SolidBrush(c_Colors[i]);
                mi_SchemePens   [i] = new Pen(mi_SchemeBrushes[i], f_LineWidth);
            }
            Invalidate(); // repaint only
        }

        /// <summary>
        /// returns the total count of points currently loaded
        /// </summary>
        [ReadOnly(true)]
        [Browsable(false)]
        public int TotalPoints
        {
            get { return ms32_Points; }
        }

        /// <summary>
        /// Trackbars are optional for user interaction.
        /// If this function is never called thetrackbars are not used.
        /// </summary>
        public void AssignTrackBars(TrackBar i_Rho, TrackBar i_Theta, TrackBar i_Phi)
        {
            Debug.Assert(!InvokeRequired); // Do not call from other threads or use Invoke()
            mi_Mouse.AssignTrackbar(eMouseAction.Rho,   i_Rho,   new EventHandler(OnTrackbarScroll));
            mi_Mouse.AssignTrackbar(eMouseAction.Theta, i_Theta, new EventHandler(OnTrackbarScroll));
            mi_Mouse.AssignTrackbar(eMouseAction.Phi,   i_Phi,   new EventHandler(OnTrackbarScroll));
        }

        // ============================================================================

        // Constructor
        public Graph3D()
        {
            // avoid flicker
            SetStyle(ControlStyles.AllPaintingInWmPaint,  true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            SetAxisColor(eCoord.X, Color.Red);
            SetAxisColor(eCoord.Y, Color.Green);
            SetAxisColor(eCoord.Z, Color.Blue);

            mi_PolyLinePen    = new Pen       (Color.Black, 1);
            mi_BorderPen      = new Pen       (Color.FromArgb(255,180,180,180), 1); // bright gray
            mi_TopLegendBrush = new SolidBrush(Color.FromArgb(255,200,200,150));    // beige

            mi_Transform.SetCoeficients(mi_Mouse);
        }

        // ============================================================================
   
        /// <summary>
        /// Here you can set a callback function delegate which will be called to calculate the 3D values.
        /// Use either SetFunction() or SetSurfacePoints() or SetScatterPoints() or SetScatterLines()
        /// </summary>
        public void SetFunction(delRendererFunction f_Function, PointF k_Start, PointF k_End, double d_Density, eNormalize e_Normalize)
        {
            Debug.Assert(!InvokeRequired); // Do not call from other threads or use Invoke()

            int s32_CountX = (int)((k_End.X - k_Start.X) / d_Density + 1);
            int s32_CountY = (int)((k_End.Y - k_Start.Y) / d_Density + 1);

            cPoint3D[,] i_Points3D = new cPoint3D[s32_CountX, s32_CountY];

            for (int C = 0; C < s32_CountX; C++)
            {
                double X = k_Start.X + d_Density * C;

                for (int R = 0; R < s32_CountY; R++)
                {
                    double Y = k_Start.Y + d_Density * R;
                    double Z = f_Function(X, Y);

                    i_Points3D[C, R] = new cPoint3D(X, Y, Z);
                }
            }

            SetSurfacePoints(i_Points3D, e_Normalize);
        }

        /// <summary>
        /// Here you can assign an Array of 3d points.
        /// Use either SetFunction() or SetSurfacePoints() or SetScatterPoints() or SetScatterLines()
        /// </summary>
        public void SetSurfacePoints(cPoint3D[,] i_Points3D, eNormalize e_Normalize)
        {
            Debug.Assert(!InvokeRequired); // Do not call from other threads or use Invoke()
            mi_MinMax = new cMinMax3D(i_Points3D);
            this.SetSurfacePoints(i_Points3D, mi_MinMax, e_Normalize);
        }

        /// <summary>
        /// Here you can assign an Array of 3d points.
        /// Use either SetFunction() or SetSurfacePoints() or SetScatterPoints() or SetScatterLines()
        /// </summary>
        public void SetSurfacePoints(cPoint3D[,] i_Points3D, cMinMax3D i_MinMax, eNormalize e_Normalize)
        {
            Debug.Assert(!InvokeRequired); // Do not call from other threads or use Invoke()

            mi_ScatterArr = null;
            mi_PolyArr = i_Points3D;
            ms32_Points = i_Points3D.Length;
            mi_MinMax = i_MinMax;
            if (ms32_Points < 4)
                throw new Exception("Insufficient 3D points specified");

            NormalizeRanges(e_Normalize);

            mi_Mouse.mk_Offset = Point.Empty;
            mi_DrawObjects.Clear(); // recalculate
            Invalidate();           // repaint
        }

        /// <summary>
        /// Here you can assign scatterplot points
        /// Each point can define it's own color otherwise a color from ColorScheme will be used.
        /// Use either SetFunction() or SetSurfacePoints() or SetScatterPoints()
        /// </summary>
        public void SetScatterPoints(cScatter[] i_Scatter, eNormalize e_Normalize)
        {
            Debug.Assert(!InvokeRequired); // Do not call from other threads or use Invoke()

            mi_PolyArr    = null;
            mi_ScatterArr = i_Scatter;
            ms32_Points   = i_Scatter.Length;
            mi_MinMax     = new cMinMax3D(i_Scatter);

            NormalizeRanges(e_Normalize);

            mi_Mouse.mk_Offset = Point.Empty;
            mi_DrawObjects.Clear(); // recalculate
            Invalidate();           // repaint
        }

        /// <summary>
        /// Here you can assign scatterplot points which will be connected by lines from one point to the next.
        /// IMPORTANT: The order of the points in the array matters!
        /// Each point can define it's own color otherwise a color from ColorScheme will be used.
        /// Use either SetFunction() or SetSurfacePoints() or SetScatterPoints() or SetScatterLines()
        /// </summary>
        public void SetScatterLines(cScatter[] i_ScatterArr, eNormalize e_Normalize, float f_LineWidth)
        {
            cScatter i_Prev = null;
            foreach (cScatter i_Scatter in i_ScatterArr)
            {
                i_Scatter.mb_Combine  = true;
                i_Scatter.mi_Previous = i_Prev; // Build a single chained list

                if (i_Scatter.mi_Brush != null)
                    i_Scatter.mi_Pen = new Pen(i_Scatter.mi_Brush, f_LineWidth);

                i_Prev = i_Scatter;
            }

            SetScatterPoints(i_ScatterArr, e_Normalize);
        }

        /// <summary>
        /// This function normalizes the 3D ranges for the X,Y,Z coordinates.
        /// Otherwise a 3D range of X,Y from -10 to +10 will appear much smaller than a range from -100 to +100.
        /// It adapts the values so that rotation (phi) goes through the center of the X, Y pane.
        /// </summary>
        private void NormalizeRanges(eNormalize e_Normalize)
        {
            if (mi_MinMax.md_MaxX == mi_MinMax.md_MinX) { mi_MinMax.md_MinX -= 1.0; mi_MinMax.md_MaxX += 1.0; }
            if (mi_MinMax.md_MaxY == mi_MinMax.md_MinY) { mi_MinMax.md_MinY -= 1.0; mi_MinMax.md_MaxY += 1.0; }
            if (mi_MinMax.md_MaxZ == mi_MinMax.md_MinZ) { mi_MinMax.md_MinZ -= 1.0; mi_MinMax.md_MaxZ += 1.0; }

            // Normalize 3D X,Y values to compensate different ranges of X min ... X max and Y min ... Y max
            double d_RangeX = mi_MinMax.md_MaxX - mi_MinMax.md_MinX;
            double d_RangeY = mi_MinMax.md_MaxY - mi_MinMax.md_MinY;

            // Normalize 3D Z value to fit on screen
            double d_RangeZ;
            if (me_Raster == eRaster.Off)
                d_RangeZ = mi_MinMax.md_MaxZ - mi_MinMax.md_MinZ;
            else
                d_RangeZ = Math.Max(0, mi_MinMax.md_MaxZ) - Math.Min(0, mi_MinMax.md_MinZ);

            switch (e_Normalize)
            {
                case eNormalize.MaintainXY:
                    double d_RangeXY = (d_RangeX + d_RangeY) / 2; // average
                    d_RangeX = d_RangeXY;
                    d_RangeY = d_RangeXY;
                    break;

                case eNormalize.MaintainXYZ:
                    double d_RangeXYZ = (d_RangeX + d_RangeY + d_RangeZ) / 3; // average
                    d_RangeX = d_RangeXYZ;
                    d_RangeY = d_RangeXYZ;
                    d_RangeZ = d_RangeXYZ;
                    break;
            }

            mi_Transform.md_NormalizeX = 250.0 / d_RangeX;
            mi_Transform.md_NormalizeY = 250.0 / d_RangeY;
            mi_Transform.md_NormalizeZ = 250.0 / d_RangeZ; // RangeZ will never be zero.

            mi_MinMax.mi_Center3D.md_X = (mi_MinMax.md_MaxX + mi_MinMax.md_MinX) / 2.0;
            mi_MinMax.mi_Center3D.md_Y = (mi_MinMax.md_MaxY + mi_MinMax.md_MinY) / 2.0;
            
            mi_MinMax.mi_Center3D.md_Z = (mi_MinMax.md_MaxZ + mi_MinMax.md_MinZ) / 2.0;
        }

        public void SetCoefficients(double d_Rho, double d_Theta, double d_Phi)
        {
            Debug.Assert(!InvokeRequired); // Do not call from other threads or use Invoke()

            mi_Mouse.SetRho  (d_Rho);
            mi_Mouse.SetTheta(d_Theta);
            mi_Mouse.SetPhi  (d_Phi);

            mi_Transform.SetCoeficients(mi_Mouse);

            mi_DrawObjects.Clear(); // recalculate
            Invalidate();           // repaint
        }

        // ============================================================================

        private void CreateCoordinateSystem(Graphics i_Graph)
        {
            mk_Offset2D = new Point(0, VERT_OFFSET);

            if (me_Raster == eRaster.Off)
                return;

            List<cLine> i_Lines = new List<cLine>();

            // Add the 3 coordinate system main axis
            for (int A=0; A<3; A++)
            {
                cLine i_Axis  = new cLine();
                i_Axis.mi_Pen = mi_AxisPens[A];

                switch ((eCoord)A)
                {
                    case eCoord.X: // Red
                        i_Axis.mi_Points3D[0].md_X = (mi_MinMax.md_MinX * (1.0 / AXIS_EXCESS));
                        i_Axis.mi_Points3D[1].md_X = (mi_MinMax.md_MaxX * AXIS_EXCESS);
                        // ---------------------------
                        i_Axis.mi_Points3D[0].md_Y = (mi_MinMax.md_MinY * (1.0 / AXIS_EXCESS)); // X axis at minimum Y position
                        i_Axis.mi_Points3D[1].md_Y = (mi_MinMax.md_MinY * (1.0 / AXIS_EXCESS)); // X axis at minimum Y position
                        // ---------------------------
                        i_Axis.mi_Points3D[0].md_Z = (mi_MinMax.md_MinZ * (1.0 / AXIS_EXCESS));
                        i_Axis.mi_Points3D[1].md_Z = (mi_MinMax.md_MinZ * (1.0 / AXIS_EXCESS));
                        i_Axis.me_Line   = eCoord.X;
                        i_Axis.me_Offset = eCoord.X; // Hide zero label at X axis (X,X invalid)
                        break;
                    case eCoord.Y: // Green
                        i_Axis.mi_Points3D[0].md_Y = (mi_MinMax.md_MinY * (1.0 / AXIS_EXCESS));
                        i_Axis.mi_Points3D[1].md_Y = (mi_MinMax.md_MaxY * AXIS_EXCESS);
                        // ---------------------------
                        i_Axis.mi_Points3D[0].md_X = (mi_MinMax.md_MinX * (1.0 / AXIS_EXCESS)); // Y axis at minimum X position
                        i_Axis.mi_Points3D[1].md_X = (mi_MinMax.md_MinX * (1.0 / AXIS_EXCESS)); // Y axis at minimum X position
                        // ---------------------------
                        i_Axis.mi_Points3D[0].md_Z = (mi_MinMax.md_MinZ * (1.0 / AXIS_EXCESS));
                        i_Axis.mi_Points3D[1].md_Z = (mi_MinMax.md_MinZ * (1.0 / AXIS_EXCESS));
                        i_Axis.me_Line   = eCoord.Y;
                        i_Axis.me_Offset = eCoord.Z; // Show zero label at Y axis

                        i_Axis.md_Label = (mi_MinMax.md_MinZ * (1.0 / AXIS_EXCESS)); // Show Z min value on Z axis
                        break;
                    case eCoord.Z: // Blue
                        i_Axis.mi_Points3D[0].md_Z = (mi_MinMax.md_MinZ * (1.0 / AXIS_EXCESS));
                        i_Axis.mi_Points3D[1].md_Z = (mi_MinMax.md_MaxZ * AXIS_EXCESS);
                        // ---------------------------
                        i_Axis.mi_Points3D[0].md_X = (mi_MinMax.md_MinX * (1.0 / AXIS_EXCESS)); // Z axis start at minimum X position
                        i_Axis.mi_Points3D[1].md_X = (mi_MinMax.md_MinX * (1.0 / AXIS_EXCESS)); // Z axis start at minimum X position
                        i_Axis.mi_Points3D[0].md_Y = (mi_MinMax.md_MinY * (1.0 / AXIS_EXCESS)); // Z axis start at minimum Y position
                        i_Axis.mi_Points3D[1].md_Y = (mi_MinMax.md_MinY * (1.0 / AXIS_EXCESS)); // Z axis start at minimum Y position
                        i_Axis.me_Line   = eCoord.Z;
                        i_Axis.me_Offset = eCoord.Z; // Hide zero label at Z axis (Z,Z invalid)
                        break;
                }

                i_Axis.mi_Points2D[0] = mi_Transform.Project(i_Axis.mi_Points3D[0], mi_MinMax.mi_Center3D);
                i_Axis.mi_Points2D[1] = mi_Transform.Project(i_Axis.mi_Points3D[1], mi_MinMax.mi_Center3D);
                i_Axis.CalcAngle2D();

                i_Lines.Add(i_Axis);
            }

            // Calculate currently visible quadrant
            mi_Quadrant = new cQuadrant(mi_Mouse.md_Phi, i_Lines[(int)eCoord.X], i_Lines[(int)eCoord.Y], i_Lines[(int)eCoord.Z]);

            // Add raster lines in 6 different directions
            if (me_Raster >= eRaster.Raster)
            {
                for (int A=0; A<3; A++) // iterate axis X,Y,Z
                {
                    // Combine X+Y, Y+Z, Z+X
                    eCoord e_First  = (eCoord)(A);
                    eCoord e_Second = (eCoord)((A+1) % 3);

                    for (int D=0; D<2; D++) // iterate first, second
                    {
                        cLine i_FirstAxis  = i_Lines[(int)e_First];
                        cLine i_SecondAxis = i_Lines[(int)e_Second];

                        double d_SecndStart = i_SecondAxis.mi_Points3D[0].GetValue(e_Second);
                        double d_SecndEnd   = i_SecondAxis.mi_Points3D[1].GetValue(e_Second);

                        // Distance between raster lines
                        double d_Interval = CalculateInterval(d_SecndEnd - d_SecndStart);

                        for (int L=-11; L<11; L++) // iterate raster line
                        {
                            double d_Offset = (d_SecndEnd - d_SecndStart / 2) + (d_Interval * L);
                            if (d_Offset < d_SecndStart || d_Offset > d_SecndEnd)
                                continue;

                            cLine i_Raster = new cLine();
                            i_Raster.mi_Pen         = mi_RasterPens[(int)e_Second];
                            i_Raster.me_Line        = e_First;
                            i_Raster.me_Offset      = e_Second;
                            i_Raster.md_Label       = d_Offset;
                            i_Raster.mi_Points3D[0] = i_FirstAxis.mi_Points3D[0].Clone();
                            i_Raster.mi_Points3D[1] = i_FirstAxis.mi_Points3D[1].Clone();

                            i_Raster.mi_Points3D[0].SetValue(e_Second, d_Offset);
                            i_Raster.mi_Points3D[1].SetValue(e_Second, d_Offset);

                            // Do not draw a raster line which equals a main axis
                            if (i_Raster.CoordEquals(i_Lines[(int)eCoord.X]) ||
                                i_Raster.CoordEquals(i_Lines[(int)eCoord.Y]) ||
                                i_Raster.CoordEquals(i_Lines[(int)eCoord.Z]))
                                continue;

                            if ((e_First == eCoord.X && e_Second == eCoord.Z) || // Red
                                (e_First == eCoord.Z && e_Second == eCoord.X))
                            {
                                i_Raster.md_Sort = mi_Quadrant.md_SortXZ;
                            }
                            else if ((e_First == eCoord.Z && e_Second == eCoord.Y) || // Green
                                     (e_First == eCoord.Y && e_Second == eCoord.Z))
                            {
                                i_Raster.md_Sort = mi_Quadrant.md_SortYZ;
                            }
                            else // X + Y Blue
                            {
                                i_Raster.md_Sort = mi_Quadrant.md_SortXY;

                                // Special case: XY raster lines must be shifted down to negative end of Z axis
                                cLine i_AxisZ = i_Lines[(int)eCoord.Z];
                                i_Raster.mi_Points3D[0].md_Z = i_AxisZ.mi_Points3D[0].md_Z;
                                i_Raster.mi_Points3D[1].md_Z = i_AxisZ.mi_Points3D[0].md_Z;
                            }

                            i_Lines.Add(i_Raster);
                        }

                        // Swap first and second
                        eCoord e_Temp = e_First;
                        e_First  = e_Second;
                        e_Second = e_Temp;
                    }
                }
            }

            // Convert axis and raster lines 3D --> 2D
            foreach (cLine i_Line in i_Lines)
            {
                i_Line.mi_Points2D[0] = mi_Transform.Project(i_Line.mi_Points3D[0], mi_MinMax.mi_Center3D);
                i_Line.mi_Points2D[1] = mi_Transform.Project(i_Line.mi_Points3D[1], mi_MinMax.mi_Center3D);

                AddDrawObject(new cDrawObj(i_Line, i_Line.md_Sort));
            }

            // Move the graph to the left when labels are enabled
            if (me_Raster == eRaster.Labels)
            {
                int s32_LabelWidth = 0;
                foreach (cLine i_Line in i_Lines)
                {
                    if (i_Line.me_Line == eCoord.Y && i_Line.me_Offset == eCoord.Z)
                    {
                        String s_Label = FormatLabel(i_Line.md_Label);
                        SizeF  k_Size  = i_Graph.MeasureString(s_Label, Font);
                        s32_LabelWidth = Math.Max(s32_LabelWidth, (int)k_Size.Width);
                    }
                }
                mk_Offset2D.X -= s32_LabelWidth / 2;
            }
        }

        private void CreatePolygons()
        {
            cPoint2D[,] i_Points2D = new cPoint2D[mi_PolyArr.GetLength(0), mi_PolyArr.GetLength(1)];

            // Convert 3D --> 2D
            for (int X = 0; X < mi_PolyArr.GetLength(0); X++)
            {
                for (int Y = 0; Y < mi_PolyArr.GetLength(1); Y++)
                {
                    i_Points2D[X,Y] = mi_Transform.Project(mi_PolyArr[X,Y], mi_MinMax.mi_Center3D);
                }
            }

            // Create polygons
            for (int X = 0; X < mi_PolyArr.GetLength(0) -1; X++)
            {
                for (int Y = 0; Y < mi_PolyArr.GetLength(1) -1; Y++)
                {
                    cPolygon i_Poly = new cPolygon(i_Points2D[X,   Y],
                                                   i_Points2D[X,   Y+1],
                                                   i_Points2D[X+1, Y+1],
                                                   i_Points2D[X+1, Y]);
                    
                    double Z1 = mi_PolyArr[X,   Y]  .md_Z;
                    double Z2 = mi_PolyArr[X,   Y+1].md_Z;
                    double Z3 = mi_PolyArr[X+1, Y+1].md_Z;
                    double Z4 = mi_PolyArr[X+1, Y]  .md_Z;
                    double Zavrg = (Z1 + Z2 + Z3 + Z4) / 4.0;

                    i_Poly.md_FactorZ = (Zavrg - mi_MinMax.md_MinZ) / (mi_MinMax.md_MaxZ - mi_MinMax.md_MinZ);

                    // Polygons must be painted in correct order: from back to front. Order depends on rotation angle.
                    double d_Sort = mi_Transform.ProjectXY(X +1, Y +1); // +1 because Z axis is at 0,0

                    AddDrawObject(new cDrawObj(i_Poly, d_Sort));
                }
            }
        }

        private void CreateScatterDots()
        {
            // Convert 3D --> 2D
            foreach (cScatter i_Scatter in mi_ScatterArr)
            {
                i_Scatter.SetPoint2D(mi_Transform.Project(i_Scatter.mi_Point3D, mi_MinMax.mi_Center3D));

                // Do not store the Brush from the ColorScheme in i_Scatter.mi_Brush.
                // The colors would not change when the user changes the colorscheme.
                if (i_Scatter.mi_Brush == null)
                    i_Scatter.md_FactorZ = (i_Scatter.mi_Point3D.md_Z - mi_MinMax.md_MinZ) / (mi_MinMax.md_MaxZ - mi_MinMax.md_MinZ);

                // Scatter circles must be painted in correct order: from back to front. Order depends on rotation angle.
                double d_Sort = mi_Transform.ProjectXY(i_Scatter.mi_Point3D.md_X + 1.0, 
                                                       i_Scatter.mi_Point3D.md_Y + 1.0); // +1 because Z axis is at 0,0

                AddDrawObject(new cDrawObj(i_Scatter, d_Sort));
            }
        }

        /// <summary>
        /// polygons, scatter points and axis must be sorted for correct drawing order
        /// </summary>
        private void AddDrawObject(cDrawObj i_DrawObj)
        {
            int P;
            for (P=0; P<mi_DrawObjects.Count; P++)
            {
                if (mi_DrawObjects[P].md_Sort > i_DrawObj.md_Sort)
                    break;
            }
            mi_DrawObjects.Insert(P, i_DrawObj);
        }

        private Brush GetSchemeBrush(double d_FactorZ)
        {
            if (mi_SchemeBrushes == null || double.IsNaN(d_FactorZ))
                return Brushes.Goldenrod;

            d_FactorZ = Math.Min(1.0, d_FactorZ);
            d_FactorZ = Math.Max(0.0, d_FactorZ);

            // d_FactorZ is a value between 0.0 and 1.0
            int s32_Index = (int)(d_FactorZ * (mi_SchemeBrushes.Length - 1));
            return mi_SchemeBrushes[s32_Index];
        }

        private Pen GetSchemePen(double d_FactorZ)
        {
            if (mi_SchemePens == null || double.IsNaN(d_FactorZ))
                return Pens.Goldenrod;

            d_FactorZ = Math.Min(1.0, d_FactorZ);
            d_FactorZ = Math.Max(0.0, d_FactorZ);

            // d_FactorZ is a value between 0.0 and 1.0
            int s32_Index = (int)(d_FactorZ * (mi_SchemePens.Length - 1));
            return mi_SchemePens[s32_Index];
        }

        private void SetAxisColor(eCoord e_Coord, Color c_Color)
        {
            mi_AxisBrushes[(int)e_Coord] = new SolidBrush(c_Color);            // Label text
            mi_AxisPens   [(int)e_Coord] = new Pen(c_Color, 3);                // Main coordinate axis
            mi_RasterPens [(int)e_Coord] = new Pen(BrightenColor(c_Color), 1); // Raster lines
        }

        /// <summary>
        /// Makes a color brigther
        /// </summary>
        private Color BrightenColor(Color c_Color)
        {
            int s32_Red   = c_Color.R + (255 - c_Color.R) / 2;
            int s32_Green = c_Color.G + (255 - c_Color.G) / 2;
            int s32_Blue  = c_Color.B + (255 - c_Color.B) / 2;

            return Color.FromArgb(255, s32_Red, s32_Green, s32_Blue);
        }

        /// <summary>
        /// returns intervals of  0.1, 0.2, 0.5,  1, 2, 5,  10, 20, 50,  etc...
        /// The count of intervals which fit into the range is always between 5 and 10
        /// </summary>
        private double CalculateInterval(double d_Range)
        {
            double d_Factor = Math.Pow(10.0, Math.Floor(Math.Log10(d_Range)));
            if (d_Range / d_Factor >= 5.0)
                return d_Factor;
            else if (d_Range / (d_Factor / 2.0) >= 5.0)
                return d_Factor / 2.0;
            else
                return d_Factor / 5.0;
        }

        // md_Label = 123.000 --> display "123"
        // md_Label =  15.700 --> display "15.7"  
        // md_Label =   4.260 --> display "4.26"
        // md_Label =   0.834 --> display "0.834"
        private String FormatLabel(double d_Label)
        {
            return d_Label.ToString("0.000", CultureInfo.InvariantCulture).TrimEnd('0').TrimEnd('.');
        }

        // ============================================================================

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Draw(e.Graphics);
        }

        public Bitmap GetScreenshot()
        {
            Bitmap i_Bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
            using (Graphics i_Graph = Graphics.FromImage(i_Bmp))
            {
                Draw(i_Graph);
            }
            return i_Bmp;
        }

        private void Draw(Graphics i_Graph)
        {
            if (mi_DrawObjects.Count == 0)
            {
                CreateCoordinateSystem(i_Graph);

                if (mi_PolyArr    != null) CreatePolygons();
                if (mi_ScatterArr != null) CreateScatterDots();
            }

            i_Graph.Clear(BackColor);

            // Draw axis legends at bottom
            int X = 4;
            int Y = ClientSize.Height - Font.Height - 4;
            for (int i=2; i>=0; i--)
            {
                if (String.IsNullOrEmpty(ms_AxisLegends[i]))
                    continue;

                String s_Disp = String.Format("{0}: {1}", (eCoord)i, ms_AxisLegends[i]);
                i_Graph.DrawString(s_Disp, Font, mi_AxisBrushes[i], X,  Y);
                Y -= Font.Height;
            }

            // Draw rotation legend at top
            if (mi_TopLegendBrush != null)
            {
                String[] s_Legend = new String[] { "Rotation:", "Elevation:", "Distance:" };
                String[] s_Value  = new String[] { String.Format("{0:+#;-#;0}°", (int)mi_Mouse.md_Phi),
                                                   String.Format("{0:+#;-#;0}°", (int)mi_Mouse.md_Theta),
                                                   String.Format("{0}",          (int)mi_Mouse.md_Rho) };

                SizeF k_Size = i_Graph.MeasureString(s_Legend[1], Font); // measure the widest string
                X = 4;
                Y = 3;
                for (int i=0; i<3; i++)
                {
                    i_Graph.DrawString(s_Legend[i], Font, mi_TopLegendBrush, X,  Y);
                    i_Graph.DrawString(s_Value [i], Font, mi_TopLegendBrush, X + k_Size.Width, Y);
                    Y += Font.Height;
                }
            }

            // Set X, Y offset which user has set by mouse dragging with SHIFT key pressed
            i_Graph.TranslateTransform(mi_Mouse.mk_Offset.X + mk_Offset2D.X, 
                                       mi_Mouse.mk_Offset.Y + mk_Offset2D.Y);

            SmoothingMode e_Smooth = SmoothingMode.Invalid;

            foreach (cDrawObj i_DrawObj in mi_DrawObjects)
            {
                if (!i_DrawObj.IsValid)
                    continue; // avoid overflow exception or hanging

                if (i_DrawObj.mi_Polygon != null)
                {
                    if (e_Smooth != SmoothingMode.None) // avoid unneccessary calls into GDI+ (speed optimization)
                    {
                        // Drawing polygon lines with antialias would make them very thick and black.
                        e_Smooth = SmoothingMode.None;
                        i_Graph.SmoothingMode = SmoothingMode.None;
                    }

                    cPolygon i_Poly  = i_DrawObj.mi_Polygon;
                    Brush    i_Brush = GetSchemeBrush(i_Poly.md_FactorZ);
                    i_Graph.FillPolygon(i_Brush, i_Poly.mk_Points);

                    if (mi_PolyLinePen != null)
                    {
                        i_Graph.DrawPolygon(mi_PolyLinePen, i_Poly.mk_Points);
                    }
                }
                else if (i_DrawObj.mi_Scatter != null)
                {
                    if (e_Smooth != SmoothingMode.AntiAlias) // avoid unneccessary calls into GDI+ (speed optimization)
                    {
                        e_Smooth = SmoothingMode.AntiAlias;
                        i_Graph.SmoothingMode = SmoothingMode.AntiAlias;
                    }

                    cScatter i_Scatter = i_DrawObj.mi_Scatter;

                    if (i_Scatter.mb_Combine) // points connected by lines
                    {
                        if (i_Scatter.mi_Previous != null)
                        {
                            Pen i_Pen = i_Scatter.mi_Pen;
                            if (i_Pen == null) // user has not defined a color
                                i_Pen = GetSchemePen(i_Scatter.md_FactorZ);

                            i_Graph.DrawLine(i_Pen, i_Scatter.mi_Previous.mk_Point, i_Scatter.mk_Point);
                        }
                    }
                    else // separate scatter points
                    {
                        Brush i_Brush = i_Scatter.mi_Brush;
                        if (i_Brush == null) // user has not defined a color
                            i_Brush = GetSchemeBrush(i_Scatter.md_FactorZ);

                        i_Graph.FillEllipse(i_Brush, i_Scatter.mk_Point.X, i_Scatter.mk_Point.Y, SCATTER_SIZE * 2, SCATTER_SIZE * 2);
                    }
                }
                else // cLine
                {
                    if (e_Smooth != SmoothingMode.AntiAlias) // avoid unneccessary calls into GDI+ (speed optimization)
                    {
                        e_Smooth = SmoothingMode.AntiAlias;
                        i_Graph.SmoothingMode = SmoothingMode.AntiAlias;
                    }

                    cLine i_Line = i_DrawObj.mi_Line;
                    i_Graph.DrawLine(i_Line.mi_Pen, i_Line.mi_Points2D[0].Coord, i_Line.mi_Points2D[1].Coord);

                    // ------------ Label ------------

                    if (me_Raster == eRaster.Labels        && 
                        mi_Quadrant.mb_BottomView == false && // no label in bottom view
                        mi_Quadrant.ms32_Quadrant == 3)       // only in quadrant 3 showing labels makes sense
                    {
                        PointF k_Pos = i_Line.mi_Points2D[1].Coord;
                        StringFormat i_Align = new StringFormat();
                        if (i_Line.me_Line == eCoord.Y && i_Line.me_Offset == eCoord.Z)
                        {
                            k_Pos.X += 5;
                            k_Pos.Y -= Font.Height / 2;
                        }
                        else if (i_Line.me_Line == eCoord.Y && i_Line.me_Offset == eCoord.X)
                        {
                            k_Pos.X += (float)mi_Transform.ProjectXY(5, -5);
                            k_Pos.Y += (float)mi_Transform.ProjectXY(-Font.Height / 2, 5);
                        }
                        else if (i_Line.me_Line == eCoord.X && i_Line.me_Offset == eCoord.Y)
                        {
                            k_Pos.X += (float)mi_Transform.ProjectXY(5, -5);
                            k_Pos.Y += (float)mi_Transform.ProjectXY(5, -Font.Height / 2);
                            i_Align.Alignment = StringAlignment.Far;
                        }
                        else continue;

                        String s_Label = FormatLabel(i_Line.md_Label);
                        Brush  i_Brush = mi_AxisBrushes[(int)i_Line.me_Offset];
                        i_Graph.DrawString(s_Label, Font, i_Brush, k_Pos, i_Align);
                    }
                }
            }

            if (mi_BorderPen != null)
            {
                i_Graph.ResetTransform();
                Rectangle r_Border = ClientRectangle;
                i_Graph.DrawRectangle(mi_BorderPen, r_Border.X, r_Border.Y, r_Border.Width - 1, r_Border.Height - 1);
            }

            i_Graph.TranslateClip(0, -50);
        }

        // ============================================================================

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            mi_Mouse.mk_LastPos = e.Location;

            if (mi_DrawObjects.Count == 0)
                return;
           
            switch (Control.ModifierKeys)
            {
                case Keys.None:
                    if (e.Button == MouseButtons.Left)
                    {
                        Cursor = Cursors.NoMoveVert;
                        mi_Mouse.me_Action = eMouseAction.Theta;
                    }

                    if (e.Button == MouseButtons.Right)
                    {
                        Cursor = Cursors.NoMoveHoriz;
                        mi_Mouse.me_Action = eMouseAction.Phi;
                    }
                    break;

                case Keys.Shift:
                    if (e.Button == MouseButtons.Left)
                    {
                        Cursor = Cursors.NoMove2D;
                        mi_Mouse.me_Action = eMouseAction.Move;
                    }
                    break;

                case Keys.Control:
                    if (e.Button == MouseButtons.Left)
                    {
                        Cursor = Cursors.SizeNS;
                        mi_Mouse.me_Action = eMouseAction.Rho;
                    }
                    break;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            mi_Mouse.me_Action = eMouseAction.None;
            Cursor = Cursors.Arrow;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            mi_Mouse.me_Action = eMouseAction.None;
            Cursor = Cursors.Arrow;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (mi_Mouse.OnMouseWheel(-e.Delta))
            {
                mi_Transform.SetCoeficients(mi_Mouse);

                mi_DrawObjects.Clear(); // recalculate
                Invalidate();           // repaint
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            int s32_DiffX = e.X - mi_Mouse.mk_LastPos.X;
            int s32_DiffY = e.Y - mi_Mouse.mk_LastPos.Y;
            mi_Mouse.mk_LastPos = e.Location;

            switch (mi_Mouse.me_Action)
            {
                case eMouseAction.Move:
                    mi_Mouse.mk_Offset.X += s32_DiffX;
                    mi_Mouse.mk_Offset.Y += s32_DiffY;

                    Invalidate(); // repaint only
                    break;

                case eMouseAction.Rho:
                case eMouseAction.Theta:
                case eMouseAction.Phi:
                    mi_Mouse.OnMouseMove(s32_DiffX, s32_DiffY);
                    mi_Transform.SetCoeficients(mi_Mouse);

                    mi_DrawObjects.Clear(); // recalculate
                    Invalidate();           // repaint
                    break;
            }
        }

        // ============================================================================

        /// <summary>
        /// This is only called when the user moves the trackbar, not when TrackBar.Value is set programmatically.
        /// </summary>
        void OnTrackbarScroll(object sender, EventArgs e)
        {
            mi_Mouse.OnTrackBarScroll();
            mi_Transform.SetCoeficients(mi_Mouse);

            mi_DrawObjects.Clear(); // recalculate
            Invalidate();           // repaint
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            mi_Transform.SetSize(ClientSize);

            mi_DrawObjects.Clear(); // recalculate
            Invalidate();           // repaint
        }
    }
}