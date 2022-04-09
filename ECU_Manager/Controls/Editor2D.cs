using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECU_Manager.Controls
{
    public partial class Editor2D : Form
    {
        private string sName;
        private int sArraySizeX;
        private string sConfigSizeX;
        private string sConfigSizeY;
        private string sConfigDepX;
        private string sConfigDepY;
        private string sParamsStatusX;
        private string sParamsStatusY;
        private string sParamsStatusD;
        private string sTitleStatusX;
        private string sTitleStatusY;
        private string sTitleStatusD;
        private string sArrayName;
        private float fMinY;
        private float fMaxY;
        private float fStepSize;
        private bool bLog10;

        public Editor2D(string name, float min, float max, float step, bool log10 = false)
        {
            InitializeComponent();

            sName = name;
            fMinY = min;
            fMaxY = max;
            fStepSize = step;

            bLog10 = log10;
        }

        public void SetConfig(string arrayname, string sizex, string sizey, string depx, string depy, int arraysizex)
        {
            sArrayName = arrayname;
            sConfigSizeX = sizex;
            sConfigSizeY = sizey;
            sConfigDepX = depx;
            sConfigDepY = depy;
            sArraySizeX = arraysizex;
        }

        public void SetX(string param, string title)
        {
            sParamsStatusX = param;
            sTitleStatusX = title;
        }

        public void SetY(string param, string title)
        {
            sParamsStatusY = param;
            sTitleStatusY = title;
        }

        public void SetD(string param, string title)
        {
            sParamsStatusD = param;
            sTitleStatusD = title;
        }
    }
}
