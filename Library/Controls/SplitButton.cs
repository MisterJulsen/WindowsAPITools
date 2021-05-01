using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAPITools.Controls
{
    public delegate void DropDownClicked();
    public class SplitButton : IconButton
    {

        const int BS_SPLITBUTTON = 0x0000000C;

        //Import
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr SendMessage(HandleRef hWnd, UInt32 Msg, IntPtr wParam, string lParam);

        public SplitButton()
        {
            //Set the flatstyle
            this.FlatStyle = FlatStyle.System;
            this.DropDown_Clicked += new DropDownClicked(this.launchmenu);
            this.ddm_.Collapse += new EventHandler(this.CloseMenuDropdown);
        }

        //Set the theme
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cParams = base.CreateParams;
                cParams.Style |= BS_SPLITBUTTON;
                return cParams;
            }
        }

        const int BCM_FIRST = 0x1600;

        //Receives messages sent to the control
        public int isatdropdown = 0;
        public int ismousedown = 0;
        public int isbumped = 0;
        public int dropdownpushed = 0;
        protected override void WndProc(ref Message m)
        {
            // Listen for operating system messages; Filter out a lot of messages here
            //The dropdown glyph changes:
            //Mousedown (at dropdown area) => Dropdown Event fired => Glyph changes => MouseUp => Glyph Changes
            switch (m.Msg)
            {
                case (CntrlConst.BCM_SETDROPDOWNSTATE):
                    //PROBLEM: other buttons also have would encounter this message
                    if (m.HWnd == this.Handle)
                    {
                        ////This message seems to occur when the drop down is clicked; Occures several times, but one of them have the value of 1 in WParams
                        if (m.WParam.ToString() == "1")
                        {
                            if (dropdownpushed == 0)
                            {
                                //One time-initiation per mouse press
                                this.dropdownpushed = 1;
                                DropDown_Clicked();
                            }
                        }
                        if (this.ismousedown == 1)
                        {
                            this.isatdropdown = 1;
                        }
                    }
                    break;
                case (CntrlConst.WM_PAINT):
                    //Paints the control to have the dropdown when needed
                    if (this.dropdownpushed == 1)
                    {
                        this.SetDropDownState(1);
                    }
                    break;
                case (CntrlConst.WM_LBUTTONDOWN):
                    this.ismousedown = 1;
                    break;
                case (CntrlConst.WM_MOUSELEAVE):
                    if (this.isatdropdown == 1)
                    {
                        this.SetDropDownState(0);
                        this.isatdropdown = 0;
                        this.ismousedown = 0;
                    }
                    break;
                case (CntrlConst.WM_LBUTTONUP):
                    if (this.isatdropdown == 1)
                    {
                        this.SetDropDownState(0);
                        this.isatdropdown = 0;
                        this.ismousedown = 0;
                    }
                    break;
                case (CntrlConst.WM_KILLFOCUS):
                    if (this.isatdropdown == 1)
                    {
                        this.SetDropDownState(0);
                        this.isatdropdown = 0;
                        this.ismousedown = 0;
                    }
                    break;
            }
            base.WndProc(ref m);
        }
        public void SetDropDownState(int Pushed)
        {
            if (Pushed == 0)
            {
                this.dropdownpushed = 0; //Removes dropdown pushed state
            }
            SendMessage(this.Handle, BCM_FIRST + 0x0006, Pushed, 0);
        }
        public event DropDownClicked DropDown_Clicked; //The event which would be fired whenever the drop-down is clicked on.
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SplitButton
            // 
            this.ResumeLayout(false);
        }

        public void launchmenu()
        {
            if (ddm_.MenuItems.Count != 0)
            {
                this.ddm_.Show(this, new Point(this.Width, this.Height), LeftRightAlignment.Left);
            }
        }

        public void CloseMenuDropdown(object sender, EventArgs e)
        {
            //Hmm... contextmenu "collapse" state doesn't seem to fire up when a menuitem is clicked. Does not work here yet.
            this.SetDropDownState(0);
        }

        private ContextMenu ddm_ = new ContextMenu();
        public ContextMenu Dropdownmenu
        {
            get
            {
                return ddm_;
            }
            set
            {
                ddm_ = value;
            }
        }
    }
}
