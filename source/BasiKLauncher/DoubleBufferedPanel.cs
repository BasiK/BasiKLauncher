using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace BasiK.BasiKLauncher
{
    class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            this.DoubleBuffered = true;
        }
    }
}
