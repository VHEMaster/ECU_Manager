using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECU_Manager.Controls
{
    public partial class NumericUpDownOneWheel : NumericUpDown
    {

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            HandledMouseEventArgs hme = e as HandledMouseEventArgs;
            if (hme != null)
                hme.Handled = true;

            if (e.Delta > 0 && (this.Value + this.Increment) <= this.Maximum)
                this.Value += this.Increment;
            else if (e.Delta < 0 && (this.Value - this.Increment) >= this.Minimum)
                this.Value -= this.Increment;
        }
        
    }
}
