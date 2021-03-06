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
    public class IconButton : Button
    {
        public bool ShowIconOnly { get; set; }
        public IconButton()
        {
            this.FlatStyle = FlatStyle.System;
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        private Boolean useicon = true; //Checks if user wants to use an icon instead of a bitmap
        private Bitmap image_;
        //Image alignment is ignored at the moment. Property overrides inherited image property
        //Supports images other than bitmap, supports transparency on .NET 2.0
        [Description("Gets or sets the image that is displayed on a button control."), Category("Appearance"), DefaultValue(null)]
        public new Bitmap Image
        {
            get
            {
                return image_;
            }
            set
            {
                image_ = value;
                if (value != null)
                {
                    System.Windows.Forms.CreateParams _ = CreateParams;
                    this.useicon = false;
                    this.Icon = null;
                }
                this.SetShield(false);
                SetImage();
            }
        }
        private Icon icon_;
        [Description("Gets or sets the icon that is displayed on a button control."), Category("Appearance"), DefaultValue(null)]
        public Icon Icon
        {
            get
            {
                return icon_;
            }
            set
            {
                icon_ = value;
                if (icon_ != null)
                {
                    this.useicon = true;
                }
                this.SetShield(false);
                SetImage();
                
            }
        }

        [Description("Refreshes the image displayed on the button.")]
        public void SetImage()
        {
            IntPtr iconhandle = IntPtr.Zero;
            if (!this.useicon)
            {
                if (this.image_ != null)
                {
                    iconhandle = image_.GetHicon(); //Gets the handle of the bitmap
                }
            }
            else
            {
                if (this.icon_ != null)
                {
                    iconhandle = Icon.Handle;
                }
            }

            //Set the button to use the icon. If no icon or bitmap is used, no image is set.
            SendMessage(this.Handle, CntrlConst.BM_SETIMAGE, 1, (int)iconhandle);
        }
        private Boolean showshield_ = false;
        [Description("Gets or sets whether if the control should use an elevated shield icon."), Category("Appearance"), DefaultValue(false)]
        public Boolean ShowShield
        {
            get
            {
                return showshield_;
            }
            set
            {
                showshield_ = value;
                this.SetShield(value);
                if (!value)
                {
                    this.SetImage();
                }
            }
        }
        public void SetShield(Boolean Value)
        {
            SendMessage(this.Handle, CntrlConst.BCM_SETSHIELD, (int)IntPtr.Zero, (int)new IntPtr(showshield_ ? 1 : 0));
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cParams = base.CreateParams;
                if (Text == "") cParams.Style |= 0x00000040;
                if (this.ShowIconOnly == true)
                {
                    //Render button without text
                    cParams.Style |= 0x00000040; // BS_ICON value
                }
                return cParams;
            }
        }

    }
}
