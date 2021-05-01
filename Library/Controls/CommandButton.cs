using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAPITools.Controls
{
    [ToolboxBitmap(typeof(Button))]
    public class CommandButton : IconButton
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);

        public CommandButton()
        {
            this.FlatStyle = FlatStyle.System;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= CntrlConst.BS_COMMANDLINK;
                return cp;
            }
        }

        [Description("Gets or sets the note that is displayed on a button control."), Category("Appearance"), DefaultValue("")]
        private string note_ = "";
        public string Note
        {
            get
            {
                return this.note_;
            }
            set
            {
                this.note_ = value;
                this.SetNote(this.note_);
            }
        }
        [Description("Sets the note displayed on the button.")]
        void SetNote(string NoteText)
        {
            //Sets the note
            SendMessage(this.Handle, CntrlConst.BCM_SETNOTE, (int)IntPtr.Zero, NoteText);
        }
    }
}
