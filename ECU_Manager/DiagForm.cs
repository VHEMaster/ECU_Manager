using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ECU_Manager.Packets;
using ECU_Manager.Structs;
using ECU_Manager.Tools;

namespace ECU_Manager
{
    public partial class DiagForm : Form, IEcuEventHandler
    {

        public DiagForm(MiddleLayer middleLayer)
        {
            InitializeComponent();
        }

        public void UpdateParametersEvent(EcuParameters parameters)
        {
            Action action = new Action(() => { this.UpdateParameters(parameters); });
            if (this.InvokeRequired)
                this.BeginInvoke(action);
            else action.Invoke();
        }

        public void DragStartAckEvent(PK_DragStartAcknowledge dsaa)
        {
        }

        public void SynchronizedEvent()
        {
        }

        public void DragStopAckEvent(PK_DragStopAcknowledge dsta)
        {
        }

        public void UpdateDragPointEvent(PK_DragPointResponse dpr)
        {
        }

        public void UpdateDragStatusEvent(PK_DragUpdateResponse dur)
        {
        }

        private void UpdateParameters(EcuParameters parameters)
        {

        }
    }
}