using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAPITools.Controls
{
    public class CueTextbox : TextBox
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);

        private string cuetext_ = "";
        public string CueText
        {
            get
            {
                return cuetext_;
            }
            set
            {
                cuetext_ = value;
                this.SetCueText(this.cuetext_);
            }
        }
        public void SetCueText(string Cue_Text)
        {
            SendMessage(this.Handle, CntrlConst.EM_SETCUEBANNER, IntPtr.Zero, Cue_Text);
        }
    }
}
