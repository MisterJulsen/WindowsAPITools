using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAPITools
{
    public class DefaultCtrlData
    {
        public FlatStyle flatstyle { get; set; } = FlatStyle.System;
        public Color backcolor { get; set; } = Color.FromKnownColor(KnownColor.Control);
        public Color forecolor { get; set; } = Color.Black;


        public MeasureItemEventHandler measureitem { get; set; } = null;
        public DrawItemEventHandler drawitem1 { get; set; } = null;
        public DrawItemEventHandler drawitem2 { get; set; } = null;
    }
}
